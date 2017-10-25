using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenderLibrary
{
    public class TenderPlanRecordChange
    {
        public long Id { get; set; }

        public virtual TenderPlanRecord Record { get; set; }
        public int TenderPlanRecordId { get; set; }

        public DateTime DateOfChange { get; set; }

        // Описание изменения в плане (комментарий)
        public string Description { get; set; }

        // Изменение сумы на коде
        public decimal ChangeOfSum { get; set; }

        public string ChangedConcreteName { get; set; }
    }
}
