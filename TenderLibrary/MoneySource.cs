using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenderLibrary
{
    // Источник финансирования (местный бюджет, медицинская субвенция и т.д.)
    public class MoneySource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual TenderYear Year { get; set; }
        public virtual int TenderYearId { get; set; }

        public virtual ICollection<JuristicGuaranty> Guaranties { get; set; }

        public MoneySource()
        {
            Guaranties = new List<JuristicGuaranty>();
        }

        // Приоритет отображения источника
        public int ViewPriority { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            MoneySource castedObj = obj as MoneySource;
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
