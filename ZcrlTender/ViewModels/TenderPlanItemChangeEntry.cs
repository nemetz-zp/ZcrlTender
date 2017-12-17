using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZcrlTender.ViewModels
{
    public class TenderPlanItemChangeEntry
    {
        public DateTime DateOfChange { get; set;}
        public decimal SumChange { get; set; }
        public string Kekv { get; set; }
        public string DkCode { get; set; }
        public string Description { get; set; }
    }
}
