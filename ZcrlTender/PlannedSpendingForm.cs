using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TenderLibrary;

namespace ZcrlTender
{
    public partial class PlannedSpendingForm : Form
    {
        private List<MoneySource> sourcesList;
        private List<decimal> moneysOnSources;
        private PlannedSpending spendingRecord;

        private object locker;

        private TenderYear year;

        private bool controlValueWasChangedByUser;

        private List<decimal> kekvRemains;

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

        public PlannedSpendingForm(TenderYear year)
        {
            spendingRecord = new PlannedSpending();
            InitializeFormControls(year);

            LoadKekvsList();
        }

        public PlannedSpendingForm(TenderYear year, PlannedSpending spending)
        {
            spendingRecord = spending;
            InitializeFormControls(year);

            if (!UserSession.IsAuthorized)
            {
                this.Text = "Перегляд даних витрати";
            }
            else
            {
                this.Text = "Редагування даних витрати";
            }

            using (TenderContext tc = new TenderContext())
            {
                tc.PlannedSpending.Attach(spendingRecord);

                controlValueWasChangedByUser = false;
                estimateCBList.DataSource = new List<Estimate> { spendingRecord.Estimate };
                mainKekvCBList.DataSource = new List<KekvCode> { spendingRecord.PrimaryKekv };
                altKekvCBList.DataSource = new List<KekvCode> { spendingRecord.SecondaryKekv };
                estimateCBList.Enabled = mainKekvCBList.Enabled = altKekvCBList.Enabled = false;
                controlValueWasChangedByUser = true;

                foreach (var file in spendingRecord.RelatedFiles)
                    relatedFiles.Add(file);

                var lst = tc.BalanceChanges
                    .Where(p => (p.EstimateId == spendingRecord.EstimateId)
                             && (p.PrimaryKekvId == spendingRecord.PrimaryKekvId)
                             && ((p.PrimaryKekvSum > 0) || (p.PlannedSpendingId != null)))
                             .Select(p => p.PrimaryKekvSum)
                             .ToList();

                decimal notPlannedMoneyOnKekv = tc.BalanceChanges
                    .Where(p => (p.EstimateId == spendingRecord.EstimateId) 
                             && (p.PrimaryKekvId == spendingRecord.PrimaryKekvId)
                             && ((p.PrimaryKekvSum > 0) || (p.PlannedSpendingId != null)))
                             .Select(p => p.PrimaryKekvSum)
                             .ToList()
                             .DefaultIfEmpty(0)
                             .Sum();
                decimal plannedMoneyOnKekv = tc.TenderPlanRecords.ToList()
                    .Where(p => (p.EstimateId == spendingRecord.EstimateId)
                             && (p.PrimaryKekvId == spendingRecord.PrimaryKekvId))
                    .Select(p => p.UsedByRecordSum)
                    .ToList()
                    .DefaultIfEmpty(0)
                    .Sum();

                decimal moneyRemainAtMainKekv = notPlannedMoneyOnKekv - plannedMoneyOnKekv;

                label6.Text = string.Format("Доступно: {0:N2} грн.", moneyRemainAtMainKekv);
                if (moneyRemainAtMainKekv > 0)
                {
                    spendingFullSum.Maximum = spendingRecord.Sum + moneyRemainAtMainKekv;
                }
                else
                {
                    spendingFullSum.Maximum = spendingRecord.Sum;
                }

                LoadMoneysTable();

                // Заполняем затраты счёта по источникам финансирования
                if (spendingRecord.Changes.Count > 0)
                {
                    for (int i = 0; i < sourcesList.Count; i++)
                    {
                        foreach (var rec in spendingRecord.Changes)
                        {
                            if (rec.MoneySourceId == sourcesList[i].Id)
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

            label6.Text = string.Empty;
            this.year = year;
            controlValueWasChangedByUser = false;
            locker = new object();
            kekvRemains = new List<decimal>();
            moneysOnSources = new List<decimal>();
            relatedFiles = new BindingList<UploadedFile>();
            deletingFiles = new BindingList<UploadedFile>();
            DataGridViewHelper.ConfigureFileTable(filesTable, relatedFiles, deletingFiles, linkLabel1, linkLabel2, linkLabel3);

            filesTable.DataSource = relatedFiles;

            spendingDescription.Text = spendingRecord.Description;
            spendingFullSum.Value = spendingRecord.Sum;

            if(spendingRecord.Id == 0)
            {
                dateTimePicker1.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0).AddYears(Convert.ToInt32(year.Year) - DateTime.Now.Year);
            }
            else
            {
                dateTimePicker1.Value = spendingRecord.CreationDate;
            }

            dateTimePicker1.MinDate = new DateTime(Convert.ToInt32(year.Year), 1, 1, 0, 0, 0);
            dateTimePicker1.MaxDate = new DateTime(Convert.ToInt32(year.Year), 12, 31, 0, 0, 0);

            switch (spendingRecord.Status)
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

            using (TenderContext tc = new TenderContext())
            {
                sourcesList = tc.MoneySources.OrderBy(p => p.ViewPriority).ToList();
                estimateCBList.DataSource = tc.Estimates
                    .Where(t => (t.TenderYearId == year.Id) && (t.Changes.Sum(p => p.PrimaryKekvSum) > 0))
                    .OrderBy(t => t.Id)
                    .ToList();

                Estimate selectedEstimate = estimateCBList.SelectedItem as Estimate;

                altKekvCBList.DataSource = tc.KekvCodes.OrderBy(p => p.Code).ToList();
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

            for (int i = 0; i < (balanceChangesTable.RowCount - 1); i++)
            {
                balanceChangesTable.Rows[i].Cells[0].Value = 0;
                balanceChangesTable.Rows[i].Cells[1].Value = 0;
                moneysOnSources.Add(0);
            }
            controlValueWasChangedByUser = true;
        }

        private void ToggleLoadingKekvListAnimation()
        {
            mainKekvCBList.Tag = (mainKekvCBList.Tag == null) ? locker : null;
            bool loadingProccess = (mainKekvCBList.Tag != null);

            if(loadingProccess)
            {
                label6.Text = "Завантаження залишків";
            }
            else if(mainKekvCBList.SelectedIndex < 0)
            {
                label6.Text = string.Empty;
            }

            estimateCBList.Enabled = mainKekvCBList.Enabled = altKekvCBList.Enabled = label6.Visible = !loadingProccess;

            estimateCBList.Enabled = mainKekvCBList.Enabled = altKekvCBList.Enabled = (spendingRecord.Id == 0);
        }

        private void ToggleLoadingMoneysAnimation()
        {
            balanceChangesTable.Tag = (balanceChangesTable.Tag == null) ? locker : null;
            bool loadingProccess = (balanceChangesTable.Tag != null);

            newStatusRButton.Enabled = onPayStatusRButton.Enabled = paidStatusRButton.Enabled  = !loadingProccess;

            moneyRemainsloadingPicture.Visible = loadingProccess;
        }

        private void LoadKekvsList()
        {
            if(!updateKekvListWorker.IsBusy && controlValueWasChangedByUser)
            {
                Estimate selectedEstimate = estimateCBList.SelectedItem as Estimate;

                if (selectedEstimate != null)
                {
                    ToggleLoadingKekvListAnimation();
                    updateKekvListWorker.RunWorkerAsync(selectedEstimate);
                }
            }
        }

        class UpdateMoneysWorkerArgument
        {
            public KekvCode Kekv { get; set; }
            public Estimate Estimate { get; set; }
            public DateTime DocumentDate { get; set; }
        }

        private void LoadMoneysTable()
        {
            if (!updateMoneysRemainsWorker.IsBusy && controlValueWasChangedByUser)
            {
                Estimate selectedEstimate = estimateCBList.SelectedItem as Estimate;
                KekvCode selectedMainKekv = mainKekvCBList.SelectedItem as KekvCode;

                if (selectedEstimate != null && selectedMainKekv != null)
                {
                    ToggleLoadingMoneysAnimation();
                    updateMoneysRemainsWorker.RunWorkerAsync(new UpdateMoneysWorkerArgument 
                    { 
                        Estimate = selectedEstimate, 
                        Kekv = selectedMainKekv,
                        DocumentDate = dateTimePicker1.Value
                    });
                }
            }
        }

        private void updateMoneysRemainsWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            UpdateMoneysWorkerArgument arg = e.Argument as UpdateMoneysWorkerArgument;

            using (TenderContext tc = new TenderContext())
            {
                moneysOnSources.Clear();
                DateTime lastDayOfMonth = new DateTime(arg.DocumentDate.Year,
                    arg.DocumentDate.Month,
                    DateTime.DaysInMonth(arg.DocumentDate.Year, arg.DocumentDate.Month));
                foreach (var source in sourcesList)
                {
                    decimal remain = tc.BalanceChanges
                        .Where(p => (p.EstimateId == arg.Estimate.Id)
                            && (p.PrimaryKekvId == arg.Kekv.Id)
                        && (p.DateOfReceiving <= lastDayOfMonth)
                        && (p.MoneySourceId == source.Id)).Select(p => p.PrimaryKekvSum).DefaultIfEmpty(0).Sum();

                    if (spendingRecord.Id > 0)
                    {
                        var rec = tc.BalanceChanges
                            .Where(p => (p.PlannedSpendingId == spendingRecord.Id)
                                && (p.EstimateId == arg.Estimate.Id)
                                && (p.MoneySourceId == source.Id)).FirstOrDefault();

                        if (rec != null)
                        {
                            remain -= rec.PrimaryKekvSum;
                        }
                    }

                    moneysOnSources.Add(remain);
                }
            }
        }

        private void updateMoneysRemainsWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            for (int i = 0; i < sourcesList.Count; i++)
            {
                balanceChangesTable.Rows[i].Cells[1].Value = moneysOnSources[i];
            }

            CorrectMoneysOnSources();
            ToggleLoadingMoneysAnimation();
        }

        private class KekvRemain
        {
            public KekvCode Kekv { get; set; }
            public decimal Remain { get; set; }
        }

        private void updateKekvListWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Estimate arg = e.Argument as Estimate;

            using(TenderContext tc = new TenderContext())
            {
                tc.Estimates.Attach(arg);
                List<KekvCode> mainKekvList = new List<KekvCode>();
                List<KekvRemain> notPlannedEstimateMoney = (from item in arg.Changes.ToList()
                                                            where ((item.PrimaryKekvSum > 0) || (item.PlannedSpendingId != null))
                                                            group item by item.PrimaryKekv into g1
                                                            select new KekvRemain 
                                                            { 
                                                                Kekv = g1.Key, 
                                                                Remain = g1.Sum(p => p.PrimaryKekvSum) 
                                                            }).ToList();
                List<KekvRemain> plannedEstimateMoney = (from item in tc.TenderPlanRecords.ToList()
                                                         where (item.EstimateId == arg.Id)
                                                         group item by item.PrimaryKekv into g1
                                                         select new KekvRemain
                                                         {
                                                             Kekv = g1.Key,
                                                             Remain = g1.Sum(p => p.UsedByRecordSum)
                                                         }).ToList();

                List<KekvRemain> availableMoneyForPlannedSpending = (from rec1 in notPlannedEstimateMoney
                                                                     join rec2 in plannedEstimateMoney on rec1.Kekv equals rec2.Kekv into j1
                                                                     from jr in j1.DefaultIfEmpty(new KekvRemain())
                                                                     select new KekvRemain
                                                                     {
                                                                         Kekv = rec1.Kekv,
                                                                         Remain = rec1.Remain - jr.Remain
                                                                     } into s1
                                                                     orderby s1.Kekv.Code ascending
                                                                     select s1).ToList();

                kekvRemains.Clear();
                foreach (var item in availableMoneyForPlannedSpending)
                {
                    mainKekvList.Add(item.Kekv);
                    kekvRemains.Add(item.Remain);
                }

                e.Result = mainKekvList;
            }
        }

        private void updateKekvListWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            controlValueWasChangedByUser = false;
            mainKekvCBList.DataSource = e.Result as List<KekvCode>;
            controlValueWasChangedByUser = true;

            LoadSelectedMainKekvRemain();
            
            ToggleLoadingKekvListAnimation();
        }

        private void balanceChangesTable_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (!controlValueWasChangedByUser)
            {
                return;
            }

            if (e.ColumnIndex != 0)
            {
                return;
            }

            if (spendingFullSum.Value == 0)
            {
                balanceChangesTable.CancelEdit();
                return;
            }

            decimal cellValue = 0;

            if (!decimal.TryParse(e.FormattedValue.ToString(), out cellValue))
            {
                balanceChangesTable.CancelEdit();
                return;
            }

            // Проверяем запрошеную сумму по источнику
            if (cellValue > moneysOnSources[e.RowIndex])
            {
                NotificationHelper.ShowError("На вказаному джерелі фінансування недостатньо коштів");
                balanceChangesTable.CancelEdit();
                return;
            }

            // Проверяем общую сумму по счёту
            decimal sum = 0;
            for (int i = 0; i < sourcesList.Count; i++)
            {
                if (i == e.RowIndex)
                {
                    continue;
                }
                else
                {
                    sum += Convert.ToDecimal(balanceChangesTable.Rows[i].Cells[0].Value);
                }
            }
            if ((sum + cellValue) > spendingFullSum.Value)
            {
                balanceChangesTable.CancelEdit();
                controlValueWasChangedByUser = false;
                balanceChangesTable.Rows[e.RowIndex].Cells[0].Value = (spendingFullSum.Value - sum);
                controlValueWasChangedByUser = true;
                return;
            }
        }

        private void balanceChangesTable_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
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

        // Корректировка сумм запрашиваемых для оплаты с источников при обновлении максимально доступных средств на источниках
        private void CorrectMoneysOnSources()
        {
            for (int i = 0; i < sourcesList.Count; i++)
            {
                decimal enteredByUserSum = Convert.ToDecimal(balanceChangesTable.Rows[i].Cells[0].Value);
                if (enteredByUserSum > moneysOnSources[i])
                {
                    balanceChangesTable.Rows[i].Cells[0].Value = moneysOnSources[i];
                }
            }
        }

        private void mainKekvCBList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(controlValueWasChangedByUser)
            {
                LoadSelectedMainKekvRemain();
            }
        }

        private void LoadSelectedMainKekvRemain()
        {
            int selectedMainKekvIndex = mainKekvCBList.SelectedIndex;

            if (selectedMainKekvIndex >= 0)
            {
                label6.Text = string.Format("Доступно: {0:N2} грн.", kekvRemains[selectedMainKekvIndex]);
                
                if(spendingRecord.Id == 0)
                {
                    spendingFullSum.Maximum = kekvRemains[selectedMainKekvIndex];
                }
                else
                {
                    spendingFullSum.Maximum = spendingRecord.Sum + kekvRemains[selectedMainKekvIndex];
                }

                LoadMoneysTable();
            }
            else
            {
                label6.Text = string.Empty;
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if(controlValueWasChangedByUser)
            {
                LoadMoneysTable();
            }
        }

        private decimal GetTableTotalSum()
        {
            return Convert.ToDecimal(balanceChangesTable.Rows[sourcesList.Count].Cells[0].Value);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(spendingDescription.Text))
            {
                NotificationHelper.ShowError("Ви не вказали стислий опис витрати");
                return;
            }

            if(estimateCBList.SelectedIndex < 0)
            {
                NotificationHelper.ShowError("Ви не обрали кошторис");
                return;
            }

            if(mainKekvCBList.SelectedIndex < 0)
            {
                NotificationHelper.ShowError("Ви не обрали основний КЕКВ");
                return;
            }
            if (altKekvCBList.SelectedIndex < 0)
            {
                NotificationHelper.ShowError("Ви не обрали старий КЕКВ");
                return;
            }
            if (spendingFullSum.Value <= 0)
            {
                NotificationHelper.ShowError("Сумма витрати повинна бути быльна за нуль");
                return;
            }

            if (!newStatusRButton.Checked && spendingFullSum.Value != GetTableTotalSum())
            {
                NotificationHelper.ShowError("Загальна сума витрати не дорівнює загальній сумі яку потрібно зняти з джерел фінансування");
                return;
            }

            using (TenderContext tc = new TenderContext())
            {
                Estimate selectedEstimate = estimateCBList.SelectedItem as Estimate;
                KekvCode selectedMainKekv = mainKekvCBList.SelectedItem as KekvCode;
                KekvCode selectedAltKekv = altKekvCBList.SelectedItem as KekvCode;

                if (spendingRecord.Id > 0)
                {
                    tc.PlannedSpending.Attach(spendingRecord);
                    tc.Entry<PlannedSpending>(spendingRecord).State = EntityState.Modified;
                }

                spendingRecord.Description = spendingDescription.Text.Trim();
                spendingRecord.CreationDate = dateTimePicker1.Value;
                spendingRecord.EstimateId = selectedEstimate.Id;
                spendingRecord.PrimaryKekvId = selectedMainKekv.Id;
                spendingRecord.SecondaryKekvId = selectedAltKekv.Id;
                spendingRecord.Sum = spendingFullSum.Value;

                // Удаляем старые записи об изменениях в балансе
                foreach (var item in spendingRecord.Changes.ToList())
                {
                    tc.BalanceChanges.Attach(item);
                    tc.Entry<BalanceChanges>(item).State = EntityState.Deleted;
                }
                tc.SaveChanges();

                if (onPayStatusRButton.Checked || paidStatusRButton.Checked)
                {
                    for (int i = 0; i < sourcesList.Count; i++)
                    {
                        decimal balanceChangeOnSource = Convert.ToDecimal(balanceChangesTable.Rows[i].Cells[0].Value);

                        if (balanceChangeOnSource == 0)
                        {
                            continue;
                        }

                        spendingRecord.Changes.Add(new BalanceChanges
                        {
                            DateOfReceiving = spendingRecord.CreationDate,
                            MoneySourceId = sourcesList[i].Id,
                            PrimaryKekvId = spendingRecord.PrimaryKekvId,
                            SecondaryKekvId = spendingRecord.SecondaryKekvId,
                            EstimateId = spendingRecord.EstimateId,
                            PrimaryKekvSum = -balanceChangeOnSource,
                            SecondaryKekvSum = -balanceChangeOnSource,
                        });
                    }

                    spendingRecord.IsPaid = paidStatusRButton.Checked;
                    tc.SaveChanges();
                }

                if (spendingRecord.Id == 0)
                {
                    tc.PlannedSpending.Add(spendingRecord);
                }
                tc.SaveChanges();

                FileManager.UpdateRelatedFilesOfEntity(tc, spendingRecord.RelatedFiles, relatedFiles, deletingFiles);

                NotificationHelper.ShowInfo("Дані успішно збережені!");
                dbWasChanged = true;
                this.Close();
            }
        }

        private void estimateCBList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(controlValueWasChangedByUser)
            {
                LoadKekvsList();
            }
        }

        private void newStatusRButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rButton = sender as RadioButton;
            if(rButton != null)
            {
                if(rButton.Name.Equals("newStatusRButton") && rButton.Checked)
                {
                    balanceChangesTable.Enabled = false;
                }
                else
                {
                    balanceChangesTable.Enabled = true;
                }
            }
        }
    }
}
