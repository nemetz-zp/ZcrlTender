using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenderLibrary
{
    public class KekvCode
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            KekvCode castedObj = obj as KekvCode;
            if (castedObj == null)
                return false;

            return (Id == castedObj.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Code;
        }
    }
}
