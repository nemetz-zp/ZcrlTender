using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TenderLibrary;

namespace ZcrlTender
{
    public partial class ReserveMoneyOnSourceControl : UserControl
    {
        private object locker;

        private List<MoneySource> sourcesList;
        private List<decimal> moneysOnSources;

        private bool controlValueWasChangedByUser;

        private DateTime currentDate;
        private TenderPlanRecord record;
        private Contract contract;
        private decimal fullSum;

        public decimal FullSum
        {
            get
            {
                return fullSum;
            }
            set
            {
                fullSum = value;
            }
        }

        public DateTime CurrentDate
        {
            get
            {
                return currentDate;
            }
            set
            {
                this.currentDate = value;
            }
        }

        public ReserveMoneyOnSourceControl(TenderPlanRecord record)
        {
            InitializeComponent();
            currentDate = new DateTime(Convert.ToInt32(record.Estimate.Year.Year), 12, 31);
            ConfigureTable();
            LoadMoneyRemains();
        }

        public ReserveMoneyOnSourceControl(Contract contract)
        {
            InitializeComponent();
            currentDate = new DateTime(Convert.ToInt32(contract.RecordInPlan.Estimate.Year.Year), 12, 31);
            ConfigureTable();
            LoadMoneyRemains();
        }

        private void ConfigureTable()
        {
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

        public void SetMoneySourceValue(int index, decimal val)
        {
            controlValueWasChangedByUser = false;
            balanceChangesTable.Rows[index].Cells[0].Value = val;
            controlValueWasChangedByUser = true;
        }

        // Загрузка остатков средств на дату регистрации счёта
        private void LoadMoneyRemains()
        {
            if (!updateMoneyRemainWorker.IsBusy && fullSum > 0)
            {
                updateMoneyRemainWorker.RunWorkerAsync();
                ToggleMoneyRemainLoadingAnimation();
            }
        }

        private void ToggleMoneyRemainLoadingAnimation()
        {
            balanceChangesTable.Tag = (balanceChangesTable.Tag == null) ? locker : null;
            bool isLoadingProcess = (balanceChangesTable.Tag != null);

            moneyRemainsLoadingPicture.Visible = isLoadingProcess;
        }

        // Автосуммирование введённых сумм по источникам финансирования
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
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

        private void updateMoneyRemainsWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            using (TenderContext tc = new TenderContext())
            {
                moneysOnSources.Clear();
                foreach (var source in sourcesList)
                {
                    decimal remain = tc.BalanceChanges
                        .Where(p => (p.EstimateId == record.EstimateId)
                            && (p.PrimaryKekvId == record.PrimaryKekvId)
                        && (p.DateOfReceiving <= currentDate)
                        && (p.MoneySourceId == source.Id)).Select(p => p.PrimaryKekvSum).DefaultIfEmpty(0).Sum();

                    if (contract != null)
                    {
                        var rec = contract.Guaranties.Where(p => p.MoneySourceId == source.Id).FirstOrDefault();

                        if (rec != null)
                        {
                            remain += rec.ReservedSum;
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
            for (int i = 0; i < sourcesList.Count; i++)
            {
                balanceChangesTable.Rows[i].Cells[1].Value = moneysOnSources[i];
            }

            CorrectMoneysOnSources();
            ToggleMoneyRemainLoadingAnimation();
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (!controlValueWasChangedByUser)
            {
                return;
            }

            if (e.ColumnIndex != 0)
            {
                return;
            }
            if (fullSum == 0)
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
            if ((sum + cellValue) > fullSum)
            {
                balanceChangesTable.CancelEdit();
                controlValueWasChangedByUser = false;
                balanceChangesTable.Rows[e.RowIndex].Cells[0].Value = (fullSum - sum);
                controlValueWasChangedByUser = true;
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
    }
}
