using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZcrlTender.ViewModels;
using TenderLibrary;

namespace ZcrlTender
{
    // Форма добавления/изменения сметы
    public partial class EstimateForm : Form
    {
        private Estimate currentEstimate;

        private bool tableDataWasChangedByUser;

        private List<KekvCode> kekvsList;
        private BindingList<MoneySource> sourcesList;
        private string[] monthes;

        private List<EstimateCellRecord> estimatesRecords;

        public EstimateForm(TenderYear year)
        {
            InitializeComponent();

            currentEstimate = new Estimate();
            currentEstimate.Year = year;
            currentEstimate.TenderYearId = year.Id;

            estimatesRecords = new List<EstimateCellRecord>();

            monthes = new string[] 
            { 
                "Січень",   "Лютий",    "Березень", 
                "Квітень",  "Травень",  "Червень", 
                "Липень",   "Серпень",  "Вересень", 
                "Жовтень",  "Листопад", "Грудень" 
            };

            using(TenderContext tc = new TenderContext())
            {
                kekvsList = tc.KekvCodes.ToList();
                sourcesList = new BindingList<MoneySource>(tc.MoneySources.ToList());
            }

            moneySourceCBList.DataSource = sourcesList;
            moneySourceCBList.DisplayMember = "Name";
            moneySourceCBList.ValueMember = "Id";

            DataGridViewHelper.DrawMoneyTotalsTableSchema<KekvCode, string>(estimateTotalsTable, 
                kekvsList, monthes.ToList(), t => t.Code, t => t);

            tableDataWasChangedByUser = false;
            for(int i = 0; i < estimateTotalsTable.RowCount; i++)
            {
                for(int k = 0; k < estimateTotalsTable.ColumnCount; k++)
                {
                    estimateTotalsTable.Rows[i].Cells[k].Value = 0.00;
                }
            }
            tableDataWasChangedByUser = true;
        }

        public EstimateForm(Estimate est) : this(est.Year)
        {
            currentEstimate = est;
            estimateNameTextBox.Text = currentEstimate.Name;

            using(TenderContext tc = new TenderContext())
            {
                var allEstimatesRecords = (from item in tc.BalanceChanges
                                           where (item.EstimateId == currentEstimate.Id) && 
                                                 (item.PlannedSpending == null && item.Invoice == null)
                                           group item by item.MoneySource).ToList();
                
                foreach(var rec in allEstimatesRecords)
                {
                    EstimateCellRecord record = new EstimateCellRecord();
                    record.Source = rec.Key;
                    record.PrimarySumValues = new decimal[kekvsList.Count, monthes.Length];
                    record.SecondarySumValues = new decimal[kekvsList.Count, monthes.Length];
                    record.PrimarySumValues = new decimal[kekvsList.Count, monthes.Length];
                    record.SecondarySumValues = new decimal[kekvsList.Count, monthes.Length];
                    
                    foreach(var item in rec)
                    {
                        int rowIndex = kekvsList.IndexOf(item.PrimaryKekv);
                        int columnIndex = item.DateOfReceiving.Month - 1;
                        record.PrimarySumValues[rowIndex, columnIndex] = item.PrimaryKekvSum;
                        record.SecondarySumValues[rowIndex, columnIndex] = item.SecondaryKekvSum;
                    }

                    CreateMoneySourceTab(record);
                }
            }
        }

        private void SaveDataToDb()
        {
            using(TenderContext tc = new TenderContext())
            {
                currentEstimate.Name = estimateNameTextBox.Text.Trim();
                if(currentEstimate.Id > 0)
                {
                    tc.Estimates.Attach(currentEstimate);
                    
                    // Удаляем старые записи сметы
                    var oldBalanceChanges = tc.BalanceChanges.Where(p => (p.EstimateId == currentEstimate.Id && p.PrimaryKekvSum >= 0 && p.SecondaryKekvSum >= 0));
                    tc.BalanceChanges.RemoveRange(oldBalanceChanges);

                    tc.SaveChanges();
                }
                else
                {
                    tc.TenderYears.Attach(currentEstimate.Year);
                    tc.Estimates.Add(currentEstimate);
                    tc.SaveChanges();
                }

                for(int i = 0; i < estimatesRecords.Count; i++)
                {
                    for(int j = 0; j < kekvsList.Count; j++)
                    {
                        for(int k = 0; k < monthes.Length; k++)
                        {
                            BalanceChanges incommingMoney = new BalanceChanges();
                            incommingMoney.DateOfReceiving = new DateTime((int)currentEstimate.Year.Year, (k+1), 1);
                            incommingMoney.EstimateId = currentEstimate.Id;
                            incommingMoney.MoneySourceId = estimatesRecords[i].Source.Id;
                            incommingMoney.PrimaryKekvId = incommingMoney.SecondaryKekvId = kekvsList[j].Id;
                            incommingMoney.PrimaryKekvSum = estimatesRecords[i].PrimarySumValues[j, k];
                            incommingMoney.SecondaryKekvSum = estimatesRecords[i].SecondarySumValues[j, k];
                            tc.BalanceChanges.Add(incommingMoney);
                        }
                    }
                }

                tc.SaveChanges();
            }
        }

        private DataGridView CopyDataGridView(DataGridView dgv_orn)
        {
            DataGridView dgv_copy = new DataGridView();
            dgv_copy.Dock = dgv_orn.Dock;
            dgv_copy.RowHeadersWidth = dgv_orn.RowHeadersWidth;
            dgv_copy.AllowUserToAddRows = dgv_orn.AllowUserToAddRows;
            dgv_copy.AllowUserToDeleteRows = dgv_orn.AllowUserToDeleteRows;
            dgv_copy.AllowUserToOrderColumns = dgv_orn.AllowUserToOrderColumns;
            dgv_copy.AllowUserToResizeRows = dgv_orn.AllowUserToResizeRows;
            dgv_copy.AllowUserToResizeColumns = dgv_orn.AllowUserToResizeColumns;
            dgv_copy.RowHeadersWidthSizeMode = dgv_orn.RowHeadersWidthSizeMode;
            dgv_copy.SelectionMode = dgv_orn.SelectionMode;
            dgv_copy.MultiSelect = dgv_orn.MultiSelect;
            dgv_copy.RowHeadersDefaultCellStyle = dgv_orn.RowHeadersDefaultCellStyle;
            dgv_copy.ColumnHeadersDefaultCellStyle = dgv_orn.ColumnHeadersDefaultCellStyle;
            dgv_copy.ColumnHeadersHeightSizeMode = dgv_orn.ColumnHeadersHeightSizeMode;
            dgv_copy.CellValueChanged += estimateTotalsTable_CellValueChanged;
            dgv_copy.CellValidating += estimateTotalsTable_CellValidating;

            for(int i = 0; i < dgv_orn.ColumnCount; i++)
            {
                DataGridViewColumn col = dgv_orn.Columns[i].Clone() as DataGridViewColumn;
                col.ValueType = typeof(decimal);
                col.DefaultCellStyle = dgv_orn.Columns[i].DefaultCellStyle;
                dgv_copy.Columns.Add(col);
            }

            for (int i = 0; i < dgv_orn.RowCount; i++ )
            {
                dgv_copy.Rows.Add(dgv_orn.Rows[i].Clone() as DataGridViewRow);
            }

            return dgv_copy;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            deleteMoneySourceButton.Enabled = tabControl1.SelectedIndex > 0;
        }

        private void RecalculateTotals()
        {
            for (int j = 0; j < estimateTotalsTable.RowCount; j++ )
            {
                for(int k = 0; k < estimateTotalsTable.ColumnCount; k++)
                {
                    decimal cellSum = 0;
                    for (int i = 1; i < tabControl1.TabCount; i++)
                    {
                        DataGridView dgv = tabControl1.TabPages[i].Controls.OfType<DataGridView>().First();
                        cellSum += (dgv.Rows[j].Cells[k].Value == null)
                                    ? 0
                                    : decimal.Parse(dgv.Rows[j].Cells[k].Value.ToString());
                    }
                    estimateTotalsTable.Rows[j].Cells[k].Value = cellSum;
                }
            }
        }

        private void addMoneySourceButton_Click(object sender, EventArgs e)
        {
            // Получаем выбранный источник финансирования
            MoneySource selectedItem = moneySourceCBList.SelectedItem as MoneySource;

            // Создаём вкладку под выбранный источник
            CreateMoneySourceTab(new EstimateCellRecord 
            { 
                Source = selectedItem,
                PrimarySumValues = new decimal[kekvsList.Count, monthes.Length],
                SecondarySumValues = new decimal[kekvsList.Count, monthes.Length]
            });
        }

        private void CreateMoneySourceTab(EstimateCellRecord source)
        {
            // Создаём новую вкладку под источник с тем же форматированием, что и первой вкладки
            TabPage newTab = new TabPage();
            newTab.Text = source.Source.Name;
            newTab.Padding = tabControl1.TabPages[0].Padding;

            DataGridView newDGV = CopyDataGridView(estimateTotalsTable);
            newTab.Controls.Add(newDGV);

            tableDataWasChangedByUser = false;
            for (int i = 0; i < kekvsList.Count; i++)
            {
                for (int k = 0; k < monthes.Length; k++)
                {
                    if (newSystemRButton.Checked)
                    {
                        newDGV.Rows[i].Cells[k].Value = source.PrimarySumValues[i, k];
                    }
                    else
                    {
                        newDGV.Rows[i].Cells[k].Value = source.SecondarySumValues[i, k];
                    }
                }
            }
            tableDataWasChangedByUser = true;

            tabControl1.TabPages.Add(newTab);

            estimatesRecords.Add(source);
            sourcesList.Remove(source.Source);
            CheckPossibilityToAddMoneySources();
        }

        private void CheckPossibilityToAddMoneySources()
        {
            addMoneySourceButton.Enabled = moneySourceCBList.Enabled = (sourcesList.Count > 0);
            saveEstimateDataButton.Enabled = (estimatesRecords.Count > 0);
        }

        private void deleteMoneySourceButton_Click(object sender, EventArgs e)
        {
            int selectedTabIndex = tabControl1.SelectedIndex;
            sourcesList.Add(estimatesRecords[selectedTabIndex - 1].Source);
            estimatesRecords.RemoveAt(selectedTabIndex - 1);
            tabControl1.TabPages.RemoveAt(selectedTabIndex);
            CheckPossibilityToAddMoneySources();
            RecalculateTotals();
        }

        private void estimateTotalsTable_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView changedTable = sender as DataGridView;
            DataGridViewHelper.RecalculateMoneyTotals(changedTable, e.RowIndex, e.ColumnIndex);

            // Если изменения не в итоговой таблице ...
            if((changedTable.Name != "estimateTotalsTable") && (e.RowIndex < kekvsList.Count && e.ColumnIndex < monthes.Length))
            {
                // ... вычисляем новую сумму и записываем в итоговую таблицу
                decimal cellSum = 0;
                for(int i = 1; i < tabControl1.TabCount; i++)
                {
                    DataGridView dgv = tabControl1.TabPages[i].Controls.OfType<DataGridView>().First();
                    cellSum += (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null) 
                        ? 0 
                        : decimal.Parse(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                }
                estimateTotalsTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = cellSum;

                // ... и сохраняем их
                if (tableDataWasChangedByUser)
                {
                    if (newSystemRButton.Checked)
                    {
                        estimatesRecords[tabControl1.SelectedIndex - 1].PrimarySumValues[e.RowIndex, e.ColumnIndex]
                            = decimal.Parse(changedTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                    }
                    else
                    {
                        estimatesRecords[tabControl1.SelectedIndex - 1].SecondarySumValues[e.RowIndex, e.ColumnIndex]
                            = decimal.Parse(changedTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                    }
                }
            }
        }

        private void estimateTotalsTable_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            DataGridViewHelper.MoneyCellValidating(sender as DataGridView, e.FormattedValue.ToString());
        }

        private void newSystemRButton_CheckedChanged(object sender, EventArgs e)
        {
            for(int i = 1; i < tabControl1.TabCount; i++)
            {
                DataGridView dgv = tabControl1.TabPages[i].Controls.OfType<DataGridView>().First();
                for(int j = 0; j < kekvsList.Count; j++)
                {
                    for(int k = 0; k < monthes.Length; k++)
                    {
                        tableDataWasChangedByUser = false;
                        if (newSystemRButton.Checked)
                        {
                            dgv.Rows[j].Cells[k].Value = estimatesRecords[i - 1].PrimarySumValues[j, k];
                        }
                        else
                        {
                            dgv.Rows[j].Cells[k].Value = estimatesRecords[i - 1].SecondarySumValues[j, k];
                        }
                        tableDataWasChangedByUser = true;
                    }
                }
            }
        }

        private void saveEstimateDataButton_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(estimateNameTextBox.Text))
            {
                MyHelper.ShowError("Ви не вказали назву кошторису!");
                return;
            }


            WaitingForm wf = new WaitingForm("Іде збеження даних до бази ...", SaveDataToDb);
            wf.ShowDialog();

            if(!string.IsNullOrWhiteSpace(wf.Error))
            {
                MyHelper.ShowError("Помилка! Деталі: " + wf.Error);
                return;
            }
            else
            {
                MyHelper.ShowInfo("Дані по кошторису успішно оновлені в базі даних!");
                Close();
            }
        }
    }
}
