using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenderLibrary;

namespace ZcrlTender.ViewModels
{
    public class InvoiceRecordsTableEntry
    {
        public Invoice RelatedInvoice { get; set; }
        public DateTime Date { get; set; }
        public string Number { get; set; }
        public Contractor Contractor { get; set; }
        public Contract Contract { get; set; }
        public bool IsCredit { get; set; }

        public string IsCreditString
        {
            get
            {
                if(IsCredit)
                {
                    return "БОРГ";
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public decimal Sum { get; set; }
        public PaymentStatus Status { get; set; }
        public string StatusString
        {
            get
            {
                string result = string.Empty;

                switch (Status)
                {
                    case PaymentStatus.New:
                        result = "Новий";
                        break;
                    case PaymentStatus.OnPayment:
                        result = "На оплаті";
                        break;
                    case PaymentStatus.Payed:
                        result = "Сплачений";
                        break;
                }

                return result;
            }
        }
    }
}
