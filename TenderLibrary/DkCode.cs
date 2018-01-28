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
                return this.Code + " " + this.Name;
            }
        }

        // Сравнение кодов ДК по конкретному количеству цифр слева
        public bool CompareDkByDigit(DkCode anotherCode, int digitsNum)
        {
            bool result = false;

            if(digitsNum < 9)
            {
                result = this.Code.Substring(0, digitsNum).Equals(anotherCode.Code.Substring(0, digitsNum));
            }
            else
            {
                result = (this.Id == anotherCode.Id);
            }

            return result;
        }

        public override string ToString()
        {
            return FullName;
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
