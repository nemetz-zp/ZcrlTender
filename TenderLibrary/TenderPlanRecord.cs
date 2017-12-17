using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenderLibrary
{
    // Изменение по кодам и КЕКВам в плане
    public class TenderPlanRecord
    {
        public int Id { get; set; }

        public virtual Estimate Estimate { get; set; }
        public int EstimateId { get; set; }

        public DateTime DateOfCreating { get; set; }
        public DateTime DateOfLastChange { get; set; }

        public virtual KekvCode PrimaryKekv { get; set; }
        public int PrimaryKekvId { get; set; }

        public virtual KekvCode SecondaryKekv { get; set; }
        public int SecondaryKekvId { get; set; }

        public virtual DkCode Dk { get; set; }
        public int DkCodeId { get; set; }

        public decimal Sum { get; set; }

        // Конкретное название предмета закупки
        public string ConcreteName { get; set; }

        public virtual ICollection<TenderPlanRecordChange> Changes { get; set; }

        public TenderPlanRecord()
        {
            Changes = new List<TenderPlanRecordChange>();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            TenderPlanRecord castedObj = obj as TenderPlanRecord;
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
