using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenderLibrary
{
    // Тип процедуры закупки
    public enum ProcedureType
    {
        WithoutSystem,
        Limited,
        ContractReport,
        Open,
        Private,
        Dialog
    }

    // Изменение по кодам и КЕКВам в плане
    public class TenderPlanRecord
    {
        public int Id { get; set; }

        public virtual Estimate Estimate { get; set; }
        public int EstimateId { get; set; }

        public DateTime DateOfCreating { get; set; }
        public DateTime DateOfLastChange { get; set; }

        public virtual KekvCode PrimaryKekv { get; set; }
        public int PrimaryKekvId { get; set; }

        public virtual KekvCode SecondaryKekv { get; set; }
        public int SecondaryKekvId { get; set; }

        public ProcedureType ProcedureType { get; set; }

        public virtual DkCode Dk { get; set; }
        public int DkCodeId { get; set; }

        // Номер и дата протокола, которым утверждена данная запись в плане
        public string ProtocolNum { get; set; }
        public DateTime ProtocolDate { get; set; }

        // Ориентировочное начало закупки
        public DateTime TenderBeginDate { get; set; }

        // Причина дублирования кода в годовом плане (если это дубль)
        public string CodeRepeatReason { get; set; }

        // Запланированная сумма
        public decimal PlannedSum { get; set; }

        // Запись основана на потребности, а не на запланированном финансировании
        public bool BasedOnNeed { get; set; }

        // Доступные средства под регистрацию договоров
        [NotMapped]
        public decimal AvailableForContractsMoney
        {
            get
            {
                decimal result = 0;
                if(BasedOnNeed || IsTenderComplete)
                {
                    decimal estimateMoneyOnKekv = Estimate.Changes
                        .Where(p => ((p.PrimaryKekvId == PrimaryKekvId) && ((p.PrimaryKekvSum > 0) || (p.PlannedSpendingId != null))))
                        .Select(p => p.PrimaryKekvSum).DefaultIfEmpty(0)
                        .Sum();
                    decimal usedByAnotherRecordsMoney = Estimate.PlanRecords.Where(p => p.PrimaryKekvId == PrimaryKekvId)
                        .Select(p => p.UsedByRecordSum)
                        .DefaultIfEmpty(0)
                        .Sum();
                    decimal freeMoney = estimateMoneyOnKekv - usedByAnotherRecordsMoney;
                    result = (PlannedSum > freeMoney) ? freeMoney : PlannedSum;
                }
                else
                {
                    result = PlannedSum - RegisteredContracts.Select(p => p.Sum).DefaultIfEmpty(0).Sum();
                }

                return result;
            }
        }

        // Фактически занятая часть финансирования
        [NotMapped]
        public decimal UsedByRecordSum
        {
            get
            {
                if(!IsTenderComplete && !BasedOnNeed)
                {
                    return PlannedSum;
                }
                else
                {
                    return RegisteredContracts.Select(p => p.Sum).DefaultIfEmpty(0).Sum();
                }
            }
        }

        // Завершена ли закупка по данной записи в годовом плане
        public bool IsTenderComplete { get; set; }

        // Конкретное название предмета закупки
        public string ConcreteName { get; set; }

        // Список изменений по записи годового плана
        public virtual ICollection<TenderPlanRecordChange> Changes { get; set; }
        // Зарегистрированные под данную запись договора
        public virtual ICollection<Contract> RegisteredContracts { get; set; }
        // Приобщённые файлы
        public virtual ICollection<UploadedFile> RelatedFiles { get; set; }

        public TenderPlanRecord()
        {
            Changes = new List<TenderPlanRecordChange>();
            RegisteredContracts = new List<Contract>();
            RelatedFiles = new List<UploadedFile>();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            TenderPlanRecord castedObj = obj as TenderPlanRecord;
            if (castedObj == null)
                return false;

            return (Id == castedObj.Id);
        }

        public static string GetProcedureName(ProcedureType type)
        {
            string result = string.Empty;
            
            switch(type)
            {
                case ProcedureType.WithoutSystem:
                    result = "Без застосування електронної системи";
                    break;
                case ProcedureType.Limited:
                    result = "Допорогова закупівля";
                    break;
                case ProcedureType.ContractReport:
                    result = "Звіт про укладений договір";
                    break;
                case ProcedureType.Open:
                    result = "Відкриті торги";
                    break;
                case ProcedureType.Private:
                    result = "Переговорна процедура";
                    break;
                case ProcedureType.Dialog:
                    result = "Конкурентний діалог";
                    break;
            }

            return result;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
