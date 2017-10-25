using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenderLibrary;

namespace ZcrlTender.ViewModels
{
    // Представление записи о договоре в таблице приложения
    public class ContractsTableEntry
    {
        public int ContractId { get; set; }
        public string ContractNum { get; set; }
        public DateTime ContractDate { get; set; }
        public string Description { get; set; }
        public Contractor Contractor { get; set; }
        public decimal FullSum { get; set; }
        public decimal UsedSum { get; set; }
        //public string Status { get; set; }
        public decimal Remain
        {
            get
            {
                return FullSum - UsedSum;
            }
        }
    }
}
