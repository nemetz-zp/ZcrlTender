using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenderLibrary;
using ZcrlTender.ViewModels;
using Excel = Microsoft.Office.Interop.Excel;

namespace ZcrlTender.ExcelReports
{
    public class YearPlanChangesReport : ExcelReportMaker
    {
        private Estimate est;
        private TenderYear year;
        private bool isNewSystem;
        private string yearCell = "A3";
        private string dateCell = "A6";
        private string estimateNameCell = "A4";
        private string numColumnLetter = "A";
        private string dateOfCodeChangeColumnLetter = "B";
        private string dkCodeColumnLetter = "C";
        private string plannedMoneyColumnLetter = "D";
        private int beginRowNumber = 8;
        private int currentRowNumber = 8;

        private List<string> dkCodesInKekv = new List<string>();
        private List<string> kekvsList = new List<string>();
        private List<int> kekvsHeaders = new List<int>();

        public YearPlanChangesReport(Estimate est, bool isNewSystem)
        {
            this.est = est;
            this.year = est.Year;
            this.isNewSystem = isNewSystem;
        }

        public YearPlanChangesReport(TenderYear year, bool isNewSystem)
        {
            this.year = year;
            this.isNewSystem = isNewSystem;
        }

        protected override string SaveReportFile
        {
            get 
            {
                return string.Format("Історія змін річного плану на {0} рік", year.Year); 
            }
        }

        protected override string TemplateFile
        {
            get 
            {
                return FileManager.YearPlanChangesHistoryTemplateFile; 
            }
        }

        protected override void WriteDataToFile()
        {
            List<TenderPlanItemsTableEntry> planRecords = null;

            List<TenderPlanRecord> allPlanRecords = null;

            string freeCellLetter = GetNextColumnLetter(plannedMoneyColumnLetter);
            xlWorksheet.get_Range(freeCellLetter + currentRowNumber).ColumnWidth =
                xlWorksheet.get_Range(dateOfCodeChangeColumnLetter + currentRowNumber).ColumnWidth + xlWorksheet.get_Range(dkCodeColumnLetter + currentRowNumber).ColumnWidth;

            using (TenderContext tc = new TenderContext())
            {
                string estimateName = string.Empty;
                if (est != null)
                {
                    estimateName = est.Name;
                    allPlanRecords = (from r in tc.TenderPlanRecords
                                      where r.EstimateId == est.Id
                                      select r).ToList();
                }
                else
                {
                    estimateName = "Всі кошториси від початку року";
                    allPlanRecords = (from r in tc.TenderPlanRecords
                                      where r.Estimate.TenderYearId == year.Id
                                      select r).ToList();
                }

                xlWorksheet.get_Range(yearCell).Value = string.Format("на {0} рік", year.Year);
                xlWorksheet.get_Range(estimateNameCell).Value = string.Format("Кошторис: \"{0}\"", estimateName);
                xlWorksheet.get_Range(dateCell).Value = string.Format("Інформація станом на {0} року", DateTime.Now.ToShortDateString());

                if (isNewSystem)
                {
                    planRecords = (from r in allPlanRecords.ToList()
                                   select new TenderPlanItemsTableEntry
                                   {
                                       Kekv = r.PrimaryKekv,
                                       Dk = r.Dk,
                                       MoneyOnCode = r.Sum,
                                       RelatedTenderPlanRecord = r,
                                       Estimate = r.Estimate
                                   }).ToList();
                }
                else
                {
                    planRecords = (from r in allPlanRecords
                                   select new TenderPlanItemsTableEntry
                                   {
                                       Kekv = r.SecondaryKekv,
                                       Dk = r.Dk,
                                       MoneyOnCode = r.Sum,
                                       RelatedTenderPlanRecord = r,
                                       Estimate = r.Estimate
                                   }).ToList();
                }

                var groupedByKekvResult = planRecords.GroupBy(p => p.Kekv);
                foreach (var kekv in groupedByKekvResult)
                {
                    xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString(),
                        plannedMoneyColumnLetter + currentRowNumber.ToString()).Merge();
                    xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
                    xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString()).Font.Italic = true;
                    xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString()).Value =
                        string.Format("{0} - {1}", kekv.Key.Code, kekv.Key.Name);
                    kekvsHeaders.Add(currentRowNumber);

                    int dkCodeNum = 0;
                    foreach (var code in kekv)
                    {
                        currentRowNumber++;
                        int dkCodeIndex = currentRowNumber;
                        xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString(),
                            plannedMoneyColumnLetter + currentRowNumber.ToString()).Font.Size = 11;
                        xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString(),
                            plannedMoneyColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
                        xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString()).Value = (dkCodeNum + 1).ToString();

                        xlWorksheet.get_Range(freeCellLetter + currentRowNumber.ToString()).Font.Bold = true;
                        if (est != null)
                        {
                            xlWorksheet.get_Range(freeCellLetter + currentRowNumber.ToString()).Value = string.Format("{0} ({1})", code.Dk.FullName, code.RelatedTenderPlanRecord.ConcreteName);
                        }
                        else
                        {
                            string dkCodeName = string.Format("{0} ({1})\n({2})", code.Dk.FullName, code.RelatedTenderPlanRecord.ConcreteName, code.Estimate.Name);
                            xlWorksheet.get_Range(freeCellLetter + currentRowNumber.ToString()).Value = dkCodeName;
                            int estimateRowBeginIndex = dkCodeName.IndexOf('\n') + 1;
                            xlWorksheet.get_Range(freeCellLetter + currentRowNumber.ToString())
                                .get_Characters(estimateRowBeginIndex).Font.Size = 9;
                            xlWorksheet.get_Range(freeCellLetter + currentRowNumber.ToString())
                                .get_Characters(estimateRowBeginIndex).Font.Italic = true;
                            xlWorksheet.get_Range(freeCellLetter + currentRowNumber.ToString())
                                .get_Characters(estimateRowBeginIndex).Font.Bold = false;

                        }

                        xlWorksheet.get_Range(dateOfCodeChangeColumnLetter + currentRowNumber.ToString()).RowHeight = xlWorksheet.get_Range(freeCellLetter + currentRowNumber.ToString()).RowHeight;
                        xlWorksheet.get_Range(freeCellLetter + currentRowNumber.ToString()).Cut(xlWorksheet.get_Range(dateOfCodeChangeColumnLetter + currentRowNumber.ToString()));
                        xlWorksheet.get_Range(dateOfCodeChangeColumnLetter + currentRowNumber.ToString(),
                                dkCodeColumnLetter + currentRowNumber.ToString()).Merge();
                        xlWorksheet.get_Range(dateOfCodeChangeColumnLetter + currentRowNumber.ToString()).WrapText = true;
                        xlWorksheet.get_Range(dateOfCodeChangeColumnLetter + currentRowNumber.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                        xlWorksheet.get_Range(dateOfCodeChangeColumnLetter + currentRowNumber.ToString()).VerticalAlignment = Excel.XlVAlign.xlVAlignTop;

                        xlWorksheet.get_Range(plannedMoneyColumnLetter + currentRowNumber.ToString()).Value = code.MoneyOnCode;

                        List<TenderPlanRecordChange> changesList = code.RelatedTenderPlanRecord.Changes.OrderByDescending(p => p.DateOfChange).ToList();

                        int firstChangeRowNumber = currentRowNumber + 1;
                        foreach (var change in changesList)
                        {
                            currentRowNumber++;
                            xlWorksheet.get_Range(dateOfCodeChangeColumnLetter + currentRowNumber.ToString()).Value = change.DateOfChange.ToShortDateString();
                            xlWorksheet.get_Range(dkCodeColumnLetter + currentRowNumber.ToString()).Value = change.Description;
                            xlWorksheet.get_Range(plannedMoneyColumnLetter + currentRowNumber.ToString()).Value = change.ChangeOfSum;
                        }

                        dkCodesInKekv.Add("{0}" + dkCodeIndex);
                        dkCodeNum++;
                    }

                    currentRowNumber++;
                    xlWorksheet.get_Range(dkCodeColumnLetter + currentRowNumber.ToString(),
                        plannedMoneyColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
                    xlWorksheet.get_Range(dkCodeColumnLetter + currentRowNumber.ToString()).Value = "ВСЬОГО";

                    string kekvSumFormula = "=SUM(" + string.Join(",", dkCodesInKekv) + ")";

                    xlWorksheet.get_Range(plannedMoneyColumnLetter + currentRowNumber.ToString()).Formula =
                                string.Format(kekvSumFormula, plannedMoneyColumnLetter);

                    dkCodesInKekv.Clear();

                    kekvsList.Add("{0}" + currentRowNumber);

                    currentRowNumber++;
                }

                if(groupedByKekvResult.Count() > 0)
                {
                    currentRowNumber++;
                    xlWorksheet.get_Range(dkCodeColumnLetter + currentRowNumber.ToString(),
                            plannedMoneyColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
                    xlWorksheet.get_Range(dkCodeColumnLetter + currentRowNumber.ToString()).Value = "ВСЬОГО ЗА РІК";

                    if (kekvsList.Count > 0)
                    {
                        string sumFormula = "=SUM(" + string.Join(",", kekvsList) + ")";
                        string sst = string.Format(sumFormula, plannedMoneyColumnLetter);

                        xlWorksheet.get_Range(plannedMoneyColumnLetter + currentRowNumber.ToString()).Formula =
                                    string.Format(sumFormula, plannedMoneyColumnLetter);
                    }
                    else
                    {
                        xlWorksheet.get_Range(plannedMoneyColumnLetter + currentRowNumber.ToString()).Value = 0;
                    }

                    DrawTableBorders(numColumnLetter + (beginRowNumber - 1).ToString(), plannedMoneyColumnLetter + currentRowNumber.ToString());
                    foreach (var kekvHeader in kekvsHeaders)
                    {
                        DrawTableBorders(numColumnLetter + kekvHeader.ToString(), plannedMoneyColumnLetter + kekvHeader.ToString());
                    } 
                }
                else
                {
                    xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString(),
                        plannedMoneyColumnLetter + currentRowNumber.ToString()).Merge();
                    xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
                    xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString()).Font.Italic = true;
                    xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString()).Value = "Записи в річному плані відсутні";
                    DrawTableBorders(numColumnLetter + (beginRowNumber - 1).ToString(), plannedMoneyColumnLetter + currentRowNumber.ToString());
                }
            }
        }
    }
}
