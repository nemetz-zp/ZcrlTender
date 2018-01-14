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
    public partial class TenderPlanRecordPicker : UserControl
    {
        private TenderPlanRecord selectedRecord;

        public TenderPlanRecord SelectedRecord 
        { 
            get
            {
                return selectedRecord;
            }
            set
            {
                selectedRecord = value;
                if (selectedRecord != null)
                {
                    textBox1.Text = value.Dk.FullName;
                }
            }
        }

        public event EventHandler RecordChanged;

        private void OnRecordChange(EventArgs e)
        {
            if(RecordChanged != null)
            {
                RecordChanged.Invoke(this, e);
            }
        }

        public TenderPlanRecordPicker()
        {
            InitializeComponent();
            //this.Height = textBox1.Height;
        }

        private void textBox1_Click_1(object sender, EventArgs e)
        {
            TenderPlanRecordSelectForm ts = new TenderPlanRecordSelectForm();
            ts.ShowDialog();

            if (ts.SelectedRecord != null)
            {
                SelectedRecord = ts.SelectedRecord;
                textBox1.Text = ts.SelectedRecord.Dk.FullName;
                OnRecordChange(null);
            }
        }
    }
}
