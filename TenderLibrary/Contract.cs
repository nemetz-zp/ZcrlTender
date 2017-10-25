using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenderLibrary
{
    // Договор
    public class Contract
    {
        public int Id { get; set; }

        public virtual KekvCode PrimaryKekv { get; set; }
        public int PrimaryKekvId { get; set; }

        public virtual Estimate Estimate { get; set; }
        public int EstimateId { get; set; }

        public virtual KekvCode SecondaryKekv { get; set; }
        public int SecondaryKekvId { get; set; }

        public virtual DkCode Dk { get; set; }
        public int DkCodeId { get; set; }

        public virtual Contractor Contractor { get; set; }
        public int ContractorId { get; set; }

        public string Number { get; set; }
        public decimal Sum { get; set; }

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

        public Contract()
        {
            RelatedFiles = new List<UploadedFile>();
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
    }
}
