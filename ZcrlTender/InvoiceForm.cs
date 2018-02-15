using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using TenderLibrary;

namespace ZcrlTender
{
    public partial class InvoiceForm : Form
    {
        private List<MoneySource> sourcesList;
        private List<decimal> moneysOnSources;
        private Invoice invoiceRecord;

        private object locker;

        private TenderYear year;

        private bool controlValueWasChangedByUser;

        private List<decimal> contractsRemains;

        private BindingList<UploadedFile> relatedFiles;
        private BindingList<UploadedFile> deletingFiles;

        private bool dbWasChanged;
        public bool DbWasChanged
        {
            get
            {
                return dbWasChanged;
            }
        }

        public InvoiceForm(TenderYear year)
        {
            invoiceRecord = new Invoice();
            InitializeFormControls(year);

            LoadContractsRecords();
        }

        public InvoiceForm(TenderYear year, Invoice inv)
        {
            invoiceRecord = inv;
            InitializeFormControls(year);

            if (!UserSession.IsAuthorized)
            {
                this.Text = "Перегляд даних рахунку";
            }
            else
            {
                this.Text = "Редагування даних рахунку";
            }

            using(TenderContext tc = new TenderContext())
            {
                tc.Invoices.Attach(invoiceRecord);

                controlValueWasChangedByUser = false;
                contractsCBList.DataSource = new List<Contract> { invoiceRecord.Contract };
                contractorsCBList.DataSource = new List<Contractor> { invoiceRecord.Contract.Contractor };
                contractsCBList.Enabled = contractorsCBList.Enabled = false;
                controlValueWasChangedByUser = true;

                foreach (var file in invoiceRecord.RelatedFiles)
                    relatedFiles.Add(file);

                decimal moneyRemainAtContract = invoiceRecord.Contract.MoneyRemain;
                label6.Text = string.Format("Доступно: {0:N2} грн.", moneyRemainAtContract);
                if(moneyRemainAtContract > 0)
                {
                    invoiceFullSum.Maximum = invoiceRecord.Sum + moneyRemainAtContract;
                }
                else
                {
                    invoiceFullSum.Maximum = invoiceRecord.Sum;
                }
                estimateNameLabel.Text = invoiceRecord.Contract.RecordInPlan.Estimate.Name;

                LoadMoneyRemains();

                // Заполняем затраты счёта по источникам финансирования
                if(invoiceRecord.Changes.Count > 0)
                {
                    for(int i = 0; i < sourcesList.Count; i++)
                    {
                        foreach(var rec in invoiceRecord.Changes)
                        {
                            if(rec.MoneySourceId == sourcesList[i].Id)
                            {
                                controlValueWasChangedByUser = false;
                                balanceChangesTable.Rows[i].Cells[0].Value = -rec.PrimaryKekvSum;
                                controlValueWasChangedByUser = true;
                            }
                        }
                    }
                }
            }
        }

        private void InitializeFormControls(TenderYear year)
        {
            InitializeComponent();

            button1.Visible = UserSession.IsAuthorized;

            label6.Text = estimateNameLabel.Text = string.Empty;
            this.year = year;
            controlValueWasChangedByUser = false;
            locker = new object();
            contractsRemains = new List<decimal>();
            moneysOnSources = new List<decimal>();

            relatedFiles = new BindingList<UploadedFile>();
            deletingFiles = new BindingList<UploadedFile>();
            DataGridViewHelper.ConfigureFileTable(filesTable, relatedFiles, deletingFiles, linkLabel1, linkLabel2, linkLabel3);
            filesTable.DataSource = relatedFiles;

            numberTextBox.Text = invoiceRecord.Number;
            descriptionTextBox.Text = invoiceRecord.Description;
            invoiceFullSum.Value = invoiceRecord.Sum;
            IsCreditCheckBox.Checked = invoiceRecord.IsCredit;

            switch(invoiceRecord.Status)
            {
                case PaymentStatus.New:
                    newStatusRButton.Checked = true;
                    break;
                case PaymentStatus.OnPayment:
                    onPayStatusRButton.Checked = true;
                    break;
                case PaymentStatus.Payed:
                    paidStatusRButton.Checked = true;
                    break;
            }

            if (invoiceRecord.Id == 0)
            {
                invoiceDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0).AddYears(Convert.ToInt32(year.Year) - DateTime.Now.Year);
            }
            else
            {
                invoiceDate.Value = invoiceRecord.Date;
            }

            invoiceDate.MaxDate = new DateTime(Convert.ToInt32(year.Year), 12, 31, 0, 0, 0);
            invoiceDate.MinDate = new DateTime(Convert.ToInt32(year.Year), 1, 1, 0, 0, 0);

            using (TenderContext tc = new TenderContext())
            {
                sourcesList = tc.MoneySources.OrderBy(p => p.ViewPriority).ToList();
                contractorsCBList.DataSource = tc.Contracts.ToList()
                    .Where(p => (p.RecordInPlan.Estimate.TenderYearId == year.Id) && (p.Status == ContractStatus.Active))
                    .Select(p => p.Contractor).Distinct().OrderBy(p => p.ShortName)
                    .ToList();
                contractorsCBList.ValueMember = "Id";
                contractorsCBList.DisplayMember = "ShortName";

                contractsCBList.ValueMember = "Id";
                contractsCBList.DisplayMember = "FullName";
            }

            for (int i = 0; i <= sourcesList.Count; i++)
            {
                DataGridViewRow newRow = new DataGridViewRow();
                newRow.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                newRow.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                if (i < sourcesList.Count)
                {
                    newRow.HeaderCell.Value = sourcesList[i].Name.ToString();
                }
                else
                {
                    newRow.HeaderCell.Value = "ВСЬОГО";
                    newRow.DefaultCellStyle.Font = FormStyles.MoneyTotalsFont;
                    newRow.ReadOnly = true;
                }
                balanceChangesTable.Rows.Add(newRow);
            }

            for (int i = 0; i < balanceChangesTable.ColumnCount; i++)
            {
                balanceChangesTable.Columns[i].ValueType = typeof(decimal);
                balanceChangesTable.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            controlValueWasChangedByUser = false;
            for (int i = 0; i < (balanceChangesTable.RowCount - 1); i++)
            {
                balanceChangesTable.Rows[i].Cells[0].Value = 0;
                balanceChangesTable.Rows[i].Cells[1].Value = 0;
                moneysOnSources.Add(0);
            }
            controlValueWasChangedByUser = true;
        }

        // Загрузка остатков средств на дату регистрации счёта
        private void LoadMoneyRemains()
        {
            if(!updateMoneyRemainsWorker.IsBusy)
            {
                Contract selectedContract = (contractsCBList.SelectedItem as Contract);

                if (selectedContract == null)
                {
                    return;
                }


                updateMoneyRemainsWorker.RunWorkerAsync(new UpdateMoneyRemainsWorkerArgument 
                {
                    Contract = selectedContract, 
                    InvoiceDate = invoiceDate.Value 
                });
                ToggleMoneyRemainLoadingAnimation();
            }
        }

        private void ToggleMoneyRemainLoadingAnimation()
        {
            balanceChangesTable.Tag = (balanceChangesTable.Tag == null) ? locker : null;
            bool isLoadingProcess = (balanceChangesTable.Tag != null);

            newStatusRButton.Enabled = onPayStatusRButton.Enabled = paidStatusRButton.Enabled = balanceChangesTable.Enabled = !isLoadingProcess;
            if(!isLoadingProcess)
            {
                balanceChangesTable.Enabled = !newStatusRButton.Checked;
            }
            moneyRemainsLoadingPicture.Visible = isLoadingProcess;
        }

        private void LoadContractsRecords()
        {
            if(!updateContractsListWorker.IsBusy)
            {
                Contractor selectedContractor = contractorsCBList.SelectedItem as Contractor;
                if(selectedContractor != null)
                {
                    updateContractsListWorker.RunWorkerAsync(selectedContractor);
                    ToggleLoadContractsAnimation();
                }
            }
        }

        private void ToggleLoadContractsAnimation()
        {
            contractorsCBList.Tag = (contractorsCBList.Tag == null) ? locker : null;
            bool loadingProccess = (contractorsCBList.Tag != null);

            contractorsCBList.Enabled = contractsCBList.Enabled = label6.Visible = !loadingProccess;
            contractRemainLoadingPicture.Visible = loadingProccess;

            contractorsCBList.Enabled = contractsCBList.Enabled = (invoiceRecord.Id == 0);
        }

        // Автосуммирование введённых сумм по источникам финансирования
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                decimal currentValue = (balanceChangesTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                        ? 0
                        : decimal.Parse(balanceChangesTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                balanceChangesTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = (currentValue < 0) ? FormStyles.WrongSumColor : FormStyles.RightSumColor;

                if (e.RowIndex != balanceChangesTable.RowCount - 1)
                {
                    decimal sum = 0;
                    for (int i = 0; i < balanceChangesTable.RowCount - 1; i++)
                    {
                        sum += (balanceChangesTable.Rows[i].Cells[e.ColumnIndex].Value == null)
                            ? 0
                            : decimal.Parse(balanceChangesTable.Rows[i].Cells[e.ColumnIndex].Value.ToString());
                    }

                    balanceChangesTable.Rows[balanceChangesTable.RowCount - 1].Cells[e.ColumnIndex].Value = sum;
                }
            }
        }

        private void updateContractsListWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Contractor selectedContractor = e.Argument as Contractor;
            using (TenderContext tc = new TenderContext())
            {
                List<Contract> contractsList = tc.Contracts.Include(p => p.RecordInPlan.Estimate).ToList()
                            .Where(p => (p.ContractorId == selectedContractor.Id)
                                    && (p.RecordInPlan.Estimate.TenderYearId == year.Id))
                            .Select(p => p).ToList(); ;
                
                if (invoiceRecord.Id == 0)
                {
                    contractsList = contractsList.Where(p => (p.Status == ContractStatus.Active)).ToList();
                }

                contractsRemains.Clear();
                foreach (var contract in contractsList)
                    contractsRemains.Add(contract.MoneyRemain);

                e.Result = contractsList;
            }
        }

        private void updateContractsListWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            List<Contract> contractsList = e.Result as List<Contract>;

            controlValueWasChangedByUser = false;
            contractsCBList.DataSource = contractsList;
            controlValueWasChangedByUser = true;
            contractsCBList.Refresh();

            int selectedContractIndex = contractsCBList.SelectedIndex;
            if (selectedContractIndex >= 0)
            {
                label6.Text = string.Format("Доступно: {0:N2} грн.", contractsRemains[selectedContractIndex]);

                if ((invoiceRecord.Id == 0) && (invoiceFullSum.Value > contractsRemains[selectedContractIndex]))
                {
                    invoiceFullSum.Value = 0;
                }
                
                if(invoiceRecord.Id == 0)
                {
                    invoiceFullSum.Maximum = contractsRemains[selectedContractIndex];
                }
                else
                {
                    invoiceFullSum.Maximum = contractsRemains[selectedContractIndex] + invoiceRecord.Sum;
                }

                estimateNameLabel.Text = contractsList[selectedContractIndex].RecordInPlan.Estimate.Name;

                Contract selectedContract = contractsCBList.SelectedItem as Contract;
                if (invoiceDate.Value < selectedContract.BeginDate)
                {
                    invoiceDate.Value = selectedContract.BeginDate;
                }

                invoiceDate.MinDate = selectedContract.BeginDate;
            }
            else
            {
                label6.Text = estimateNameLabel.Text = string.Empty;
            }

            LoadMoneyRemains();
            ToggleLoadContractsAnimation();
        }

        class UpdateMoneyRemainsWorkerArgument
        {
            public DateTime InvoiceDate { get; set; }
            public Contract Contract { get; set; }
        }

        private void updateMoneyRemainsWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            UpdateMoneyRemainsWorkerArgument arg = e.Argument as UpdateMoneyRemainsWorkerArgument;
            using (TenderContext tc = new TenderContext())
            {
                moneysOnSources.Clear();
                foreach(var source in sourcesList)
                {
                    decimal remain = tc.BalanceChanges
                        .Where(p => (p.EstimateId == arg.Contract.RecordInPlan.EstimateId)
                            && (p.PrimaryKekvId == arg.Contract.RecordInPlan.PrimaryKekvId)
                        && (p.DateOfReceiving <= arg.InvoiceDate)
                        && (p.MoneySourceId == source.Id)).Select(p => p.PrimaryKekvSum).DefaultIfEmpty(0).Sum();

                    if (invoiceRecord.Id > 0)
                    {
                        var rec = tc.BalanceChanges
                            .Where(p => (p.InvoiceId == invoiceRecord.Id) 
                                && (p.EstimateId == arg.Contract.RecordInPlan.EstimateId) 
                                && (p.MoneySourceId == source.Id)).FirstOrDefault();
                       
                        if(rec != null)
                        {
                            remain -= rec.PrimaryKekvSum;
                        }
                    }

                    moneysOnSources.Add(remain);
                }
            }
        }

        private decimal GetTableTotalSum()
        {
            return Convert.ToDecimal(balanceChangesTable.Rows[sourcesList.Count].Cells[0].Value);
        }

        private void updateMoneyRemainsWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            for(int i = 0; i < sourcesList.Count; i++)
            {
                balanceChangesTable.Rows[i].Cells[1].Value = moneysOnSources[i];
            }

            CorrectMoneysOnSources();
            ToggleMoneyRemainLoadingAnimation();
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if(!controlValueWasChangedByUser)
            {
                return;
            }

            if(e.ColumnIndex != 0)
            {
                return;
            }

            decimal cellValue = 0;

            if(!decimal.TryParse(e.FormattedValue.ToString(), out cellValue))
            {
                balanceChangesTable.CancelEdit();
                return;
            }

            // Проверяем запрошеную сумму по источнику
            if(cellValue > moneysOnSources[e.RowIndex])
            {
                NotificationHelper.ShowError("На вказаному джерелі фінансування недостатньо коштів");
                balanceChangesTable.CancelEdit();
                return;
            }

            // Проверяем общую сумму по счёту
            decimal sum = 0;
            for(int i = 0; i < sourcesList.Count; i++)
            {
                if(i == e.RowIndex)
                {
                    sum += cellValue;
                }
                else
                {
                    sum += Convert.ToDecimal(balanceChangesTable.Rows[i].Cells[0].Value);
                }

                if(sum > invoiceFullSum.Value)
                {
                    balanceChangesTable.CancelEdit();
                    return;
                }
            }
        }

        // Корректировка сумм запрашиваемых для оплаты с источников при обновлении максимально доступных средств на источниках
        private void CorrectMoneysOnSources()
        {
            for(int i = 0; i < sourcesList.Count; i++)
            {
                decimal enteredByUserSum = Convert.ToDecimal(balanceChangesTable.Rows[i].Cells[0].Value);
                if(enteredByUserSum > moneysOnSources[i])
                {
                    enteredByUserSum = moneysOnSources[i];
                }
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if(controlValueWasChangedByUser && (invoiceFullSum.Value < GetTableTotalSum()))
            {
                for (int i = 0; i < (balanceChangesTable.RowCount - 1); i++)
                    balanceChangesTable.Rows[i].Cells[0].Value = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(numberTextBox.Text))
            {
                NotificationHelper.ShowError("Ви не ввели номер рахунку");
                return;
            }
            if (invoiceFullSum.Value == 0)
            {
                NotificationHelper.ShowError("Сумма рахунку повинна бути більша за 0");
                return;
            }
            if (contractorsCBList.SelectedItem == null)
            {
                NotificationHelper.ShowError("Ви не обрали контрагента");
                return;
            }
            if (contractsCBList.SelectedItem == null)
            {
                NotificationHelper.ShowError("Ви не обрали договір");
                return;
            }
            if(!newStatusRButton.Checked && invoiceFullSum.Value != GetTableTotalSum())
            {
                NotificationHelper.ShowError("Загальна сума рахунку не дорівнює загальній сумі яку потрібно зняти з джерел фінансування");
                return;
            }

            using (TenderContext tc = new TenderContext())
            {
                Contract selectedContract = contractsCBList.SelectedItem as Contract;

                // Если мы создаём новый счёт - проверяем доступный для него остаток по договору
                if((invoiceRecord.Id == 0) && (invoiceFullSum.Value > selectedContract.MoneyRemain))
                {
                    NotificationHelper.ShowError("На договорі недостатньо коштів для проплати данного рахунку");
                    return;
                }

                if (invoiceRecord.Id > 0)
                {
                    if(invoiceFullSum.Value > (selectedContract.MoneyRemain + invoiceRecord.Sum))
                    {
                        NotificationHelper.ShowError("На договорі недостатньо коштів для проплати вказаної суми");
                        return;
                    }

                    tc.Invoices.Attach(invoiceRecord);
                    tc.Entry<Invoice>(invoiceRecord).State = EntityState.Modified;
                }


                invoiceRecord.IsCredit = IsCreditCheckBox.Checked;
                invoiceRecord.Number = numberTextBox.Text.Trim();
                invoiceRecord.Description = descriptionTextBox.Text.Trim();
                invoiceRecord.ContractId = selectedContract.Id;
                invoiceRecord.Date = invoiceDate.Value;
                invoiceRecord.Sum = invoiceFullSum.Value;

                // Удаляем старые записи об изменениях в балансе
                foreach (var item in invoiceRecord.Changes.ToList())
                {
                    tc.BalanceChanges.Attach(item);
                    tc.Entry<BalanceChanges>(item).State = EntityState.Deleted;
                }
                tc.SaveChanges();

                if(onPayStatusRButton.Checked || paidStatusRButton.Checked)
                {
                    for (int i = 0; i < sourcesList.Count; i++)
                    {
                        decimal balanceChangeOnSource = Convert.ToDecimal(balanceChangesTable.Rows[i].Cells[0].Value);

                        if(balanceChangeOnSource == 0)
                        {
                            continue;
                        }

                        invoiceRecord.Changes.Add(new BalanceChanges 
                        {
                            DateOfReceiving = invoiceRecord.Date,
                            MoneySourceId = sourcesList[i].Id,
                            PrimaryKekvId = selectedContract.RecordInPlan.PrimaryKekvId,
                            SecondaryKekvId = selectedContract.RecordInPlan.SecondaryKekvId,
                            EstimateId = selectedContract.RecordInPlan.EstimateId,
                            PrimaryKekvSum = -balanceChangeOnSource,
                            SecondaryKekvSum = -balanceChangeOnSource,
                        });
                    }

                    invoiceRecord.IsPaid = paidStatusRButton.Checked;
                    tc.SaveChanges();
                }

                if(invoiceRecord.Id == 0)
                {
                    tc.Invoices.Add(invoiceRecord);
                }
                tc.SaveChanges();

                FileManager.UpdateRelatedFilesOfEntity(tc, invoiceRecord.RelatedFiles, relatedFiles, deletingFiles);

                NotificationHelper.ShowInfo("Дані успішно збережені!");
                dbWasChanged = true;
                this.Close();
            }
        }

        private void newStatusRButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rBut = sender as RadioButton;
            if(rBut.Name.Equals("newStatusRButton") && rBut.Checked)
            {
                balanceChangesTable.Enabled = false;
            }
            else
            {
                balanceChangesTable.Enabled = true;
            }
        }

        private void contractorsCBList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(controlValueWasChangedByUser)
            {
                LoadContractsRecords();
            }
        }

        private void invoiceDate_ValueChanged(object sender, EventArgs e)
        {
            if(controlValueWasChangedByUser)
            {
                LoadMoneyRemains();
            }
        }

        private void contractsCBList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(controlValueWasChangedByUser)
            {
                Contract selectedContract = contractsCBList.SelectedItem as Contract;
                if(selectedContract != null)
                {
                    int selectedContractIndex = contractsCBList.SelectedIndex;
                    label6.Text = string.Format("Доступно: {0:N2} грн.", contractsRemains[selectedContractIndex]);

                    if (invoiceDate.Value < selectedContract.BeginDate)
                    {
                        invoiceDate.Value = selectedContract.BeginDate;
                    }

                    invoiceDate.MinDate = selectedContract.BeginDate;
                    invoiceFullSum.Maximum = contractsRemains[selectedContractIndex];
                }
            }
        }
    }
}
