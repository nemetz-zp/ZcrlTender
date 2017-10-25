using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenderLibrary;

namespace ZcrlTender.ViewModels
{
    // Представление записи в таблице годового плана
    public class TenderPlanItemsTableEntry
    {
        // ID записи о коде в плане
        public int TenderPlanRecordId { get; set; }

        // Позиция в плане 
        public DkCode Dk { get; set; }

        // КЕКВ
        public KekvCode Kekv { get; set; }

        // Деньги заложенные на код
        public decimal MoneyOnCode { get; set; }

        // Деньги под которые зарегистрированы договора
        public decimal RegisteredByContracts { get; set; }

        // Деньги потраченные по договорам (и соответственно те, которые нельзя уменьшить) 
        public decimal UsedByContracts { get; set; }

        // Остаток денег по договорам
        public decimal ContractsMoneyRemain { get; set; }
    }
}
