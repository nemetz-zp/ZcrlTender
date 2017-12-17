using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenderLibrary
{
    public enum PaymentStatus
    {
        New,
        OnPayment,
        Payed
    }

    // Счёт-заказ
    public class Invoice
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }
        public string Number { get; set; }

        public bool IsCredit { get; set; }

        public bool IsPaid { get; set; }

        // Договор
        public virtual Contract Contract { get; set; }
        public int ContractId { get; set; }

        // Краткое описание заказа
        public string Description { get; set; }

        // Общая сумма заказа
        public decimal Sum { get; set; }

        public virtual ICollection<BalanceChanges> Changes { get; set; }
        public virtual ICollection<UploadedFile> RelatedFiles { get; set; }

        [NotMapped]
        public PaymentStatus Status
        {
            get
            {
                if(IsPaid)
                {
                    return PaymentStatus.Payed;
                }
                else if(Changes.Count > 0)
                {
                    return PaymentStatus.OnPayment;
                }
                else
                {
                    return PaymentStatus.New;
                }
            }
        }

        public Invoice()
        {
            Changes = new List<BalanceChanges>();
            RelatedFiles = new List<UploadedFile>();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Invoice castedObj = obj as Invoice;
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
