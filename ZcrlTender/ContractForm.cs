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
    public partial class ContractForm : Form
    {
        private Contract currentContract;
        private TenderYear year;
        private BindingList<Estimate> estimatesList;
        private BindingList<UploadedFile> relatedFiles;
        private BindingList<Invoice> invoicesList;
        private BindingList<KekvCode> mainKekvList;
        private BindingList<KekvCode> altKekvList;
        private BindingList<DkCode> dkCodesList;

        private List<TabPage> hiddenPagesList;

        public ContractForm(TenderYear year)
        {
            this.year = year;
            InitializeComponent();
            filesTable.AutoGenerateColumns = contractChangesTable.AutoGenerateColumns = contractInvoicesTable.AutoGenerateColumns = false;

            hiddenPagesList = new List<TabPage>();
            while(tabControl1.TabCount > 1)
            {
                hiddenPagesList.Add(tabControl1.TabPages[1]);
                tabControl1.TabPages.RemoveAt(1);
            }

            relatedFiles = new BindingList<UploadedFile>();
            filesTable.DataSource = relatedFiles;

            using(TenderContext tc = new TenderContext())
            {
                mainKekvList = new BindingList<KekvCode>(
                            (from item in tc.TenderPlanRecords
                            group item by item.PrimaryKekv into g1
                            select new { Kekv = g1.Key, Sum = g1.Sum(t => t.Sum) } into g2
                            where g2.Sum > 0
                            select g2.Kekv).ToList()
                            );
                altKekvList = new BindingList<KekvCode>(tc.KekvCodes.ToList());

                estimatesList = new BindingList<Estimate>(tc.Estimates.Where(t => (t.TenderYearId == year.Id)).ToList());

                mainKekv.DataSource = mainKekvList;
                altKekv.DataSource = altKekvList;
                mainKekv.ValueMember = altKekv.ValueMember = "Id";
                mainKekv.DisplayMember = altKekv.DisplayMember = "Code";
            }

            dateOfSigning.Value = dateOfStart.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            dateOfEnd.Value = new DateTime(DateTime.Now.Year, 12, 31);

            // Показываем остатки по КЕКВ
            ShowMainKekvRemain();

            // Показываем остатки по выбранному коду
            UpdateDkList();
        }

        private void UpdateDkList()
        {
            KekvCode selectedMainKekv = mainKekv.SelectedItem as KekvCode;

            if (selectedMainKekv != null)
            {
                using (TenderContext tc = new TenderContext())
                {
                    dkCodesList = new BindingList<DkCode>(
                                    (from item in tc.TenderPlanRecords
                                     where item.PrimaryKekv.Id == selectedMainKekv.Id
                                     group item by item.Dk into g1
                                     select new { Dk = g1.Key, Sum = g1.Sum(t => t.Sum) } into g2
                                     where g2.Sum > 0
                                     select g2.Dk).ToList()
                                    );
                    ShowDkRemain(); 
                }
            }
        }

        private void ShowMainKekvRemain()
        {
            KekvCode selectedMainKekv = mainKekv.SelectedItem as KekvCode;
            decimal moneyRemain = 0;
            if(selectedMainKekv != null)
            {
                using(TenderContext tc = new TenderContext())
                {

                }
            }
        }

        private void ShowDkRemain()
        {

        }

        public ContractForm(TenderYear year, Contract contractToEdit) : this(year)
        {
            for (int i = 0; i < hiddenPagesList.Count; i++)
            {
                tabControl1.TabPages.Add(hiddenPagesList[i]);
            }

            number.Text = contractToEdit.Number;
            description.Text = contractToEdit.Description;
            dateOfSigning.Value = contractToEdit.SignDate;
            dateOfStart.Value = contractToEdit.BeginDate;
            dateOfEnd.Value = contractToEdit.EndDate;

            using (TenderContext tc = new TenderContext())
            {
                relatedFiles = new BindingList<UploadedFile>(tc.UploadedFiles.ToList());
                filesTable.DataSource = relatedFiles;

                invoicesList = new BindingList<Invoice>(tc.Invoices
                    .Where(t => (t.ContractId == contractToEdit.Id))
                    .OrderByDescending(t => t.Date)
                    .ToList());
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AddEditFileForm ef = new AddEditFileForm();
            ef.ShowDialog();

            if(ef.CreatedFile != null)
            {
                relatedFiles.Add(ef.CreatedFile);
            }
        }

        private void filesTable_SelectionChanged(object sender, EventArgs e)
        {
            linkLabel2.Enabled = linkLabel3.Enabled = filesTable.SelectedRows.Count > 0;
        }

        private void dateOfSigning_ValueChanged(object sender, EventArgs e)
        {
            if(dateOfSigning.Value > dateOfStart.Value)
            {
                dateOfStart.Value = dateOfSigning.Value;
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void filesTable_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataGridViewHelper.CalculateNewRowNumber(sender as DataGridView, 0, e.RowIndex, e.RowCount);
        }

        private void filesTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            UploadedFile selectedFile = filesTable.SelectedRows[0].DataBoundItem as UploadedFile;
            AddEditFileForm ef = new AddEditFileForm(selectedFile);
            ef.ShowDialog();

            filesTable.Refresh();
        }
    }
}
