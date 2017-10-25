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

namespace ZcrlTender
{
    public partial class InvoiceForm : Form
    {
        private List<MoneySource> sourcesList;

        public InvoiceForm()
        {
            InitializeComponent();

            using (TenderContext tc = new TenderContext())
            {
                sourcesList = tc.MoneySources.ToList();
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
                dataGridView1.Rows.Add(newRow);
            }

            for(int i = 0; i < dataGridView1.ColumnCount; i++)
                dataGridView1.Columns[i].ValueType = typeof(decimal);

            for(int i = 0; i < (dataGridView1.RowCount - 1); i++)
                dataGridView1.Rows[i].Cells[0].Value = 0;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Enabled = radioButton2.Checked;

            if(radioButton2.Checked)
            {
                dataGridView1.BackgroundColor = dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.SystemColors.WindowText;
            }
            else
            {
                dataGridView1.ForeColor = dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.GhostWhite;
            }

            dataGridView1.Refresh();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                decimal currentValue = (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                        ? 0
                        : decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = (currentValue < 0) ? FormStyles.WrongSumColor : FormStyles.RightSumColor;

                if (e.RowIndex != dataGridView1.RowCount - 1)
                {
                    decimal sum = 0;
                    for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                    {
                        sum += (dataGridView1.Rows[i].Cells[e.ColumnIndex].Value == null)
                            ? 0
                            : decimal.Parse(dataGridView1.Rows[i].Cells[e.ColumnIndex].Value.ToString());
                    }

                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[e.ColumnIndex].Value = sum;
                }
            }
        }
    }
}
