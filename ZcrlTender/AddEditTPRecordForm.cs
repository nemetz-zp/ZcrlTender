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

        private Estimate relatedEstimate;
        private List<KekvCode> primaryKekvList;
        private List<KekvCode> altKekvList;

        private List<decimal> kekvsRemains;

        class KekvRemain
        {
            public KekvCode Kekv { get; set; }
            public decimal Sum { get; set; }
        }

        // Создание нового кода в плане
        public AddEditTPRecordForm(Estimate selectedEstimate)
        {
            InitializeComponent();

            relatedEstimate = selectedEstimate;
            primaryKekvList = new List<KekvCode>();
            kekvsRemains = new List<decimal>();

            using(TenderContext tc = new TenderContext())
            {
                // Список КЕКВов с заложеными на нём (по смете) годовыми суммами
                List<KekvRemain> kekvsRemainsByEstimate = (from rec in tc.BalanceChanges
                                   where (rec.EstimateId == relatedEstimate.Id) && (rec.PrimaryKekvSum > 0)
                                   group rec by rec.PrimaryKekv into kekvGroup
                                   select new KekvRemain { Kekv = kekvGroup.Key, Sum = kekvGroup.Sum(p => p.PrimaryKekvSum) } into g1
                                   where g1.Sum > 0
                                   select g1).ToList();

                List<KekvRemain> plannedMoneyOnKekvs = (from planItem in tc.TenderPlanRecords.ToList()
                                         join k in kekvsRemainsByEstimate on planItem.PrimaryKekvId equals k.Kekv.Id
                                         select new KekvRemain { Kekv = k.Kekv, Sum = k.Sum - planItem.Sum } into g
                                         where g.Sum > 0
                                         select g).ToList();

                // Загружаем список КЕКВов с остатками нераспределённых средств по ним
                foreach(KekvRemain item in plannedMoneyOnKekvs)
                {
                    primaryKekvList.Add(item.Kekv);
                    kekvsRemains.Add(item.Sum);
                }

                altKekvList = (from k in tc.KekvCodes select k).ToList();

                mainKekv.DataSource = primaryKekvList;
                altKekv.DataSource = altKekvList;
                mainKekv.DisplayMember = altKekv.DisplayMember = "Code";
                mainKekv.ValueMember = altKekv.ValueMember = "Id";

                LoadDkCodeList();

                mainKekv.SelectedIndexChanged += mainKekv_SelectedIndexChangedHandler;
            }
        }

        private void mainKekv_SelectedIndexChangedHandler(object sender, EventArgs e)
        {
            mainKekv.Enabled = dkCodesCBList.Enabled = false;
            moneyRemainLabel.Text = string.Format("Доступно: {0:N2} грн.", kekvsRemains[mainKekv.SelectedIndex]);
            LoadDkCodeList();
            mainKekv.Enabled = dkCodesCBList.Enabled = true;
        }

        // Загрузка списка кодов ДК по выбраному КЕКВ
        private void LoadDkCodeList()
        {
            using(TenderContext tc = new TenderContext())
            {
                int selectedKekvId = Convert.ToInt32(mainKekv.SelectedValue);

                // Получаем список всех кодов ДК
                List<DkCode> allDkCodesList = tc.DkCodes.ToList();

                // Исключаем из списка кодов ДК, коды по которым уже существуют записи
                List<DkCode> existingDkCodesOnKekv = tc.TenderPlanRecords.Where(p => (p.EstimateId == relatedEstimate.Id) && (p.PrimaryKekv.Id == selectedKekvId)).Select(p => p.Dk).ToList();
                dkCodesCBList.DataSource = allDkCodesList.Except(existingDkCodesOnKekv);
                dkCodesCBList.DisplayMember = "FullName";
                dkCodesCBList.ValueMember = "Id";
                dkCodesCBList.Refresh();
            }
        }

        // Открытие кода плана для редактирование
        public AddEditTPRecordForm(int tenderPlanRecordId)
        {
            using(TenderContext tc = new TenderContext())
            {
                planRecord = tc.TenderPlanRecords.Where(p => p.Id == tenderPlanRecordId).First();

                // Получаем поступления на КЕКВ по смете
                decimal maxMoneyOnKekv = (from rec in tc.BalanceChanges.ToList()
                                          where (rec.PrimaryKekvId == planRecord.PrimaryKekvId) && (rec.PrimaryKekvSum > 0)
                                         group rec by rec.PrimaryKekvId into g1
                                          select g1.Sum(p => p.PrimaryKekvSum)).FirstOrDefault();
                // Деньги занятые договорами
                decimal kekvMoneyOnContracts = (from rec in tc.Contracts.ToList()
                                                where (rec.PrimaryKekvId == rec.PrimaryKekvId) && (rec.EstimateId == planRecord.EstimateId)
                                                group rec by rec.PrimaryKekvId into g1
                                                select g1.Sum(p => p.Sum)).FirstOrDefault();
                // Доступный остаток на КЕКВ
                maxMoneyOnKekv -= kekvMoneyOnContracts;
                moneyRemainLabel.Text = string.Format("Доступно: {0:N2} грн.", kekvMoneyOnContracts);

                mainKekv.DataSource = planRecord.PrimaryKekv;
                altKekv.DataSource = planRecord.SecondaryKekv;
                mainKekv.DisplayMember = altKekv.DisplayMember = "Code";
                mainKekv.ValueMember = altKekv.ValueMember = "Id";

                // Устанавливаем допустимые границы измененния средств на коде
                dkCodeSum.Minimum = kekvMoneyOnContracts;
                dkCodeSum.Maximum = maxMoneyOnKekv;

                mainKekv.Enabled = altKekv.Enabled = dkCodesCBList.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string msg = string.Empty;
            string actionDescriptionPrefix = string.Empty;
            if(planRecord != null)
            {
                msg = "Вкажіть причину вказаної зміни коду";
                actionDescriptionPrefix = "[ВВОД НОВОГО КОДУ В ПЛАН]";
            }
            else
            {
                msg = "Прокоментуйте створення коду (необов'язково)";
                actionDescriptionPrefix = "[ЗМІНА КОДУ В ПЛАНІ]";
            }

            ActionCommentForm af = new ActionCommentForm(msg);
            af.ShowDialog();

            string actionDescription = af.ReasonDescription;
            
            // Для изменение кода указание причины необходимо
            if(planRecord != null && actionDescription == null)
            {
                MyHelper.ShowError("Зміна коду без зазначення причини неможлива");
                return;
            }

            // Если мы добавляем новый код в план - создаём его заготовку
            if(planRecord == null)
            {
                planRecord = new TenderPlanRecord();
                planRecord.DkCodeId = Convert.ToInt32(dkCodesCBList.SelectedValue);
                planRecord.PrimaryKekvId = Convert.ToInt32(mainKekv.SelectedValue);
                planRecord.SecondaryKekvId = Convert.ToInt32(altKekv.SelectedValue);
            }

            decimal changeOfSum = dkCodeSum.Value - planRecord.Sum;
            planRecord.Sum = dkCodeSum.Value;
            planRecord.ConcreteName = conctretePlanItemName.Text;
            
            TenderPlanRecordChange tpChange = new TenderPlanRecordChange();
            tpChange.Description = actionDescriptionPrefix + '\n' + actionDescription;
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

                tc.TenderPlanRecordChanges.Add(tpChange);
                tc.SaveChanges();

                MyHelper.ShowInfo("Зміни до річного плану успішно внесено");
                Close();
            }
        }
    }
}
