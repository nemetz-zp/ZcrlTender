using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenderLibrary
{
    // Изменения на счетах
    public class BalanceChanges
    {
        public long Id { get; set; }

        public virtual Estimate Estimate { get; set; }
        public int EstimateId { get; set; }

        public DateTime DateOfReceiving { get; set; }
        
        public virtual KekvCode PrimaryKekv { get; set; }
        public int PrimaryKekvId { get; set; }

        public virtual KekvCode SecondaryKekv { get; set; }
        public int SecondaryKekvId { get; set; }

        public virtual DkCode Dk { get; set; }
        public int? DkCodeId { get; set; }

        public virtual MoneySource MoneySource { get; set; }
        public int MoneySourceId { get; set; }

        public virtual Invoice Invoice { get; set; }
        public int? InvoiceId { get; set; }

        public virtual PlannedSpending PlannedSpending { get; set; }
        public int? PlannedSpendingId { get; set; }

        public decimal PrimaryKekvSum { get; set; }
        public decimal SecondaryKekvSum { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            BalanceChanges castedObj = obj as BalanceChanges;
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
