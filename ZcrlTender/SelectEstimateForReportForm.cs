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
    public partial class SelectEstimateForReportForm : Form
    {
        private Estimate selectedEstimate;
        private bool isNewSystemSelected;

        public Estimate SelectedEstimate
        {
            get
            {
                return selectedEstimate;
            }
        }

        public bool IsNewSystemSelected
        {
            get
            {
                return isNewSystemSelected;
            }
        }

        public SelectEstimateForReportForm()
        {
            InitializeComponent();

            using (TenderContext tc = new TenderContext())
            {
                List<Estimate> estimates = tc.Estimates.Include(p => p.Year).ToList();
                estimates.Insert(0, new Estimate { Id = -1, Name = "- ВСІ КОШТОРИСИ ЗА РІК -" });
                comboBox1.DisplayMember = "Name";
                comboBox1.DataSource = estimates;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            selectedEstimate =  comboBox1.SelectedItem as Estimate;
            isNewSystemSelected = radioButton1.Checked;
            this.Close();
        }
    }
}
