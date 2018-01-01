using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using TenderLibrary;

namespace ZcrlTender.ExcelReports
{
    // Отчёт по текущим остаткам средств и новым счетам
    public class CurrentMoneyRemainsAndNewInvoicesReport : ExcelReportMaker
    {
        private TenderYear year;
        private string dateCell = "A4";

        // Ячейки листа со счетами
        private int invoiceSheetNumber = 1;
        private string invoiceNumColumnLetter = "A";
        private string contractorColumnLetter = "B";
        private string contractColumnLetter = "C";
        private string invoiceColumnLetter = "D";
        private string invoiceSumColumnLetter = "E";
        private string creditColumnLetter = "F";
        private int invoiceBeginRowNumber = 6;

        // Ячейки листа с остатками
        private int moneyRemainsSheetNumber = 2;
        private int documentHeaderRowNumber = 2;
        private string kekvColumnLetter = "A";
        private string firstMoneySourceColumnLetter = "B";
        private string lastMoneySourceColumnLetter = "B";
        private string totalMoneySourceColumnLetter = "B";
        private string currentSourceColumnLetter = "B";
        private List<MoneySource> sources;
        private int sourcesNum;
        private int moneyRemainsBeginRowNumber = 7;
        string currentMoneySourceLetter = "B";
        private Dictionary<KekvCode, List<string>> kekvs = new Dictionary<KekvCode, List<string>>();

        private List<int> estimateNameRows = new List<int>();

        private int currentRowIndex = 0;

        protected override string SaveReportFile
        {
            get 
            {
                return string.Format("Поточні залишки та рахунки"); 
            }
        }

        protected override string TemplateFile
        {
            get 
            {
                return FileManager.CurrentMoneyRemainAndNewInvoicesTemplate; 
            }
        }

        private void LoadMoneySourcesList()
        {
            using(TenderContext tc = new TenderContext())
            {
                sources = (from rec in tc.BalanceChanges.ToList()
                           where ((rec.Estimate.TenderYearId == year.Id) && (rec.DateOfReceiving <= DateTime.Now))
                           group rec by rec.MoneySource into g1
                           select new 
                           { 
                               Source = g1.Key, Sum = g1.Sum(p => p.PrimaryKekvSum) 
                           } into s1
                           where s1.Sum > 0
                           select s1.Source).ToList();
                sourcesNum = sources.Count;
            }
        }

        public CurrentMoneyRemainsAndNewInvoicesReport(TenderYear year)
        {
            this.year = year;
            LoadMoneySourcesList();
        }

        private class EstimateMoneyRemain
        {
            public Estimate Estimate { get; set; }
            public List<KekvRemainAtMoneySource> KekvRemainsList { get; set; }
        }

        private class KekvRemainAtMoneySource
        {
            public KekvCode Kekv { get; set; }
            public decimal[] Remains { get; set; }
        }

        // Заполнить столбец с суммой по строке
        private void FillTotalMoneyColumn()
        {
            xlWorksheet.get_Range(totalMoneySourceColumnLetter + currentRowIndex.ToString()).Font.Bold = true;
            xlWorksheet.get_Range(totalMoneySourceColumnLetter + currentRowIndex.ToString()).Formula =
                string.Format("=SUM({1}{0}:{2}{0})", currentRowIndex, firstMoneySourceColumnLetter, lastMoneySourceColumnLetter);
        }

        private void WriteCaption(int rowIndex, string caption)
        {
            xlWorksheet.get_Range(kekvColumnLetter + currentRowIndex.ToString(),
                        totalMoneySourceColumnLetter + currentRowIndex.ToString()).Merge();
            xlWorksheet.get_Range(kekvColumnLetter + currentRowIndex.ToString()).Font.Bold = true;
            xlWorksheet.get_Range(kekvColumnLetter + currentRowIndex.ToString()).Font.Italic = true;
            xlWorksheet.get_Range(kekvColumnLetter + currentRowIndex.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            xlWorksheet.get_Range(kekvColumnLetter + currentRowIndex.ToString()).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            xlWorksheet.get_Range(kekvColumnLetter + currentRowIndex.ToString()).Value = caption;
        }

        protected override void WriteDataToFile()
        {
            // Формируем таблицу с текущими остатками
            SetActiveSheet(moneyRemainsSheetNumber);
            xlWorksheet.get_Range(dateCell).Value = string.Format("Інформація станом на {0} року", DateTime.Now.ToShortDateString());

            // Указываем источники финансирования
            for (int i = 0; i < sourcesNum; i++)
            {
                xlWorksheet.get_Range(lastMoneySourceColumnLetter + (moneyRemainsBeginRowNumber - 1).ToString()).Font.Bold = true;
                xlWorksheet.get_Range(lastMoneySourceColumnLetter + (moneyRemainsBeginRowNumber - 1).ToString()).Value = sources[i].Name;

                if (i != (sourcesNum - 1))
                {
                    lastMoneySourceColumnLetter = GetNextColumnLetter(lastMoneySourceColumnLetter);
                }
                else
                {
                    totalMoneySourceColumnLetter = GetNextColumnLetter(lastMoneySourceColumnLetter);
                }
            }
            xlWorksheet.get_Range(totalMoneySourceColumnLetter + (moneyRemainsBeginRowNumber - 1).ToString()).Font.Bold = true;
            xlWorksheet.get_Range(totalMoneySourceColumnLetter + (moneyRemainsBeginRowNumber - 1).ToString()).Value = "ВСЬОГО";
            xlWorksheet.get_Range(firstMoneySourceColumnLetter + (moneyRemainsBeginRowNumber - 2).ToString(),
                totalMoneySourceColumnLetter + (moneyRemainsBeginRowNumber - 2).ToString()).Merge();

            using(TenderContext tc = new TenderContext())
            {
                // Получаем список остатков сгруппированных по сметам затем по КЕКВ
                List<EstimateMoneyRemain> moneyRemainsList = (from rec in tc.BalanceChanges.ToList()
                                                              where (rec.Estimate.TenderYearId == year.Id) && (rec.DateOfReceiving <= DateTime.Now)
                                                              group rec by new { rec.Estimate, rec.PrimaryKekv, rec.MoneySource } into g1
                                                              select new
                                                              {
                                                                  Estimate = g1.Key.Estimate,
                                                                  Kekv = g1.Key.PrimaryKekv,
                                                                  MoneySource = g1.Key.MoneySource,
                                                                  Sum = g1.Sum(p => p.PrimaryKekvSum)
                                                              } into s1
                                                              // Выбираем только те источники по которым есть средства
                                                              where (s1.Sum > 0)
                                                              // Двойная группировка результата - сначала по смете, затем по КЕКВ
                                                              group s1 by s1.Estimate into g2
                                                              from rec2 in
                                                                  (from rec3 in g2
                                                                   group rec3 by rec3.Kekv)
                                                              group rec2 by g2.Key into g3
                                                              select new EstimateMoneyRemain
                                                              {
                                                                  Estimate = g3.Key,
                                                                  KekvRemainsList = g3.Select(p => new KekvRemainAtMoneySource
                                                                  {
                                                                      Kekv = p.Key,
                                                                      Remains = GetMoneySourceSpendingRow(sources,
                                                                                p.Select(k => new MoneySourceSpending 
                                                                                {
                                                                                    Source = k.MoneySource,
                                                                                    Sum = k.Sum
                                                                                }).ToList())
                                                                  }).ToList()
                                                              }).ToList();

                currentRowIndex = moneyRemainsBeginRowNumber;
                foreach(var item in moneyRemainsList)
                {
                    WriteCaption(currentRowIndex, item.Estimate.Name);
                    estimateNameRows.Add(currentRowIndex);
                    currentRowIndex++;

                    int estimateBeginRowIndex = currentRowIndex;
                    foreach(var kekv in item.KekvRemainsList.OrderBy(p => p.Kekv.Code))
                    {
                        if(!kekvs.ContainsKey(kekv.Kekv))
                        {
                            kekvs.Add(kekv.Kekv, new List<string>());
                        }

                        kekvs[kekv.Kekv].Add("{0}" + currentRowIndex);
                        xlWorksheet.get_Range(kekvColumnLetter + currentRowIndex.ToString()).Value = kekv.Kekv.Code;

                        currentMoneySourceLetter = firstMoneySourceColumnLetter;
                        foreach(var remain in kekv.Remains)
                        {
                            xlWorksheet.get_Range(currentMoneySourceLetter + currentRowIndex.ToString()).Value = remain;
                            currentMoneySourceLetter = GetNextColumnLetter(currentMoneySourceLetter);
                        }
                        FillTotalMoneyColumn();
                        currentRowIndex++;
                    }
                    xlWorksheet.get_Range(kekvColumnLetter + currentRowIndex.ToString(),
                        totalMoneySourceColumnLetter + currentRowIndex.ToString()).Font.Bold = true;
                    xlWorksheet.get_Range(kekvColumnLetter + currentRowIndex.ToString()).Value = "ВСЬОГО";

                    currentMoneySourceLetter = firstMoneySourceColumnLetter;
                    foreach(var source in sources)
                    {
                        xlWorksheet.get_Range(currentMoneySourceLetter + currentRowIndex.ToString()).Formula =
                            string.Format("=SUM({0}{1}:{0}{2})", currentMoneySourceLetter, estimateBeginRowIndex, (currentRowIndex - 1));
                        currentMoneySourceLetter = GetNextColumnLetter(currentMoneySourceLetter);
                    }
                    FillTotalMoneyColumn();
                    currentRowIndex++;
                }

                // Выводим итог по всем сметам
                WriteCaption(currentRowIndex, "ЗАГАЛЬНИЙ ЗАЛИШОК");
                estimateNameRows.Add(currentRowIndex);
                currentRowIndex++;

                int totalsStartRowNumber = currentRowIndex;
                foreach (var kekv in kekvs.OrderBy(p => p.Key.Code))
                {
                    xlWorksheet.get_Range(kekvColumnLetter + currentRowIndex.ToString(),
                       totalMoneySourceColumnLetter + currentRowIndex.ToString()).Font.Bold = true;
                    xlWorksheet.get_Range(kekvColumnLetter + currentRowIndex.ToString()).Value = kekv.Key.Code;
                    
                    string kekvSumFormulaString = "=SUM(" + string.Join(",", kekv.Value) +")";
                    currentMoneySourceLetter = firstMoneySourceColumnLetter;
                    foreach (var source in sources)
                    {
                        xlWorksheet.get_Range(currentMoneySourceLetter + currentRowIndex.ToString()).Formula =
                            string.Format(kekvSumFormulaString, currentMoneySourceLetter);
                        currentMoneySourceLetter = GetNextColumnLetter(currentMoneySourceLetter);
                    }
                    FillTotalMoneyColumn();
                    currentRowIndex++;
                }
                xlWorksheet.get_Range(kekvColumnLetter + currentRowIndex.ToString(),
                       totalMoneySourceColumnLetter + currentRowIndex.ToString()).Font.Bold = true;
                xlWorksheet.get_Range(kekvColumnLetter + currentRowIndex.ToString()).Value = "ВСЬОГО";
                currentMoneySourceLetter = firstMoneySourceColumnLetter;
                foreach (var source in sources)
                {
                    xlWorksheet.get_Range(currentMoneySourceLetter + currentRowIndex.ToString()).Formula =
                        string.Format("=SUM({0}{1}:{0}{2})", currentMoneySourceLetter, totalsStartRowNumber, (currentRowIndex - 1));
                    currentMoneySourceLetter = GetNextColumnLetter(currentMoneySourceLetter);
                }
                FillTotalMoneyColumn();

                xlWorksheet.get_Range(kekvColumnLetter + documentHeaderRowNumber.ToString(),
                    totalMoneySourceColumnLetter + documentHeaderRowNumber.ToString()).Merge();
                xlWorksheet.get_Range(kekvColumnLetter + documentHeaderRowNumber.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                xlWorksheet.get_Range(kekvColumnLetter + documentHeaderRowNumber.ToString()).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                xlWorksheet.get_Range(dateCell, totalMoneySourceColumnLetter + dateCell.Last().ToString()).Merge();

                // Рисуем границы
                DrawTableBorders(kekvColumnLetter + (moneyRemainsBeginRowNumber - 2).ToString(), 
                    totalMoneySourceColumnLetter + currentRowIndex.ToString());

                foreach(var est in estimateNameRows)
                {
                    DrawTableBorders(kekvColumnLetter + est.ToString(), totalMoneySourceColumnLetter + est.ToString());
                }

                // Формируем таблицу с новыми счетами
                SetActiveSheet(invoiceSheetNumber);
                xlWorksheet.get_Range(dateCell).Value = string.Format("Інформація станом на {0} року", DateTime.Now.ToShortDateString());

                List<Invoice> newInvoices = (from rec in tc.Invoices.ToList()
                                             where ((rec.Contract.Estimate.TenderYearId == year.Id) && (rec.Status == PaymentStatus.New))
                                             orderby rec.IsCredit
                                             select rec).ToList();
                
                currentRowIndex = invoiceBeginRowNumber;
                if (newInvoices.Count == 0)
                {
                    xlWorksheet.get_Range(invoiceNumColumnLetter + currentRowIndex.ToString(), 
                        creditColumnLetter + currentRowIndex.ToString()).Merge();
                    xlWorksheet.get_Range(invoiceNumColumnLetter + currentRowIndex.ToString(),
                        creditColumnLetter + currentRowIndex.ToString()).Font.Bold = true;
                    xlWorksheet.get_Range(invoiceNumColumnLetter + currentRowIndex.ToString(),
                        creditColumnLetter + currentRowIndex.ToString()).Font.Italic = true;
                    xlWorksheet.get_Range(invoiceNumColumnLetter + currentRowIndex.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    xlWorksheet.get_Range(invoiceNumColumnLetter + currentRowIndex.ToString()).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    xlWorksheet.get_Range(invoiceNumColumnLetter + currentRowIndex.ToString()).Value = "Нові рахунки на оплату відсутні";
                }
                else
                {
                    for (int i = 0; i < newInvoices.Count; i++)
                    {
                        xlWorksheet.get_Range(invoiceNumColumnLetter + currentRowIndex.ToString()).Value = (i + 1).ToString();
                        xlWorksheet.get_Range(contractorColumnLetter + currentRowIndex.ToString()).Value = newInvoices[i].Contract.Contractor.ShortName;

                        string contractName = string.Format("Договір № {0} від {1} року",
                            newInvoices[i].Contract.Number, newInvoices[i].Contract.SignDate.ToShortDateString());
                        if (!string.IsNullOrWhiteSpace(newInvoices[i].Contract.Description))
                        {
                            contractName += string.Format(",\n({0})", newInvoices[i].Contract.Description);
                        }
                        xlWorksheet.get_Range(contractColumnLetter + currentRowIndex.ToString()).Value = contractName;

                        string invoiceName = string.Format("Рахунок/Акт № {0} від {1} року", newInvoices[i].Number, newInvoices[i].Date);
                        if (!string.IsNullOrWhiteSpace(newInvoices[i].Description))
                        {
                            contractName += string.Format(",\n({0})", newInvoices[i].Description);
                        }
                        xlWorksheet.get_Range(invoiceColumnLetter + currentRowIndex.ToString()).Value = invoiceName;

                        xlWorksheet.get_Range(invoiceSumColumnLetter + currentRowIndex.ToString()).Value = newInvoices[i].Sum;
                        xlWorksheet.get_Range(creditColumnLetter + currentRowIndex.ToString()).Value = newInvoices[i].IsCredit ? "БОРГ" : string.Empty;
                        currentRowIndex++;
                    }
                }

                DrawTableBorders(invoiceNumColumnLetter + (invoiceBeginRowNumber - 1), creditColumnLetter + currentRowIndex.ToString());
            }
        }
    }
}
