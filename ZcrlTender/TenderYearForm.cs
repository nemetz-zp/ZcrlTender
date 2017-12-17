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
    public partial class TenderYearForm : Form
    {
        private TenderYear selectedYear;
        public TenderYear SelectedYear 
        { 
            get 
            {
                return selectedYear;
            } 
        }

        public TenderYearForm()
        {
            InitializeComponent();
            ReloadListOfYears();

            addYearButton.Visible = UserSession.IsAuthorized;
        }

        private void ReloadListOfYears()
        {
            using (TenderContext tc = new TenderContext())
            {
                List<TenderYear> years = tc.TenderYears.ToList();
                listBox1.DataSource = years;
                listBox1.DisplayMember = "FullName";
                listBox1.ValueMember = "Id";
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectYearButton.Enabled = listBox1.SelectedItem != null;
        }

        private void addYearButton_Click(object sender, EventArgs e)
        {
            AddYearForm af = new AddYearForm();
            af.ShowDialog();
            if(af.YearWasAdded)
            {
                ReloadListOfYears();
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            ChooseYear();
        }

        private void selectYearButton_Click(object sender, EventArgs e)
        {
            ChooseYear();
        }

        private void ChooseYear()
        {
            selectedYear = listBox1.SelectedItem as TenderYear;
            this.Close();
        }
    }
}
