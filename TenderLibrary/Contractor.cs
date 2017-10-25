using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenderLibrary
{
    public class Contractor
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }

        // Код по ЕДРПОУ
        public string EdrCode { get; set; }

        // Юридический адрес
        public string LegalAddress { get; set; }

        // Фактический адрес
        public string ActualAddress { get; set; }

        // Контакты
        public string ContactPhone { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Contractor castedObj = obj as Contractor;
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
