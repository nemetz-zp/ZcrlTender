using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenderLibrary
{
    public class DkCode
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return Code + " - " + Name;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            DkCode castedObj = obj as DkCode;
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
