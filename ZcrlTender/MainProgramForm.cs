using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TenderLibrary;
using ZcrlTender.ViewModels;
using System.Data.Entity;
using System.Collections.Specialized;
using ZcrlTender.ExcelReports;

namespace ZcrlTender
{
    public partial class MainProgramForm : Form
    {
        private static TenderYear currentTenderYear;
        private List<KekvCode> kekvs;
        private BindingList<KekvCode> kekvsForCBList;
        private BindingList<Estimate> estimatesForCBList;
        private BindingList<Contractor> contractorsForCBList;
        private BindingList<Contract> contractsForCBList;
        private List<MoneySource> moneySources;

        private BindingList<EstimatesTableEntry> estimatesList;
        private BindingList<TenderPlanItemsTableEntry> tenderPlanItemsList;
        private BindingList<ContractsTableEntry> contractsList;
        private BindingList<InvoiceRecordsTableEntry> invoicesList;
        private BindingList<PlannedSpendingTableEntry> plannedSpendingList;

        private volatile bool hasEstimateFreeMoney;

        // Список цветов, которым выделяються записи в годовом плане с одинаковыми кодами
        private Color[] samePlanCodesColors;

        // Флаг процесса загрузки данных
        private object loadingIndicator;

        public static TenderYear CurrentTenderYear
        {
            set
            {
                // Повторная установка года (в процессе работы невозможна)
                if(currentTenderYear != null)
                {
                    // Выбрасываем исключения с целью локализации в коде места где выполняется данная недопустимая операция
                    new Exception("Спроба повторної ініціалізації року закупівлі");
                }

                currentTenderYear = value;
            }
            get
            {
                if (currentTenderYear == null)
                {
                    return null;
                }
                else
                {
                    return new TenderYear 
                    { 
                        Id = currentTenderYear.Id, 
                        Description = currentTenderYear.Description, 
                        Year = currentTenderYear.Year 
                    };
                }
            }
        }

        // Флаг показывающий то, что изменения выбранного элемента в выпадающем списке (ComboBox) 
        // производятся пользователем
        private volatile bool controlsDataWasChangedByUser;

        public MainProgramForm()
        {
            InitializeComponent();

            samePlanCodesColors = new Color[2];
            samePlanCodesColors[0] = System.Drawing.ColorTranslator.FromHtml("#DC143C");
            samePlanCodesColors[1] = System.Drawing.ColorTranslator.FromHtml("#FF0000");

            // Отключаем переключатель просмотра остатков по разным системам (оставляем доработку этого функционала в будущих версиях)
            radioButton1.Visible = radioButton2.Visible = false;

            // Настраиваем диапазоны доступных дат
            contStartDatePicker.Value = contEndDatePicker.Value = invStartDatePicker.Value = invEndDatePicker.Value 
                = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0).AddYears(Convert.ToInt32(CurrentTenderYear.Year) - DateTime.Now.Year);
            contStartDatePicker.MaxDate = contEndDatePicker.MaxDate = invStartDatePicker.MaxDate = invEndDatePicker.MaxDate
                = new DateTime(Convert.ToInt32(CurrentTenderYear.Year), 12, 31, 0, 0, 0);
            contStartDatePicker.MinDate = contEndDatePicker.MinDate = invStartDatePicker.MinDate = invEndDatePicker.MinDate
                = new DateTime(Convert.ToInt32(CurrentTenderYear.Year), 1, 1, 0, 0, 0);

            hasEstimateFreeMoney = false;
            estimateTable.AutoGenerateColumns = 
                tenderPlanTable.AutoGenerateColumns =
                moneyRemainsTable.AutoGenerateColumns = 
                contractsTable.AutoGenerateColumns = 
                invoicesTable.AutoGenerateColumns = 
                spendingTable.AutoGenerateColumns = false;

            loadingIndicator = new object();

            this.Text = "Тендерні закупівлі :: " + currentTenderYear.Year + " рік";

            using(TenderContext tc = new TenderContext())
            {
                kekvsForCBList = new BindingList<KekvCode>();
                kekvs = tc.KekvCodes.ToList();
                moneySources = tc.MoneySources.ToList();

                DataGridViewHelper.DrawMoneyTotalsTableSchema<KekvCode, MoneySource>(moneyRemainsTable,
                    kekvs, 
                    moneySources, 
                    t => t.Code, 
                    t => t.Name);

                // Формирование списка КЕКВ
                kekvsForCBList = new BindingList<KekvCode>(kekvs.ToList());
                kekvsForCBList.Insert(0, new KekvCode { Id = -1, Code = "- ВСІ -" });
                tpKekvsCBList.DataSource = 
                    contKekvsCBList.DataSource = 
                    invKekvsCBList.DataSource = 
                    spenKekvsCBList.DataSource = 
                    kekvsForCBList;
                tpKekvsCBList.DisplayMember =
                    contKekvsCBList.DisplayMember =
                    invKekvsCBList.DisplayMember =
                    spenKekvsCBList.DisplayMember =
                    "Code";
                tpKekvsCBList.ValueMember =
                    contKekvsCBList.ValueMember =
                    invKekvsCBList.ValueMember =
                    spenKekvsCBList.ValueMember =
                    "Id";

                // Формирование списка смет
                UpdateEstimateCBList();
                tpEstimateCBList.DisplayMember = 
                    contEstimateCBList.DisplayMember = 
                    invEstimateCBList.DisplayMember = 
                    spenEstimateCBList.DisplayMember = "Name";
                tpEstimateCBList.ValueMember = 
                    contEstimateCBList.ValueMember = 
                    invEstimateCBList.ValueMember = 
                    spenEstimateCBList.ValueMember = "Id";

                contContractorCBList.DisplayMember = invContractorCBList.DisplayMember = "ShortName";
                contContractorCBList.ValueMember = invContractorCBList.ValueMember = "Id";
                invContractCBList.DisplayMember = "FullName";
                invContractCBList.ValueMember = "Id";

                // Формирование списка статусов договоров
                contStatusCBList.DataSource = new[] {
                    new { Name = "Всі", Value = -1 },
                    new { Name = "Активні", Value = (int)ContractStatus.Active },
                    new { Name = "Виконані", Value = (int)ContractStatus.Complete }
                };
                contStatusCBList.DisplayMember = "Name";
                contStatusCBList.ValueMember = "Value";

                // Формирование списка статусов оплат
                invStatusCBList.DataSource = spenStatusCBList.DataSource = new[] {
                    new { Name = "Всі", Value = -1 },
                    new { Name = "Новий", Value = (int)PaymentStatus.New },
                    new { Name = "На оплаті", Value = (int)PaymentStatus.OnPayment },
                    new { Name = "Сплачений", Value = (int)PaymentStatus.Payed }
                };
                invStatusCBList.DisplayMember = spenStatusCBList.DisplayMember = "Name";
                invStatusCBList.ValueMember = spenStatusCBList.ValueMember = "Value";

                // Формирование списка контрагентов
                UpdateContractorsCBLists();

                // Формирование списка договоров
                UpdateContractsCBLists();

                // Загрузка данных в таблицы согласно установкам в фильтрах
                LoadMoneyRemainsTable();
                LoadEstimatesTable();
                LoadTenderPlanTableRecords();
                LoadContractsTableRecords();
                LoadInvoicesTableRecords();
                LoadPlannedSpendingTableRecords();

                // Подключаем обработчики нумерации строк на таблицы смет, договоров счетов
                DataGridViewRowsAddedEventHandler autoNumHandler = (sender, e) => 
                    DataGridViewHelper.CalculateNewRowNumber(sender as DataGridView, 0, e.RowIndex, e.RowCount);
                estimateTable.RowsAdded += autoNumHandler;
                contractsTable.RowsAdded += autoNumHandler;
                invoicesTable.RowsAdded += autoNumHandler;


                // Подключаем обработчики событий на фильтры поиска
                // Фильтр записей годового плана
                EventHandler tpFilterHandler = (sender, e) => LoadTenderPlanTableRecords();
                tpEstimateCBList.SelectedIndexChanged   += tpFilterHandler;
                tpKekvsCBList.SelectedIndexChanged      += tpFilterHandler;
                tpNewSystemRButton.CheckedChanged       += tpFilterHandler;
                tpAltSystemRButton.CheckedChanged       += tpFilterHandler;
                tpShowNullCodesChBox.CheckedChanged     += tpFilterHandler;

                // Фильтр записей о договорах
                EventHandler contractsFilterHander = (sender, e) => LoadContractsTableRecords();
                contEstimateCBList.SelectedIndexChanged     += contractsFilterHander;
                contContractorCBList.SelectedIndexChanged   += contractsFilterHander;
                contKekvsCBList.SelectedIndexChanged        += contractsFilterHander;
                contStatusCBList.SelectedIndexChanged       += contractsFilterHander;
                contNewSystemRButton.CheckedChanged         += contractsFilterHander;
                contAltSystemRButton.CheckedChanged         += contractsFilterHander;
                contStartDatePicker.ValueChanged            += contractsFilterHander;
                contEndDatePicker.ValueChanged              += contractsFilterHander;

                // Фильтр записей о счетах
                EventHandler invoicesFilterHander = (sender, e) => LoadInvoicesTableRecords();
                invEstimateCBList.SelectedIndexChanged      += invoicesFilterHander;
                invContractCBList.SelectedIndexChanged      += invoicesFilterHander;
                invContractorCBList.SelectedIndexChanged    += invoicesFilterHander;
                invKekvsCBList.SelectedIndexChanged         += invoicesFilterHander;
                invStatusCBList.SelectedIndexChanged        += invoicesFilterHander;
                invNewSystemRButton.CheckedChanged          += invoicesFilterHander;
                invAltSystemRButton.CheckedChanged          += invoicesFilterHander;
                invCreditCheckBox.CheckedChanged            += invoicesFilterHander;
                invStartDatePicker.ValueChanged             += invoicesFilterHander;
                invEndDatePicker.ValueChanged               += invoicesFilterHander;

                // Фильтр записей о запланированных тратах
                EventHandler plannedSpendingFilterHander = (sender, e) => LoadPlannedSpendingTableRecords();
                spenEstimateCBList.SelectedIndexChanged += plannedSpendingFilterHander;
                spenStatusCBList.SelectedIndexChanged   += plannedSpendingFilterHander;
                spenKekvsCBList.SelectedIndexChanged    += plannedSpendingFilterHander;
                spenNewSystemRButton.CheckedChanged     += plannedSpendingFilterHander;
                spenAltSystemRButton.CheckedChanged     += plannedSpendingFilterHander;

                // Добавляем обработчики событий для редактирования записей таблиц
                newInvoiceButton.Click += button10_Click;
                editInvoiceButton.Click += (sender, e) => EditInvoice();
                invoicesTable.CellDoubleClick += (sender, e) => 
                {
                    if (e.RowIndex < 0 || e.ColumnIndex < 0)
                        return;
                    EditInvoice();
                };

                newContractButton.Click += button5_Click;
                editContractButton.Click += (sender, e) => EditContract();
                contractsTable.CellDoubleClick += (sender, e) =>
                {
                    if (e.RowIndex < 0 || e.ColumnIndex < 0)
                        return;
                    EditContract();
                };

                addEstimateButton.Click += addEstimateButton_Click;
                editEstimateButton.Click += (sender, e) => EditEstimate();
                estimateTable.CellDoubleClick += (sender, e) =>
                {
                    if (e.RowIndex < 0 || e.ColumnIndex < 0)
                        return;
                    EditEstimate();
                };

                newTPRecordButton.Click += newTPRecordButton_Click;
                editTPRecordButton.Click += (sender, e) => EditTPRecord();
                tenderPlanTable.CellDoubleClick += (sender, e) =>
                {
                    if (e.RowIndex < 0 || e.ColumnIndex < 0)
                        return;
                    EditTPRecord();
                };

                newPlSpendButton.Click += newPlSpendButton_Click;
                editPlSpendButton.Click += (sender, e) => EditSpendingRecord();
                spendingTable.CellDoubleClick += (sender, e) =>
                {
                    if (e.RowIndex < 0 || e.ColumnIndex < 0)
                        return;
                    EditSpendingRecord();
                };

                // Если пользователь неавторизирован - делаем недоступными функции добавления, удаления и редактирования записей
                if(!UserSession.IsAuthorized)
                {
                    // Ограничиваем работу со сметами
                    editEstimateButton.Text = "Переглянути";
                    editEstimateButton.Location = addEstimateButton.Location;
                    addEstimateButton.Visible = deleteEstimateButton.Visible = false;

                    // Ограничиваем работу со годовым планом
                    editTPRecordButton.Text = "Переглянути";
                    editTPRecordButton.Location = newTPRecordButton.Location;
                    newTPRecordButton.Visible = deleteTPRecordButton.Visible = false;

                    // Ограничиваем работу со списком контрактов
                    editContractButton.Text = "Переглянути";
                    editContractButton.Location = newContractButton.Location;
                    newContractButton.Visible = deleteContractButton.Visible = false;

                    // Ограничиваем работу со списком счетов
                    editInvoiceButton.Text = "Переглянути";
                    editInvoiceButton.Location = newInvoiceButton.Location;
                    newInvoiceButton.Visible = deleteInvoiceButton.Visible = false;

                    // Ограничиваем работу со списком запланированых трат
                    editPlSpendButton.Text = "Переглянути";
                    editPlSpendButton.Location = newPlSpendButton.Location;
                    newPlSpendButton.Visible = deletePlSpendButton.Visible = false;

                    // Убираем возможность смены пароля
                    changePasswordMenuItem.Visible = false;
                }
            }
        }

        // Обновления выпадающего списка смет
        private void UpdateEstimateCBList()
        {
            controlsDataWasChangedByUser = false;
            using(TenderContext tc = new TenderContext())
            {
                estimatesForCBList = new BindingList<Estimate>(tc.Estimates.Where(p => p.TenderYearId == currentTenderYear.Id).ToList());
                BindingList<Estimate> estimatesForCBList2 = new BindingList<Estimate>(estimatesForCBList);
                estimatesForCBList2.Insert(0, new Estimate { Id = -1, Name = "- ВСІ -" });
                tpEstimateCBList.DataSource = estimatesForCBList;

                mainEstimateCBList.DataSource =
                contEstimateCBList.DataSource =
                invEstimateCBList.DataSource =
                spenEstimateCBList.DataSource = estimatesForCBList2;
            }
            controlsDataWasChangedByUser = true;
        }

        // Обновление выпадающего списка с договорами
        private void UpdateContractsCBLists()
        {
            // Обновляем таблицу с контрактами
            LoadContractsTableRecords();

            controlsDataWasChangedByUser = false;
            using (TenderContext tc = new TenderContext())
            {
                label5.Enabled = label6.Enabled = invContractorCBList.Enabled = invContractCBList.Enabled = false;
                
                int selectedContractorInInvGroup = Convert.ToInt32(invContractorCBList.SelectedValue);
                if (selectedContractorInInvGroup > 0)
                {
                    contractsForCBList = new BindingList<Contract>(tc.Contracts
                        .Where(p => (p.ContractorId == selectedContractorInInvGroup) && (p.Estimate.TenderYearId == MainProgramForm.CurrentTenderYear.Id))
                        .ToList());
                }
                else
                {
                    contractsForCBList = new BindingList<Contract>(new List<Contract>());
                }
                contractsForCBList.Insert(0, new Contract { Id = -1, Description = "- ВСІ -" });
                label5.Enabled = label6.Enabled = invContractorCBList.Enabled = invContractCBList.Enabled = true;
            }

            invContractCBList.DataSource = contractsForCBList;
            controlsDataWasChangedByUser = true;

            LoadInvoicesTableRecords();
        }

        // Обновление выпадающего списка с контрагентами
        private void UpdateContractorsCBLists()
        {
            controlsDataWasChangedByUser = false;
            using (TenderContext tc = new TenderContext())
            {
                contractorsForCBList = new BindingList<Contractor>(tc.Contractors.ToList());
                contractorsForCBList.Insert(0, new Contractor { Id = -1, ShortName = "- ВСІ -" });
                contContractorCBList.DataSource = invContractorCBList.DataSource = contractorsForCBList;
            }
            controlsDataWasChangedByUser = true;

            UpdateContractsCBLists();
        }

        // Обновление списка других трат
        private void LoadPlannedSpendingTableRecords()
        {
           if(!updatePlannedSpendingList.IsBusy && controlsDataWasChangedByUser)
           {
               TogglePlannedSpendingUpdateAnimation();
               PlannedSpendingRecordsFilter filter = new PlannedSpendingRecordsFilter();
               filter.EstimateId = (spenEstimateCBList.SelectedItem as Estimate).Id;
               filter.Kekv = spenKekvsCBList.SelectedItem as KekvCode;
               filter.PrimaryKekvSelected = spenNewSystemRButton.Checked;
               filter.Status = Convert.ToInt32(spenStatusCBList.SelectedValue);
               updatePlannedSpendingList.RunWorkerAsync(filter);
           }
        }

        // Обновление списка счетов на оплату
        private void LoadInvoicesTableRecords()
        {
            if (!updateInvoicesList.IsBusy && controlsDataWasChangedByUser)
            {
                ToggleInvoicesListUpdateAnimation();
                InvoiceRecordsFilter filter = new InvoiceRecordsFilter();
                filter.EstimateId = Convert.ToInt32(invEstimateCBList.SelectedValue);
                filter.Contract = invContractCBList.SelectedItem as Contract;
                filter.Contractor = invContractorCBList.SelectedItem as Contractor;
                filter.IsCredit = invCreditCheckBox.Checked;
                filter.Kekv = invKekvsCBList.SelectedItem as KekvCode;
                filter.PrimaryKekvSelected = invNewSystemRButton.Checked;
                filter.Status = Convert.ToInt32(invStatusCBList.SelectedValue);
                updateInvoicesList.RunWorkerAsync(filter);
            }
        }

        // Обновление списка контрактов (договоров)
        private void LoadContractsTableRecords()
        {
            if(!updateContractsList.IsBusy && controlsDataWasChangedByUser)
            {
                ToggleContractsListUpdateAnimation();
                ContractRecordsFilter arg = new ContractRecordsFilter();
                arg.Contractor = contContractorCBList.SelectedItem as Contractor;
                arg.Status = Convert.ToInt32(contStatusCBList.SelectedValue);
                arg.PrimaryKekvSelected = contNewSystemRButton.Checked;
                arg.Kekv = contKekvsCBList.SelectedItem as KekvCode;
                arg.Estimate = contEstimateCBList.SelectedItem as Estimate;
                arg.ContractSignBeginDate = contStartDatePicker.Value;
                arg.ContractSignEndDate = contEndDatePicker.Value;
                updateContractsList.RunWorkerAsync(arg);
            }
        }

        // Обновление таблицы с записями годового плана
        private void LoadTenderPlanTableRecords()
        {
            if (!updatePlanRecords.IsBusy && controlsDataWasChangedByUser)
            {
                ToggleTenderPlanRecordsUpdateAnimation();
                TenderPlanRecordsFilter filter = new TenderPlanRecordsFilter();
                filter.NullRecordsShow = tpShowNullCodesChBox.Checked;
                filter.Estimate = tpEstimateCBList.SelectedItem as Estimate;
                filter.PrimaryKekvSelected = tpNewSystemRButton.Checked;
                filter.Kekv = tpKekvsCBList.SelectedItem as KekvCode;
                updatePlanRecords.RunWorkerAsync(filter);
            }
        }

        // Обновление текущих остатков средств
        private void LoadMoneyRemainsTable()
        {
            if (!updateMoneyRemains.IsBusy && controlsDataWasChangedByUser)
            {
                Estimate selectedEstimate = mainEstimateCBList.SelectedItem as Estimate;
                bool isNewSystemSelected = radioButton1.Checked;
                ToggleMoneyRemainsUpdateAnimation();
                updateMoneyRemains.RunWorkerAsync(new UpdateRemainsBGWorkerArgument { SelectedEstimate = selectedEstimate, IsNewSystem = isNewSystemSelected });
            }
        }

        // Загрузка таблицы списка смет в текущем году
        public void LoadEstimatesTable()
        {
            using(TenderContext tc = new TenderContext())
            {
                var estSums = (from item in tc.BalanceChanges.Include(p => p.Estimate.Year).ToList()
                                 where item.Invoice == null && item.PlannedSpending == null && item.Estimate.TenderYearId == currentTenderYear.Id
                                 group item by item.Estimate into g
                                 select new EstimatesTableEntry
                                 {
                                     Estimate = g.Key,
                                     YearSum = g.Sum(p => p.PrimaryKekvSum)
                                 } into s1
                                 orderby s1.YearSum descending
                                 select s1).ToList();
                estimatesList = new BindingList<EstimatesTableEntry>(estSums);
                estimateTable.DataSource = estimatesList;
                estimateTable.Refresh();

                UpdateEstimateCBList();

                newTPRecordButton.Enabled = newContractButton.Enabled = newInvoiceButton.Enabled = newPlSpendButton.Enabled = (estimatesForCBList.Count > 0);
            }
        }

        private void estimateTable_SelectionChanged(object sender, EventArgs e)
        {
            editEstimateButton.Enabled = deleteEstimateButton.Enabled = printEstimateButton.Enabled = (estimateTable.SelectedRows.Count > 0);
        }

        private void TogglePlannedSpendingUpdateAnimation()
        {
            plannedSpendingLoadingPicture.Tag = (plannedSpendingLoadingPicture.Tag == null) ? loadingIndicator : null;
            bool isLoadingInProccess = (plannedSpendingLoadingPicture.Tag != null);
            groupBox4.Enabled = 
                newPlSpendButton.Enabled = 
                editPlSpendButton.Enabled = 
                deletePlSpendButton.Enabled = !isLoadingInProccess;

            plannedSpendingLoadingPicture.Visible = plannedSpendingLoadingLabel.Visible = isLoadingInProccess;

            if(!isLoadingInProccess)
            {
                editPlSpendButton.Enabled = 
                    deletePlSpendButton.Enabled = 
                    (spendingTable.SelectedRows.Count > 0);
            }
        }

        private void ToggleMoneyRemainsUpdateAnimation()
        {
            moneyRemainsloadingPicture.Visible = moneyRemainsloadingLabel.Visible = !(moneyRemainsloadingLabel.Visible);
            mainEstimateCBList.Enabled = radioButton1.Enabled = radioButton2.Enabled = !radioButton2.Enabled;
        }

        private void ToggleTenderPlanRecordsUpdateAnimation()
        {
            yearPlanLoadingLabel.Tag = (yearPlanLoadingLabel.Tag == null) ? loadingIndicator : null;
            bool isLoadingInProccess = (yearPlanLoadingLabel.Tag != null);
            yearPlanLoadingLabel.Visible = yearPlanLoadingPicture.Visible = isLoadingInProccess;
            groupBox3.Enabled =
                tpNewSystemRButton.Enabled =
                tpAltSystemRButton.Enabled =
                tpKekvsCBList.Enabled =
                tpEstimateCBList.Enabled =
                newTPRecordButton.Enabled = 
                editTPRecordButton.Enabled = 
                deleteTPRecordButton.Enabled = 
                contractsTPRecordButton.Enabled =
                historyTPRecordButton.Enabled =
                !isLoadingInProccess;

            if (newTPRecordButton.Enabled)
            {
                editTPRecordButton.Enabled = 
                    deleteTPRecordButton.Enabled = 
                    contractsTPRecordButton.Enabled =
                    historyTPRecordButton.Enabled = tenderPlanTable.SelectedRows.Count > 0;
            }
        }

        private void ToggleContractsListUpdateAnimation()
        {
            contractsLoadingLabel.Tag = (contractsLoadingLabel.Tag == null) ? loadingIndicator : null;
            bool isLoadingInProccess = (contractsLoadingLabel.Tag != null);
            contractsLoadingLabel.Visible = contractsLoadingPicture.Visible = isLoadingInProccess;
            groupBox2.Enabled = newContractButton.Enabled = editContractButton.Enabled = deleteContractButton.Enabled = !isLoadingInProccess;

            if (newContractButton.Enabled)
            {
                editContractButton.Enabled = deleteContractButton.Enabled = contractsTable.SelectedRows.Count > 0;
            }
        }

        private void ToggleInvoicesListUpdateAnimation()
        {
            invoicesLoadingLabel.Tag = (invoicesLoadingLabel.Tag == null) ? loadingIndicator : null;
            bool isLoadingInProccess = (invoicesLoadingLabel.Tag != null);
            invoicesLoadingLabel.Visible = invoicesLoadingPicture.Visible = isLoadingInProccess;
            groupBox1.Enabled = deleteInvoiceButton.Enabled = editInvoiceButton.Enabled = newInvoiceButton.Enabled = !(isLoadingInProccess);

            if (newInvoiceButton.Enabled)
            {
                editInvoiceButton.Enabled = deleteInvoiceButton.Enabled = invoicesTable.SelectedRows.Count > 0;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            LoadMoneyRemainsTable();
        }

        private void moneyRemainsTable_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            DataGridViewHelper.MoneyCellValidating(sender as DataGridView, e.FormattedValue.ToString());
        }

        private void addEstimateButton_Click(object sender, EventArgs e)
        {
            EstimateForm ef = new EstimateForm(currentTenderYear);
            ef.ShowDialog();

            if (ef.WasDbChanged)
            {
                LoadEstimatesTable();
                LoadMoneyRemainsTable();
            }
        }

        private void moneyRemainsTable_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewHelper.RecalculateMoneyTotals(sender as DataGridView, e.RowIndex, e.ColumnIndex);
        }

        // Аргумент для метода обновления текущих остатков 
        class UpdateRemainsBGWorkerArgument
        {
            // Выбранная смета
            public Estimate SelectedEstimate { get; set; }
            // По новой ли системе КЕКВ отображать данные
            public bool IsNewSystem { get; set; }
        }

        // Обновление текущих остатков средств
        private void updateMoneyRemains_DoWork(object sender, DoWorkEventArgs e)
        {
            UpdateRemainsBGWorkerArgument arg = e.Argument as UpdateRemainsBGWorkerArgument;

            decimal[,] remains = new decimal[kekvs.Count, moneySources.Count];

            try
            {
                using (TenderContext tc = new TenderContext())
                {
                    IQueryable<BalanceChanges> moneys;
                    if(arg.SelectedEstimate == null || arg.SelectedEstimate.Id < 0)
                    {
                        moneys = from item in tc.BalanceChanges
                                 where (item.DateOfReceiving <= DateTime.Now) && (item.Estimate.TenderYearId == currentTenderYear.Id)
                                 select item;
                    }
                    else
                    {
                        moneys = from item in tc.BalanceChanges
                                 where (item.EstimateId == arg.SelectedEstimate.Id) && (item.DateOfReceiving <= DateTime.Now) && (item.Estimate.TenderYearId == currentTenderYear.Id)
                                 select item;
                    }

                    List<MoneyRemainsRecord> moneyRemains;
                    if (arg.IsNewSystem)
                    {
                        moneyRemains = (from item in moneys
                                        group item by new { item.PrimaryKekv, item.MoneySource } into g
                                        select new MoneyRemainsRecord
                                        {
                                            Kekv = g.Key.PrimaryKekv,
                                            MSource = g.Key.MoneySource,
                                            Sum = g.Sum(p => p.PrimaryKekvSum)
                                        }).ToList();

                    }
                    else
                    {
                        moneyRemains = (from item in moneys
                                        group item by new { item.SecondaryKekv, item.MoneySource } into g
                                        select new MoneyRemainsRecord
                                        {
                                            Kekv = g.Key.SecondaryKekv,
                                            MSource = g.Key.MoneySource,
                                            Sum = g.Sum(p => p.SecondaryKekvSum)
                                        }).ToList();
                    }
                    foreach (var item in moneyRemains)
                    {
                        int rowIndex = kekvs.IndexOf(item.Kekv);
                        int colIndex = moneySources.IndexOf(item.MSource);
                        remains[rowIndex, colIndex] = item.Sum;
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationHelper.ShowException(ex);
            }
            finally
            {
                e.Result = remains;
            }
        }

        private void updateMoneyRemains_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            decimal[,] result = e.Result as decimal[,];
            for (int i = 0; i < kekvs.Count; i++)
            {
                for(int j = 0; j < moneySources.Count; j++)
                {
                    moneyRemainsTable.Rows[i].Cells[j].Value = result[i, j];
                }
            }

            ToggleMoneyRemainsUpdateAnimation();
        }

        private void EditEstimate()
        {
            Estimate selectedEstimate = (estimateTable.SelectedRows[0].DataBoundItem as EstimatesTableEntry).Estimate;
            EstimateForm ef = new EstimateForm(selectedEstimate);
            ef.ShowDialog();

            if (ef.WasDbChanged)
            {
                LoadEstimatesTable();
                LoadMoneyRemainsTable();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ContractForm cf = new ContractForm(currentTenderYear);
            cf.ShowDialog();

            if(cf.DbWasChanged)
            {
                LoadContractsTableRecords();
                LoadInvoicesTableRecords();
                LoadTenderPlanTableRecords();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            InvoiceForm iForm = new InvoiceForm(currentTenderYear);
            iForm.ShowDialog();

            if (iForm.DbWasChanged)
            {
                LoadInvoicesTableRecords();
                LoadContractsTableRecords();
                LoadTenderPlanTableRecords();
                LoadMoneyRemainsTable();
            }
        }

        // -------------------------------------------------
        // Классы-фильтры результатов таблиц записей о договорах, счетах и т.п.
        // -------------------------------------------------

        // Фильтр результатов таблицы записей годового плана
        private class TenderPlanRecordsFilter
        {
            public Estimate Estimate { get; set; }
            public KekvCode Kekv { get; set; }
            public bool PrimaryKekvSelected { get; set; }
            public bool NullRecordsShow { get; set; }
        }

        // Фильтр результатов таблицы записей договоров 
        private class ContractRecordsFilter
        {
            public Estimate Estimate { get; set; }
            public Contractor Contractor { get; set; }
            public KekvCode Kekv { get; set; }
            public bool PrimaryKekvSelected { get; set; }
            public DateTime ContractSignBeginDate { get; set; }
            public DateTime ContractSignEndDate { get; set; }
            public int Status { get; set; }
        }

        // Фильтр результатов таблицы записей счетов
        private class InvoiceRecordsFilter
        {
            public Contractor Contractor { get; set; }
            public Contract Contract { get; set; }
            public KekvCode Kekv { get; set; }
            public bool PrimaryKekvSelected { get; set; }
            public bool IsCredit { get; set; }
            public DateTime InvoiceBeginDate { get; set; }
            public DateTime InvoiceEndDate { get; set; }
            public int Status { get; set; }
            public int EstimateId { get; set; }
        }

        // Фильтр результатов таблицы записей счетов
        private class PlannedSpendingRecordsFilter
        {
            public KekvCode Kekv { get; set; }
            public bool PrimaryKekvSelected { get; set; }
            public int Status { get; set; }
            public int EstimateId { get; set; }
        }

        // -------------------------------------------------

        private void updatePlanRecords_DoWork(object sender, DoWorkEventArgs e)
        {
            TenderPlanRecordsFilter filter = e.Argument as TenderPlanRecordsFilter;
            List<TenderPlanItemsTableEntry> resultList = null;

            try
            {
                using (TenderContext tc = new TenderContext())
                {
                    List<TenderPlanItemsTableEntry> planRecords = null;
                    List<TenderPlanItemsTableEntry> contractsMoney = null;

                    List<TenderPlanRecord> allPlanRecords;
                    List<Contract> allContracts;

                    if(filter.Estimate.Id > 0)
                    {
                        allPlanRecords = (from r in tc.TenderPlanRecords
                                          where r.EstimateId == filter.Estimate.Id
                                          select r).ToList();
                        allContracts = (from r in tc.Contracts
                                        where r.EstimateId == filter.Estimate.Id
                                        select r).ToList();
                    }
                    else
                    {
                        allPlanRecords = (from r in tc.TenderPlanRecords
                                          where r.Estimate.TenderYearId == CurrentTenderYear.Id
                                          select r).ToList();
                        allContracts = (from r in tc.Contracts
                                        where r.Estimate.TenderYearId == CurrentTenderYear.Id
                                        select r).ToList();
                    }

                    if (filter.PrimaryKekvSelected)
                    {
                        planRecords     = (from r in allPlanRecords.ToList()
                                           select new TenderPlanItemsTableEntry
                                           {
                                               Kekv = r.PrimaryKekv,
                                               Dk = r.Dk,
                                               MoneyOnCode = r.Sum,
                                               RelatedTenderPlanRecord = r,
                                               Estimate = r.Estimate
                                           }).ToList();
                        contractsMoney =   (from c in allContracts.ToList()
                                            group c by new { c.Estimate, c.PrimaryKekv, c.Dk } into g1
                                            select new TenderPlanItemsTableEntry
                                            {
                                                Kekv = g1.Key.PrimaryKekv,
                                                Dk = g1.Key.Dk,
                                                Estimate = g1.Key.Estimate,
                                                RegisteredByContracts = g1.Sum(p => p.Sum),
                                                ContractsMoneyRemain = g1.Sum(p => p.MoneyRemain),
                                                UsedByContracts = g1.Sum(p => p.UsedMoney)
                                            }).ToList();
                    }
                    else
                    {
                        planRecords = (from r in allPlanRecords
                                       select new TenderPlanItemsTableEntry
                                       {
                                           Kekv = r.SecondaryKekv,
                                           Dk = r.Dk,
                                           MoneyOnCode = r.Sum,
                                           RelatedTenderPlanRecord = r,
                                           Estimate = r.Estimate
                                       }).ToList();
                        contractsMoney = (from c in allContracts
                                          group c by new { c.Estimate, c.SecondaryKekv, c.Dk } into g1
                                          select new TenderPlanItemsTableEntry
                                          {
                                              Kekv = g1.Key.SecondaryKekv,
                                              Dk = g1.Key.Dk,
                                              Estimate = g1.Key.Estimate,
                                              RegisteredByContracts = g1.Sum(p => p.Sum),
                                              ContractsMoneyRemain = g1.Sum(p => p.MoneyRemain),
                                              UsedByContracts = g1.Sum(p => p.UsedMoney)
                                          }).ToList();
                    }

                    if (filter.Kekv.Id > 0)
                    {
                        planRecords = planRecords.Where(p => p.Kekv.Id == filter.Kekv.Id).ToList();
                        contractsMoney = contractsMoney.Where(p => p.Kekv.Id == filter.Kekv.Id).ToList();
                    }

                    resultList = (from p in planRecords
                                  join u in contractsMoney on new { p.Estimate, p.Kekv, p.Dk } equals new { u.Estimate, u.Kekv, u.Dk } into g2
                                  from t3 in g2.DefaultIfEmpty(new TenderPlanItemsTableEntry())
                                  select new TenderPlanItemsTableEntry
                                  {
                                      RelatedTenderPlanRecord = p.RelatedTenderPlanRecord,
                                      Kekv = p.Kekv,
                                      Dk = p.Dk,
                                      MoneyOnCode = p.MoneyOnCode,
                                      Estimate = p.Estimate,
                                      RegisteredByContracts = t3.RegisteredByContracts,
                                      UsedByContracts = t3.UsedByContracts,
                                      ContractsMoneyRemain = t3.ContractsMoneyRemain
                                  }).ToList();
                    // Упорядочить по КЕКВ, потом по коду ДК
                    //resultList.resultList = resultList.OrderBy(p => p.Kekv.Id).ThenBy(p => p.Dk.Code).ToList();
                    // ... либо только по коду ДК 
                    resultList = resultList.OrderBy(p => p.Dk.Code).ToList();

                    // Добавляем элемент содержащий итоги по записям
                    if(resultList.Count > 1)
                    {
                        TenderPlanItemsTableEntry totalRow = new TenderPlanItemsTableEntry
                        {
                            Dk = new DkCode { Name = "ВСЬОГО" },
                            MoneyOnCode = resultList.Sum(p => p.MoneyOnCode),
                            RegisteredByContracts = resultList.Sum(p => p.RegisteredByContracts),
                            UsedByContracts = resultList.Sum(p => p.UsedByContracts),
                            ContractsMoneyRemain = resultList.Sum(p => p.ContractsMoneyRemain)
                        };
                        resultList.Add(totalRow);
                    }

                    if (!filter.NullRecordsShow)
                    {
                        resultList = resultList.Where(p => p.MoneyOnCode > 0).ToList();
                    }

                    e.Result = resultList;
                    hasEstimateFreeMoney = HasEstimateFreeMoney(filter.Estimate);
                }
            }
            catch (Exception ex)
            {
                e.Result = ex;
            }
        }

        private void updatePlanRecords_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(e.Result is Exception)
            {
                Exception workerError = e.Result as Exception;
                NotificationHelper.ShowException(workerError);
            }
            else
            {
                tenderPlanItemsList = new BindingList<TenderPlanItemsTableEntry>(e.Result as List<TenderPlanItemsTableEntry>);
                tenderPlanTable.DataSource = tenderPlanItemsList;

                linkLabel1.Visible = hasEstimateFreeMoney;
            }

            ToggleTenderPlanRecordsUpdateAnimation();
        }

        private void HighlightPlanTotalsRow(DataGridViewRow dataGridViewRow)
        {
            dataGridViewRow.DefaultCellStyle.Font = FormStyles.MoneyTotalsFont;
        }

        private void updateContractsList_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (TenderContext tc = new TenderContext())
                {
                    ContractRecordsFilter filter = e.Argument as ContractRecordsFilter;
                    if (filter.Estimate == null)
                    {
                        e.Result = new List<ContractsTableEntry>();
                        return;
                    }

                    List<Contract> allContracts = tc.Contracts.Where(p => (p.Estimate.TenderYearId == MainProgramForm.CurrentTenderYear.Id) &&
                        ((p.SignDate >= contStartDatePicker.Value) && (p.SignDate <= contEndDatePicker.Value))).ToList();

                    if(filter.Estimate.Id > 0)
                    {
                        allContracts = allContracts.Where(p=> (p.EstimateId == filter.Estimate.Id)).ToList();
                    }

                    if (filter.Contractor.Id > 0)
                    {
                        allContracts = allContracts.Where(p => p.ContractorId == filter.Contractor.Id).ToList();
                    }
                    if (filter.Kekv.Id > 0)
                    {
                        if (filter.PrimaryKekvSelected)
                        {
                            allContracts = allContracts.Where(p => p.PrimaryKekvId == filter.Kekv.Id).ToList();
                        }
                        else
                        {
                            allContracts = allContracts.Where(p => p.SecondaryKekvId == filter.Kekv.Id).ToList();
                        }
                    }

                    string contractStatusName = Enum.GetName(typeof(ContractStatus), filter.Status);
                    if (contractStatusName != null)
                    {
                        ContractStatus status = (ContractStatus)Enum.Parse(typeof(ContractStatus), contractStatusName);
                        allContracts = allContracts.Where(p => p.Status == status).ToList();
                    }

                    List<ContractsTableEntry> result = (from c in allContracts
                                                        select new ContractsTableEntry
                                                        {
                                                            RelatedContract = c,
                                                            ContractDate = c.SignDate,
                                                            ContractNum = c.Number,
                                                            Contractor = c.Contractor,
                                                            Description = c.Description,
                                                            FullSum = c.Sum,
                                                            UsedSum = c.Invoices.Where(p => p.Changes.Count > 0).Sum(p => p.Sum)
                                                        }).ToList();

                    e.Result = result;
                }
            }
            catch (Exception ex)
            {
                e.Result = ex;
            }
        }

        private void updateContractsList_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(e.Result is Exception)
            {
                NotificationHelper.ShowException(e.Result as Exception);
            }
            else
            {
                contractsList = new BindingList<ContractsTableEntry>(e.Result as List<ContractsTableEntry>);
                contractsTable.DataSource = contractsList;

                // Вычисляем итоги по договорам
                contractsFullSumLabel.Text = string.Format("{0:N2} грн.", contractsList.Sum(p => p.FullSum));
                contractsUsedSumLabel.Text = string.Format("{0:N2} грн.", contractsList.Sum(p => p.UsedSum));
                contractsRemainSumLabel.Text = string.Format("{0:N2} грн.", contractsList.Sum(p => p.MoneyRemain));

                contractsTable.Refresh();
            }
            ToggleContractsListUpdateAnimation();
        }

        private void updateInvoicesList_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (TenderContext tc = new TenderContext())
                {
                    InvoiceRecordsFilter filter = e.Argument as InvoiceRecordsFilter;
                    int selectedEstimateId = filter.EstimateId;
                    List<Invoice> allInvoices = tc.Invoices.Where(p => p.Contract.Estimate.TenderYearId == MainProgramForm.CurrentTenderYear.Id)
                        .ToList()
                        .Where(p => ((p.Date >= invStartDatePicker.Value) && (p.Date <= invEndDatePicker.Value)))
                        .ToList();

                    if(selectedEstimateId > 0)
                    {
                        allInvoices = allInvoices.Where(p => (p.Contract.EstimateId == selectedEstimateId)).ToList();
                    }

                    if (filter.Contractor.Id > 0)
                    {
                        allInvoices = allInvoices.Where(p => p.ContractId == filter.Contractor.Id).ToList();
                    }
                    if (filter.Kekv.Id > 0)
                    {
                        if (filter.PrimaryKekvSelected)
                        {
                            allInvoices = allInvoices.Where(p => p.Contract.PrimaryKekvId == filter.Kekv.Id).ToList();
                        }
                        else
                        {
                            allInvoices = allInvoices.Where(p => p.Contract.SecondaryKekvId == filter.Kekv.Id).ToList();
                        }
                    }

                    if(filter.IsCredit)
                    {
                        allInvoices = allInvoices.Where(p => p.IsCredit).ToList();
                    }

                    List<InvoiceRecordsTableEntry> result = (from c in allInvoices
                                                             select new InvoiceRecordsTableEntry
                                                             {
                                                                 Contract = c.Contract,
                                                                 Date = c.Date,
                                                                 RelatedInvoice = c,
                                                                 IsCredit = c.IsCredit,
                                                                 Contractor = c.Contract.Contractor,
                                                                 Number = c.Number,
                                                                 Status = c.Status,
                                                                 Sum = c.Sum
                                                            }).ToList();

                    string invStatus = Enum.GetName(typeof(PaymentStatus), filter.Status);
                    if (invStatus != null)
                    {
                        PaymentStatus status = (PaymentStatus)Enum.Parse(typeof(PaymentStatus), invStatus);
                        result = result.Where(p => p.Status == status).ToList();
                    }

                    e.Result = result;
                }
            }
            catch (Exception ex)
            {
                e.Result = ex;
            }
        }

        private void updateInvoicesList_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result is Exception)
            {
                NotificationHelper.ShowException(e.Result as Exception);
            }
            else
            {
                invoicesList = new BindingList<InvoiceRecordsTableEntry>(e.Result as List<InvoiceRecordsTableEntry>);
                invoicesTable.DataSource = invoicesList;

                invoicesSumLabel.Text = string.Format("{0:N2} грн.", invoicesList.Sum(p => p.Sum));

                invoicesTable.Refresh();
            }

            ToggleInvoicesListUpdateAnimation();
        }

        private void updatePlannedSpendingList_DoWork(object sender, DoWorkEventArgs e)
        {
            PlannedSpendingRecordsFilter filter = e.Argument as PlannedSpendingRecordsFilter;

            try
            {
                using (TenderContext tc = new TenderContext())
                {
                    List<PlannedSpending> spendings = tc.PlannedSpending.Where(p => p.Estimate.TenderYearId == MainProgramForm.CurrentTenderYear.Id).ToList();

                    if (filter.EstimateId > 0)
                    {
                        spendings = spendings.Where(p => p.EstimateId == filter.EstimateId).ToList();
                    }

                    if (filter.Kekv.Id > 0)
                    {
                        if (filter.PrimaryKekvSelected)
                        {
                            spendings = spendings.Where(p => p.PrimaryKekvId == filter.Kekv.Id).ToList();
                        }
                        else
                        {
                            spendings = spendings.Where(p => p.SecondaryKekvId == filter.Kekv.Id).ToList();
                        }
                    }

                    e.Result = spendings.Select(p => new PlannedSpendingTableEntry 
                    { 
                        Date = p.CreationDate, 
                        Description = p.Description, 
                        KekvCode = (filter.PrimaryKekvSelected) ? p.PrimaryKekv.Code : p.SecondaryKekv.Code, 
                        Sum = p.Sum, 
                        Status = p.Status,
                        RelatedPlannedSpendingRecord = p
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                e.Result = ex;
            }
        }

        private void updatePlannedSpendingList_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result is Exception)
            {
                NotificationHelper.ShowException(e.Result as Exception);
            }
            else
            {
                plannedSpendingList = new BindingList<PlannedSpendingTableEntry>(e.Result as List<PlannedSpendingTableEntry>);
                spendingTable.DataSource = plannedSpendingList;
                spendingTable.Refresh();
            }

            TogglePlannedSpendingUpdateAnimation();
        }

        // Расчитываем нераспределённые деньги по смете
        private bool HasEstimateFreeMoney(Estimate est)
        {
            using(TenderContext tc = new TenderContext())
            {
                // Деньги заложенные на год по смете
                decimal estimateYearMoney = tc.BalanceChanges.Where(p => (p.EstimateId == est.Id) && ((p.PrimaryKekvSum > 0) || (p.PlannedSpendingId != null)))
                    .Select(p => p.PrimaryKekvSum)
                    .DefaultIfEmpty(0)
                    .Sum();
                // Деньги распределённые в плане
                decimal plannedMoney = tc.TenderPlanRecords.Where(p => (p.EstimateId == est.Id))
                    .Select(p => p.Sum)
                    .DefaultIfEmpty(0)
                    .Sum();
                
                return ((estimateYearMoney - plannedMoney) > 0);
            }
        }

        private void newTPRecordButton_Click(object sender, EventArgs e)
        {
            try
            {
                if(estimatesForCBList.Count <= 0)
                {
                    NotificationHelper.ShowInfo("Для поточного року закупівлі не зареєстровано жодного кошторису. Для створення записів в річному плані створіть спочатку кошторис.");
                    return;
                }

                AddEditTPRecordForm tf = new AddEditTPRecordForm(MainProgramForm.CurrentTenderYear);
                tf.ShowDialog();

                if(tf.DbWasChanged)
                {
                    LoadTenderPlanTableRecords();
                }
            }
            catch (Exception ex)
            {
                NotificationHelper.ShowException(ex);
            }
        }

        private void EditTPRecord()
        {
            try
            {
                TenderPlanItemsTableEntry selectedTPRecord = tenderPlanTable.SelectedRows[0].DataBoundItem as TenderPlanItemsTableEntry;
                
                // Если выбрана строка с итогами
                if(selectedTPRecord.RelatedTenderPlanRecord == null)
                {
                    return;
                }
                AddEditTPRecordForm tf = new AddEditTPRecordForm(selectedTPRecord.RelatedTenderPlanRecord);
                tf.ShowDialog();

                if(tf.DbWasChanged)
                {
                    LoadTenderPlanTableRecords();
                }
            }
            catch (Exception ex)
            {
                NotificationHelper.ShowException(ex);
            }
        }

        private void EditContract()
        {
            Contract selectedContract = (contractsTable.SelectedRows[0].DataBoundItem as ContractsTableEntry).RelatedContract;
            
            ContractForm cf = new ContractForm(CurrentTenderYear, selectedContract);
            cf.ShowDialog();

            if(cf.DbWasChanged)
            {
                LoadContractsTableRecords();
                LoadInvoicesTableRecords();
                LoadTenderPlanTableRecords();
            }
        }

        private void EditInvoice()
        {
            Invoice selectedInvoice = (invoicesTable.SelectedRows[0].DataBoundItem as InvoiceRecordsTableEntry).RelatedInvoice;

            InvoiceForm cf = new InvoiceForm(CurrentTenderYear, selectedInvoice);
            cf.ShowDialog();

            if (cf.DbWasChanged)
            {
                LoadInvoicesTableRecords();
                LoadContractsTableRecords();
                LoadTenderPlanTableRecords();
                LoadMoneyRemainsTable();
            }
        }

        private void deleteTPRecordButton_Click(object sender, EventArgs e)
        {
            TenderPlanItemsTableEntry selectedTPEntry = tenderPlanTable.SelectedRows[0].DataBoundItem as TenderPlanItemsTableEntry;
            if(selectedTPEntry.RegisteredByContracts > 0)
            {
                NotificationHelper.ShowError("Видалити вказаний запис неможливо, оскілько під нього вже зареэстровано договори");
                return;
            }

            if(NotificationHelper.ShowYesNoQuestion("Ви впевнені що хочете видалити вказаний запис у річному плані?"))
            {
                this.Enabled = false;
                TenderPlanRecord selectedPlanRecord = selectedTPEntry.RelatedTenderPlanRecord;
                using(TenderContext tc = new TenderContext())
                {
                    tc.TenderPlanRecords.Attach(selectedPlanRecord);

                    foreach(var rec in selectedPlanRecord.Changes.ToList())
                    {
                        tc.TenderPlanRecordChanges.Remove(rec);
                    }
                    tc.SaveChanges();

                    tc.TenderPlanRecords.Remove(selectedPlanRecord);
                    tc.SaveChanges();

                    NotificationHelper.ShowInfo("Запис у річному плані успішно видалений");
                    this.Enabled = true;

                    LoadTenderPlanTableRecords();
                }
            }
        }

        private void historyTPRecordButton_Click(object sender, EventArgs e)
        {
            TenderPlanItemsTableEntry selectedRecord = tenderPlanTable.SelectedRows[0].DataBoundItem as TenderPlanItemsTableEntry;

            DkCodeChangesHistoryForm df = new DkCodeChangesHistoryForm(selectedRecord.RelatedTenderPlanRecord, tpNewSystemRButton.Checked);
            df.ShowDialog();
        }

        private void tenderPlanTable_SelectionChanged(object sender, EventArgs e)
        {
            if(tenderPlanTable.SelectedRows.Count > 0)
            {
                TenderPlanItemsTableEntry selectedRecord = tenderPlanTable.SelectedRows[0].DataBoundItem as TenderPlanItemsTableEntry;

                editTPRecordButton.Enabled = 
                    deleteTPRecordButton.Enabled = 
                    contractsTPRecordButton.Enabled = 
                    historyTPRecordButton.Enabled = selectedRecord.RelatedTenderPlanRecord != null;
            }
            else
            {
                editTPRecordButton.Enabled =
                    deleteTPRecordButton.Enabled =
                    contractsTPRecordButton.Enabled =
                    historyTPRecordButton.Enabled = false;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Estimate selectedEstimate = tpEstimateCBList.SelectedItem as Estimate;

            EstimateFreeMoneyForm ef = new EstimateFreeMoneyForm(selectedEstimate);
            ef.ShowDialog();
        }

        private void contractorsMenuItem_Click(object sender, EventArgs e)
        {
            ContractorsListForm cf = new ContractorsListForm();
            cf.ShowDialog();

            if(cf.WasDbUpdated)
            {
                UpdateContractorsCBLists();
            }
        }

        private void dkCodesMenuItem_Click(object sender, EventArgs e)
        {
            DkCodesListForm dkf = new DkCodesListForm();
            dkf.ShowDialog();

            if(dkf.WasDbUpdated)
            {
                LoadTenderPlanTableRecords();
            }
        }

        private void deleteInvoiceButton_Click(object sender, EventArgs e)
        {
            Invoice selectedInvoice = (invoicesTable.SelectedRows[0].DataBoundItem as InvoiceRecordsTableEntry).RelatedInvoice;

            if(NotificationHelper.ShowYesNoQuestion("Ви впевнені, що хочете видалити усю інформацію про вказаний рахунок?"))
            {
                if ((!selectedInvoice.IsCredit) || 
                    (selectedInvoice.IsCredit && NotificationHelper.ShowYesNoQuestion("У вказаному рахунку зазначено,"
                    + " що пов'язаний з ним товар/послуга/робота було надано у борг." 
                    + " Ви все ще бажаєте продовжити видалення вказаного рахунку?")))
                {
                    this.Enabled = false;
                    using(TenderContext tc = new TenderContext())
                    {
                        tc.Invoices.Attach(selectedInvoice);

                        // Удаляем все траты счёта по источникам
                        foreach(var change in selectedInvoice.Changes.ToList())
                        {
                            tc.BalanceChanges.Remove(change);
                        }
                        tc.SaveChanges();

                        // Удаляем все всязанные со счётом файлы
                        FileManager.UpdateRelatedFilesOfEntity(tc, 
                            selectedInvoice.RelatedFiles, 
                            new List<UploadedFile>(), 
                            selectedInvoice.RelatedFiles.ToList());

                        // Удаляем сам счёт
                        tc.Invoices.Remove(selectedInvoice);
                        tc.SaveChanges();
                    }
                    this.Enabled = true;

                    LoadInvoicesTableRecords();
                    LoadContractsTableRecords();
                    LoadTenderPlanTableRecords();
                    LoadMoneyRemainsTable();

                    NotificationHelper.ShowInfo("Рахунок успішно видалений");
                }
            }
        }

        private void deleteContractButton_Click(object sender, EventArgs e)
        {
            ContractsTableEntry selectedTableEntry = contractsTable.SelectedRows[0].DataBoundItem as ContractsTableEntry;
            Contract selectedContract = selectedTableEntry.RelatedContract;
            
            if(NotificationHelper.ShowYesNoQuestion("Ви впевнені, що хочете видалити даний договір?"))
            {
                if (selectedTableEntry.UsedSum > 0)
                {
                    NotificationHelper.ShowError("Видалення даного договору неможливе, так як по ньому зареєстровано рахунки.");
                    return;
                }

                this.Enabled = false;
                using(TenderContext tc = new TenderContext())
                {
                    tc.Contracts.Attach(selectedContract);

                    foreach(var change in selectedContract.ContractChanges.ToList())
                    {
                        tc.ContractChanges.Remove(change);
                    }
                    tc.SaveChanges();

                    FileManager.UpdateRelatedFilesOfEntity(tc, 
                        selectedContract.RelatedFiles, 
                        new List<UploadedFile>(), 
                        selectedContract.RelatedFiles.ToList());

                    tc.Contracts.Remove(selectedContract);
                    tc.SaveChanges();
                }
                this.Enabled = true;

                LoadContractsTableRecords();
                LoadTenderPlanTableRecords();
                LoadInvoicesTableRecords();
                UpdateContractsCBLists();

                NotificationHelper.ShowInfo("Договір успішно видалений");
            }
        }

        private void deleteEstimateButton_Click(object sender, EventArgs e)
        {
            Estimate selectedEstimate = (estimateTable.SelectedRows[0].DataBoundItem as EstimatesTableEntry).Estimate;
            if(NotificationHelper.ShowYesNoQuestion("Ви впевнені, що хочете видалити вказаний кошторис?"))
            {
                this.Enabled = false;
                using(TenderContext tc = new TenderContext())
                {
                    tc.Estimates.Attach(selectedEstimate);

                    if(tc.TenderPlanRecords.Where(p => p.EstimateId == selectedEstimate.Id).Any())
                    {
                        NotificationHelper.ShowError("Видалення кошторису неможливе, оскільки під нього наявні записи у річному плані!");
                        this.Enabled = true;
                        return;
                    }

                    if(tc.Contracts.Where(p => p.EstimateId == selectedEstimate.Id).Any())
                    {
                        NotificationHelper.ShowError("Видалення кошторису неможливе, оскільки під нього є зареєстровані договори!");
                        this.Enabled = true;
                        return;
                    }

                    if(tc.PlannedSpending.Where(p => p.EstimateId == selectedEstimate.Id).Any())
                    {
                        NotificationHelper.ShowError("Видалення кошторису неможливе, оскільки під нього є зареєстровані заплановані витрати!");
                        this.Enabled = true;
                        return;
                    }

                    FileManager.UpdateRelatedFilesOfEntity(tc, 
                        selectedEstimate.RelatedFiles, 
                        new List<UploadedFile>(), 
                        selectedEstimate.RelatedFiles.ToList());

                    tc.Estimates.Remove(selectedEstimate);
                    tc.SaveChanges();

                    LoadEstimatesTable();
                    LoadMoneyRemainsTable();
                    UpdateEstimateCBList();

                    NotificationHelper.ShowInfo("Кошторис успішно видалений");
                    this.Enabled = true;
                }
            }
        }

        private void mainEstimateCBList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(controlsDataWasChangedByUser)
            {
                LoadMoneyRemainsTable();
            }
        }

        private void contractsTPRecordButton_Click(object sender, EventArgs e)
        {
            TenderPlanItemsTableEntry selectedRecord = tenderPlanTable.SelectedRows[0].DataBoundItem as TenderPlanItemsTableEntry;

            ContractsOnDkCodeForm cf = new ContractsOnDkCodeForm(selectedRecord.RelatedTenderPlanRecord, tpNewSystemRButton.Checked);
            cf.ShowDialog();
        }

        private void invoicesTable_SelectionChanged(object sender, EventArgs e)
        {
            editInvoiceButton.Enabled = deleteInvoiceButton.Enabled = (invoicesTable.SelectedRows.Count > 0);
        }

        private void EditSpendingRecord()
        {
            PlannedSpendingTableEntry selectedRow = spendingTable.SelectedRows[0].DataBoundItem as PlannedSpendingTableEntry;
            PlannedSpending selectedSpending = selectedRow.RelatedPlannedSpendingRecord;

            PlannedSpendingForm pf = new PlannedSpendingForm(MainProgramForm.CurrentTenderYear, selectedSpending);
            pf.ShowDialog();

            if (pf.DbWasChanged)
            {
                LoadPlannedSpendingTableRecords();
                LoadMoneyRemainsTable();
            }
        }

        private void deletePlSpendButton_Click(object sender, EventArgs e)
        {
            PlannedSpendingTableEntry selectedRow = spendingTable.SelectedRows[0].DataBoundItem as PlannedSpendingTableEntry;
            PlannedSpending selectedSpending = selectedRow.RelatedPlannedSpendingRecord;

            if (NotificationHelper.ShowYesNoQuestion("Ви впевнені, що хочете видалити виділену витрату?"))
            {
                using (TenderContext tc = new TenderContext())
                {
                    tc.PlannedSpending.Attach(selectedSpending);

                    // Удаляем все изменения по балансу
                    foreach(var item in selectedSpending.Changes.ToList())
                    {
                        tc.Entry<BalanceChanges>(item).State = EntityState.Deleted;
                    }
                    tc.SaveChanges();

                    // Видаляємо долучені файли
                    FileManager.UpdateRelatedFilesOfEntity(tc, 
                        selectedSpending.RelatedFiles, 
                        new List<UploadedFile>(), 
                        selectedSpending.RelatedFiles.ToList());

                    // Видалямо саму витрату
                    tc.PlannedSpending.Remove(selectedSpending);
                    tc.SaveChanges();

                }

                LoadPlannedSpendingTableRecords();
                LoadMoneyRemainsTable();

                NotificationHelper.ShowInfo("Витрата упісшно видалена!");
            }
        }

        private void newPlSpendButton_Click(object sender, EventArgs e)
        {
            PlannedSpendingForm pf = new PlannedSpendingForm(MainProgramForm.CurrentTenderYear);
            pf.ShowDialog();

            if(pf.DbWasChanged)
            {
                LoadPlannedSpendingTableRecords();
                LoadMoneyRemainsTable();
            }
        }

        private void spendingTable_SelectionChanged(object sender, EventArgs e)
        {
            editPlSpendButton.Enabled = deletePlSpendButton.Enabled = spendingTable.SelectedRows.Count > 0;
        }

        private void changePasswordMenuItem_Click(object sender, EventArgs e)
        {
            ChangeUserPasswordForm cf = new ChangeUserPasswordForm();
            cf.ShowDialog();
        }

        private void printEstimateButton_Click(object sender, EventArgs e)
        {
            Estimate selectedEstimate = (estimateTable.SelectedRows[0].DataBoundItem as EstimatesTableEntry).Estimate;

            EstimateYearMoneyReport rMaker = new EstimateYearMoneyReport(selectedEstimate);
            WaitingForm wf = new WaitingForm("Друк кошторису", 
                () => rMaker.MakeReport());
            wf.ShowDialog();

            if(string.IsNullOrWhiteSpace(wf.Error))
            {
                NotificationHelper.ShowInfo("Файл з даними кошторису успішно збережено!");
            }
            else
            {
                NotificationHelper.ShowError(wf.Error);
            }
        }

        private void yearPlanWithContractsMenuItem_Click(object sender, EventArgs e)
        {
            SelectEstimateForReportForm sf = new SelectEstimateForReportForm();
            sf.ShowDialog();

            if(sf.SelectedEstimate != null)
            {
                YearPlanReport report = null;
                if (sf.SelectedEstimate.Id > 0)
                {
                    report = new YearPlanReport(sf.SelectedEstimate, sf.IsNewSystemSelected);
                }
                else
                {
                    report = new YearPlanReport(MainProgramForm.CurrentTenderYear, sf.IsNewSystemSelected);
                }

                WaitingForm wf = new WaitingForm("Формування звіту", () => report.MakeReport());
                wf.ShowDialog();

                if (string.IsNullOrWhiteSpace(wf.Error))
                {
                    NotificationHelper.ShowInfo("Звіт успішно збережено у директорію \"Мої документи\"!");
                }
                else
                {
                    NotificationHelper.ShowError(wf.Error);
                }
            }
        }

        private void estimateWithBalanceChangesMenuItem_Click(object sender, EventArgs e)
        {
            SelectEstimateForReportForm sf = new SelectEstimateForReportForm();
            sf.ShowDialog();

            if (sf.SelectedEstimate != null)
            {
                EstimateYearSpendingReport report = null;
                if (sf.SelectedEstimate.Id > 0)
                {
                    report = new EstimateYearSpendingReport(sf.SelectedEstimate, sf.IsNewSystemSelected);
                }
                else
                {
                    report = new EstimateYearSpendingReport(MainProgramForm.CurrentTenderYear, sf.IsNewSystemSelected);
                }

                WaitingForm wf = new WaitingForm("Формування звіту", () => report.MakeReport());
                wf.ShowDialog();

                if (string.IsNullOrWhiteSpace(wf.Error))
                {
                    NotificationHelper.ShowInfo("Звіт успішно збережено у директорію \"Мої документи\"!");
                }
                else
                {
                    NotificationHelper.ShowError(wf.Error);
                }
            }
        }

        private void yearPlanWithChangesMenuItem_Click(object sender, EventArgs e)
        {
            SelectEstimateForReportForm sf = new SelectEstimateForReportForm();
            sf.ShowDialog();

            if (sf.SelectedEstimate != null)
            {
                YearPlanChangesReport report = null;
                if (sf.SelectedEstimate.Id > 0)
                {
                    report = new YearPlanChangesReport(sf.SelectedEstimate, sf.IsNewSystemSelected);
                }
                else
                {
                    report = new YearPlanChangesReport(MainProgramForm.CurrentTenderYear, sf.IsNewSystemSelected);
                }

                WaitingForm wf = new WaitingForm("Формування звіту", () => report.MakeReport());
                wf.ShowDialog();

                if (string.IsNullOrWhiteSpace(wf.Error))
                {
                    NotificationHelper.ShowInfo("Звіт успішно збережено у директорію \"Мої документи\"!");
                }
                else
                {
                    NotificationHelper.ShowError(wf.Error);
                }
            }
        }

        private void contractsReportMenuItem_Click(object sender, EventArgs e)
        {
            SelectEstimateForReportForm sf = new SelectEstimateForReportForm();
            sf.ShowDialog();

            if (sf.SelectedEstimate != null)
            {
                ContractsSpendingReport report = null;
                if (sf.SelectedEstimate.Id > 0)
                {
                    report = new ContractsSpendingReport(sf.SelectedEstimate, sf.IsNewSystemSelected);
                }
                else
                {
                    report = new ContractsSpendingReport(MainProgramForm.CurrentTenderYear, sf.IsNewSystemSelected);
                }

                WaitingForm wf = new WaitingForm("Формування звіту", () => report.MakeReport());
                wf.ShowDialog();

                if (string.IsNullOrWhiteSpace(wf.Error))
                {
                    NotificationHelper.ShowInfo("Звіт успішно збережено у директорію \"Мої документи\"!");
                }
                else
                {
                    NotificationHelper.ShowError(wf.Error);
                }
            }
        }

        private void tenderPlanTable_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (tenderPlanItemsList.Count > 2)
            {
                HighlightPlanTotalsRow(tenderPlanTable.Rows[tenderPlanTable.RowCount - 1]);
            }
            if (highlightSameCodesRowsCheckBox.Checked)
            {
                HighlightSameCodesRows();
            }
            tenderPlanTable.Refresh();
        }

        // Выделение записей в годовом плане с одинаковым кодом
        private void HighlightSameCodesRows()
        {
            int currentColorIndex = 0;
            int recordsCount = tenderPlanItemsList.Count;
            int colorsCount = samePlanCodesColors.Length;

            if(recordsCount < 3)
            {
                return;
            }
            else
            {
                for(int i = 0; i < recordsCount - 1; i++)
                {
                    if(tenderPlanItemsList[i].Dk.Equals(tenderPlanItemsList[i+1].Dk))
                    {
                        while(i < (recordsCount - 1))
                        {
                            if(tenderPlanItemsList[i].Dk.Equals(tenderPlanItemsList[i+1].Dk))
                            {
                                tenderPlanTable.Rows[i].DefaultCellStyle.BackColor =
                                    tenderPlanTable.Rows[i+1].DefaultCellStyle.BackColor = samePlanCodesColors[currentColorIndex];
                            }
                            else
                            {
                                currentColorIndex = (++currentColorIndex) % colorsCount;
                                break;
                            }
                            i++;
                        }
                    }
                }
            }
        }

        private void currentRemainReportMenuItem_Click(object sender, EventArgs e)
        {
            CurrentMoneyRemainsAndNewInvoicesReport report = new CurrentMoneyRemainsAndNewInvoicesReport(MainProgramForm.CurrentTenderYear);
            WaitingForm wf = new WaitingForm("Формування звіту", () => report.MakeReport());
            wf.ShowDialog();

            if (string.IsNullOrWhiteSpace(wf.Error))
            {
                NotificationHelper.ShowInfo("Звіт успішно збережено у директорію \"Мої документи\"!");
            }
            else
            {
                NotificationHelper.ShowError(wf.Error);
            }
        }

        private void highlightSameCodesRowsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if(highlightSameCodesRowsCheckBox.Checked)
            {
                HighlightSameCodesRows();
            }
            else
            {
                for(int i = 0; i < tenderPlanTable.RowCount - 1; i++)
                {
                    tenderPlanTable.Rows[i].DefaultCellStyle.BackColor = System.Drawing.SystemColors.Window;
                }
            }
        }
    }
}
