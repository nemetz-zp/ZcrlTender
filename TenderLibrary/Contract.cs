using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenderLibrary
{
    // Статус договора
    public enum ContractStatus
    {
        Active,     // Активный (в работе)
        Complete    // Выполненый или расторгнутый
    }
    // Договор
    public class Contract
    {
        public int Id { get; set; }

        public virtual TenderPlanRecord RecordInPlan { get; set; }
        public int TenderPlanRecordId { get; set; }

        public virtual Contractor Contractor { get; set; }
        public int ContractorId { get; set; }

        public string Number { get; set; }
        public decimal Sum { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Number))
                {
                    return Description;
                }
                else
                {
                    return string.Format("№ {0} від {1}", Number, SignDate.ToShortDateString());
                }
            }
        }

        // Примечание (комментарий) к договору
        public string Description { get; set; }

        // Даты подписания, вступления в силу и окончания
        public DateTime SignDate { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }

        // Связанные файлы
        public virtual ICollection<UploadedFile> RelatedFiles { get; set; }
        // Счёта взятые по договору
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<ContractChange> ContractChanges { get; set; }
        public virtual ICollection<JuristicGuaranty> Guaranties { get; set; }

        public Contract()
        {
            RelatedFiles = new List<UploadedFile>();
            Invoices = new List<Invoice>();
            ContractChanges = new List<ContractChange>();
            Guaranties = new List<JuristicGuaranty>();
        }

        [NotMapped]
        public ContractStatus Status
        {
            get
            {
                if(Sum == 0)
                {
                    return ContractStatus.Complete;
                }
                else if ((Invoices.Count > 0) && (Invoices.Sum(p => p.Sum) == Sum))
                {
                    return ContractStatus.Complete;
                }
                else
                {
                    return ContractStatus.Active;
                }
            }
        }

        // Использованные деньги
        [NotMapped]
        public decimal UsedMoney
        {
            get
            {
                return Invoices.Select(p => p.Sum).DefaultIfEmpty(0).Sum();
            }
        }

        // Остаток средств на договоре
        [NotMapped]
        public decimal MoneyRemain
        {
            get
            {
                return Sum - UsedMoney;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Contract castedObj = obj as Contract;
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
            return FullName.ToString();
        }
    }
}
