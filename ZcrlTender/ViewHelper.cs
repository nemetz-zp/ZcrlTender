using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenderLibrary;

namespace ZcrlTender
{
    public class ViewHelper
    {
        public string PrintPaymentStatus(PaymentStatus status)
        {
            string result = string.Empty;

            switch(status)
            {
                case PaymentStatus.New:
                    result = "Новий";
                    break;
                case PaymentStatus.OnPayment:
                    result = "На оплаті";
                    break;
                case PaymentStatus.Payed:
                    result = "Сплачен";
                    break;
            }

            return result;
        }

    }
}
