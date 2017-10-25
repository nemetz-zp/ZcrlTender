using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenderLibrary;

namespace ZcrlTender.ViewModels
{
    public class EstimateCellRecord
    {
        public MoneySource Source { get; set; }
        public decimal[,] PrimarySumValues { get; set; }
        public decimal[,] SecondarySumValues { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            EstimateCellRecord castedObj = obj as EstimateCellRecord;
            if (castedObj == null)
                return false;

            return Source.Id == castedObj.Source.Id;
        }

        public override int GetHashCode()
        {
            return Source.GetHashCode();
        }
    }
}
