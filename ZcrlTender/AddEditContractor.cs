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
    public partial class AddEditContractor : Form
    {
        private Contractor contractorRecord;
        
        private bool wasDbUpdated;
        public bool WasDbUpdated
        {
            get
            {
                return wasDbUpdated;
            }
        }

        private BindingList<ContactPerson> contactsList;
        private List<ContactPerson> deletingContacts;

        private BindingList<UploadedFile> relatedFiles;
        private List<UploadedFile> deletingFilesList;

        public AddEditContractor()
        {
            InitializeFormControls();

            contactsTable.AutoGenerateColumns = false;
            contractorRecord = new Contractor();
        }

        public AddEditContractor(Contractor record)
        {
            InitializeFormControls();

            if(!UserSession.IsAuthorized)
            {
                button1.Visible = linkLabel1.Visible = linkLabel2.Visible = false;
            }

            using (TenderContext tc = new TenderContext())
            {
                tc.Contractors.Attach(record);
                contractorRecord = record;

                fullNameTextBox.Text = contractorRecord.LongName;
                shortNameTextBox.Text = contractorRecord.ShortName;
                edrTextBox.Text = contractorRecord.EdrCode;
                legalAdressTextBox.Text = contractorRecord.LegalAddress;
                actualAdressTextBox.Text = contractorRecord.ActualAddress;
                descriptionTextBox.Text = contractorRecord.Description;

                relatedFiles.Clear();
                foreach (var item in record.RelatedFiles)
                    relatedFiles.Add(item);

                contactsList.Clear();
                foreach (var item in record.Persons)
                    contactsList.Add(item);
            }
        }

        private void InitializeFormControls()
        {
            InitializeComponent();
            button1.Visible = UserSession.IsAuthorized;
            wasDbUpdated = false;
            deletingFilesList = new List<UploadedFile>();
            relatedFiles = new BindingList<UploadedFile>();
            contactsList = new BindingList<ContactPerson>();
            deletingContacts = new List<ContactPerson>();

            contactsTable.RowsAdded += (sender, e) => DataGridViewHelper.CalculateNewRowNumber(contactsTable, 0, e.RowIndex, e.RowCount);
            DataGridViewHelper.ConfigureFileTable(filesTable, relatedFiles, deletingFilesList, linkLabel5, linkLabel4, linkLabel3);

            contactsTable.AutoGenerateColumns = false;
            contactsTable.DataSource = contactsList;
            filesTable.DataSource = relatedFiles;
        }

        private void contractorsTable_SelectionChanged(object sender, EventArgs e)
        {
            linkLabel2.Enabled = (contactsTable.SelectedRows.Count > 0);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            contactsList.Add(new ContactPerson { FullName = "- НОВА ОСОБА -", ContactPhone = "000" });
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ContactPerson selectedPerson = contactsTable.SelectedRows[0].DataBoundItem as ContactPerson;
            int contactIndex = contactsTable.SelectedRows[0].Index;
            
            if(selectedPerson.Id > 0)
            {
                deletingContacts.Add(selectedPerson);
            }
            contactsList.RemoveAt(contactIndex);

            DataGridViewHelper.RecalculateRowNumberColumn(contactsTable, 0, contactIndex);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(shortNameTextBox.Text))
            {
                NotificationHelper.ShowError("Ви не вказали скорочену назву контрагента");
                return;
            }
            if (string.IsNullOrWhiteSpace(edrTextBox.Text))
            {
                NotificationHelper.ShowError("Ви не вказали код ЄДРПОУ");
                return;
            }
            if (string.IsNullOrWhiteSpace(legalAdressTextBox.Text))
            {
                NotificationHelper.ShowError("Ви не вказали юридичну адресу");
                return;
            }
            if (string.IsNullOrWhiteSpace(actualAdressTextBox.Text))
            {
                NotificationHelper.ShowError("Ви не вказали фактичну адресу");
                return;
            }

            using(TenderContext tc = new TenderContext())
            {
                if(contractorRecord.Id > 0)
                {
                    tc.Contractors.Attach(contractorRecord);
                }
                else
                {
                    // Проверяем существует ли в базе контрагент с таким кодом ЕГРПОУ
                    Contractor existingContractor = tc.Contractors.Where(p => p.EdrCode.Equals(edrTextBox.Text.Trim())).FirstOrDefault();
                    
                    if (existingContractor != null)
                    {
                        NotificationHelper.ShowError(string.Format("За даним кодом ЄДРПОУ вже зареєстрований контрагент '{0}'", existingContractor.ShortName));
                        return;
                    }
                }

                contractorRecord.LongName = fullNameTextBox.Text.Trim();
                contractorRecord.ShortName = shortNameTextBox.Text.Trim();
                contractorRecord.EdrCode = edrTextBox.Text.Trim();
                contractorRecord.LegalAddress = legalAdressTextBox.Text.Trim();
                contractorRecord.ActualAddress = actualAdressTextBox.Text.Trim();
                contractorRecord.Description = descriptionTextBox.Text.Trim();

                if(contractorRecord.Id == 0)
                {
                    tc.Contractors.Add(contractorRecord);
                    tc.SaveChanges();
                }

                foreach(var item in deletingContacts)
                {
                    tc.ContactPersons.Attach(item);
                    tc.ContactPersons.Remove(item);
                }
                tc.SaveChanges();

                contractorRecord.Persons.Clear();
                foreach(var item in contactsList)
                {
                    if (item.Id > 0)
                    {
                        tc.ContactPersons.Attach(item);
                        tc.Entry<ContactPerson>(item).State = System.Data.Entity.EntityState.Modified;
                    }

                    contractorRecord.Persons.Add(item);
                }

                FileManager.UpdateRelatedFilesOfEntity(tc, contractorRecord.RelatedFiles, relatedFiles, deletingFilesList);

                tc.SaveChanges();
                NotificationHelper.ShowInfo("Дані успішно збережені!");
                wasDbUpdated = true;
                this.Close();
            }
        }

        private void contactsTable_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if ((e.RowIndex >= 0) && (e.ColumnIndex == 1 || e.ColumnIndex == 2))
            {
                if (e.FormattedValue == null || string.IsNullOrWhiteSpace(e.FormattedValue.ToString()))
                {
                    contactsTable.CancelEdit();
                    return;
                }
            }
        }
    }
}
