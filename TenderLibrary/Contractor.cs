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

        // Комментарий к контрагенту
        public string Description { get; set; }

        public virtual ICollection<UploadedFile> RelatedFiles { get; set; }
        public virtual ICollection<ContactPerson> Persons { get; set; }

        public Contractor()
        {
            RelatedFiles = new List<UploadedFile>();
            Persons = new List<ContactPerson>();
        }

        public override string ToString()
        {
            return ShortName.ToString();
        }

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
