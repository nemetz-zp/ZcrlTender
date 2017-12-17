using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenderLibrary;

namespace ZcrlTender.ViewModels
{
    public class PlannedSpendingTableEntry
    {
        public DateTime Date { get; set; }
        public PlannedSpending RelatedPlannedSpendingRecord { get; set; }
        public string KekvCode { get; set; }
        public string Description { get; set; }
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
