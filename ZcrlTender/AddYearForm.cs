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
    public partial class AddYearForm : Form
    {
        private bool yearWasAdded;
        public bool YearWasAdded
        {
            get
            {
                return yearWasAdded;
            }
        }

        public AddYearForm()
        {
            InitializeComponent();
            yearWasAdded = false;
        }

        private void addYearButton_Click(object sender, EventArgs e)
        {
            using(TenderContext tc = new TenderContext())
            {
                tc.TenderYears.Add(new TenderYear 
                { 
                    Year = Convert.ToInt16(yearValue.Value), Description = yearDescription.Text 
                });
                tc.SaveChanges();
                yearWasAdded = true;

                MyHelper.ShowInfo("Рік успішно додано!");
                this.Close();
            }
        }
    }
}
