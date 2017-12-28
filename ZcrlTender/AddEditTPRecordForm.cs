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
    public partial class AddEditTPRecordForm : Form
    {
        private TenderPlanRecord planRecord;

        private List<KekvRemain> primaryKekvList;
        private List<KekvRemain> altKekvList;
        private List<Estimate> estimateList;

        private volatile bool controlWasChangedByUser;

        private bool dbWasChanged;
        public bool DbWasChanged
        {
            get
            {
                return dbWasChanged;
            }
        }

        class KekvRemain
        {
            public KekvCode Kekv { get; set; }
            public decimal Sum { get; set; }
        }

        // Создание нового кода в плане
        public AddEditTPRecordForm(TenderYear year)
        {
            InitializeComponent();

            button1.Visible = UserSession.IsAuthorized;

            dbWasChanged = false;
            moneyRemainLabel.Text = string.Empty;
            
            using(TenderContext tc = new TenderContext())
            {
                estimateList = tc.Estimates.Where(p => p.TenderYearId == year.Id).ToList();
                controlWasChangedByUser = false;
                estimatesCBList.DataSource = estimateList;
                controlWasChangedByUser = true;
                estimatesCBList.DisplayMember = "Name";

                altKekvList = tc.KekvCodes.Select(p => new KekvRemain { Kekv = p }).ToList();
                altKekv.DataSource = altKekvList;

                mainKekv.DisplayMember = altKekv.DisplayMember = "Kekv";

                Estimate selectedEstimate = estimatesCBList.SelectedItem as Estimate;
                LoadKekvsOnEstimate(selectedEstimate);
                LoadDkCodeList();

                mainKekv.SelectedIndexChanged += mainKekv_SelectedIndexChangedHandler;
            }
        }

        private void LoadKekvsOnEstimate(Estimate est)
        {
            if(est == null)
            {
                return;
            }

            using (TenderContext tc = new TenderContext())
            {
                // Список КЕКВов с заложеными на нём (по смете) годовыми суммами
                List<KekvRemain> kekvsRemainsByEstimate = (from rec in tc.BalanceChanges
                                                           where (rec.EstimateId == est.Id) && ((rec.PrimaryKekvSum > 0) || (rec.PlannedSpendingId != null))
                                                           group rec by rec.PrimaryKekv into kekvGroup
                                                           select new KekvRemain { Kekv = kekvGroup.Key, Sum = kekvGroup.Sum(p => p.PrimaryKekvSum) } into g1
                                                           where g1.Sum > 0
                                                           select g1).ToList();

                // Отминусовываем от них суммы заложенные в годовом плане
                List<KekvRemain> plannedMoneyOnKekvs = (from planItem in tc.TenderPlanRecords.ToList()
                                                        where planItem.EstimateId == est.Id
                                                        group planItem by planItem.PrimaryKekv into g1
                                                        select new KekvRemain { Kekv = g1.Key, Sum = g1.Sum(p => p.Sum) }).ToList();
                List<KekvRemain> result = (from rec in kekvsRemainsByEstimate
                                           join planItem in plannedMoneyOnKekvs on rec.Kekv equals planItem.Kekv into j1
                                           from rightSide in j1.DefaultIfEmpty(new KekvRemain { Kekv = rec.Kekv, Sum = 0 })
                                           select new KekvRemain { Kekv = rec.Kekv, Sum = rec.Sum - rightSide.Sum }).ToList();

                controlWasChangedByUser = false;
                mainKekv.DataSource = result;
                controlWasChangedByUser = true;
                ShowKekvRemain();
                LoadDkCodeList();
            }
        }

        private void ShowKekvRemain()
        {
            mainKekv.Enabled = dkCodesCBList.Enabled = false;
            KekvRemain selectedMainKekv = mainKekv.SelectedItem as KekvRemain;
            moneyRemainLabel.Text = string.Format("Вільні (нерозподілені) кошти: {0:N2} грн.", selectedMainKekv.Sum);
            dkCodeSum.Value = (dkCodeSum.Value > selectedMainKekv.Sum) ? selectedMainKekv.Sum : dkCodeSum.Value;
            dkCodeSum.Maximum = selectedMainKekv.Sum;
            
            mainKekv.Enabled = dkCodesCBList.Enabled = true;
        }

        private void mainKekv_SelectedIndexChangedHandler(object sender, EventArgs e)
        {
            if (controlWasChangedByUser)
            {
                ShowKekvRemain();
                LoadDkCodeList();
            }
        }

        // Загрузка списка кодов ДК по выбраному КЕКВ
        private void LoadDkCodeList()
        {
            using(TenderContext tc = new TenderContext())
            {
                KekvRemain selectedMainKekv = mainKekv.SelectedItem as KekvRemain;
                Estimate selectedEstimate = estimatesCBList.SelectedItem as Estimate;

                if (selectedEstimate == null || selectedMainKekv == null)
                    return;

                // Получаем список всех кодов ДК
                List<DkCode> allDkCodesList = tc.DkCodes.ToList();

                // Исключаем из списка кодов ДК, коды по которым уже существуют записи
                List<DkCode> existingDkCodesOnKekv = tc.TenderPlanRecords.Where(p => (p.EstimateId == selectedEstimate.Id) && (p.PrimaryKekv.Id == selectedMainKekv.Kekv.Id)).Select(p => p.Dk).ToList();
                dkCodesCBList.DataSource = allDkCodesList.Except(existingDkCodesOnKekv).ToList();
                dkCodesCBList.DisplayMember = "FullName";
                dkCodesCBList.ValueMember = "Id";
                dkCodesCBList.Refresh();
            }
        }

        // Открытие кода плана для редактирование
        public AddEditTPRecordForm(TenderPlanRecord tenderPlanRecord)
        {
            InitializeComponent();

            button1.Visible = UserSession.IsAuthorized;

            if (!UserSession.IsAuthorized)
            {
                this.Text = "Перегляд запису у річному плані";
            }
            else
            {
                this.Text = "Редагування запису у річному плані";
            }

            dbWasChanged = false;

            this.Text = "Редагування запису в річному плані";

            using(TenderContext tc = new TenderContext())
            {
                tc.TenderPlanRecords.Attach(tenderPlanRecord);
                planRecord = tenderPlanRecord;

                // Получаем поступления на КЕКВ по смете
                decimal maxMoneyOnKekv = (from rec in tc.BalanceChanges.ToList()
                                          where (rec.PrimaryKekvId == planRecord.PrimaryKekvId) 
                                                && (rec.EstimateId == planRecord.EstimateId) 
                                                && ((rec.PrimaryKekvSum > 0) || (rec.PlannedSpendingId != null))
                                          group rec by rec.PrimaryKekvId into g1
                                          select g1.Sum(p => p.PrimaryKekvSum)).FirstOrDefault();
                
                // Деньги занятые другими записями в годовом плане по этому КЕКВ
                decimal moneyOnOtherRecords = (from rec in tc.TenderPlanRecords.ToList()
                                               where (rec.PrimaryKekvId == planRecord.PrimaryKekvId) && (rec.EstimateId == planRecord.EstimateId)
                                               group rec by rec.PrimaryKekvId into g1
                                               select g1.Sum(p => p.Sum)).FirstOrDefault();

                // Деньги занятые договорами
                decimal kekvMoneyOnContracts = (from rec in tc.Contracts.ToList()
                                                where (rec.PrimaryKekvId == rec.PrimaryKekvId) && (rec.EstimateId == planRecord.EstimateId) && (rec.DkCodeId == planRecord.DkCodeId)
                                                group rec by rec.PrimaryKekvId into g1
                                                select g1.Sum(p => p.Sum)).FirstOrDefault();
                if (kekvMoneyOnContracts > 0)
                {
                    label6.Visible = true;
                    label6.Text = string.Format("З них зайнято договорами: {0:N2} грн.", kekvMoneyOnContracts);
                }

                // Доступный остаток на КЕКВ
                maxMoneyOnKekv -= moneyOnOtherRecords;
                moneyRemainLabel.Text = string.Format("Вільні (нерозподілені) кошти: {0:N2} грн.", maxMoneyOnKekv);

                estimatesCBList.DataSource = new List<Estimate> { planRecord.Estimate };
                estimatesCBList.DisplayMember = "Name";
                mainKekv.DataSource = new List<KekvRemain> { new KekvRemain { Kekv = planRecord.PrimaryKekv } };
                altKekv.DataSource = new List<KekvRemain> { new KekvRemain { Kekv = planRecord.SecondaryKekv } };
                mainKekv.DisplayMember = altKekv.DisplayMember = "Kekv";
                dkCodesCBList.DataSource = new List<DkCode> { planRecord.Dk };
                conctretePlanItemName.Text = planRecord.ConcreteName;
                dkCodeSum.Value = planRecord.Sum;

                // Устанавливаем допустимые границы измененния средств на коде
                dkCodeSum.Minimum = kekvMoneyOnContracts;
                dkCodeSum.Maximum = maxMoneyOnKekv + planRecord.Sum;

                estimatesCBList.Enabled = mainKekv.Enabled = altKekv.Enabled = dkCodesCBList.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (estimatesCBList.SelectedItem == null)
            {
                NotificationHelper.ShowError("Ви не вибрали кошторис");
                return;
            }

            if (mainKekv.SelectedItem == null)
            {
                NotificationHelper.ShowError("Ви не вибрали основний КЕКВ");
                return;
            }

            if (altKekv.SelectedItem == null)
            {
                NotificationHelper.ShowError("Ви не вибрали старий КЕКВ");
                return;
            }

            if (dkCodeSum.Value == 0)
            {
                NotificationHelper.ShowError("Сума повинна бути більша за 0");
                return;
            }

            if (string.IsNullOrWhiteSpace(conctretePlanItemName.Text.Trim()))
            {
                NotificationHelper.ShowError("Ви не вказали конкретну назву предмету");
                return;
            }

            if(dkCodesCBList.SelectedItem == null)
            {
                NotificationHelper.ShowError("Ви не вибрали код за ДК");
                return;
            }

            string msg = string.Empty;
            string actionDescriptionPrefix = string.Empty;
            if(planRecord != null)
            {
                msg = "Вкажіть причину вказаної зміни коду";
                actionDescriptionPrefix = "[ЗМІНА КОДУ]";
            }
            else
            {
                msg = "Прокоментуйте створення коду (необов'язково)";
                actionDescriptionPrefix = "[СТВОРЕННЯ КОДУ]";
            }

            ActionCommentForm af = new ActionCommentForm(msg);
            af.ShowDialog();

            string actionDescription = af.ReasonDescription;
            
            // Для изменение кода указание причины необходимо
            if(planRecord != null && actionDescription == null)
            {
                NotificationHelper.ShowError("Зміна коду без зазначення причини неможлива");
                return;
            }

            // Если мы добавляем новый код в план - создаём его заготовку
            if(planRecord == null)
            {
                Estimate selectedEstimate = estimatesCBList.SelectedItem as Estimate;
                KekvCode selectedPrimaryKekv = (mainKekv.SelectedValue as KekvRemain).Kekv;
                KekvCode selectedAltKekv = (altKekv.SelectedValue as KekvRemain).Kekv;
                planRecord = new TenderPlanRecord();
                planRecord.EstimateId = selectedEstimate.Id;
                planRecord.DkCodeId = Convert.ToInt32(dkCodesCBList.SelectedValue);
                planRecord.PrimaryKekvId = selectedPrimaryKekv.Id;
                planRecord.SecondaryKekvId = selectedAltKekv.Id;
                planRecord.DateOfCreating = planRecord.DateOfLastChange = DateTime.Now;
            }

            decimal changeOfSum = dkCodeSum.Value;
            planRecord.Sum = dkCodeSum.Value;
            planRecord.ConcreteName = conctretePlanItemName.Text;
            planRecord.DateOfLastChange = DateTime.Now;
            
            TenderPlanRecordChange tpChange = new TenderPlanRecordChange();
            if (!string.IsNullOrWhiteSpace(actionDescription))
            {
                tpChange.Description = actionDescriptionPrefix + '\n' + actionDescription;
            }
            else
            {
                tpChange.Description = actionDescriptionPrefix;
            }
            tpChange.DateOfChange = DateTime.Now;
            tpChange.TenderPlanRecordId = planRecord.Id;
            tpChange.ChangeOfSum = changeOfSum;
            tpChange.ChangedConcreteName = planRecord.ConcreteName;

            using(TenderContext tc = new TenderContext())
            {
                if(planRecord.Id != 0)
                {
                    tc.TenderPlanRecords.Attach(planRecord);
                    tc.Entry<TenderPlanRecord>(planRecord).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    tc.TenderPlanRecords.Add(planRecord);
                }
                tc.SaveChanges();

                planRecord.Changes.Add(tpChange);
                tc.TenderPlanRecordChanges.Add(tpChange);
                tc.SaveChanges();

                NotificationHelper.ShowInfo("Зміни до річного плану успішно внесено");
                dbWasChanged = true;
                Close();
            }
        }

        private void estimatesCBList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(controlWasChangedByUser)
            {
                LoadKekvsOnEstimate(estimatesCBList.SelectedItem as Estimate);
            }
        }
    }
}
