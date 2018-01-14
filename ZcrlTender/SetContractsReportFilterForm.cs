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
    public partial class SetContractsReportFilterForm : Form
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
        private Contractor selectedContractor;
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

        public Contractor SelectedContractor
        {
            get
            {
                return selectedContractor;
            }
        }

        public bool IsNewSystemSelected
        {
            get
            {
                return isNewSystemSelected;
            }
        }

        public SetContractsReportFilterForm()
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

                List<KekvCode> kekvList = tc.KekvCodes.ToList();
                kekvList.Insert(0, new KekvCode { Id = -1, Code = allString });
                comboBox2.DataSource = kekvList;
                comboBox2.DisplayMember = "Code";
                comboBox2.ValueMember = "Id";

                List<Contractor> contractorsList = tc.Contractors.ToList();
                contractorsList.Insert(0, new Contractor { Id = -1, ShortName = allString });
                comboBox3.DataSource = contractorsList;
                comboBox3.DisplayMember = "ShortName";
                comboBox3.ValueMember = "Id";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            selectedEstimate = comboBox1.SelectedItem as Estimate;
            selectedKekv = comboBox2.SelectedItem as KekvCode;
            selectedContractor = comboBox3.SelectedItem as Contractor;
            isNewSystemSelected = radioButton1.Checked;
            filteDataWasSelected = true;
            Close();
        }
    }
}
