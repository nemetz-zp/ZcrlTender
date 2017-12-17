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
    public class YearPlanReport : ExcelReportMaker
    {
        private Estimate est;
        private TenderYear year;
        private bool isNewSystem;
        private string yearCell = "A3";
        private string dateCell = "A6";
        private string estimateNameCell = "A4";
        private string numColumnLetter = "A";
        private string dkCodeColumnLetter = "B";
        private string plannedMoneyColumnLetter = "C";
        private string registedByContractsMoneyColumnLetter = "D";
        private string usedByContractsMoneyColumnLetter = "E";
        private string contractRemainColumnLetter = "F";
        private string dkCodeRemainColumnLetter = "G";
        private int beginRowNumber = 8;
        private int currentRowNumber = 8;

        private List<string> dkCodesInKekv = new List<string>();
        private List<string> kekvsList = new List<string>();
        private List<int> kekvsHeaders = new List<int>();

        public YearPlanReport(Estimate est, bool isNewSystem)
        {
            this.est = est;
            this.year = est.Year;
            this.isNewSystem = isNewSystem;
        }

        public YearPlanReport(TenderYear year, bool isNewSystem)
        {
            this.year = year;
            this.isNewSystem = isNewSystem;
        }

        public override void MakeReport()
        {
            List<TenderPlanItemsTableEntry> planRecords = null;
            List<TenderPlanItemsTableEntry> contractsMoney = null;
            List<TenderPlanItemsTableEntry> resultList = null;

            List<TenderPlanRecord> allPlanRecords = null;
            List<Contract> allContracts = null;

            OpenExcelFile(FileManager.YearPlanTemplateFile);

            using(TenderContext tc = new TenderContext())
            {
                string estimateName = string.Empty;
                if (est != null)
                {
                    estimateName = est.Name;
                    allPlanRecords = (from r in tc.TenderPlanRecords
                                      where r.EstimateId == est.Id
                                      select r).ToList();
                    allContracts = (from r in tc.Contracts
                                    where r.EstimateId == est.Id
                                    select r).ToList();
                }
                else
                {
                    estimateName = "Всі кошториси від початку року";
                    allPlanRecords = (from r in tc.TenderPlanRecords
                                      where r.Estimate.TenderYearId == year.Id
                                      select r).ToList();
                    allContracts = (from r in tc.Contracts
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
                    contractsMoney = (from c in allContracts.ToList()
                                      group c by new { c.Estimate, c.PrimaryKekv, c.Dk } into g1
                                      select new TenderPlanItemsTableEntry
                                      {
                                          Kekv = g1.Key.PrimaryKekv,
                                          Dk = g1.Key.Dk,
                                          Estimate = g1.Key.Estimate,
                                          RegisteredByContracts = g1.Sum(p => p.Sum),
                                          ContractsMoneyRemain = g1.Sum(p => p.MoneyRemain),
                                          UsedByContracts = g1.Sum(p => p.UsedMoney)
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
                    contractsMoney = (from c in allContracts
                                      group c by new { c.Estimate, c.SecondaryKekv, c.Dk } into g1
                                      select new TenderPlanItemsTableEntry
                                      {
                                          Kekv = g1.Key.SecondaryKekv,
                                          Dk = g1.Key.Dk,
                                          Estimate = g1.Key.Estimate,
                                          RegisteredByContracts = g1.Sum(p => p.Sum),
                                          ContractsMoneyRemain = g1.Sum(p => p.MoneyRemain),
                                          UsedByContracts = g1.Sum(p => p.UsedMoney)
                                      }).ToList();
                }

                resultList = (from p in planRecords
                              join u in contractsMoney on new { p.Estimate, p.Kekv, p.Dk } equals new { u.Estimate, u.Kekv, u.Dk } into g2
                              from t3 in g2.DefaultIfEmpty(new TenderPlanItemsTableEntry())
                              select new TenderPlanItemsTableEntry
                              {
                                  RelatedTenderPlanRecord = p.RelatedTenderPlanRecord,
                                  Kekv = p.Kekv,
                                  Dk = p.Dk,
                                  MoneyOnCode = p.MoneyOnCode,
                                  Estimate = p.Estimate,
                                  RegisteredByContracts = t3.RegisteredByContracts,
                                  UsedByContracts = t3.UsedByContracts,
                                  ContractsMoneyRemain = t3.ContractsMoneyRemain
                              }).ToList();

                var groupedByKekvResult = resultList.GroupBy(p => p.Kekv);
                foreach(var kekv in groupedByKekvResult)
                {
                    xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString(),
                        dkCodeRemainColumnLetter + currentRowNumber.ToString()).Merge();
                    xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
                    xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString()).Font.Italic = true;
                    xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString()).Value = 
                        string.Format("{0} - {1}", kekv.Key.Code, kekv.Key.Name);
                    kekvsHeaders.Add(currentRowNumber);

                    int dkCodeNum = 0;
                    foreach(var code in kekv)
                    {
                        currentRowNumber++;
                        int dkCodeIndex = currentRowNumber;
                        xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString(), 
                            dkCodeRemainColumnLetter + currentRowNumber.ToString()).Font.Size = 11;
                        xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString(),
                            dkCodeRemainColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
                        xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString()).Value = (dkCodeNum + 1).ToString();
                        
                        if(est != null)
                        {
                            xlWorksheet.get_Range(dkCodeColumnLetter + currentRowNumber.ToString()).Value = string.Format("{0} ({1})", code.Dk.FullName, code.RelatedTenderPlanRecord.ConcreteName);
                        }
                        else
                        {
                            string dkCodeName = string.Format("{0} ({1})\n({2})", code.Dk.FullName, code.RelatedTenderPlanRecord.ConcreteName, code.Estimate.Name);
                            xlWorksheet.get_Range(dkCodeColumnLetter + currentRowNumber.ToString()).Value = dkCodeName;
                            int estimateRowBeginIndex = dkCodeName.IndexOf('\n') + 1;
                            xlWorksheet.get_Range(dkCodeColumnLetter + currentRowNumber.ToString())
                                .get_Characters(estimateRowBeginIndex).Font.Size = 9;
                            xlWorksheet.get_Range(dkCodeColumnLetter + currentRowNumber.ToString())
                                .get_Characters(estimateRowBeginIndex).Font.Italic = true;
                            xlWorksheet.get_Range(dkCodeColumnLetter + currentRowNumber.ToString())
                                .get_Characters(estimateRowBeginIndex).Font.Bold = false;

                        }

                        xlWorksheet.get_Range(plannedMoneyColumnLetter + currentRowNumber.ToString()).Value = code.MoneyOnCode;

                        List<Contract> contractsOnCode = null;

                        if (isNewSystem)
                        {
                            contractsOnCode = tc.Contracts
                                .Where(p => (p.DkCodeId == code.Dk.Id) && (p.PrimaryKekvId == code.Kekv.Id)).ToList();
                        }
                        else
                        {
                            contractsOnCode = tc.Contracts
                                .Where(p => (p.DkCodeId == code.Dk.Id) && (p.SecondaryKekvId == code.Kekv.Id)).ToList();
                        }

                        int firstCodeContract = currentRowNumber + 1;
                        foreach(var contract in contractsOnCode)
                        {
                            currentRowNumber++;
                            xlWorksheet.get_Range(dkCodeColumnLetter + currentRowNumber.ToString()).Value = 
                                string.Format("Договір {0}\n({1})\n{2}", contract.FullName, contract.Description, contract.Contractor.ShortName);
                            xlWorksheet.get_Range(registedByContractsMoneyColumnLetter + currentRowNumber.ToString()).Value = contract.Sum;
                            xlWorksheet.get_Range(usedByContractsMoneyColumnLetter + currentRowNumber.ToString()).Value = contract.UsedMoney;
                            xlWorksheet.get_Range(contractRemainColumnLetter + currentRowNumber.ToString()).Value = contract.MoneyRemain;
                        }

                        if(contractsOnCode.Count > 0)
                        {
                            xlWorksheet.get_Range(registedByContractsMoneyColumnLetter + dkCodeIndex.ToString()).Formula =
                                string.Format("=SUM({0}{1}:{0}{2})", registedByContractsMoneyColumnLetter, firstCodeContract, currentRowNumber);
                            xlWorksheet.get_Range(usedByContractsMoneyColumnLetter + dkCodeIndex.ToString()).Formula =
                                string.Format("=SUM({0}{1}:{0}{2})", usedByContractsMoneyColumnLetter, firstCodeContract, currentRowNumber);
                            xlWorksheet.get_Range(contractRemainColumnLetter + dkCodeIndex.ToString()).Formula =
                                string.Format("=SUM({0}{1}:{0}{2})", contractRemainColumnLetter, firstCodeContract, currentRowNumber);
                        }
                        else
                        {
                            xlWorksheet.get_Range(registedByContractsMoneyColumnLetter + dkCodeIndex.ToString()).Value =
                            xlWorksheet.get_Range(usedByContractsMoneyColumnLetter + dkCodeIndex.ToString()).Value =
                            xlWorksheet.get_Range(contractRemainColumnLetter + dkCodeIndex.ToString()).Value = 0;
                        }
                        xlWorksheet.get_Range(dkCodeRemainColumnLetter + dkCodeIndex.ToString()).Formula =
                                string.Format("={1}{0} - {2}{0}", dkCodeIndex, plannedMoneyColumnLetter, registedByContractsMoneyColumnLetter);

                        dkCodesInKekv.Add("{0}" + dkCodeIndex);
                        dkCodeNum++;
                    }

                    currentRowNumber++;
                    xlWorksheet.get_Range(dkCodeColumnLetter + currentRowNumber.ToString(), 
                        dkCodeRemainColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
                    xlWorksheet.get_Range(dkCodeColumnLetter + currentRowNumber.ToString()).Value = "ВСЬОГО";

                    string kekvSumFormula = "=SUM(" + string.Join(",", dkCodesInKekv) + ")";

                    xlWorksheet.get_Range(plannedMoneyColumnLetter + currentRowNumber.ToString()).Formula =
                                string.Format(kekvSumFormula, plannedMoneyColumnLetter);
                    xlWorksheet.get_Range(registedByContractsMoneyColumnLetter + currentRowNumber.ToString()).Formula =
                                string.Format(kekvSumFormula, registedByContractsMoneyColumnLetter);
                    xlWorksheet.get_Range(usedByContractsMoneyColumnLetter + currentRowNumber.ToString()).Formula =
                        string.Format(kekvSumFormula, usedByContractsMoneyColumnLetter);
                    xlWorksheet.get_Range(contractRemainColumnLetter + currentRowNumber.ToString()).Formula =
                        string.Format(kekvSumFormula, contractRemainColumnLetter);
                    xlWorksheet.get_Range(dkCodeRemainColumnLetter + currentRowNumber.ToString()).Formula =
                                string.Format("={1}{0} - {2}{0}", currentRowNumber, plannedMoneyColumnLetter, registedByContractsMoneyColumnLetter);

                    dkCodesInKekv.Clear();

                    kekvsList.Add("{0}" + currentRowNumber);

                    currentRowNumber++;
                }

                if (groupedByKekvResult.Count() > 0)
                {
                    currentRowNumber++;
                    xlWorksheet.get_Range(dkCodeColumnLetter + currentRowNumber.ToString(),
                            dkCodeRemainColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
                    xlWorksheet.get_Range(dkCodeColumnLetter + currentRowNumber.ToString()).Value = "ВСЬОГО ЗА РІК";

                    if (kekvsList.Count > 0)
                    {
                        string sumFormula = "=SUM(" + string.Join(",", kekvsList) + ")";
                        string sst = string.Format(sumFormula, plannedMoneyColumnLetter);

                        xlWorksheet.get_Range(plannedMoneyColumnLetter + currentRowNumber.ToString()).Formula =
                                    string.Format(sumFormula, plannedMoneyColumnLetter);
                        xlWorksheet.get_Range(registedByContractsMoneyColumnLetter + currentRowNumber.ToString()).Formula =
                                    string.Format(sumFormula, registedByContractsMoneyColumnLetter);
                        xlWorksheet.get_Range(usedByContractsMoneyColumnLetter + currentRowNumber.ToString()).Formula =
                            string.Format(sumFormula, usedByContractsMoneyColumnLetter);
                        xlWorksheet.get_Range(contractRemainColumnLetter + currentRowNumber.ToString()).Formula =
                            string.Format(sumFormula, contractRemainColumnLetter);
                    }
                    else
                    {
                        xlWorksheet.get_Range(plannedMoneyColumnLetter + currentRowNumber.ToString()).Value =
                        xlWorksheet.get_Range(registedByContractsMoneyColumnLetter + currentRowNumber.ToString()).Value =
                        xlWorksheet.get_Range(usedByContractsMoneyColumnLetter + currentRowNumber.ToString()).Value =
                        xlWorksheet.get_Range(contractRemainColumnLetter + currentRowNumber.ToString()).Value = 0;
                    }
                    xlWorksheet.get_Range(dkCodeRemainColumnLetter + currentRowNumber.ToString()).Formula =
                                string.Format("={1}{0} - {2}{0}", currentRowNumber, plannedMoneyColumnLetter, registedByContractsMoneyColumnLetter);

                    DrawTableBorders(numColumnLetter + (beginRowNumber - 1).ToString(), dkCodeRemainColumnLetter + currentRowNumber.ToString());
                    foreach (var kekvHeader in kekvsHeaders)
                    {
                        DrawTableBorders(numColumnLetter + kekvHeader.ToString(), dkCodeRemainColumnLetter + kekvHeader.ToString());
                    } 
                }
                else
                {
                    xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString(),
                        dkCodeRemainColumnLetter + currentRowNumber.ToString()).Merge();
                    xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
                    xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString()).Font.Italic = true;
                    xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString()).Value = "Записи в річному плані відсутні";
                    DrawTableBorders(numColumnLetter + (beginRowNumber - 1).ToString(), dkCodeRemainColumnLetter + currentRowNumber.ToString());
                }
            }

            xlWorksheet.SaveAs(System.IO.Path.Combine(FileManager.ReportDirectoryFullPath,
                FileManager.ClearIllegalFileNameSymbols(string.Format("Річний план на {0} рік", year.Year))));

            TerminateExcelProcessInstance();
        }
    }
}
