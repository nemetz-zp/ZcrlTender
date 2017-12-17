using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenderLibrary;

namespace ZcrlTender.ViewModels
{
    // Средтва заложенные сметой на конкретном источнике финансирования
    public class EstimateMoneyOnSourceTable
    {
        // Источник финансирования
        public MoneySource Source { get; set; }

        // Поступление средств по основной системе (первый индекс - КЕКВ, второй индекс - месяц)
        public decimal[,] PrimarySumValues { get; set; }

        // Поступление средств по альтернативной системе
        public decimal[,] SecondarySumValues { get; set; }

        public static List<EstimateMoneyOnSourceTable> GetEstimateMoneyTable(Estimate currentEstimate, 
            IList<KekvCode> kekvsList, 
            string[] monthes,
            Func<BalanceChanges, bool> filter,
            Action<EstimateMoneyOnSourceTable> handler = null)
        {
            List<EstimateMoneyOnSourceTable> resultList = new List<EstimateMoneyOnSourceTable>();
            using(TenderContext tc = new TenderContext())
            {
                var allEstimatesRecords = (from item in tc.BalanceChanges.ToList()
                                           where (item.EstimateId == currentEstimate.Id) && filter(item)
                                           group item by item.MoneySource).ToList();

                foreach (var rec in allEstimatesRecords)
                {
                    EstimateMoneyOnSourceTable record = new EstimateMoneyOnSourceTable();
                    record.Source = rec.Key;
                    record.PrimarySumValues = new decimal[kekvsList.Count, monthes.Length];
                    record.SecondarySumValues = new decimal[kekvsList.Count, monthes.Length];

                    foreach (var item in rec)
                    {
                        int rowIndex = kekvsList.IndexOf(item.PrimaryKekv);
                        int columnIndex = item.DateOfReceiving.Month - 1;
                        record.PrimarySumValues[rowIndex, columnIndex] = item.PrimaryKekvSum;
                        record.SecondarySumValues[rowIndex, columnIndex] = item.SecondaryKekvSum;
                    }

                    resultList.Add(record);

                    if(handler != null)
                    {
                        handler(record);
                    }
                }
            }

            return resultList;
        }

        public static string CheckPlannedKekvSum(IList<EstimateMoneyOnSourceTable> moneyList, 
            IList<KekvMoneyRecord> plannedMoneyOnKekv,
            IList<KekvCode> kekvsList,
            string[] monthes)
        {
            string result = null;

            if(plannedMoneyOnKekv == null || (plannedMoneyOnKekv.Count == 0))
            {
                return result;
            }

            foreach(var kekv in plannedMoneyOnKekv)
            {
                int kekvIndex = kekvsList.IndexOf(kekv.Kekv);
                
                decimal estimateMoneyOnKekv = 0;
                foreach (var moneySource in moneyList)
                {
                    for (int j = 0; j < monthes.Length; j++)
                    {
                        estimateMoneyOnKekv += moneySource.PrimarySumValues[kekvIndex, j];
                    }
                }

                if(estimateMoneyOnKekv < kekv.Sum)
                {
                    result = string.Format("У новій редакції кошторису фінансування по КЕКВ {0} складає {1} грн., " + 
                        "але по річному плану по данному КЕКВ заплановано використати {2} грн.\n" +
                        "Для внесення вказаних змін у кошторис потрібно спочатку зменшити заплановану суму по КЕКВ {0} на {3} грн.",
                        kekv.Kekv.Code, estimateMoneyOnKekv, kekv.Sum, (kekv.Sum - estimateMoneyOnKekv));
                    return result;
                }

            }

            return result;
        }


        /// <summary>
        /// Поиск отрицательных остатков как результата разности поступлений по смете и расходов
        /// </summary>
        /// <param name="moneyList">Список поступлений по смете</param>
        /// <param name="spendingList">Список расходов по смете</param>
        /// <param name="kekvsList">Список КЕКВов</param>
        /// <param name="monthes">Список месяцев</param>
        /// <returns>Сообщение о местонахождении отрицательного остатка. Возвращает null, если отрицательные остатки не найдены</returns>
        public static string FindIncorrectSpending(IList<EstimateMoneyOnSourceTable> moneyList, 
            IList<EstimateMoneyOnSourceTable> spendingList, 
            IList<KekvCode> kekvsList, 
            string[] monthes)
        {
            string result = null;

            if(spendingList == null || (spendingList.Count == 0))
            {
                return result;
            }

            int kekvsNum = kekvsList.Count;
            int monthesNum = monthes.Length;
            foreach(var spen in spendingList)
            {
                EstimateMoneyOnSourceTable moneyOnSource = moneyList.First(p => p.Source.Equals(spen.Source));
                if (moneyOnSource == null)
                {
                    result = string.Format("У новій редакції кошторису відсутнє джерело фінансування '{0}' за яким у поточному році наявні фактичні витрати", spen.Source.Name);
                    return result;
                }

                for(int i = 0; i < kekvsNum; i++)
                {
                    decimal currentMonthRemain = 0;
                    for(int j = 0; j < monthesNum; j++)
                    {
                        currentMonthRemain += moneyOnSource.PrimarySumValues[i, j] + spen.PrimarySumValues[i, j];

                        if(currentMonthRemain < 0)
                        {
                            result = string.Format("У новій редакції наявні залишки менші за нуль.\nДеталі:\nДжерело: {0}\nКЕКВ: {1}\nМісяць: {2}\nЗалишок: {3:N2}", 
                                spen.Source.Name, kekvsList[i], monthes[j], currentMonthRemain);
                            return result;
                        }
                    }
                }
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            EstimateMoneyOnSourceTable castedObj = obj as EstimateMoneyOnSourceTable;
            if (castedObj == null)
                return false;

            return Source.Id == castedObj.Source.Id;
        }

        public override int GetHashCode()
        {
            return Source.GetHashCode();
        }
    }
}
