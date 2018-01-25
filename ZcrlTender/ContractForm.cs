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

        private bool isPossibleToChangeContractMainData;

        // Список связанных с договором файлов
        private BindingList<UploadedFile> relatedFiles;
        // Список файлов на удаление
        private List<UploadedFile> deletingFiles;

        // Список скрытых вкладок
        private List<TabPage> hiddenPagesList;

        private object locker;

        // Были ли произведены изменения в БД
        private bool dbWasChanged;
        public bool DbWasChanged
        {
            get
            {
                return dbWasChanged;
            }
        }

        public ContractForm(TenderYear year)
        {
            currentContract = new Contract();
            InitializeFormControls(year);

            while (tabControl1.TabCount > 2)
            {
                hiddenPagesList.Add(tabControl1.TabPages[2]);
                tabControl1.TabPages.RemoveAt(2);
            }
        }

        private void InitializeFormControls(TenderYear year)
        {
            InitializeComponent();
            
            button1.Visible = UserSession.IsAuthorized;

            using(TenderContext tc = new TenderContext())
            {
                contractorCBList.DataSource = tc.Contractors.OrderBy(p => p.ShortName).ToList();
                contractorCBList.DisplayMember = "ShortName";
                contractorCBList.ValueMember = "Id";
            }

            isPossibleToChangeContractMainData = true;
            locker = new object();
            label8.Text = string.Empty;
            contractUsedSumLabel.Text = contractRemainLabel.Text = string.Empty;
            filesTable.AutoGenerateColumns = contractChangesTable.AutoGenerateColumns = contractInvoicesTable.AutoGenerateColumns = false;

            deletingFiles = new List<UploadedFile>();
            relatedFiles = new BindingList<UploadedFile>();
            filesTable.DataSource = relatedFiles;
            DataGridViewHelper.ConfigureFileTable(filesTable, relatedFiles, deletingFiles, linkLabel1, linkLabel2, linkLabel3);

            dateOfSigning.Value = dateOfStart.Value = dateOfEnd.Value =
                new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0).AddYears(Convert.ToInt32(year.Year) - DateTime.Now.Year);
            dateOfSigning.MinDate = dateOfStart.MinDate = dateOfEnd.MinDate = new DateTime(Convert.ToInt32(year.Year), 1, 1, 0, 0, 0);
            dateOfSigning.MaxDate = dateOfStart.MaxDate = dateOfEnd.MaxDate = new DateTime(Convert.ToInt32(year.Year), 12, 31, 0, 0, 0);
            dateOfEnd.Value = new DateTime(Convert.ToInt32(year.Year), 12, 31, 0, 0, 0);

            hiddenPagesList = new List<TabPage>();

            contractChangesTable.RowsAdded += (sender, e) => DataGridViewHelper.CalculateNewRowNumber(contractChangesTable, 0, e.RowIndex, e.RowCount);
            contractInvoicesTable.RowsAdded += (sender, e) => DataGridViewHelper.CalculateNewRowNumber(contractInvoicesTable, 0, e.RowIndex, e.RowCount);
        }

        // Устанавливаем максимум для данного контракта согласно записи в годовом плане
        private void SetContractMaximumFromTPrecord(TenderPlanRecord record)
        {
            if(record == null)
            {
                return;
            }

            decimal currentContractSum = (currentContract != null) ? currentContract.Sum: 0;
            decimal currentContractMaximumSum = record.AvailableForContractsMoney + currentContractSum;
            label8.Text = string.Format("Максимум за річним планом {0:N2} грн.", currentContractMaximumSum);
            fullSum.Maximum = currentContractMaximumSum;
        }

        public ContractForm(TenderYear year, Contract contractToEdit)
        {
            if (!UserSession.IsAuthorized)
            {
                this.Text = "Перегляд даних договору";
            }
            else
            {
                this.Text = "Редагування даних договору";
            }

            using (TenderContext tc = new TenderContext())
            {
                tc.Contracts.Attach(contractToEdit);
                currentContract = contractToEdit;
                InitializeFormControls(year);
                number.Text = currentContract.Number;
                fullSum.Value = currentContract.Sum;
                description.Text = currentContract.Description;
                dateOfSigning.Value = currentContract.SignDate;
                dateOfStart.Value = currentContract.BeginDate;
                dateOfEnd.Value = currentContract.EndDate;
                tenderPlanRecordPicker1.SelectedRecord = currentContract.RecordInPlan;
                SetContractMaximumFromTPrecord(currentContract.RecordInPlan);

                dateOfSigning.MinDate = dateOfStart.MinDate = dateOfEnd.MinDate = new DateTime(Convert.ToInt32(year.Year), 1, 1, 0, 0, 0);
                dateOfSigning.MaxDate = dateOfStart.MaxDate = dateOfEnd.MaxDate = new DateTime(Convert.ToInt32(year.Year), 12, 31, 0, 0, 0);

                contractorCBList.SelectedItem = currentContract.Contractor;

                relatedFiles.Clear();
                foreach (var item in currentContract.RelatedFiles)
                    relatedFiles.Add(item);

                contractInvoicesTable.DataSource = new BindingList<Invoice>(currentContract
                    .Invoices.OrderByDescending(t => t.Date).ToList());
                contractChangesTable.DataSource = new BindingList<ContractChange>(currentContract
                    .ContractChanges.OrderByDescending(t => t.DateOfChange).ToList());

                // Убираем возможность менять смету, кекв и код по дк если есть проплаты по Договору
                isPossibleToChangeContractMainData = (currentContract.UsedMoney == 0);
                contractorCBList.Enabled = 
                    tenderPlanRecordPicker1.Enabled =  isPossibleToChangeContractMainData;
                fullSum.Minimum = currentContract.UsedMoney;

                if(currentContract.Invoices.Count > 0)
                {
                    contractUsedSumLabel.Text = string.Format("Проплачено на загальну суму: {0:N2} грн.", currentContract.UsedMoney);
                    fullSum.Minimum = currentContract.UsedMoney;
                    contractRemainLabel.Text = string.Format("Залишок по договору: {0:N2} грн.", currentContract.MoneyRemain);
                }
                else
                {
                    contractUsedSumLabel.Text = contractRemainLabel.Text = string.Empty;
                }
            }

            for (int i = 0; i < hiddenPagesList.Count; i++)
            {
                tabControl1.TabPages.Add(hiddenPagesList[i]);
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

        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(number.Text))
            {
                NotificationHelper.ShowError("Ви не вказали номер договору");
                return;
            }

            if(tenderPlanRecordPicker1.SelectedRecord == null)
            {
                NotificationHelper.ShowError("Для реєстрації договору потрібно обрати запис у річному плані.");
                return;
            }

            if(fullSum.Value == 0)
            {
                NotificationHelper.ShowError("Сума договору повинна бути більша за нуль");
                return;
            }

            if(contractorCBList.SelectedItem == null)
            {
                NotificationHelper.ShowError("Ви не обрали контрагента");
                return;
            }

            StringBuilder contractChanges = new StringBuilder(string.Empty);
            using(TenderContext tc = new TenderContext())
            {
                if(currentContract.Id > 0)
                {
                    tc.Contracts.Attach(currentContract);

                    // Записываем изменения внесённые пользователем
                    if(currentContract.Sum != fullSum.Value)
                    {
                        contractChanges.Append(string.Format("[Сумма договору змінена з '{0}' на '{1}']\n", 
                            currentContract.Sum, fullSum.Value)); 
                    }
                    if (!currentContract.Number.Equals(number.Text))
                    {
                        contractChanges.Append(string.Format("[Номер договору змінений з '{0}' на '{1}']\n",
                            currentContract.Number, number.Text));
                    }
                    if(currentContract.SignDate != dateOfSigning.Value)
                    {
                        contractChanges.Append(string.Format("[Дата підписання договору змінена з '{0}' на '{1}']\n",
                            currentContract.SignDate.ToShortDateString(), dateOfSigning.Value.ToShortDateString()));
                    }
                    if (currentContract.BeginDate != dateOfStart.Value)
                    {
                        contractChanges.Append(string.Format("[Дата набуття чинності договору змінена з '{0}' на '{1}']\n",
                            currentContract.BeginDate.ToShortDateString(), dateOfStart.Value.ToShortDateString()));
                    }
                    if (currentContract.EndDate != dateOfEnd.Value)
                    {
                        contractChanges.Append(string.Format("[Дата закінчення договору змінена з '{0}' на '{1}']\n",
                            currentContract.EndDate.ToShortDateString(), dateOfEnd.Value.ToShortDateString()));
                    }

                    // Если были изменения - необходимо указать причину изменений
                    if(!string.IsNullOrWhiteSpace(contractChanges.ToString()))
                    {
                        ActionCommentForm af = new ActionCommentForm();
                        af.ShowDialog();

                        if(af.ReasonDescription == null)
                        {
                            NotificationHelper.ShowError("Без зазначення причини змін вони не будуть внесені в базу даних!");
                            return;
                        }
                        else
                        {
                            contractChanges.Append(string.Format("Причина: {0}", af.ReasonDescription));
                            currentContract.ContractChanges.Add(new ContractChange 
                            { 
                                DateOfChange = DateTime.Now, 
                                ContractId = currentContract.Id, 
                                Description = contractChanges.ToString()
                            });
                        }
                    }
                }

                currentContract.Number = number.Text.Trim();
                currentContract.SignDate = dateOfSigning.Value;
                currentContract.BeginDate = dateOfStart.Value;
                currentContract.EndDate = dateOfEnd.Value;
                currentContract.ContractorId = (contractorCBList.SelectedItem as Contractor).Id;
                currentContract.TenderPlanRecordId = tenderPlanRecordPicker1.SelectedRecord.Id;
                currentContract.Description = description.Text.Trim();
                currentContract.Sum = fullSum.Value;

                if(currentContract.Id == 0)
                {
                    tc.Contracts.Add(currentContract);
                }
                else
                {
                    tc.Entry<Contract>(currentContract).State = System.Data.Entity.EntityState.Modified;
                }
                tc.SaveChanges();

                FileManager.UpdateRelatedFilesOfEntity(tc, currentContract.RelatedFiles, relatedFiles, deletingFiles);

                dbWasChanged = true;
                NotificationHelper.ShowInfo("Дані успішно збережено до бази!");
                this.Close();
            }
        }

        private void tenderPlanRecordPicker1_RecordChanged(object sender, EventArgs e)
        {
            SetContractMaximumFromTPrecord(tenderPlanRecordPicker1.SelectedRecord);
        }
    }
}
