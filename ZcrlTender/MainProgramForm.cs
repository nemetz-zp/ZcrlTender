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

namespace ZcrlTender
{
    public partial class MainProgramForm : Form
    {
        private TenderYear currentTenderYear;
        private List<KekvCode> kekvs;
        private BindingList<KekvCode> kekvsForCBList;
        private BindingList<Estimate> estimatesForCBList;
        private List<MoneySource> moneySources;

        private BindingList<EstimatesTableEntry> estimatesList;
        private BindingList<TenderPlanItemsTableEntry> tenderPlanItemsList;
        private BindingList<ContractsTableEntry> contractsList;
        private BindingList<Invoice> invoicesList;
        private BindingList<PlannedSpending> anotherExpensesList;

        private volatile bool invgrControlStateChangedByUser;

        public MainProgramForm()
        {
            TenderYearForm tf = new TenderYearForm();
            tf.ShowDialog();

            if (tf.SelectedYear == null)
            {
                Environment.Exit(0);
            }
            else
            {
                currentTenderYear = tf.SelectedYear;
            }

            InitializeComponent();
            estimateTable.AutoGenerateColumns = moneyRemainsTable.AutoGenerateColumns = false;

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

                kekvsForCBList = new BindingList<KekvCode>(kekvs);
                kekvsForCBList.Add(new KekvCode { Id = -1, Code = "- ВСІ -" });
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

                // Формирование списка статусов договоров
                contStatusCBList.DataSource = new[] {
                    new { Name = "Всі", Value = ContractStatus.All },
                    new { Name = "Активні", Value = ContractStatus.Active },
                    new { Name = "Виконані", Value = ContractStatus.Complete }
                };
                contStatusCBList.DisplayMember = "Name";
                contStatusCBList.ValueMember = "Value";

                // Формирование списка статусов счетов
                invStatusCBList.DataSource = new[] {
                    new { Name = "Всі", Value = -1 },
                    new { Name = "Новий", Value = (int)InvoiceStatus.New },
                    new { Name = "На оплаті", Value = (int)InvoiceStatus.OnPayment },
                    new { Name = "Сплачений", Value = (int)InvoiceStatus.Payed }
                };
                invStatusCBList.DisplayMember = "Name";
                invStatusCBList.ValueMember = "Value";

                LoadMoneyRemainsTable();
                LoadEstimatesTable();
                LoadTenderPlanRecords();
                LoadContractsList();
                LoadInvoicesList();
                LoadAnotherExpensesList();
            }
        }

        // Обновление списка других трат
        private void LoadAnotherExpensesList()
        {
            throw new NotImplementedException();
        }

        // Обновление списка счетов на оплату
        private void LoadInvoicesList()
        {
            throw new NotImplementedException();
        }

        // Обновление списка контрактов (договоров)
        private void LoadContractsList()
        {
            throw new NotImplementedException();
        }

        // Обновление таблицы с записями годового плана
        private void LoadTenderPlanRecords()
        {
            if (!updatePlanRecords.IsBusy)
            {
                ToggleTenderPlanRecordsUpdateAnimation();
                TenderPlanRecordsFilterArguments filter = new TenderPlanRecordsFilterArguments();
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
            Estimate selectedEstimate = mainEstimateCBList.SelectedItem as Estimate;
            bool isNewSystemSelected = radioButton1.Checked;

            if (!updateMoneyRemains.IsBusy)
            {
                ToggleMoneyRemainsUpdateAnimation();
                updateMoneyRemains.RunWorkerAsync(new UpdateRemainsBGWorkerArgument { SelectedEstimate = selectedEstimate, IsNewSystem = isNewSystemSelected });
            }
        }

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
                                 }).ToList();
                estimatesList = new BindingList<EstimatesTableEntry>(estSums);
                estimateTable.DataSource = estimatesList;
                estimateTable.Refresh();

                estimatesForCBList = new BindingList<Estimate>(tc.Estimates.ToList());
                mainEstimateCBList.DataSource =
                    tpEstimateCBList.DataSource =
                    contEstimateCBList.DataSource =
                    spenEstimateCBList.DataSource =
                    estimatesForCBList;
                mainEstimateCBList.DataSource = estimatesList;
                mainEstimateCBList.DisplayMember = "Name";
                mainEstimateCBList.ValueMember = "Id";
                mainEstimateCBList.Refresh();
                tpEstimateCBList.Refresh();
                contEstimateCBList.Refresh();
                spenEstimateCBList.Refresh();

                newTPRecordButton.Enabled = newContractButton.Enabled = newInvoiceButton.Enabled = newPlSpendButton.Enabled = (estimatesForCBList.Count > 0);
            }
        }

        private void estimateTable_SelectionChanged(object sender, EventArgs e)
        {
            editEstimateButton.Enabled = deleteEstimateButton.Enabled = (estimateTable.SelectedRows.Count > 0);
        }

        private void ToggleMoneyRemainsUpdateAnimation()
        {
            moneyRemainsloadingPicture.Visible = moneyRemainsloadingLabel.Visible = !(moneyRemainsloadingLabel.Visible);
            mainEstimateCBList.Enabled = radioButton1.Enabled = radioButton2.Enabled = !radioButton2.Enabled;
        }

        private void ToggleTenderPlanRecordsUpdateAnimation()
        {
            bool isLoadingInProccess = !yearPlanLoadingLabel.Visible;
            yearPlanLoadingLabel.Visible = yearPlanLoadingPicture.Visible = isLoadingInProccess;
            groupBox3.Enabled =
                tpNewSystemRButton.Enabled =
                tpAltSystemRButton.Enabled =
                tpKekvsCBList.Enabled =
                tpEstimateCBList.Enabled =
                newTPRecordButton.Enabled = 
                editTPRecordButton.Enabled = 
                deleteTPRecordButton.Enabled = 
                historyTPRecordButton.Enabled =
                !isLoadingInProccess;

            if (newTPRecordButton.Enabled)
            {
                editTPRecordButton.Enabled = deleteTPRecordButton.Enabled = tenderPlanTable.SelectedRows.Count > 0;
            }
        }

        private void ToggleContractsListUpdateAnimation()
        {
            contractsLoadingLabel.Visible = contractsLoadingPicture.Visible = !(contractsLoadingPicture.Visible);
            groupBox2.Enabled = newContractButton.Enabled = editContractButton.Enabled = deleteContractButton.Enabled = !(newContractButton.Enabled);

            if (newContractButton.Enabled)
            {
                editContractButton.Enabled = deleteContractButton.Enabled = contractsTable.SelectedRows.Count > 0;
            }
        }

        private void ToggleInvoicesListUpdateAnimation()
        {
            invoicesLoadingLabel.Visible = invoicesLoadingPicture.Visible = !(invoicesLoadingPicture.Visible);
            groupBox1.Enabled = deleteInvoiceButton.Enabled = editInvoiceButton.Enabled = newInvoiceButton.Enabled = !(deleteInvoiceButton.Enabled);

            if (deleteInvoiceButton.Enabled)
            {
                editInvoiceButton.Enabled = newInvoiceButton.Enabled = dataGridView2.SelectedRows.Count > 0;
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

            LoadEstimatesTable();
            LoadMoneyRemainsTable();
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
                                 where item.DateOfReceiving <= DateTime.Now && item.Estimate.TenderYearId == currentTenderYear.Id
                                 select item;
                    }
                    else
                    {
                        moneys = from item in tc.BalanceChanges
                                 where item.EstimateId == arg.SelectedEstimate.Id && item.DateOfReceiving <= DateTime.Now && item.Estimate.TenderYearId == currentTenderYear.Id
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
                MyHelper.ShowException(ex);
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

        private void estimateTable_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataGridViewHelper.CalculateNewRowNumber(sender as DataGridView, 1, e.RowIndex, e.RowCount);
        }

        private void editEstimateButton_Click(object sender, EventArgs e)
        {
            Estimate selectedEstimate = (estimateTable.SelectedRows[0].DataBoundItem as EstimatesTableEntry).Estimate;
            EstimateForm ef = new EstimateForm(selectedEstimate);
            ef.ShowDialog();

            LoadEstimatesTable();
            LoadMoneyRemainsTable();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ContractForm cf = new ContractForm(currentTenderYear);
            cf.ShowDialog();
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            InvoiceForm iForm = new InvoiceForm();
            iForm.ShowDialog();
        }

        // -------------------------------------------------
        // Классы-фильтры результатов таблиц записей о договорах, счетах и т.п.
        // -------------------------------------------------

        // Фильтр результатов таблицы записей годового плана
        private class TenderPlanRecordsFilterArguments
        {
            public Estimate Estimate { get; set; }
            public KekvCode Kekv { get; set; }
            public bool PrimaryKekvSelected { get; set; }
            public bool NullRecordsShow { get; set; }
        }

        // Фильтр результатов таблицы записей договоров 
        private class ContractRecordsFilterArguments
        {
            public Estimate Estimate { get; set; }
            public Contractor Contractor { get; set; }
            public KekvCode Kekv { get; set; }
            public bool PrimaryKekvSelected { get; set; }
            public DateTime ContractSignBeginDate { get; set; }
            public DateTime ContractSignEndDate { get; set; }
            public ContractStatus Status { get; set; }
        }

        // Фильтр результатов таблицы записей счетов
        private class InvoiceRecordsFilterArguments
        {
            public Contractor Contractor { get; set; }
            public Contract Contract { get; set; }
            public KekvCode Kekv { get; set; }
            public bool PrimaryKekvSelected { get; set; }
            public bool IsCredit { get; set; }
            public DateTime InvoiceBeginDate { get; set; }
            public DateTime InvoiceEndDate { get; set; }
            public int Status { get; set; }
        }

        // Статус договора
        private enum ContractStatus
        {
            All,        // Все
            Active,     // Действующий
            Complete    // Выполненый
        }

        // -------------------------------------------------

        private void updatePlanRecords_DoWork(object sender, DoWorkEventArgs e)
        {
            TenderPlanRecordsFilterArguments filter = e.Argument as TenderPlanRecordsFilterArguments;
            List<TenderPlanItemsTableEntry> resultList = null;

            try
            {
                using (TenderContext tc = new TenderContext())
                {
                    List<TenderPlanItemsTableEntry> planRecords = null;
                    List<TenderPlanItemsTableEntry> contractsSum = null;
                    List<TenderPlanItemsTableEntry> usedSum = null;

                    if (filter.PrimaryKekvSelected)
                    {
                        planRecords = (from r in tc.TenderPlanRecords.ToList()
                                       where r.EstimateId == filter.Estimate.Id
                                       select new TenderPlanItemsTableEntry
                                       {
                                           Kekv = r.PrimaryKekv,
                                           Dk = r.Dk,
                                           MoneyOnCode = r.Sum,
                                           TenderPlanRecordId = r.Id
                                       }).ToList();
                        contractsSum = (from c in tc.Contracts.ToList()
                                        where c.EstimateId == filter.Estimate.Id
                                        group c by new { c.PrimaryKekv, c.Dk } into g1
                                        select new TenderPlanItemsTableEntry
                                        {
                                            Kekv = g1.Key.PrimaryKekv,
                                            Dk = g1.Key.Dk,
                                            MoneyOnCode = g1.Sum(p => p.Sum)
                                        }).ToList();
                        usedSum = (from c in tc.BalanceChanges.ToList()
                                   where (c.EstimateId == filter.Estimate.Id) && (c.InvoiceId != null)
                                   group c by new { c.PrimaryKekv, c.Dk } into g1
                                   select new TenderPlanItemsTableEntry
                                   {
                                       Kekv = g1.Key.PrimaryKekv,
                                       Dk = g1.Key.Dk,
                                       MoneyOnCode = g1.Sum(p => p.PrimaryKekvSum)
                                   }).ToList();
                    }
                    else
                    {
                        planRecords = (from r in tc.TenderPlanRecords.ToList()
                                       where r.EstimateId == filter.Estimate.Id
                                       select new TenderPlanItemsTableEntry
                                       {
                                           Kekv = r.SecondaryKekv,
                                           Dk = r.Dk,
                                           MoneyOnCode = r.Sum,
                                           TenderPlanRecordId = r.Id
                                       }).ToList();
                        contractsSum = (from c in tc.Contracts.ToList()
                                        where c.EstimateId == filter.Estimate.Id
                                        group c by new { c.SecondaryKekv, c.Dk } into g1
                                        select new TenderPlanItemsTableEntry
                                        {
                                            Kekv = g1.Key.SecondaryKekv,
                                            Dk = g1.Key.Dk,
                                            RegisteredByContracts = g1.Sum(p => p.Sum)
                                        }).ToList();
                        usedSum = (from c in tc.BalanceChanges.ToList()
                                   where (c.EstimateId == filter.Estimate.Id) && (c.InvoiceId != null)
                                   group c by new { c.SecondaryKekv, c.Dk } into g1
                                   select new TenderPlanItemsTableEntry
                                   {
                                       Kekv = g1.Key.SecondaryKekv,
                                       Dk = g1.Key.Dk,
                                       UsedByContracts = g1.Sum(p => p.SecondaryKekvSum)
                                   }).ToList();
                    }

                    if (filter.Kekv.Id > 0)
                    {
                        planRecords = planRecords.Where(p => p.Kekv.Id == filter.Kekv.Id).ToList();
                        contractsSum = planRecords.Where(p => p.Kekv.Id == filter.Kekv.Id).ToList();
                        usedSum = planRecords.Where(p => p.Kekv.Id == filter.Kekv.Id).ToList();
                    }

                    resultList = (from p in planRecords
                                  join c in contractsSum on new { p.Kekv, p.Dk } equals new { c.Kekv, c.Dk } into g1
                                  from t2 in g1.DefaultIfEmpty()
                                  join u in usedSum on new { p.Kekv, p.Dk } equals new { u.Kekv, u.Dk } into g2
                                  from t3 in g2.DefaultIfEmpty()
                                  select new TenderPlanItemsTableEntry
                                  {
                                      TenderPlanRecordId = p.TenderPlanRecordId,
                                      Kekv = p.Kekv,
                                      Dk = p.Dk,
                                      MoneyOnCode = p.MoneyOnCode,
                                      RegisteredByContracts = t2.RegisteredByContracts,
                                      UsedByContracts = t3.UsedByContracts,
                                      ContractsMoneyRemain = t2.RegisteredByContracts
                                  }).ToList();

                    if (!filter.NullRecordsShow)
                    {
                        resultList = resultList.Where(p => p.MoneyOnCode > 0).ToList();
                    }

                    e.Result = resultList;
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
                MyHelper.ShowException(workerError);
            }
            else
            {
                tenderPlanItemsList = new BindingList<TenderPlanItemsTableEntry>(e.Result as List<TenderPlanItemsTableEntry>);
                tenderPlanTable.DataSource = tenderPlanItemsList;
                tenderPlanTable.Refresh();
            }

            ToggleTenderPlanRecordsUpdateAnimation();
        }

        private void updateContractsList_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (TenderContext tc = new TenderContext())
                {
                    ContractRecordsFilterArguments filter = e.Argument as ContractRecordsFilterArguments;
                    List<Contract> allContracts = tc.Contracts.Where(p => (p.EstimateId == filter.Estimate.Id) &&
                        ((p.SignDate >= contrStartDatePicker.Value) && (p.SignDate <= contrEndDatePicker.Value))).ToList();

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

                    List<ContractsTableEntry> result = (from c in allContracts
                                                        select new ContractsTableEntry
                                                        {
                                                            ContractId = c.Id,
                                                            ContractDate = c.SignDate,
                                                            ContractNum = c.Number,
                                                            Contractor = c.Contractor,
                                                            Description = c.Description,
                                                            FullSum = c.Sum,
                                                            UsedSum = c.Invoices.Where(p => p.Changes.Count > 0).Sum(p => p.Sum)
                                                        }).ToList();
                    switch (filter.Status)
                    {
                        case ContractStatus.Active:
                            result = result.Where(p => p.UsedSum < p.FullSum).ToList();
                            break;
                        case ContractStatus.Complete:
                            result = result.Where(p => p.UsedSum == p.FullSum).ToList();
                            break;
                    }

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
                MyHelper.ShowException(e.Result as Exception);
            }
            else
            {
                contractsList = new BindingList<ContractsTableEntry>(e.Result as List<ContractsTableEntry>);
                contractsTable.DataSource = contractsList;
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
                    InvoiceRecordsFilterArguments filter = e.Argument as InvoiceRecordsFilterArguments;
                    List<Invoice> allInvoices = tc.Invoices.ToList().Where(p => (p.Contract.Estimate.Year.Id == currentTenderYear.Id) &&
                        ((p.Date >= invStartDatePicker.Value) && (p.Date <= invEndDatePicker.Value))).ToList();

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

                    List<InvoiceRecordsTableEntry> result = (from c in allInvoices
                                                             select new InvoiceRecordsTableEntry
                                                             {
                                                                 Contract = c.Contract,
                                                                 Date = c.Date,
                                                                 InvoiceId = c.Id,
                                                                 IsCredit = c.IsCredit,
                                                                 Contractor = c.Contract.Contractor,
                                                                 Number = c.Number,
                                                                 Status = c.Status
                                                            }).ToList();

                    string invStatus = Enum.GetName(typeof(InvoiceStatus), filter.Status);
                    if (invStatus != null)
                    {
                        InvoiceStatus status = (InvoiceStatus)Enum.Parse(typeof(InvoiceStatus), invStatus);
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

        }

        private void updateAnotherExpensesList_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void updateAnotherExpensesList_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void tpShowNullCodesChBox_CheckedChanged(object sender, EventArgs e)
        {
            LoadTenderPlanRecords();
        }

        private void newTPRecordButton_Click(object sender, EventArgs e)
        {
            try
            {
                Estimate selectedEstimate = tpEstimateCBList.SelectedItem as Estimate;
                AddEditTPRecordForm tf = new AddEditTPRecordForm(selectedEstimate);
                tf.ShowDialog();
            }
            catch (Exception ex)
            {
                MyHelper.ShowException(ex);
            }
        }

        private void editTPRecordButton_Click(object sender, EventArgs e)
        {
            try
            {
                TenderPlanItemsTableEntry selectedTPRecord = tenderPlanTable.SelectedRows[0].DataBoundItem as TenderPlanItemsTableEntry;
                AddEditTPRecordForm tf = new AddEditTPRecordForm(selectedTPRecord.TenderPlanRecordId);
                tf.ShowDialog();
            }
            catch (Exception ex)
            {
                MyHelper.ShowException(ex);
            }
        }

        private void deleteTPRecordButton_Click(object sender, EventArgs e)
        {

        }

        private void historyTPRecordButton_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void invContractCBList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
