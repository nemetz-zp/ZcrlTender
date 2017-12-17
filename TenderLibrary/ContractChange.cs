using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenderLibrary
{
    // Изменение договора
    public class ContractChange
    {
        public long Id { get; set; }

        public DateTime DateOfChange { get; set; }

        public string Description { get; set; }

        public virtual Contract Contract { get; set; }
        public int ContractId { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            ContractChange castedObj = obj as ContractChange;
            if (castedObj == null)
                return false;

            return (Id == castedObj.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
