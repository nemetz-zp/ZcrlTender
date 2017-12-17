using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenderLibrary;
using Excel = Microsoft.Office.Interop.Excel;

namespace ZcrlTender.ExcelReports
{
    public class EstimateYearSpendingReport : ExcelReportMaker
    {
        private Estimate est;
        private TenderYear year;
        private bool isNewSystem;
        private int reportTitleRow = 2;
        private int yearCell = 3;
        private int dateCell = 6;
        private int estimateNameCell = 4;
        private string numColumnLetter = "A";
        private string moneySourceSpendingBeginColumn = "F";
        private string lastMoneySourceColumnLetter = "F";
        private string totalMoneySourceColumnLetter = "F";
        private string dateColumnLetter = "B";
        private string kekvColumnLetter = "C";
        private string documentColumnLetter = "D";
        private string contractColumnLetter = "E";
        private string currentSourceColumnLetter = "F";
        private List<MoneySource> sources;
        private int sourcesNum = 0;
        private int beginRowNumber = 8;
        private int currentRowNumber = 8;

        // Список ячеек с названиями месяцев
        private List<int> monthesHeaders = new List<int>();

        private string[] monthes = { 
                                     "Січень", "Лютий", "Березень", "Квітень", "Травень", "Червень", 
                                     "Липень", "Серпень", "Вересень", "Жовтень", "Листопад", "Грудень" 
                                   };

        // Словари с итогами до конца года (string здесь адреса ячеек таблицы)
        // Словарь-итог сметы по затратам
        Dictionary<KekvCode, List<string>> estimateKekvSpendingTotals = new Dictionary<KekvCode, List<string>>();
        // Словарь-итог сметы по финансированию
        Dictionary<KekvCode, List<string>> estimateKekvPlannedMoneyTotals = new Dictionary<KekvCode, List<string>>();
        // Словарь-итог сметы по остаткам средст
        Dictionary<KekvCode, List<string>> estimateKekvMoneyRemainTotals = new Dictionary<KekvCode, List<string>>();

        // Список ячеек с затратами по КЕКВ с пределах месяца
        List<string> monthSpendingCells = new List<string>();
        // Список ячеек с финансованием по КЕКВ с пределах месяца
        List<string> monthPlannedMoneyCells = new List<string>();
        // Список ячеек с остатками средсты по КЕКВ с пределах месяца
        List<string> lastMonthRemainCells = new List<string>();

        private void LoadMoneySourcesList()
        {
            using (TenderContext tc = new TenderContext())
            {
                if (est != null)
                {
                    sources = tc.BalanceChanges.Where(p => p.EstimateId == est.Id).GroupBy(p => p.MoneySource).Select(p => p.Key).ToList();
                }
                else
                {
                    sources = tc.BalanceChanges.Where(p => p.Estimate.TenderYearId == year.Id).GroupBy(p => p.MoneySource).Select(p => p.Key).ToList();
                }

                sourcesNum = sources.Count;
            }
        }

        public EstimateYearSpendingReport(TenderYear year, bool isNewSystem = true)
        {
            this.year = year;
            this.isNewSystem = isNewSystem;
            LoadMoneySourcesList();
        }

        public EstimateYearSpendingReport(Estimate est, bool isNewSystem = true)
        {
            this.est = est;
            this.year = est.Year;
            this.isNewSystem = isNewSystem;
            LoadMoneySourcesList();
        }

        // Строка таблицы-отчёта ещемесячных затрат по смете
        private class EstimateSpendingRow
        {
            public static string PLANNED_MONEY_TEXT = "ВИДІЛЕНО ЗА КОШТОРИСОМ";
            public static string PREV_MONTH_MONEY_REMAIN_TEXT = "ЗАЛИШОК КОШТІВ З ПОПЕРЕДНЬОГО МІСЯЦЯ";

            public KekvCode Kekv { get; set; }
            public string Contract { get; set; }
            public string SpendingDescription { get; set; }
            public DateTime Date { get; set; }

            public BalanceChangeType Type { get; set; }

            public decimal[] SpendingList { get; set; }

            public override bool Equals(object obj)
            {
                return Kekv.Equals(obj);
            }

            public override int GetHashCode()
            {
                return Kekv.GetHashCode();
            }
        }

        // Сгруппированные по КЕКВ записи тратах
        private class GroupedByKekvEstimateSpendingRow
        {
            public KekvCode Kekv { get; set; }

            public List<EstimateSpendingRow> SpendingRows { get; set; }

            // Запланированные средства по смете
            public EstimateSpendingRow PlannedMoneysRow { get; set; }

            public GroupedByKekvEstimateSpendingRow()
            {
                SpendingRows = new List<EstimateSpendingRow>();
                PlannedMoneysRow = new EstimateSpendingRow();
            }
        }

        private enum BalanceChangeType
        {
            InvoiceSpending,
            PlannedSpending,
            PlannedMoney,
            RemainFromPrevMonth
        }

        private class EstimateMonthSpending
        {
            public List<GroupedByKekvEstimateSpendingRow> KekvRows { get; set; }
            public EstimateMonthSpending()
            {
                KekvRows = new List<GroupedByKekvEstimateSpendingRow>();
            }
        }

        private List<EstimateMonthSpending> GetEstimateMonthesReportList()
        {
            using (TenderContext tc = new TenderContext())
            {
                List<BalanceChanges> estimateBalanceChanges;
                if (est != null)
                {
                    estimateBalanceChanges = tc.BalanceChanges.Where(p => p.EstimateId == est.Id).ToList();
                }
                else
                {
                    estimateBalanceChanges = tc.BalanceChanges.Where(p => p.Estimate.TenderYearId == year.Id).ToList();
                }

                // Затраты по счетам
                List<EstimateSpendingRow> invoices;
                // Запланированные траты
                List<EstimateSpendingRow> plannedSpending;
                // Деньги поступившие по смете за месяц
                List<EstimateSpendingRow> estimateMoneyOnMonth;
                // Список КЕКВ с остатками на начало месяца
                List<EstimateSpendingRow> kekvsWithRemainOnMonthBegin;

                if (isNewSystem)
                {
                    invoices = (from spen in estimateBalanceChanges
                                where (spen.InvoiceId != null)
                                group spen by new { spen.PrimaryKekv, spen.Invoice, spen.DateOfReceiving } into g1
                                select new EstimateSpendingRow
                                {
                                    Date = g1.Key.DateOfReceiving,
                                    Kekv = g1.Key.PrimaryKekv,
                                    SpendingDescription = string.Format("Рахунок {0} від {1} року\n({2})",
                                        g1.Key.Invoice.Number, g1.Key.Invoice.Date.ToShortDateString(), g1.Key.Invoice.Description),
                                    Contract = string.Format("Договір № {0} від {1} року\n({2})\n{3}",
                                        g1.Key.Invoice.Contract.Number, g1.Key.Invoice.Contract.SignDate.ToShortDateString(),
                                        g1.Key.Invoice.Contract.Description, g1.Key.Invoice.Contract.Contractor.ShortName),
                                    Type = BalanceChangeType.InvoiceSpending,
                                    SpendingList = GetMoneySourceSpendingRow(sources, g1.Select(p => new MoneySourceSpending
                                    {
                                        Source = p.MoneySource,
                                        Sum = -p.PrimaryKekvSum
                                    }).ToList())
                                }).ToList();
                    plannedSpending = (from spen in estimateBalanceChanges
                                      where (spen.PlannedSpendingId != null)
                                      group spen by new { spen.PrimaryKekv, spen.PlannedSpending, spen.DateOfReceiving } into g1
                                      select new EstimateSpendingRow
                                      {
                                          Date = g1.Key.DateOfReceiving,
                                          Kekv = g1.Key.PrimaryKekv,
                                          SpendingDescription = g1.Key.PlannedSpending.Description,
                                          Contract = "-",
                                          Type = BalanceChangeType.PlannedSpending,
                                          SpendingList = GetMoneySourceSpendingRow(sources, g1.Select(p => new MoneySourceSpending
                                          {
                                              Source = p.MoneySource,
                                              Sum = -p.PrimaryKekvSum
                                          }).ToList())
                                      }).ToList();
                    estimateMoneyOnMonth = (from spen in estimateBalanceChanges
                                            where (spen.PlannedSpendingId == null) && (spen.InvoiceId == null) && (spen.PrimaryKekvSum > 0)
                                            group spen by new { spen.PrimaryKekv, spen.DateOfReceiving.Month } into g1
                                            select new EstimateSpendingRow
                                            {
                                                Date = new DateTime(year.Year, g1.Key.Month, 1),
                                                Kekv = g1.Key.PrimaryKekv,
                                                SpendingDescription = EstimateSpendingRow.PLANNED_MONEY_TEXT,
                                                Contract = "-",
                                                Type = BalanceChangeType.PlannedMoney,
                                                SpendingList = GetMoneySourceSpendingRow(sources, g1.Select(p => new MoneySourceSpending
                                                {
                                                    Source = p.MoneySource,
                                                    Sum = p.PrimaryKekvSum
                                                }).ToList())
                                            }).ToList();

                }
                else
                {
                    invoices = (from spen in estimateBalanceChanges
                                where (spen.InvoiceId != null)
                                group spen by new { spen.SecondaryKekv, spen.Invoice, spen.DateOfReceiving } into g1
                                select new EstimateSpendingRow
                                {
                                    Date = g1.Key.DateOfReceiving,
                                    Kekv = g1.Key.SecondaryKekv,
                                    SpendingDescription = string.Format("Рахунок {0} від {1} року\n({2})",
                                        g1.Key.Invoice.Number, g1.Key.Invoice.Date.ToShortDateString(), g1.Key.Invoice.Description),
                                    Contract = string.Format("Договір № {0} від {1} року\n({2})\n{3}",
                                        g1.Key.Invoice.Contract.Number, g1.Key.Invoice.Contract.SignDate.ToShortDateString(),
                                        g1.Key.Invoice.Contract.Description, g1.Key.Invoice.Contract.Contractor.ShortName),
                                    Type = BalanceChangeType.InvoiceSpending,   
                                    SpendingList = GetMoneySourceSpendingRow(sources, g1.Select(p => new MoneySourceSpending
                                    {
                                        Source = p.MoneySource,
                                        Sum = -p.SecondaryKekvSum
                                    }).ToList())
                                }).ToList();
                    plannedSpending = (from spen in estimateBalanceChanges
                                      where (spen.PlannedSpendingId != null)
                                      group spen by new { spen.SecondaryKekv, spen.PlannedSpending, spen.DateOfReceiving } into g1
                                      select new EstimateSpendingRow
                                      {
                                          Date = g1.Key.DateOfReceiving,
                                          Kekv = g1.Key.SecondaryKekv,
                                          SpendingDescription = g1.Key.PlannedSpending.Description,
                                          Contract = "-",
                                          Type = BalanceChangeType.PlannedSpending,
                                          SpendingList = GetMoneySourceSpendingRow(sources, g1.Select(p => new MoneySourceSpending
                                          {
                                              Source = p.MoneySource,
                                              Sum = -p.SecondaryKekvSum
                                          }).ToList())
                                      }).ToList();
                    estimateMoneyOnMonth = (from spen in estimateBalanceChanges
                                            where (spen.PlannedSpendingId == null) && (spen.InvoiceId == null) && (spen.SecondaryKekvSum > 0)
                                            group spen by new { spen.SecondaryKekv, spen.DateOfReceiving.Month } into g1
                                            select new EstimateSpendingRow
                                            {
                                                Date = new DateTime(year.Year, g1.Key.Month, 1),
                                                Kekv = g1.Key.SecondaryKekv,
                                                SpendingDescription = EstimateSpendingRow.PLANNED_MONEY_TEXT,
                                                Contract = "-",
                                                Type = BalanceChangeType.PlannedMoney,
                                                SpendingList = GetMoneySourceSpendingRow(sources, g1.Select(p => new MoneySourceSpending
                                                {
                                                    Source = p.MoneySource,
                                                    Sum = p.SecondaryKekvSum
                                                }).ToList())
                                            }).ToList();
                }


                List<EstimateSpendingRow> allSpendings = invoices.Union(plannedSpending)
                    .Union(estimateMoneyOnMonth)
                    .ToList();

                List<EstimateMonthSpending> result = new List<EstimateMonthSpending>();
                for (int i = 1; i <= 12; i++)
                {
                    if(isNewSystem)
                    {
                        kekvsWithRemainOnMonthBegin = (from spen in estimateBalanceChanges
                                where (spen.DateOfReceiving.Month <= i)
                                group spen by new { spen.PrimaryKekv, spen.DateOfReceiving.Month } into g1
                                select new { Kekv = g1.Key.PrimaryKekv, Sum = g1.Sum(p => p.PrimaryKekvSum) } into s1
                                where s1.Sum > 0
                                select new EstimateSpendingRow
                                {
                                    Kekv = s1.Kekv,
                                    Date = new DateTime(Convert.ToInt32(year.Year), i, 1),
                                    Type = BalanceChangeType.RemainFromPrevMonth,
                                }).ToList();
                    }
                    else
                    {
                        kekvsWithRemainOnMonthBegin = (from spen in estimateBalanceChanges
                                where (spen.DateOfReceiving.Month <= i)
                                group spen by new { spen.SecondaryKekv, spen.DateOfReceiving.Month } into g1
                                select new { Kekv = g1.Key.SecondaryKekv, Sum = g1.Sum(p => p.SecondaryKekvSum) } into s1
                                where s1.Sum > 0
                                select new EstimateSpendingRow
                                {
                                    Kekv = s1.Kekv,
                                    Date = new DateTime(Convert.ToInt32(year.Year), i, 1),
                                    Type = BalanceChangeType.RemainFromPrevMonth,
                                }).Distinct().ToList();
                    }

                    EstimateMonthSpending month = new EstimateMonthSpending();
                    month.KekvRows = (from item in allSpendings.Union(kekvsWithRemainOnMonthBegin)
                                      where (item.Date.Month == i)
                                      group item by item.Kekv into g1
                                      select new GroupedByKekvEstimateSpendingRow
                                      {
                                          Kekv = g1.Key,
                                          SpendingRows = g1.Where(k => (k.Type == BalanceChangeType.InvoiceSpending) || (k.Type == BalanceChangeType.PlannedSpending)).ToList(),
                                          PlannedMoneysRow = g1.Where(p => (p.Date.Month == i) && (p.Type == BalanceChangeType.PlannedMoney))
                                                             .DefaultIfEmpty(new EstimateSpendingRow 
                                                              { 
                                                                  Kekv = g1.Key, 
                                                                  SpendingList = new decimal[sourcesNum] 
                                                              }).First()
                                      }).ToList();
                    result.Add(month);
                }

                return result;
            }
        }

        private enum TotalsType
        {
            KekvTotal,
            PeriodTotal
        }

        private void WriteTotals(TotalsType type,
            KekvCode kekv = null, 
            string totalSpendingFormula = null,
            string prevMonthRemainFormula = null,
            string plannedSpendingFormula = null,
            decimal[] plannedSpendingRow = null)
        {
            // Итоги по затратам
            xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString(),
                lastMoneySourceColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
            if (type == TotalsType.KekvTotal)
            {
                xlWorksheet.get_Range(kekvColumnLetter + currentRowNumber.ToString()).Value = kekv.Code;
            }
            else
            {
                xlWorksheet.get_Range(kekvColumnLetter + currentRowNumber.ToString()).Value = "ВСІ";
            }
            xlWorksheet.get_Range(documentColumnLetter + currentRowNumber.ToString()).Value = "ВСЬОГО ВИТРАЧЕНО";
            currentSourceColumnLetter = moneySourceSpendingBeginColumn;
            for (int j = 0; j < sourcesNum; j++)
            {
                if (totalSpendingFormula != null)
                {
                    xlWorksheet.get_Range(currentSourceColumnLetter + currentRowNumber.ToString()).Formula =
                        string.Format(totalSpendingFormula, currentSourceColumnLetter);
                }
                else
                {
                    xlWorksheet.get_Range(currentSourceColumnLetter + currentRowNumber.ToString()).Value = 0;
                }

                if (j != (sourcesNum - 1))
                {
                    currentSourceColumnLetter = GetNextColumnLettter(currentSourceColumnLetter);
                }
            }
            xlWorksheet.get_Range(totalMoneySourceColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
            xlWorksheet.get_Range(totalMoneySourceColumnLetter + currentRowNumber.ToString()).Formula
                    = string.Format("=SUM({1}{0}:{2}{0})", currentRowNumber, moneySourceSpendingBeginColumn, lastMoneySourceColumnLetter);
            if (type == TotalsType.KekvTotal)
            {
                estimateKekvSpendingTotals[kekv].Add("{0}" + currentRowNumber);
                monthSpendingCells.Add("{0}" + currentRowNumber);
            }
            currentRowNumber++;

            // Сколько было заложено сметой
            xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString(),
                lastMoneySourceColumnLetter + currentRowNumber.ToString()).Font.Bold = true;

            if (type == TotalsType.KekvTotal)
            {
                xlWorksheet.get_Range(kekvColumnLetter + currentRowNumber.ToString()).Value = kekv.Code;
            }
            else
            {
                xlWorksheet.get_Range(kekvColumnLetter + currentRowNumber.ToString()).Value = "ВСІ";
            }
            xlWorksheet.get_Range(documentColumnLetter + currentRowNumber.ToString()).Value = "ВИДІЛЕНО ЗА КОШТОРИСОМ";
            currentSourceColumnLetter = moneySourceSpendingBeginColumn;
            for (int j = 0; j < sourcesNum; j++)
            {
                if (plannedSpendingFormula != null)
                {
                    xlWorksheet.get_Range(currentSourceColumnLetter + currentRowNumber.ToString()).Formula =
                        string.Format(plannedSpendingFormula, currentSourceColumnLetter);
                }
                else
                {
                    xlWorksheet.get_Range(currentSourceColumnLetter + currentRowNumber.ToString()).Value 
                        = plannedSpendingRow[j];
                }
                if (j != (sourcesNum - 1))
                {
                    currentSourceColumnLetter = GetNextColumnLettter(currentSourceColumnLetter);
                }
            }
            xlWorksheet.get_Range(totalMoneySourceColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
            xlWorksheet.get_Range(totalMoneySourceColumnLetter + currentRowNumber.ToString()).Formula
                    = string.Format("=SUM({1}{0}:{2}{0})", currentRowNumber, moneySourceSpendingBeginColumn, lastMoneySourceColumnLetter);

            if (type == TotalsType.KekvTotal)
            {
                estimateKekvPlannedMoneyTotals[kekv].Add("{0}" + currentRowNumber);
                monthPlannedMoneyCells.Add("{0}" + currentRowNumber);
            }
            currentRowNumber++;

            // Сколько осталось с предыдущего месяца
            if (prevMonthRemainFormula != null)
            {
                xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString(),
                    lastMoneySourceColumnLetter + currentRowNumber.ToString()).Font.Bold = true;

                if (type == TotalsType.KekvTotal)
                {
                    xlWorksheet.get_Range(kekvColumnLetter + currentRowNumber.ToString()).Value = kekv.Code;
                }
                else
                {
                    xlWorksheet.get_Range(kekvColumnLetter + currentRowNumber.ToString()).Value = "ВСІ";
                }
                xlWorksheet.get_Range(documentColumnLetter + currentRowNumber.ToString()).Value = "ЗАЛИШОК З ПОПЕРЕДНЬОГО МІСЯЦЯ";
                currentSourceColumnLetter = moneySourceSpendingBeginColumn;
                for (int j = 0; j < sourcesNum; j++)
                {
                    xlWorksheet.get_Range(currentSourceColumnLetter + currentRowNumber.ToString()).Formula =
                        string.Format(prevMonthRemainFormula, currentSourceColumnLetter);
                    if (j != (sourcesNum - 1))
                    {
                        currentSourceColumnLetter = GetNextColumnLettter(currentSourceColumnLetter);
                    }
                }
                xlWorksheet.get_Range(totalMoneySourceColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
                xlWorksheet.get_Range(totalMoneySourceColumnLetter + currentRowNumber.ToString()).Formula
                        = string.Format("=SUM({1}{0}:{2}{0})", currentRowNumber, moneySourceSpendingBeginColumn, lastMoneySourceColumnLetter);
                lastMonthRemainCells.Add("{0}" + currentRowNumber);
                currentRowNumber++;
            }

            // Сколько осталось по истечению месяца (либо года)
            xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString(),
                lastMoneySourceColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
            if (type == TotalsType.KekvTotal)
            {
                xlWorksheet.get_Range(kekvColumnLetter + currentRowNumber.ToString()).Value = kekv.Code;
            }
            else
            {
                xlWorksheet.get_Range(kekvColumnLetter + currentRowNumber.ToString()).Value = "ВСІ";
            }
            xlWorksheet.get_Range(documentColumnLetter + currentRowNumber.ToString()).Value = "ЗАЛИШОК НА КІНЕЦЬ ПОТОЧНОГО МІСЯЦЯ";
            currentSourceColumnLetter = moneySourceSpendingBeginColumn;
            for (int j = 0; j < sourcesNum; j++)
            {
                if (prevMonthRemainFormula != null)
                {
                    xlWorksheet.get_Range(currentSourceColumnLetter + currentRowNumber.ToString()).Formula
                        = string.Format("={0}{1} + {0}{2} - {0}{3}",
                            currentSourceColumnLetter,
                            (currentRowNumber - 2),
                            (currentRowNumber - 1),
                            (currentRowNumber - 3));
                }
                else
                {
                    xlWorksheet.get_Range(currentSourceColumnLetter + currentRowNumber.ToString()).Formula
                        = string.Format("={0}{1} - {0}{2}",
                            currentSourceColumnLetter,
                            (currentRowNumber - 1),
                            (currentRowNumber - 2));
                }
                if (j != (sourcesNum - 1))
                {
                    currentSourceColumnLetter = GetNextColumnLettter(currentSourceColumnLetter);
                }
            }
            xlWorksheet.get_Range(totalMoneySourceColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
            xlWorksheet.get_Range(totalMoneySourceColumnLetter + currentRowNumber.ToString()).Formula
                    = string.Format("=SUM({1}{0}:{2}{0})", currentRowNumber, moneySourceSpendingBeginColumn, lastMoneySourceColumnLetter);
            if (type == TotalsType.KekvTotal)
            {
                estimateKekvMoneyRemainTotals[kekv].Add("{0}" + currentRowNumber);
            }
            currentRowNumber++;
        }

        private void WriteCaption(string caption)
        {
            xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString(),
                lastMoneySourceColumnLetter + currentRowNumber.ToString()).Merge();
            xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString(),
                lastMoneySourceColumnLetter + currentRowNumber.ToString()).Font.Italic = true;
            xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString(),
                lastMoneySourceColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
            xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString(),
               lastMoneySourceColumnLetter + currentRowNumber.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString(),
               lastMoneySourceColumnLetter + currentRowNumber.ToString()).Value = caption;
            currentRowNumber++;
        }

        // Смета с фактическими тратами по месяцам
        public override void MakeReport()
        {
            OpenExcelFile(FileManager.EstimateMonthSpendingTemplateFile);

            using (TenderContext tc = new TenderContext())
            {
                string estimateName = string.Empty;
                if (est != null)
                {
                    estimateName = est.Name;
                }
                else
                {
                    estimateName = "Всі кошториси від початку року";
                }

                xlWorksheet.get_Range(numColumnLetter + yearCell).Value = string.Format("на {0} рік", year.Year);
                xlWorksheet.get_Range(numColumnLetter + estimateNameCell).Value = string.Format("Кошторис: \"{0}\"", estimateName);
                xlWorksheet.get_Range(numColumnLetter + dateCell).Value = string.Format("Інформація станом на {0} року", DateTime.Now.ToShortDateString());
                
                // Указываем источники финансирования
                for (int i = 0; i < sourcesNum; i++)
                {
                    xlWorksheet.get_Range(lastMoneySourceColumnLetter + beginRowNumber.ToString()).Font.Bold = true;
                    xlWorksheet.get_Range(lastMoneySourceColumnLetter + beginRowNumber.ToString()).Value = sources[i].Name;

                    if(i != (sourcesNum - 1))
                    {
                        lastMoneySourceColumnLetter = GetNextColumnLettter(lastMoneySourceColumnLetter);
                    }
                    else
                    {
                        totalMoneySourceColumnLetter = GetNextColumnLettter(lastMoneySourceColumnLetter);
                    }
                }
                xlWorksheet.get_Range(totalMoneySourceColumnLetter + beginRowNumber.ToString()).Font.Bold = true;
                xlWorksheet.get_Range(totalMoneySourceColumnLetter + beginRowNumber.ToString()).Value = "ВСЬОГО";

                xlWorksheet.get_Range(moneySourceSpendingBeginColumn + (beginRowNumber - 1).ToString(),
                    totalMoneySourceColumnLetter + (beginRowNumber - 1).ToString()).Merge();

                List<EstimateMonthSpending> monthSpendings = GetEstimateMonthesReportList();

                currentRowNumber++;
                for (int i = 0; i < monthSpendings.Count; i++)
                {
                    monthesHeaders.Add(currentRowNumber);
                    WriteCaption(string.Format("{0} {1} року", monthes[i], year.Year));

                    foreach (var kekv in monthSpendings[i].KekvRows)
                    {
                        int k = 0;
                        int spendingStartRow = currentRowNumber;
                        foreach (var spending in kekv.SpendingRows)
                        {
                            xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString()).Value = (k + 1).ToString();
                            xlWorksheet.get_Range(dateColumnLetter + currentRowNumber.ToString()).Value = spending.Date.ToShortDateString();
                            xlWorksheet.get_Range(kekvColumnLetter + currentRowNumber.ToString()).Value = spending.Kekv.Code;
                            xlWorksheet.get_Range(documentColumnLetter + currentRowNumber.ToString()).Value = spending.SpendingDescription;
                            xlWorksheet.get_Range(contractColumnLetter + currentRowNumber.ToString()).Value = spending.Contract;

                            // Выделяем курсивом другие траты (зарплата, налоги и т.п.)
                            if(spending.Type == BalanceChangeType.PlannedSpending)
                            {
                                xlWorksheet.get_Range(documentColumnLetter + currentRowNumber.ToString()).Font.Italic = true;
                            }

                            currentSourceColumnLetter = moneySourceSpendingBeginColumn;
                            for (int j = 0; j < spending.SpendingList.Length; j++)
                            {
                                xlWorksheet.get_Range(currentSourceColumnLetter + currentRowNumber.ToString()).Value = spending.SpendingList[j];
                                currentSourceColumnLetter = GetNextColumnLettter(currentSourceColumnLetter);
                            }
                            xlWorksheet.get_Range(totalMoneySourceColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
                            xlWorksheet.get_Range(totalMoneySourceColumnLetter + currentRowNumber.ToString()).Formula
                                = string.Format("=SUM({1}{0}:{2}{0})", currentRowNumber, moneySourceSpendingBeginColumn, lastMoneySourceColumnLetter);

                            k++;
                            currentRowNumber++;
                        }

                        // Итоги по КЕКВ
                        if (!estimateKekvSpendingTotals.ContainsKey(kekv.Kekv))
                        {
                            estimateKekvSpendingTotals.Add(kekv.Kekv, new List<string>());
                        }
                        if (!estimateKekvPlannedMoneyTotals.ContainsKey(kekv.Kekv))
                        {
                            estimateKekvPlannedMoneyTotals.Add(kekv.Kekv, new List<string>());
                        }
                        if (!estimateKekvMoneyRemainTotals.ContainsKey(kekv.Kekv))
                        {
                            estimateKekvMoneyRemainTotals.Add(kekv.Kekv, new List<string>());
                        }

                        // Итоги по КЕКВ
                        string kekvSpendingFormula = null;
                        string prevMonthKekvRemainFormula = null;
                        decimal[] plannedSpending = kekv.PlannedMoneysRow.SpendingList;
                        if(kekv.SpendingRows.Count > 0)
                        {
                            kekvSpendingFormula = "=SUM({0}" + spendingStartRow + ":{0}" + (currentRowNumber - 1) + ")";
                        }
                        if((i > 0) && (estimateKekvMoneyRemainTotals[kekv.Kekv].Count > 0))
                        {
                            prevMonthKekvRemainFormula = "=" + estimateKekvMoneyRemainTotals[kekv.Kekv].Last();
                        }

                        WriteTotals(TotalsType.KekvTotal, kekv.Kekv, kekvSpendingFormula, 
                            prevMonthKekvRemainFormula, null, plannedSpending);
                        currentRowNumber++;
                    }

                    // Итоги за месяц
                    string totalSpendingFormula = null;
                    if(monthSpendingCells.Count > 0)
                    {
                        totalSpendingFormula = "=SUM(" + string.Join(",", monthSpendingCells) + ")";
                    }
                    string totalPlannedMoney = null;
                    decimal[] plannedMoneyRow = new decimal[sourcesNum];
                    if (monthPlannedMoneyCells.Count > 0)
                    {
                        totalPlannedMoney = "=SUM(" + string.Join(",", monthPlannedMoneyCells) + ")";
                    }
                    string moneyFromLastMonth = null;
                    if ((i > 0) && (lastMonthRemainCells.Count > 0))
                    {
                        moneyFromLastMonth = "=SUM(" + string.Join(",", lastMonthRemainCells) + ")";
                    }
                    WriteTotals(TotalsType.PeriodTotal, null, totalSpendingFormula,
                        moneyFromLastMonth, totalPlannedMoney, plannedMoneyRow);

                    monthSpendingCells.Clear();
                    monthPlannedMoneyCells.Clear();
                    lastMonthRemainCells.Clear();
                }

                if (monthSpendings.Count > 0)
                {
                    monthesHeaders.Add(currentRowNumber);
                    // Итоги за ГОД
                    WriteCaption(string.Format("ПІДСУМКОВА ІНФОРМАЦІЯ ЗА {0} РІК", year.Year));

                    // По КЕКВ
                    foreach (var kekv in estimateKekvSpendingTotals.Keys)
                    {
                        string totalKekvSpendingFormula = "=SUM(" + string.Join(",", estimateKekvSpendingTotals[kekv]) + ")";
                        string totalPlannedKekvMoney = "=SUM(" + string.Join(",", estimateKekvPlannedMoneyTotals[kekv]) + ")";
                        WriteTotals(TotalsType.KekvTotal, kekv, totalKekvSpendingFormula,
                            null, totalPlannedKekvMoney, null);
                        currentRowNumber++;
                    }

                    // ВСЕГО
                    string yearTotalSpendingFormula = "=SUM(" + string.Join(",", monthSpendingCells) + ")";
                    string yearTotalPlannedMoney = "=SUM(" + string.Join(",", monthPlannedMoneyCells) + ")";
                    WriteTotals(TotalsType.PeriodTotal, null, yearTotalSpendingFormula,
                        null, yearTotalPlannedMoney, null);

                    // Рисуем границы таблицы
                    DrawTableBorders(numColumnLetter + (beginRowNumber - 1).ToString(),
                        totalMoneySourceColumnLetter + currentRowNumber.ToString());

                    // Рисуем границы ячеек с названиями месяцев
                    foreach (var cell in monthesHeaders)
                    {
                        DrawTableBorders(numColumnLetter + cell.ToString(), totalMoneySourceColumnLetter + cell.ToString());
                    }
                }
                else
                {
                    WriteCaption("Витраты відсутні");
                    // Рисуем границы таблицы
                    DrawTableBorders(numColumnLetter + (beginRowNumber - 1).ToString(),
                        totalMoneySourceColumnLetter + currentRowNumber.ToString());
                }

                // Центрируем заголовок отчёта
                xlWorksheet.get_Range(numColumnLetter + reportTitleRow, totalMoneySourceColumnLetter + reportTitleRow).Merge();
                xlWorksheet.get_Range(numColumnLetter + yearCell, totalMoneySourceColumnLetter + yearCell).Merge();
                xlWorksheet.get_Range(numColumnLetter + estimateNameCell, totalMoneySourceColumnLetter + estimateNameCell).Merge();
                xlWorksheet.get_Range(numColumnLetter + dateCell, totalMoneySourceColumnLetter + dateCell).Merge();
            }

            xlWorkbook.SaveAs(System.IO.Path.Combine(FileManager.ReportDirectoryFullPath,
                FileManager.ClearIllegalFileNameSymbols(string.Format("Звіт по кошторисним витратам за {0} рік", year.Year))));

            TerminateExcelProcessInstance();
        }
    }
}
