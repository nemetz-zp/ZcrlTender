using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenderLibrary;

namespace ZcrlTender.ViewModels
{
    // Деньги на КЕКВ
    public class KekvMoneyRecord
    {
        public KekvCode Kekv { get; set; }
        public decimal Sum { get; set; }
    }
}
