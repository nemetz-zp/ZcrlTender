﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenderLibrary;

namespace ZcrlTender.ViewModels
{
    // Запись в таблице списка смет
    public class EstimatesTableEntry
    {
        public Estimate Estimate { get; set; }
        public decimal YearSum { get; set; }
    }
}
