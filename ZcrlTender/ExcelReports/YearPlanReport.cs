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
        private TenderYear year;
        private string yearCell = "A3";
        private string dateCell = "A6";
        private string estimateNameCell = "A4";
        private string numColumnLetter = "A";
        private string dkCodeColumnLetter = "B";
        private string procedureNameColumnLetter = "C";
        private string plannedPeriodColumnLetter = "D";
        private string plannedMoneyColumnLetter = "E";
        private string registedByContractsMoneyColumnLetter = "F";
        private string usedByContractsMoneyColumnLetter = "G";
        private string contractRemainColumnLetter = "H";
        private string dkCodeRemainColumnLetter = "I";
        private int beginRowNumber = 8;
        private int currentRowNumber = 8;

        // Критерии отбора записей в годовом плане
        private Estimate estFilter;
        private KekvCode kekvFilter;
        private bool isNewSystem;

        private List<string> dkCodesInKekv = new List<string>();
        private List<string> kekvsList = new List<string>();
        private List<int> kekvsHeaders = new List<int>();

        private string[] monthes = { 
                                       "Січень", "Лютий", "Березень",
                                       "Квітень", "Травень", "Червень",
                                       "Липень", "Серпень", "Вересень",
                                       "Жовтень", "Листопад", "Грудень"
                                   };

        public YearPlanReport(TenderYear year, Estimate est, bool isNewSystem, KekvCode kekv)
        {
            this.year = year;
            this.estFilter = est;
            this.isNewSystem = isNewSystem;
            this.kekvFilter = kekv;
        }

        protected override string TemplateFile
        {
            get 
            {
                return FileManager.YearPlanTemplateFile; 
            }
        }

        protected override string SaveReportFile
        {
            get 
            {
                return string.Format("Річний план на {0} рік", year.Year);
            }
        }

        protected override void WriteDataToFile()
        {
            List<TenderPlanItemsTableEntry> resultList = null;

            List<TenderPlanRecord> allPlanRecords = null;

            using(TenderContext tc = new TenderContext())
            {
                string estimateName = string.Empty;
                if (estFilter.Id > 0)
                {
                    estimateName = estFilter.Name;
                    allPlanRecords = (from r in tc.TenderPlanRecords
                                      where r.EstimateId == estFilter.Id
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
                    resultList = (from r in allPlanRecords.ToList()
                                   select new TenderPlanItemsTableEntry
                                   {
                                       Kekv = r.PrimaryKekv,
                                       Dk = r.Dk,
                                       MoneyOnCode = r.PlannedSum,
                                       RelatedTenderPlanRecord = r,
                                       Estimate = r.Estimate,
                                       RegisteredByContracts = r.RegisteredContracts.Sum(p => p.Sum),
                                       UsedByContracts = r.RegisteredContracts.Sum(p => p.UsedMoney),
                                       ContractsMoneyRemain = r.RegisteredContracts.Sum(p => p.MoneyRemain)
                                   }).ToList();
                }
                else
                {
                    resultList = (from r in allPlanRecords
                                   select new TenderPlanItemsTableEntry
                                   {
                                       Kekv = r.SecondaryKekv,
                                       Dk = r.Dk,
                                       MoneyOnCode = r.PlannedSum,
                                       RelatedTenderPlanRecord = r,
                                       Estimate = r.Estimate,
                                       RegisteredByContracts = r.RegisteredContracts.Sum(p => p.Sum),
                                       UsedByContracts = r.RegisteredContracts.Sum(p => p.UsedMoney),
                                       ContractsMoneyRemain = r.RegisteredContracts.Sum(p => p.MoneyRemain)
                                   }).ToList();
                }

                if(kekvFilter.Id > 0)
                {
                    resultList = resultList.Where(p => p.Kekv.Id == kekvFilter.Id).ToList();
                }

                if(resultList.Count == 0)
                {
                    xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString(),
                        dkCodeRemainColumnLetter + currentRowNumber.ToString()).Merge();
                    xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
                    xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString()).Font.Italic = true;
                    xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString()).Value = "Записи в річному плані відсутні";
                }

                var groupedByKekvResult = resultList.GroupBy(p => p.Kekv);
                foreach (var kekv in groupedByKekvResult.OrderBy(p => p.Key.Code))
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

                        string dkCodeName = string.Format("{0} ({1})",
                                code.Dk.FullName,
                                code.RelatedTenderPlanRecord.ConcreteName);
                        if (!string.IsNullOrWhiteSpace(code.RelatedTenderPlanRecord.ProtocolNum))
                        {
                            dkCodeName = string.Format("{0}\n Затверджено протоколом № {1} від {2} року",
                                dkCodeName,
                                code.RelatedTenderPlanRecord.ProtocolNum,
                                code.RelatedTenderPlanRecord.ProtocolDate.ToShortDateString());
                        }
                        if(code.RelatedTenderPlanRecord.CodeRepeatReason != null)
                        {
                            dkCodeName = string.Format("{0}\nОбгрунтування повторення коду: {1}", dkCodeName, code.RelatedTenderPlanRecord.CodeRepeatReason);
                        }
                        xlWorksheet.get_Range(dkCodeColumnLetter + currentRowNumber.ToString()).Value = dkCodeName;
                        int estimateRowBeginIndex = dkCodeName.IndexOf('\n') + 1;
                        if (estimateRowBeginIndex > 0)
                        {
                            xlWorksheet.get_Range(dkCodeColumnLetter + currentRowNumber.ToString())
                                .get_Characters(estimateRowBeginIndex).Font.Size = 9;
                            xlWorksheet.get_Range(dkCodeColumnLetter + currentRowNumber.ToString())
                                .get_Characters(estimateRowBeginIndex).Font.Italic = true;
                            xlWorksheet.get_Range(dkCodeColumnLetter + currentRowNumber.ToString())
                                .get_Characters(estimateRowBeginIndex).Font.Bold = false;
                        }

                        xlWorksheet.get_Range(plannedMoneyColumnLetter + currentRowNumber.ToString()).Value = code.MoneyOnCode;
                        xlWorksheet.get_Range(procedureNameColumnLetter + currentRowNumber.ToString()).Value = TenderPlanRecord.GetProcedureName(code.RelatedTenderPlanRecord.ProcedureType);
                        xlWorksheet.get_Range(plannedPeriodColumnLetter + currentRowNumber.ToString()).Value =
                           string.Format("{0} {1} року", monthes[code.RelatedTenderPlanRecord.TenderBeginDate.Month - 1], code.RelatedTenderPlanRecord.TenderBeginDate.Year);

                        List<Contract> contractsOnCode = code.RelatedTenderPlanRecord.RegisteredContracts.ToList();

                        int firstCodeContract = currentRowNumber + 1;
                        foreach(var contract in contractsOnCode)
                        {
                            currentRowNumber++;

                            xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString(), 
                                dkCodeRemainColumnLetter + currentRowNumber.ToString()).Font.Size = 10;
                            string contractDescription = string.Empty;
                            if(!string.IsNullOrWhiteSpace(contract.Description))
                            {
                                contractDescription = string.Format("\n({0}),", contract.Description);
                            }
                            string contractName = string.Format("Договір {0},{1}\n{2}", contract.FullName, contractDescription, contract.Contractor.ShortName);

                            xlWorksheet.get_Range(dkCodeColumnLetter + currentRowNumber.ToString()).Value = contractName;
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
        }
    }
}
