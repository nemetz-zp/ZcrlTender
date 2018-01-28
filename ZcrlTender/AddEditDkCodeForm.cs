using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using TenderLibrary;

namespace ZcrlTender
{
    public partial class AddEditDkCodeForm : Form
    {
        private DkCode codeRecord;
        private bool wasDbUpdated;

        public DkCode CodeRecord
        {
            get
            {
                return codeRecord;
            }
        }

        public bool WasDbUpdated
        {
            get
            {
                return wasDbUpdated;
            }
        }

        public AddEditDkCodeForm()
        {
            wasDbUpdated = false;
            InitializeComponent();
        }

        public AddEditDkCodeForm(DkCode code)
        {
            wasDbUpdated = false;
            InitializeComponent();
            codeRecord = code;
            codeTextBox.Text = codeRecord.Code;
            nameTextBox.Text = codeRecord.Name;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(codeTextBox.Text))
            {
                NotificationHelper.ShowError("Ви не зазначили код");
                return;
            }
            if(!Regex.IsMatch(codeTextBox.Text.Trim(), "[0-9]{8}-[0-9]"))
            {
                NotificationHelper.ShowError("Неправильний формат коду. Код повинен відповідати формату ХХХХХХХХ-Х. Де Х - число від 0 до 9");
                return;
            }
            if (string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                NotificationHelper.ShowError("Ви не зазначили назву для кода");
                return;
            }

            using(TenderContext tc = new TenderContext())
            {
                if(codeRecord == null)
                {
                    codeRecord = new DkCode();
                }
                else
                {
                    tc.DkCodes.Attach(codeRecord);
                }

                bool codeExist = tc.DkCodes.Where(p => p.Code.Equals(codeTextBox.Text.Trim())).Any();
                if(codeExist && (codeRecord.Id == 0))
                {
                    NotificationHelper.ShowError("Такий код вже існує");
                    return;
                }

                codeRecord.Code = codeTextBox.Text.Trim();
                codeRecord.Name = nameTextBox.Text.Trim();

                if(codeRecord.Id == 0)
                {
                    tc.DkCodes.Add(codeRecord);
                }
                else
                {
                    tc.Entry<DkCode>(codeRecord).State = System.Data.Entity.EntityState.Modified;
                }

                tc.SaveChanges();
                wasDbUpdated = true;
                this.Close();
            }
        }
    }
}
