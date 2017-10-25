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
        public int InvoiceId { get; set; }
        public DateTime Date { get; set; }
        public string Number { get; set; }
        public Contractor Contractor { get; set; }
        public Contract Contract { get; set; }
        public bool IsCredit { get; set; }
        public InvoiceStatus Status { get; set; }
        public string StatusString
        {
            get
            {
                string result = string.Empty;

                switch (Status)
                {
                    case InvoiceStatus.New:
                        result = "Новий";
                        break;
                    case InvoiceStatus.OnPayment:
                        result = "На оплаті";
                        break;
                    case InvoiceStatus.Payed:
                        result = "Сплачений";
                        break;
                }

                return result;
            }
        }
    }
}
