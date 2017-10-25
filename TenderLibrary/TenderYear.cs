using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenderLibrary
{
    public class TenderYear
    {
        public int Id { get; set; }
        public short Year { get; set; }
        public string Description { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                if(!string.IsNullOrWhiteSpace(Description))
                {
                    return string.Format("{0} ({1})", Year, Description);
                }
                else
                {
                    return string.Format("{0}", Year);
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            TenderYear castedObj = obj as TenderYear;
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
