using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenderLibrary;
using Excel = Microsoft.Office.Interop.Excel;

namespace ZcrlTender.ExcelReports
{
    public class EstimateYearMoneyReport : ExcelReportMaker
    {
        private Estimate est;
        private string kekvColumnLetter = "A";
        private string yearTotalColumnLetter = "N";
        private string yearCell = "A3";
        private string dateCell = "A6";
        private string estimateNameCell = "A4";
        private int startRowNumber = 8;
        private int currentRowNumber = 8;
        private string[] monthColumnLetters = { "B", "C", "D",
                                                "E", "F", "G",
                                                "H", "I", "J",
                                                "K", "L", "M"
                                              };
        private List<int> headersList = new List<int>();


        public EstimateYearMoneyReport(Estimate est)
        {
            this.est = est;
        }

        // Отчёт по годовым поступлениям по смете
        public override void MakeReport()
        {
            Dictionary<KekvCode, decimal[]> EstimateKekvTotals = new Dictionary<KekvCode, decimal[]>();

            OpenExcelFile(FileManager.FullEstimateTemplateFile);

            using (TenderContext tc = new TenderContext())
            {
                tc.Estimates.Attach(est);
                xlWorksheet.get_Range(yearCell).Value = string.Format("на {0} рік", est.Year.Year);
                xlWorksheet.get_Range(estimateNameCell).Value = string.Format("Кошторис: \"{0}\"", est.Name);
                xlWorksheet.get_Range(dateCell).Value = string.Format("Інформація станом на {0} року", DateTime.Now.ToShortDateString());

                var mList = (from item in est.Changes.ToList()
                             where (item.PrimaryKekvSum > 0)
                             group item by new { item.MoneySource, item.PrimaryKekv, item.DateOfReceiving.Month } into g1
                             select new
                             {
                                 Source = g1.Key.MoneySource,
                                 Kekv = g1.Key.PrimaryKekv,
                                 Month = g1.Key.Month,
                                 Sum = g1.Sum(p => p.PrimaryKekvSum)
                             } into s1
                             group s1 by s1.Source into g2
                             from g3Item in
                                 (from item in g2
                                  group item by item.Kekv)
                             group g3Item by g2.Key).ToList();
                foreach (var source in mList)
                {
                    headersList.Add(currentRowNumber);
                    xlWorksheet.get_Range(kekvColumnLetter + currentRowNumber.ToString(),
                        yearTotalColumnLetter + currentRowNumber.ToString()).Merge();
                    xlWorksheet.get_Range(kekvColumnLetter + currentRowNumber.ToString()).Value = source.Key.Name;
                    xlWorksheet.get_Range(kekvColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
                    xlWorksheet.get_Range(kekvColumnLetter + currentRowNumber.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    currentRowNumber++;
                    int firstKekvRowNumber = currentRowNumber;
                    foreach (var kekv in source)
                    {
                        xlWorksheet.get_Range(kekvColumnLetter + currentRowNumber.ToString()).Value = kekv.Key.Code;

                        decimal[] monthesRemain = new decimal[12];
                        int monthCount = kekv.Count();
                        foreach (var monthSum in kekv)
                        {
                            monthesRemain[monthSum.Month - 1] = monthSum.Sum;
                        }

                        if (!EstimateKekvTotals.ContainsKey(kekv.Key))
                        {
                            EstimateKekvTotals.Add(kekv.Key, new decimal[12]);
                        }

                        for (int i = 0; i < monthesRemain.Length; i++)
                        {
                            xlWorksheet.get_Range(monthColumnLetters[i] + currentRowNumber.ToString()).Value = monthesRemain[i];
                            EstimateKekvTotals[kekv.Key][i] += monthesRemain[i];
                        }

                        // Годовой итог по КЕКВ
                        xlWorksheet.get_Range(yearTotalColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
                        xlWorksheet.get_Range(yearTotalColumnLetter + currentRowNumber.ToString()).Formula
                            = string.Format("=SUM({1}{0}:{2}{0})",
                                    currentRowNumber,
                                    monthColumnLetters[0],
                                    monthColumnLetters[monthColumnLetters.Count() - 1]);

                        currentRowNumber++;
                    }

                    // Строка с итогами источнику финансирования
                    xlWorksheet.get_Range(kekvColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
                    xlWorksheet.get_Range(kekvColumnLetter + currentRowNumber.ToString()).Value = "ВСЬОГО";
                    for (int i = 0; i < 12; i++)
                    {
                        xlWorksheet.get_Range(monthColumnLetters[i] + currentRowNumber.ToString()).Font.Bold = true;
                        xlWorksheet.get_Range(monthColumnLetters[i] + currentRowNumber.ToString()).Formula
                        = string.Format("=SUM({0}{1}:{0}{2})", monthColumnLetters[i], firstKekvRowNumber, currentRowNumber - 1);
                    }
                    xlWorksheet.get_Range(yearTotalColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
                    xlWorksheet.get_Range(yearTotalColumnLetter + currentRowNumber.ToString()).Formula
                        = string.Format("=SUM({0}{1}:{0}{2})", yearTotalColumnLetter, firstKekvRowNumber, currentRowNumber - 1);
                    currentRowNumber++;
                }

                // Итоги по смете
                xlWorksheet.get_Range(kekvColumnLetter + currentRowNumber.ToString(),
                        yearTotalColumnLetter + currentRowNumber.ToString()).Merge();
                currentRowNumber++;

                headersList.Add(currentRowNumber);
                xlWorksheet.get_Range(kekvColumnLetter + currentRowNumber.ToString(),
                        yearTotalColumnLetter + currentRowNumber.ToString()).Merge();
                xlWorksheet.get_Range(kekvColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
                xlWorksheet.get_Range(kekvColumnLetter + currentRowNumber.ToString()).Value = "ВСЬОГО ПО КОШТОРИСУ";
                currentRowNumber++;

                int totalKekvRowIndex = currentRowNumber;
                foreach (var kekv in EstimateKekvTotals)
                {
                    xlWorksheet.get_Range(kekvColumnLetter + currentRowNumber.ToString()).Value = kekv.Key.Code;
                    xlWorksheet.get_Range(kekvColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
                    xlWorksheet.get_Range(kekvColumnLetter + currentRowNumber.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    int monthCount = kekv.Value.Count();
                    for (int i = 0; i < monthCount; i++)
                    {
                        xlWorksheet.get_Range(monthColumnLetters[i] + currentRowNumber.ToString()).Font.Bold = true;
                        xlWorksheet.get_Range(monthColumnLetters[i] + currentRowNumber.ToString()).Value = kekv.Value[i];
                    }

                    // Годовой итог по КЕКВ
                    xlWorksheet.get_Range(yearTotalColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
                    xlWorksheet.get_Range(yearTotalColumnLetter + currentRowNumber.ToString()).Value
                        = string.Format("=SUM({1}{0}:{2}{0})",
                                currentRowNumber,
                                monthColumnLetters[0],
                                monthColumnLetters[monthColumnLetters.Count() - 1]);

                    currentRowNumber++;
                }

                xlWorksheet.get_Range(kekvColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
                xlWorksheet.get_Range(kekvColumnLetter + currentRowNumber.ToString()).Value = "ВСЬОГО";
                for (int i = 0; i < 12; i++)
                {
                    xlWorksheet.get_Range(monthColumnLetters[i] + currentRowNumber.ToString()).Font.Bold = true;
                    xlWorksheet.get_Range(monthColumnLetters[i] + currentRowNumber.ToString()).Formula
                        = string.Format("=SUM({0}{1}:{0}{2})", monthColumnLetters[i], totalKekvRowIndex, currentRowNumber - 1);
                }
                xlWorksheet.get_Range(yearTotalColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
                xlWorksheet.get_Range(yearTotalColumnLetter + currentRowNumber.ToString()).Formula
                        = string.Format("=SUM({0}{1}:{0}{2})", yearTotalColumnLetter, totalKekvRowIndex, currentRowNumber - 1);
            }

            // Рисуем границы таблицы
            DrawTableBorders(kekvColumnLetter + (startRowNumber - 1).ToString(), yearTotalColumnLetter + currentRowNumber);

            // Рисуем границы заголовков
            foreach(var header in headersList)
            {
                DrawTableBorders(kekvColumnLetter + header.ToString(), yearTotalColumnLetter + header.ToString());
            }

            xlWorkbook.SaveAs(System.IO.Path.Combine(FileManager.ReportDirectoryFullPath,
                FileManager.ClearIllegalFileNameSymbols(est.Name)));

            TerminateExcelProcessInstance();
        }
    }
}
