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
    public partial class ContractorsListForm : Form
    {
        private BindingList<Contractor> contractorsList;
        private bool wasDbUpdated;

        public bool WasDbUpdated
        {
            get
            {
                return wasDbUpdated;
            }
        }

        public ContractorsListForm()
        {
            wasDbUpdated = false;
            InitializeComponent();

            if(!UserSession.IsAuthorized)
            {
                editContractorButton.Text = "Переглянути";
                editContractorButton.Location = addContractorButton.Location;
                addContractorButton.Visible = deleteContractorButton.Visible = false;
            }

            contractorsTable.AutoGenerateColumns = false;
            using (TenderContext tc = new TenderContext())
            {
                contractorsList = new BindingList<Contractor>(tc.Contractors.ToList());
            }
            contractorsTable.DataSource = contractorsList;
        }

        private void estimateTable_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataGridViewHelper.CalculateNewRowNumber(sender as DataGridView, 0, e.RowIndex, e.RowCount);
        }

        private void contractorsTable_SelectionChanged(object sender, EventArgs e)
        {
            editContractorButton.Enabled = deleteContractorButton.Enabled = (contractorsTable.SelectedRows.Count > 0);
        }

        private void addContractorButton_Click(object sender, EventArgs e)
        {
            AddEditContractor af = new AddEditContractor();
            af.ShowDialog();

            if (af.WasDbUpdated)
            {
                if (!reloadContractorsListWorker.IsBusy)
                {
                    ToggleLoadAnimation();
                    reloadContractorsListWorker.RunWorkerAsync();
                    wasDbUpdated = true;
                }
            }
        }

        private void ToggleLoadAnimation()
        {
            bool loadingProcess = !(loadingPicture.Visible);

            loadingPicture.Visible = loadingLabel.Visible = loadingProcess;
            addContractorButton.Enabled = editContractorButton.Enabled = deleteContractorButton.Enabled = !loadingProcess;
        }

        private void reloadContractorsListWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            using(TenderContext tc = new TenderContext())
            {
                e.Result = tc.Contractors.OrderBy(p => p.ShortName).ToList();
            }
        }

        private void reloadContractorsListWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            List<Contractor> contractors = e.Result as List<Contractor>;
            
            contractorsList.Clear();
            foreach (var item in contractors)
                contractorsList.Add(item);
            ToggleLoadAnimation();
        }

        private void deleteContractorButton_Click(object sender, EventArgs e)
        {
            Contractor selectedContractor = contractorsTable.SelectedRows[0].DataBoundItem as Contractor;
            
            if(NotificationHelper.ShowYesNoQuestion(string.Format("Ви впевнені, що хочете видалити інформацію про \"{0}\"?", selectedContractor.ShortName)))
            {
                using(TenderContext tc = new TenderContext())
                {
                    bool entityHasReferences = tc.Contracts.Where(p => p.ContractorId == selectedContractor.Id).Any();
                    if(entityHasReferences)
                    {
                        NotificationHelper.ShowError(string.Format("Видалення інформації про контрагента \"{0}\" неможливе, оскільки під нього є зареєстровані договори!", selectedContractor.ShortName));
                        return;
                    }
                    else
                    {
                        tc.Contractors.Attach(selectedContractor);
                        tc.Contractors.Remove(selectedContractor);
                        tc.SaveChanges();

                        NotificationHelper.ShowInfo("Контрагент успішно видалений!");
                        if(!reloadContractorsListWorker.IsBusy)
                        {
                            ToggleLoadAnimation();
                            reloadContractorsListWorker.RunWorkerAsync();
                            wasDbUpdated = true;
                        }
                    }
                }
            }
        }

        private void editContractorButton_Click(object sender, EventArgs e)
        {
            Contractor selectedContractor = contractorsTable.SelectedRows[0].DataBoundItem as Contractor;

            AddEditContractor af = new AddEditContractor(selectedContractor);
            af.ShowDialog();

            if (af.WasDbUpdated)
            {
                if (!reloadContractorsListWorker.IsBusy)
                {
                    ToggleLoadAnimation();
                    reloadContractorsListWorker.RunWorkerAsync();
                    wasDbUpdated = true;
                }
            }
        }

        private void contractorsTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            editContractorButton_Click(sender, null);
        }
    }
}
