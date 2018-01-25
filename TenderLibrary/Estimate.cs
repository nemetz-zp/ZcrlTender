using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenderLibrary
{
    // Тип сметы
    public enum EstimateType
    {
        Planned,    // Запланированное поступление
        Additional  // Дополнительные средства
    }

    // Смета под поступившие средства
    public class Estimate
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public EstimateType Type { get; set; }

        public virtual TenderYear Year { get; set; }
        public int TenderYearId { get; set; }

        public virtual ICollection<BalanceChanges> Changes { get; set; }
        public virtual ICollection<UploadedFile> RelatedFiles { get; set; }
        public virtual ICollection<TenderPlanRecord> PlanRecords { get; set; }

        // Средства доступные по КЕКВ для создания записи в годовом плане
        public decimal MoneyForYearPlanning(KekvCode kekv)
        {
            decimal result = Changes.Where(p => (p.PrimaryKekvId == kekv.Id) && ((p.PrimaryKekvSum > 0) || (p.PlannedSpendingId != null)))
                .Select(p => p.PrimaryKekvSum).DefaultIfEmpty(0).Sum();

            return result;
        }

        // Запланированные средства по КЕКВ
        public decimal PlannedMoney(KekvCode kekv)
        {
            decimal result = PlanRecords.Where(p => p.PrimaryKekvId == kekv.Id).Select(p => p.UsedByRecordSum).Sum();
            
            return result;
        }

        public Estimate()
        {
            Changes = new List<BalanceChanges>();
            RelatedFiles = new List<UploadedFile>();
            PlanRecords = new List<TenderPlanRecord>();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Estimate castedObj = obj as Estimate;
            if (castedObj == null)
                return false;

            return (Id == castedObj.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
