using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenderLibrary
{
    // Запланированные траты
    public class PlannedSpending
    {
        public long Id { get; set; }

        public virtual KekvCode PrimaryKekv { get; set; }
        public int PrimaryKekvId { get; set; }

        public virtual KekvCode SecondaryKekv { get; set; }
        public int SecondaryKekvId { get; set; }

        public virtual Estimate Estimate { get; set; }
        public int? EstimateId { get; set; }

        public decimal Sum { get; set; }

        public ICollection<BalanceChanges> Changes { get; set; }

        public string Description { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            PlannedSpending castedObj = obj as PlannedSpending;
            if (castedObj == null)
                return false;

            return (Id == castedObj.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
