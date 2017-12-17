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
    public partial class DkCodesListForm : Form
    {
        private BindingList<DkCode> codesList;
        private bool wasDbUpdated;

        public bool WasDbUpdated
        {
            get
            {
                return wasDbUpdated;
            }
        }

        public DkCodesListForm()
        {
            InitializeComponent();

            if(UserSession.IsAuthorized)
            {
                dkCodesTable.CellDoubleClick += dkCodesTable_CellDoubleClick;
            }
            else
            {
                linkLabel1.Visible = linkLabel2.Visible = false;
            }

            wasDbUpdated = false;

            dkCodesTable.RowsAdded += (sender, e) => DataGridViewHelper.CalculateNewRowNumber(dkCodesTable, 0, e.RowIndex, e.RowCount);

            using(TenderContext tc = new TenderContext())
            {
                dkCodesTable.AutoGenerateColumns = false;
                codesList = new BindingList<DkCode>(tc.DkCodes.ToList());
                dkCodesTable.DataSource = codesList;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AddEditDkCodeForm af = new AddEditDkCodeForm();
            af.ShowDialog();

            if(af.WasDbUpdated)
            {
                using (TenderContext tc = new TenderContext())
                {
                    codesList = new BindingList<DkCode>(tc.DkCodes.ToList());
                    dkCodesTable.DataSource = codesList;
                }
                wasDbUpdated = true;
            }
        }

        private void dkCodesTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DkCode selectedCode = dkCodesTable.SelectedRows[0].DataBoundItem as DkCode;

            using(TenderContext tc = new TenderContext())
            {
                bool codeHasReferences = tc.TenderPlanRecords.Where(p => (p.DkCodeId == selectedCode.Id)).Any();
                if(codeHasReferences)
                {
                    NotificationHelper.ShowError("Неможливо видалити код, оскільки під нього існують записи у річних планах закупівель");
                    return;
                }
                else
                {
                    tc.DkCodes.Attach(selectedCode);
                    tc.DkCodes.Remove(selectedCode);
                    tc.SaveChanges();

                    codesList.Remove(selectedCode);
                    NotificationHelper.ShowInfo("Код успішно видалений!");
                    wasDbUpdated = true;
                }
            }
        }

        private void dkCodesTable_SelectionChanged(object sender, EventArgs e)
        {
            linkLabel2.Enabled = (dkCodesTable.SelectedRows.Count > 0);
        }
    }
}
