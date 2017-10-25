using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenderLibrary;

namespace ZcrlTender
{
    public class MoneyRemainsRecord
    {
        public KekvCode Kekv { get; set; }
        public MoneySource MSource { get; set; }
        public decimal Sum { get; set; }
    }
}
