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
        private BindingList<Estimate> estimatesList;
        private BindingList<KekvCode> mainKekvList;
        private BindingList<KekvCode> altKekvList;
        private BindingList<DkRemain> dkCodesList;

        private bool isPossibleToChangeContractMainData;

        // Список связанных с договором файлов
        private BindingList<UploadedFile> relatedFiles;
        // Список файлов на удаление
        private List<UploadedFile> deletingFiles;

        // Список скрытых вкладок
        private List<TabPage> hiddenPagesList;

        // Флаг, показывающий был ли контрол изменён пользователем
        private volatile bool controlValueWasChangedByUser;

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

        private class DkRemain
        {
            public DkCode Dk { get; set; }
            public decimal Remain { get; set; }

            public override bool Equals(object obj)
            {
                if (obj == null)
                    return false;

                DkRemain castedObj = obj as DkRemain;
                if (castedObj == null)
                    return false;

                return (Dk.Id == castedObj.Dk.Id);
            }
        }

        private class CalculateDkRemainWorkerArgument
        {
            public Estimate Est { get; set; }
            public KekvCode Kekv { get; set; }
            public DkCode CurrectContractDkCode { get; set; }
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

            isPossibleToChangeContractMainData = true;
            locker = new object();
            label8.Text = string.Empty;
            controlValueWasChangedByUser = false;
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

            mainKekv.DataSource = mainKekvList;
            altKekv.DataSource = altKekvList;
            mainKekv.ValueMember = altKekv.ValueMember = "Id";
            mainKekv.DisplayMember = altKekv.DisplayMember = "Code";
            dkCode.DisplayMember = "Dk";

            using (TenderContext tc = new TenderContext())
            {
                // Выбираем только те сметы, по которым есть средства
                estimatesList = new BindingList<Estimate>(tc.Estimates
                    .Where(t => (t.TenderYearId == year.Id) && (t.Changes.Sum(p => p.PrimaryKekvSum) > 0))
                    .ToList());

                contractorCBList.DataSource = tc.Contractors.ToList();

                controlValueWasChangedByUser = false;
                estimate.DataSource = estimatesList;
                controlValueWasChangedByUser = true;

                altKekvList = new BindingList<KekvCode>(tc.KekvCodes.ToList());
                altKekv.DataSource = altKekvList;

                LoadKekvList();

                // Показываем остатки по выбранному коду
                LoadDkList(currentContract.Dk);
            }

            estimate.SelectedIndexChanged += (sender, e) => {
                if (controlValueWasChangedByUser)
                    LoadKekvList();
            };
            mainKekv.SelectedIndexChanged += (sender, e) => {
                if(controlValueWasChangedByUser)
                    LoadDkList(currentContract.Dk);
            };
            dkCode.SelectedIndexChanged += (sender, e) => { 
                if(controlValueWasChangedByUser)
                    ShowDkRemain(); 
            };

            contractChangesTable.RowsAdded += (sender, e) => DataGridViewHelper.CalculateNewRowNumber(contractChangesTable, 0, e.RowIndex, e.RowCount);
            contractInvoicesTable.RowsAdded += (sender, e) => DataGridViewHelper.CalculateNewRowNumber(contractInvoicesTable, 0, e.RowIndex, e.RowCount);
        }

        // Загружаем список КЕКВ согласно выбраной сметы
        private void LoadKekvList()
        {
            Estimate selectedEstimate = estimate.SelectedItem as Estimate;
            if (selectedEstimate == null)
            {
                dkCode.DataSource = null;
                label8.Text = string.Empty;
                return;
            }

            using (TenderContext tc = new TenderContext())
            {
                mainKekvList = new BindingList<KekvCode>(
                                    (from item in tc.TenderPlanRecords
                                     where (item.EstimateId == selectedEstimate.Id)
                                     group item by item.PrimaryKekv into g1
                                     select new { Kekv = g1.Key, Sum = g1.Sum(t => t.Sum) } into g2
                                     where g2.Sum > 0
                                     select g2.Kekv).ToList()
                                    );

                controlValueWasChangedByUser = false;
                mainKekv.DataSource = mainKekvList;
                controlValueWasChangedByUser = true;
            }

            LoadDkList(currentContract.Dk);
        }

        // Обновляем список кодов классификатора с неиспользованным остатком по выбранному коду
        private void LoadDkList(DkCode currectContractDkCode)
        {
            if (controlValueWasChangedByUser && !calculateDkRemain.IsBusy)
            {
                Estimate selectedEstimate = estimate.SelectedItem as Estimate;
                KekvCode selectedKekv = mainKekv.SelectedItem as KekvCode;
                if (selectedEstimate == null || selectedKekv == null)
                {
                    dkCode.DataSource = null;
                    label8.Text = string.Empty;
                    return;
                }

                ToggleDkRemainAnimation();
                calculateDkRemain.RunWorkerAsync(new CalculateDkRemainWorkerArgument 
                { 
                    Est = selectedEstimate, 
                    Kekv = selectedKekv,
                    CurrectContractDkCode = currectContractDkCode
                });
            }
        }

        private void ShowDkRemain()
        {
            if (controlValueWasChangedByUser)
            {
                int selectedDkCodeIndex = dkCode.SelectedIndex;
                DkRemain selectedDk = dkCode.SelectedItem as DkRemain;
                
                if(selectedDkCodeIndex < 0)
                {
                    return;
                }

                decimal currentDkRemain = dkCodesList[selectedDkCodeIndex].Remain;
                label8.Text = string.Format("доступно: {0:N2} грн.", currentDkRemain);

                if(((currentContract.Id == 0) && (fullSum.Value > currentDkRemain)) ||
                    ((currentContract.Id > 0) && (!currentContract.Dk.Equals(selectedDk.Dk))))
                {
                    fullSum.Value = currentDkRemain;
                }

                if (currentContract.Id > 0)
                {
                    fullSum.Maximum = currentContract.Sum + currentDkRemain;
                }
                else
                {
                    fullSum.Maximum = currentDkRemain;
                }
            }
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
                number.Text = contractToEdit.Number;
                fullSum.Value = contractToEdit.Sum;
                description.Text = contractToEdit.Description;
                dateOfSigning.Value = contractToEdit.SignDate;
                dateOfStart.Value = contractToEdit.BeginDate;
                dateOfEnd.Value = contractToEdit.EndDate;

                dateOfSigning.MinDate = dateOfStart.MinDate = dateOfEnd.MinDate = new DateTime(Convert.ToInt32(year.Year), 1, 1, 0, 0, 0);
                dateOfSigning.MaxDate = dateOfStart.MaxDate = dateOfEnd.MaxDate = new DateTime(Convert.ToInt32(year.Year), 12, 31, 0, 0, 0);

                contractorCBList.SelectedItem = currentContract.Contractor;
                mainKekv.SelectedItem = currentContract.PrimaryKekv;
                altKekv.SelectedItem = currentContract.SecondaryKekv;
                estimate.SelectedItem = currentContract.Estimate;

                relatedFiles.Clear();
                foreach (var item in currentContract.RelatedFiles)
                    relatedFiles.Add(item);

                contractInvoicesTable.DataSource = new BindingList<Invoice>(currentContract
                    .Invoices.OrderByDescending(t => t.Date).ToList());
                contractChangesTable.DataSource = new BindingList<ContractChange>(currentContract
                    .ContractChanges.OrderByDescending(t => t.DateOfChange).ToList());

                // Убираем возможность менять смету, кекв и код по дк если есть проплаты по Договору
                isPossibleToChangeContractMainData = (currentContract.UsedMoney == 0);
                estimate.Enabled = 
                    contractorCBList.Enabled = 
                    mainKekv.Enabled = 
                    altKekv.Enabled = 
                    dkCode.Enabled = isPossibleToChangeContractMainData;

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

        private void ToggleDkRemainAnimation()
        {
            dkCode.Tag = (dkCode.Tag == null) ? locker : null;
            bool loadingProcess = (dkCode.Tag != null);

            pictureBox2.Visible = loadingProcess;

            label8.Text = loadingProcess ? "Завантаження залишку" : string.Empty;

            dkCode.Enabled = mainKekv.Enabled = estimate.Enabled = !loadingProcess;

            estimate.Enabled =
                    contractorCBList.Enabled =
                    mainKekv.Enabled =
                    altKekv.Enabled =
                    dkCode.Enabled = isPossibleToChangeContractMainData;
        }


        private void calculateDkRemain_DoWork(object sender, DoWorkEventArgs e)
        {
            CalculateDkRemainWorkerArgument arg = e.Argument as CalculateDkRemainWorkerArgument;

            using (TenderContext tc = new TenderContext())
            {
                // Деньги зарегистрированные на кодах договорами
                List<DkRemain> registeredOnContractsDkMoney = (from item in tc.Contracts.ToList()
                                                               where ((item.EstimateId == arg.Est.Id) && (item.PrimaryKekv.Id == arg.Kekv.Id))
                                                               group item by item.Dk into g1
                                                               select new DkRemain { Dk = g1.Key, Remain = g1.Sum(p => p.Sum) }).ToList();
                
                List<DkRemain> registeredOnPlanDkMoney = (from item in tc.TenderPlanRecords.ToList()
                             where ((item.EstimateId == arg.Est.Id) && (item.PrimaryKekv.Id == arg.Kekv.Id))
                             select new DkRemain { Dk = item.Dk, Remain = item.Sum }).ToList();
                
                if(registeredOnContractsDkMoney.Count > registeredOnPlanDkMoney.Count)
                {
                    throw new InvalidOperationException("Критична помилка: наявні договори під які відсутні записи у річному плані");
                }

                // Если мы редактируем контракт - отображаем для его кода остаток (даже если он нулевой)
                if (arg.CurrectContractDkCode != null)
                {
                    dkCodesList = new BindingList<DkRemain>(
                                (from item in registeredOnPlanDkMoney
                                 join item2 in registeredOnContractsDkMoney on item.Dk equals item2.Dk into g1
                                 from nrec in g1.DefaultIfEmpty(new DkRemain())
                                 select new DkRemain { Dk = item.Dk, Remain = item.Remain - nrec.Remain } into s1
                                 where ((s1.Remain > 0) || s1.Dk.Equals(arg.CurrectContractDkCode))
                                 select s1).ToList());
                }
                // ... в противном случае отображаем только коды по которым есть реальные остатки
                else
                {
                    dkCodesList = new BindingList<DkRemain>(
                                (from item in registeredOnPlanDkMoney
                                 join item2 in registeredOnContractsDkMoney on item.Dk equals item2.Dk into g1
                                 from nrec in g1.DefaultIfEmpty(new DkRemain())
                                 select new DkRemain { Dk = item.Dk, Remain = item.Remain - nrec.Remain } into s1
                                 where s1.Remain > 0
                                 select s1).ToList());
                }
            }
        }

        private void calculateDkRemain_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ToggleDkRemainAnimation();

            controlValueWasChangedByUser = false;
            dkCode.DataSource = dkCodesList;
            KekvCode selectedPrimaryKekv = mainKekv.SelectedItem as KekvCode;

            if((currentContract.Id > 0) && currentContract.PrimaryKekv.Equals(selectedPrimaryKekv))
            {
                dkCode.SelectedIndex = dkCodesList.IndexOf(new DkRemain { Dk = currentContract.Dk });
            }
            controlValueWasChangedByUser = true;

            ShowDkRemain();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(number.Text))
            {
                NotificationHelper.ShowError("Ви не вказали номер договору");
                return;
            }

            if(estimate.SelectedItem == null)
            {
                NotificationHelper.ShowError("Для реєстрації договору відсутні будь-які кошториси із вільними коштами");
                return;
            }

            if(mainKekv.SelectedItem == null || dkCode.SelectedItem == null)
            {
                NotificationHelper.ShowError("Для реєстрації договору відсутні записи у річному плані. Саме тому ви не можете обрати код КЕКВ чи код за ДК.");
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
                currentContract.PrimaryKekvId = (mainKekv.SelectedItem as KekvCode).Id;
                currentContract.SecondaryKekvId = (altKekv.SelectedItem as KekvCode).Id;
                currentContract.EstimateId = (estimate.SelectedItem as Estimate).Id;
                currentContract.DkCodeId = (dkCode.SelectedItem as DkRemain).Dk.Id;
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
    }
}
