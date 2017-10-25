using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZcrlTender
{
    public class DataGridViewHelper
    {
        // Нумерация строк таблицы
        public static void CalculateNewRowNumber(DataGridView table, int rowNumberColumn, int beginRowIndex, int rowCount)
        {
            for (int i = 0; i < rowCount; i++)
            {
                table.Rows[beginRowIndex + i].Cells[rowNumberColumn].Value = (beginRowIndex + i + 1);
            }
        }

        public static void DrawMoneyTotalsTableSchema<R, C>(DataGridView table, 
            IList<R> rows,
            IList<C> columns,
            Func<R, object> rowPropertyName,
            Func<C, object> columnPropertyName)
        {
            DrawTotalsTableSchema<R, C>(table, rows, columns, 
                rowPropertyName, columnPropertyName, typeof(decimal), 
                FormStyles.MoneyTotalsFont, "ВСЬОГО", "ВСЬОГО", "N2");
        }

        // Рисуем схему таблицы
        public static void DrawTotalsTableSchema<R, C>(DataGridView table, 
            IList<R> rows,
            IList<C> columns,
            Func<R, object> rowPropertyName,
            Func<C, object> columnPropertyName,
            Type columnsType,
            System.Drawing.Font totalsFont,
            string rowTotalsHeaderText,
            string colTotalsHeaderText,
            string columnFormat)
        {
            table.Rows.Clear();
            table.Columns.Clear();

            int rowsNum = rows.Count();
            int colNum = columns.Count();

            for (int i = 0; i <= colNum; i++)
            {
                DataGridViewColumn newCol = new DataGridViewTextBoxColumn();
                newCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                newCol.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                newCol.ValueType = columnsType;
                newCol.DefaultCellStyle.Format = columnFormat;
                if (i < colNum)
                {
                    newCol.HeaderText = columnPropertyName.Invoke(columns[i]).ToString();
                }
                else
                {
                    newCol.HeaderText = colTotalsHeaderText;
                    newCol.DefaultCellStyle.Font = totalsFont;
                    newCol.ReadOnly = true;
                }
                table.Columns.Add(newCol);
            }

            for (int i = 0; i <= rowsNum; i++)
            {
                DataGridViewRow newRow = new DataGridViewRow();
                newRow.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                newRow.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                if (i < rowsNum)
                {
                    newRow.HeaderCell.Value = rowPropertyName.Invoke(rows[i]).ToString();
                }
                else
                {
                    newRow.HeaderCell.Value = rowTotalsHeaderText;
                    newRow.DefaultCellStyle.Font = totalsFont;
                    newRow.ReadOnly = true;
                }
                table.Rows.Add(newRow);
            }
        }

        // Пересчёт номеров строк при удалении строки таблицы
        public static void RecalculateRowNumberColumn(DataGridView table, int rowNumberColumn, int deletedRowIndex)
        {
            for(int i = deletedRowIndex; i < table.RowCount; i++)
            {
                table.Rows[i].Cells[rowNumberColumn].Value = (i + 1).ToString();
            }
        }

        // Метод валидации ячейки с суммой
        public static void MoneyCellValidating(DataGridView table, string value)
        {
            decimal result = 0;

            if (!decimal.TryParse(value, out result))
            {
                table.CancelEdit();
                return;
            }
        }

        public static void RecalculateMoneyTotals(DataGridView table, int changedCellRowIndex, int changedCellColumnIndex)
        {
            int lastColumnIndex = table.ColumnCount - 1;
            int lastRowIndex = table.RowCount - 1;

            if ((changedCellColumnIndex >= 0 && changedCellColumnIndex < lastColumnIndex) && (changedCellRowIndex >= 0 && changedCellRowIndex < lastRowIndex))
            {
                decimal currentValue = (table.Rows[changedCellRowIndex].Cells[changedCellColumnIndex].Value == null)
                        ? 0
                        : decimal.Parse(table.Rows[changedCellRowIndex].Cells[changedCellColumnIndex].Value.ToString());
                table.Rows[changedCellRowIndex].Cells[changedCellColumnIndex].Style.ForeColor = (currentValue < 0) ? FormStyles.WrongSumColor : FormStyles.RightSumColor;

                decimal rowsSum = 0;
                decimal columnsSum = 0;
                decimal totalSum = 0;

                for (int i = 0; i < lastColumnIndex; i++)
                {
                    decimal tabValue = (table.Rows[changedCellRowIndex].Cells[i].Value == null)
                        ? 0
                        : decimal.Parse(table.Rows[changedCellRowIndex].Cells[i].Value.ToString());
                    columnsSum += tabValue;
                }
                for (int i = 0; i < lastRowIndex; i++)
                {
                    decimal tabValue = (table.Rows[i].Cells[changedCellColumnIndex].Value == null)
                        ? 0
                        : decimal.Parse(table.Rows[i].Cells[changedCellColumnIndex].Value.ToString());
                    rowsSum += tabValue;
                }

                table.Rows[changedCellRowIndex].Cells[lastColumnIndex].Value = columnsSum;
                table.Rows[changedCellRowIndex].Cells[lastColumnIndex].Style.ForeColor = (columnsSum < 0) ? FormStyles.WrongSumColor : FormStyles.RightSumColor;
                table.Rows[lastRowIndex].Cells[changedCellColumnIndex].Value = rowsSum;
                table.Rows[lastRowIndex].Cells[changedCellColumnIndex].Style.ForeColor = (rowsSum < 0) ? FormStyles.WrongSumColor : FormStyles.RightSumColor;

                for (int i = 0; i < lastRowIndex; i++)
                {
                    decimal tabValue = (table.Rows[i].Cells[lastColumnIndex].Value == null)
                        ? 0
                        : decimal.Parse(table.Rows[i].Cells[lastColumnIndex].Value.ToString());
                    totalSum += tabValue;
                }
                table.Rows[lastRowIndex].Cells[lastColumnIndex].Value = totalSum;
                table.Rows[lastRowIndex].Cells[lastColumnIndex].Style.ForeColor = (totalSum < 0) ? FormStyles.WrongSumColor : FormStyles.RightSumColor;
            }
        }
    }
}
