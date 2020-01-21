using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenderLibrary
{
    // Юридические обязательства по договору
    public class JuristicGuaranty
    {
        public long Id { get; set; }
        
        public virtual MoneySource Source { get; set; }
        public int MoneySourceId { get; set; }

        public virtual Contract Contract { get; set; }
        public virtual int ContractId { get; set; }

        public decimal ReservedSum { get; set; }
    }
}
