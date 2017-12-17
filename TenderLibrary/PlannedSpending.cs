using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenderLibrary
{
    // Запланированные траты
    public class PlannedSpending
    {
        public int Id { get; set; }

        public DateTime CreationDate { get; set; }

        public virtual KekvCode PrimaryKekv { get; set; }
        public int PrimaryKekvId { get; set; }

        public virtual KekvCode SecondaryKekv { get; set; }
        public int SecondaryKekvId { get; set; }

        public bool IsPaid { get; set; }

        public virtual Estimate Estimate { get; set; }
        public int EstimateId { get; set; }

        public decimal Sum { get; set; }

        public virtual ICollection<BalanceChanges> Changes { get; set; }
        public virtual ICollection<UploadedFile> RelatedFiles { get; set; }

        public string Description { get; set; }

        public PlannedSpending()
        {
            Changes = new List<BalanceChanges>();
            RelatedFiles = new List<UploadedFile>();
        }

        [NotMapped]
        public PaymentStatus Status
        {
            get
            {
                if (IsPaid)
                {
                    return PaymentStatus.Payed;
                }
                else if (Changes.Count > 0)
                {
                    return PaymentStatus.OnPayment;
                }
                else
                {
                    return PaymentStatus.New;
                }
            }
        }

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
