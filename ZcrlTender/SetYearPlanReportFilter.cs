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
    public partial class SetYearPlanReportFilter : Form
    {
        private bool filteDataWasSelected;
        public bool FilteDataWasSelected
        {
            get
            {
                return filteDataWasSelected;
            }
        }

        private Estimate selectedEstimate;
        private KekvCode selectedKekv;
        private bool isNewSystemSelected;

        public Estimate SelectedEstimate
        {
            get
            {
                return selectedEstimate;
            }
        }

        public KekvCode SelectedKekv
        {
            get
            {
                return selectedKekv;
            }
        }

        public bool IsNewSystemSelected
        {
            get
            {
                return isNewSystemSelected;
            }
        }
        public SetYearPlanReportFilter()
        {
            filteDataWasSelected = false;
            InitializeComponent();

            using(TenderContext tc = new TenderContext())
            {
                string allString = "- ВСІ -";
                List<Estimate> estList = tc.Estimates.ToList();
                estList.Insert(0, new Estimate { Id = -1, Name = allString });
                comboBox1.DataSource = estList;
                comboBox1.DisplayMember = "Name";
                comboBox1.ValueMember = "Id";

                List<KekvCode> kekvList = tc.KekvCodes.OrderBy(p => p.Code).ToList();
                kekvList.Insert(0, new KekvCode { Id = -1, Code = allString });
                comboBox2.DataSource = kekvList;
                comboBox2.DisplayMember = "Code";
                comboBox2.ValueMember = "Id";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            selectedEstimate = comboBox1.SelectedItem as Estimate;
            selectedKekv = comboBox2.SelectedItem as KekvCode;
            isNewSystemSelected = radioButton1.Checked;
            filteDataWasSelected = true;
            this.Close();
        }
    }
}
