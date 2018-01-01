using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenderLibrary;
using Excel = Microsoft.Office.Interop.Excel;

namespace ZcrlTender.ExcelReports
{
    // Отчёт по затратам по договорам
    public class ContractsSpendingReport : ExcelReportMaker
    {
        private Estimate est;
        private TenderYear year;
        private bool isNewSystem;
        private int reportTitleRow = 2;
        private int yearCell = 3;
        private int dateCell = 6;
        private int estimateNameCell = 4;
        private string numColumnLetter = "A";
        private string firstMoneySourceColumnLetter = "E";
        private string lastMoneySourceColumnLetter = "E";
        private string totalMoneySourceColumnLetter = "E";
        private string contractorColumnLetter = "B";
        private string contractColumnLetter = "C";
        private string contractSumColumnLetter = "D";
        private string currentSourceColumnLetter = "E";
        private string contractRemainColumnLetter = "F";
        private List<MoneySource> sources;
        private List<string> contractsInKekv = new List<string>();
        private List<string> kekvsTotals = new List<string>();
        private List<int> kekvsHeaders = new List<int>(); 
        private int sourcesNum = 0;
        private int beginRowNumber = 8;
        private int currentRowNumber = 8;

        protected override string TemplateFile
        {
            get 
            {
                return FileManager.ContractSpendingTemplateFile; 
            }
        }

        protected override string SaveReportFile
        {
            get 
            {
                return string.Format("Звіт по витратам за договорами за {0} рік", year.Year); 
            }
        }

        private void LoadMoneySourceList()
        {
            using(TenderContext tc = new TenderContext())
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

        public ContractsSpendingReport(Estimate est, bool isNewSystem)
        {
            this.est = est;
            this.year = est.Year;
            this.isNewSystem = isNewSystem;
            LoadMoneySourceList();
        }

        public ContractsSpendingReport(TenderYear year, bool isNewSystem)
        {
            this.year = year;
            this.isNewSystem = isNewSystem;
            LoadMoneySourceList();
        }

        private class GroupedByKekvContracts
        {
            public KekvCode Kekv { get; set; }
            public List<Contract> Contracts { get; set; }
        }

        protected override void WriteDataToFile()
        {
            using (TenderContext tc = new TenderContext())
            {
                string estimateName = string.Empty;
                List<Contract> contractsList = null;
                if (est != null)
                {
                    estimateName = est.Name;
                    contractsList = tc.Contracts.Where(p => p.EstimateId == est.Id).ToList();
                }
                else
                {
                    estimateName = "Всі кошториси від початку року";
                    contractsList = tc.Contracts.Where(p => p.Estimate.TenderYearId == year.Id).ToList();
                }

                xlWorksheet.get_Range(numColumnLetter + yearCell).Value = string.Format("на {0} рік", year.Year);
                xlWorksheet.get_Range(numColumnLetter + estimateNameCell).Value = string.Format("Кошторис: \"{0}\"", estimateName);
                xlWorksheet.get_Range(numColumnLetter + dateCell).Value = string.Format("Інформація станом на {0} року", DateTime.Now.ToShortDateString());

                // Указываем источники финансирования
                for (int i = 0; i < sourcesNum; i++ )
                {
                    xlWorksheet.get_Range(lastMoneySourceColumnLetter + beginRowNumber.ToString()).Font.Bold = true;
                    xlWorksheet.get_Range(lastMoneySourceColumnLetter + beginRowNumber.ToString()).Value = sources[i].Name;

                    if (i == (sourcesNum - 1))
                    {
                        totalMoneySourceColumnLetter = GetNextColumnLetter(lastMoneySourceColumnLetter);
                    }
                    else
                    {
                        lastMoneySourceColumnLetter = GetNextColumnLetter(lastMoneySourceColumnLetter);
                    }
                }
                xlWorksheet.get_Range(totalMoneySourceColumnLetter + beginRowNumber.ToString()).Font.Bold = true;
                xlWorksheet.get_Range(totalMoneySourceColumnLetter + beginRowNumber.ToString()).Value = "ВСЬОГО";
                xlWorksheet.get_Range(firstMoneySourceColumnLetter + (beginRowNumber - 1).ToString(),
                    totalMoneySourceColumnLetter + (beginRowNumber - 1).ToString()).Merge();

                // Столбец с остатком по договору
                contractRemainColumnLetter = GetNextColumnLetter(totalMoneySourceColumnLetter);
                xlWorksheet.get_Range(contractRemainColumnLetter + (beginRowNumber - 1).ToString(),
                    contractRemainColumnLetter + beginRowNumber.ToString()).Merge();
                xlWorksheet.get_Range(contractRemainColumnLetter + (beginRowNumber - 1).ToString()).Font.Bold = true;
                xlWorksheet.get_Range(contractRemainColumnLetter + (beginRowNumber - 1).ToString()).Value= "Залишок";

                List<GroupedByKekvContracts> groupedByKekvContracts = null;
                if (isNewSystem)
                {
                    groupedByKekvContracts = (from contr in contractsList
                                              group contr by contr.PrimaryKekv into g1
                                              select new GroupedByKekvContracts
                                              {
                                                  Kekv = g1.Key,
                                                  Contracts = g1.OrderBy(p => p.Contractor).ToList()
                                              }).ToList();
                }
                else
                {
                    groupedByKekvContracts = (from contr in contractsList
                                              group contr by contr.SecondaryKekv into g1
                                              select new GroupedByKekvContracts
                                              {
                                                  Kekv = g1.Key,
                                                  Contracts = g1.OrderBy(p => p.Contractor).ToList()
                                              }).ToList();
                }

                foreach(var kekv in groupedByKekvContracts)
                {
                    currentRowNumber++;
                    kekvsHeaders.Add(currentRowNumber);
                    xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString(),
                        contractRemainColumnLetter + currentRowNumber.ToString()).Merge();
                    xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
                    xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString()).Font.Italic = true;
                    xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString()).Value = 
                        string.Format("{0} - {1}", kekv.Kekv.Code, kekv.Kekv.Name);
                    currentRowNumber++;

                    int contractNum = 0;
                    foreach(var contract in kekv.Contracts)
                    {
                        int currentContractRowIndex = currentRowNumber;
                        contractsInKekv.Add("{0}" + currentContractRowIndex);
                        xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString(), 
                            contractRemainColumnLetter + currentRowNumber.ToString()).Font.Size = 11;
                        xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString(),
                            contractRemainColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
                        xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString()).Value = (contractNum + 1).ToString();
                        xlWorksheet.get_Range(contractorColumnLetter + currentRowNumber.ToString()).Value = contract.Contractor.ShortName;

                        string contractName = contract.FullName;
                        if (!string.IsNullOrWhiteSpace(contract.Description))
                        {
                            contractName += string.Format(",\n{0}", contract.Description);
                        }
                        xlWorksheet.get_Range(contractColumnLetter + currentRowNumber.ToString()).Value = contractName;
                        xlWorksheet.get_Range(contractSumColumnLetter + currentRowNumber.ToString()).Value = contract.Sum;

                        int firstInvoiceRowIndex = currentRowNumber + 1;
                        foreach(var invoice in contract.Invoices)
                        {
                            currentRowNumber++;
                            xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString(),
                                contractRemainColumnLetter + currentRowNumber.ToString()).Font.Size = 11;
                            string invoiceName = string.Format("Рахунок/Акт № {0} від {1} року", invoice.Number, invoice.Date);
                            if(!string.IsNullOrWhiteSpace(invoice.Description))
                            {
                                invoiceName += string.Format(",\n({0})", invoice.Description);
                            }
                            xlWorksheet.get_Range(contractColumnLetter + currentRowNumber.ToString()).Value = invoiceName;
                            decimal[] spending = null;
                            if (isNewSystem)
                            {
                                spending = GetMoneySourceSpendingRow(sources, invoice.Changes
                                                        .Select(p => new MoneySourceSpending
                                                        {
                                                            Source = p.MoneySource,
                                                            Sum = -p.PrimaryKekvSum
                                                        }).ToList());
                            }
                            else
                            {
                                spending = GetMoneySourceSpendingRow(sources, invoice.Changes
                                                        .Select(p => new MoneySourceSpending
                                                        {
                                                            Source = p.MoneySource,
                                                            Sum = -p.SecondaryKekvSum
                                                        }).ToList());
                            }

                            currentSourceColumnLetter = firstMoneySourceColumnLetter;
                            foreach(var sum in spending)
                            {
                                xlWorksheet.get_Range(currentSourceColumnLetter + currentRowNumber.ToString()).Value = sum;
                                currentSourceColumnLetter = GetNextColumnLetter(currentSourceColumnLetter);
                            }
                            xlWorksheet.get_Range(totalMoneySourceColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
                            xlWorksheet.get_Range(totalMoneySourceColumnLetter + currentRowNumber.ToString()).Formula
                                = string.Format("=SUM({1}{0}:{2}{0})", currentRowNumber, firstMoneySourceColumnLetter, lastMoneySourceColumnLetter);
                        }

                        if(contract.Invoices.Count > 0)
                        {
                            currentSourceColumnLetter = firstMoneySourceColumnLetter;
                            for(int i = 0; i < sourcesNum; i++)
                            {
                                xlWorksheet.get_Range(currentSourceColumnLetter + currentContractRowIndex.ToString()).Formula = 
                                    string.Format("=SUM({0}{1}:{0}{2})", currentSourceColumnLetter, firstInvoiceRowIndex, currentRowNumber);
                                currentSourceColumnLetter = GetNextColumnLetter(currentSourceColumnLetter);
                            }
                        }
                        else
                        {
                            currentSourceColumnLetter = firstMoneySourceColumnLetter;
                            for (int i = 0; i < sourcesNum; i++)
                            {
                                xlWorksheet.get_Range(currentSourceColumnLetter + currentContractRowIndex.ToString()).Value = 0;
                                currentSourceColumnLetter = GetNextColumnLetter(currentSourceColumnLetter);
                            }
                        }
                        xlWorksheet.get_Range(totalMoneySourceColumnLetter + currentContractRowIndex.ToString()).Font.Bold = true;
                        xlWorksheet.get_Range(totalMoneySourceColumnLetter + currentContractRowIndex.ToString()).Formula =
                            string.Format("=SUM({1}{0}:{2}{0})", currentContractRowIndex, firstMoneySourceColumnLetter, lastMoneySourceColumnLetter);

                        xlWorksheet.get_Range(contractRemainColumnLetter + currentContractRowIndex.ToString()).Font.Bold = true;
                        xlWorksheet.get_Range(contractRemainColumnLetter + currentContractRowIndex.ToString()).Formula =
                            string.Format("={1}{0} - {2}{0}", currentContractRowIndex, contractSumColumnLetter, totalMoneySourceColumnLetter);

                        contractNum++;
                    }

                    currentRowNumber++;
                    kekvsTotals.Add("{0}" + currentRowNumber);

                    xlWorksheet.get_Range(contractColumnLetter + currentRowNumber.ToString(),
                            contractRemainColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
                    xlWorksheet.get_Range(contractColumnLetter + currentRowNumber.ToString()).Value = "ВСЬОГО ЗА КЕКВ";
                    string kekvTotalsFormulaString = "=SUM(" + string.Join(",", contractsInKekv) + ")";

                    WriteTotalsInCurrentRow(kekvTotalsFormulaString);

                    contractsInKekv.Clear();
                }

                currentRowNumber++;
                if (groupedByKekvContracts.Count > 0)
                {
                    currentRowNumber++;

                    xlWorksheet.get_Range(contractColumnLetter + currentRowNumber.ToString(),
                            contractRemainColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
                    xlWorksheet.get_Range(contractColumnLetter + currentRowNumber.ToString()).Value = "ВСЬОГО ЗА РІК";
                    string yearTotalsFormulaString = "=SUM(" + string.Join(",", kekvsTotals) + ")";

                    WriteTotalsInCurrentRow(yearTotalsFormulaString);

                    DrawTableBorders(numColumnLetter + (beginRowNumber - 1).ToString(), contractRemainColumnLetter + currentRowNumber.ToString());
                    foreach (var kekvHeader in kekvsHeaders)
                        DrawTableBorders(numColumnLetter + kekvHeader.ToString(), contractRemainColumnLetter + kekvHeader.ToString()); 
                }
                else
                {
                    DrawTableBorders(numColumnLetter + (beginRowNumber - 1).ToString(), contractRemainColumnLetter + currentRowNumber.ToString());
                    xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString(),
                        contractRemainColumnLetter + currentRowNumber.ToString()).Merge();
                    xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString()).Font.Bold = true;
                    xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString()).Font.Italic = true;
                    xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString()).Value = "Інформація по договорам відсутня";
                }

                // Выравниваем заголовки отчёта
                xlWorksheet.get_Range(numColumnLetter + reportTitleRow, contractRemainColumnLetter + reportTitleRow).Merge();
                xlWorksheet.get_Range(numColumnLetter + yearCell, contractRemainColumnLetter + yearCell).Merge();
                xlWorksheet.get_Range(numColumnLetter + estimateNameCell, contractRemainColumnLetter + estimateNameCell).Merge();
                xlWorksheet.get_Range(numColumnLetter + dateCell, contractRemainColumnLetter + dateCell).Merge();
            }
        }

        private void WriteTotalsInCurrentRow(string totalFormula)
        {
            xlWorksheet.get_Range(numColumnLetter + currentRowNumber.ToString(),
                contractRemainColumnLetter + currentRowNumber.ToString()).Font.Bold = true;

            xlWorksheet.get_Range(contractSumColumnLetter + currentRowNumber.ToString()).Formula =
                string.Format(totalFormula, contractSumColumnLetter);

            currentSourceColumnLetter = firstMoneySourceColumnLetter;
            for (int i = 0; i < sourcesNum; i++)
            {
                xlWorksheet.get_Range(currentSourceColumnLetter + currentRowNumber.ToString()).Formula =
                    string.Format(totalFormula, currentSourceColumnLetter);
                currentSourceColumnLetter = GetNextColumnLetter(currentSourceColumnLetter);
            }
            xlWorksheet.get_Range(totalMoneySourceColumnLetter + currentRowNumber.ToString()).Formula =
                string.Format("=SUM({1}{0}:{2}{0})", currentRowNumber, firstMoneySourceColumnLetter, lastMoneySourceColumnLetter);

            xlWorksheet.get_Range(contractRemainColumnLetter + currentRowNumber.ToString()).Formula =
                string.Format("={1}{0} - {2}{0}", currentRowNumber, contractSumColumnLetter, lastMoneySourceColumnLetter);
        }
    }
}
