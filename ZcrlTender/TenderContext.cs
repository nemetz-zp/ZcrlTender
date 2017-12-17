using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenderLibrary;
using System.Data.Entity;

namespace ZcrlTender
{
    public class TenderContext : ZcrlTenderContext
    {
        static TenderContext()
        {
            //Database.SetInitializer<TenderContext>(new TenderDbInitializer());
        }

        public TenderContext() : base("Default") { }
    }
}
