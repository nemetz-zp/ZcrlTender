using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TenderLibrary;
using Excel = Microsoft.Office.Interop.Excel;

namespace ZcrlTender.ExcelReports
{
    abstract public class ExcelReportMaker
    {
        protected Excel.Application xlApp;
        protected Excel.Workbooks xlWorkbooksCollection;
        protected Excel.Workbook xlWorkbook;
        protected Excel.Worksheet xlWorksheet;

        // Путь файлу-шаблону отчёта
        protected abstract string TemplateFile { get; }
        // Полное имя файла под которым будет сохранён сформированный файл-отчёт
        protected abstract string SaveReportFile { get; }

        protected bool OpenExcelFile(string fileName)
        {
            xlApp = new Excel.Application();

            if (xlApp == null)
            {
                NotificationHelper.ShowError("Неможливо створити екземпляр програми Excel");
                return false;
            }

            xlApp.Visible = false;

            xlWorkbooksCollection = xlApp.Workbooks;
            xlWorkbook = xlWorkbooksCollection.Open(fileName);

            if (xlWorkbook == null)
            {
                NotificationHelper.ShowError("Неможливо відкрити файл-шаблон Excel");
                return false;
            }

            xlWorksheet = (Excel.Worksheet)xlWorkbook.ActiveSheet;
            xlApp.DisplayAlerts = false;

            return xlWorksheet != null;
        }

        protected void SetActiveSheet(int sheetNumber)
        {
            xlWorksheet = (Excel.Worksheet)xlWorkbook.Sheets[sheetNumber];
        }

        protected void TerminateExcelProcessInstance()
        {
            if (xlApp != null && xlWorkbook != null && xlWorksheet != null)
            {
                xlApp.Quit();
                Marshal.ReleaseComObject(xlWorksheet);
                Marshal.ReleaseComObject(xlWorkbook);
                Marshal.ReleaseComObject(xlWorkbooksCollection);
                Marshal.ReleaseComObject(xlApp);
            }
        }
        
        // Получить букву следующего столбца (в Excel)
        protected string GetNextColumnLetter(string currentColumnLetter)
        {
            int lastSymbolIndex = currentColumnLetter.Count() - 1;
            char lastSymbol = currentColumnLetter[lastSymbolIndex];

            if (currentColumnLetter[lastSymbolIndex].Equals('Z'))
            {
                if (lastSymbolIndex > 0)
                {
                    return GetNextColumnLetter(currentColumnLetter.Substring(0, lastSymbolIndex)) + "A";
                }
                else
                {
                    return "AA";
                }
            }
            else
            {
                return currentColumnLetter.Substring(0, lastSymbolIndex) + (++lastSymbol);
            }
        }

        // Затрата по источнику финансирования
        protected class MoneySourceSpending
        {
            public MoneySource Source { get; set; }
            public decimal Sum { get; set; }
        }

        // Заполнение строки с тратами по источникам финансирования
        protected decimal[] GetMoneySourceSpendingRow(
            List<MoneySource> sources,
            List<MoneySourceSpending> spending)
        {
            decimal[] row = new decimal[sources.Count];
            for (int i = 0; i < spending.Count; i++)
            {
                int indexOfSource = sources.IndexOf(spending[i].Source);
                row[indexOfSource] += spending[i].Sum;
            }

            return row;
        }

        protected void DrawTableBorders(string firstCoordOfRange, string secondCoordOfRange = null)
        {
            Excel.Range range = null;

            if(secondCoordOfRange != null)
            {
                range = xlWorksheet.get_Range(firstCoordOfRange, secondCoordOfRange);
            }
            else
            {
                range = xlWorksheet.get_Range(firstCoordOfRange);
            }

            DrawTableBorders(range);
        }

        protected void DrawTableBorders(Excel.Range range)
        {
            range.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight =
            range.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight =
            range.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight =
            range.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = 3d;
            range.Borders[Excel.XlBordersIndex.xlInsideHorizontal].Weight =
            range.Borders[Excel.XlBordersIndex.xlInsideVertical].Weight = 2d;
        }

        public void MakeReport()
        {
            try
            {
                OpenExcelFile(TemplateFile);
                WriteDataToFile();
                SaveReportToFile();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                TerminateExcelProcessInstance();
            }
        }

        // Сохранение отчёта в файл
        private void SaveReportToFile()
        {
            string fileNameToSave = System.IO.Path.Combine(FileManager.ReportDirectoryFullPath,
                FileManager.ClearIllegalFileNameSymbols(SaveReportFile));
            string newFileName = fileNameToSave;

            string extOfTemplate = System.IO.Path.GetExtension(TemplateFile);

            int i = 1;
            while (System.IO.File.Exists(newFileName + extOfTemplate))
            {
                newFileName = string.Format("{0} ({1})", fileNameToSave, i);
                i++;
            }

            xlWorkbook.SaveAs(newFileName);
        }

        protected abstract void WriteDataToFile();
    }
}
