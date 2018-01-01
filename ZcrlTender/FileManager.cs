using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using TenderLibrary;
using ZcrlTender.ViewModels;
using System.Text.RegularExpressions;

namespace ZcrlTender
{
    // Логика работы с файлами
    public class FileManager
    {
        // Название каталога с загружаемыми файлами
        private static string uploadFilesDir = "files";
        // Название каталога с шаблонами отчётов
        private static string templateFilesDir = "templates";

        // Путь к каталогу с загружаемыми файлами
        public static string UploadFilesDir
        {
            get 
            { 
                if(MainProgramForm.CurrentTenderYear == null)
                {
                    throw new Exception("Рік закупівлі не встановлено");
                }

                return Path.Combine(GetApplicationDirectory(), FileManager.uploadFilesDir, MainProgramForm.CurrentTenderYear.Year.ToString()); 
            }
        }

        static FileManager()
        {
            // Проверяем наличие директории с шаблонами
            if(!Directory.Exists(TemplateFilesDir))
            {
                Directory.CreateDirectory(TemplateFilesDir);
            }

            // Проверяем наличие файла-шаблона поступлений по смете
            if(!File.Exists(FullEstimateTemplateFile))
            {
                byte[] templateBytes = Properties.Resources.full_estimate_template;
                using(FileStream fs = new FileStream(FullEstimateTemplateFile, FileMode.Create))
                {
                    fs.Write(templateBytes, 0, templateBytes.Length);
                    fs.Close();
                }
            }

            // Проверяем наличие файла-шаблона фактических трат по смете
            if (!File.Exists(EstimateMonthSpendingTemplateFile))
            {
                byte[] templateBytes = Properties.Resources.estimate_month_spending_template;
                using (FileStream fs = new FileStream(EstimateMonthSpendingTemplateFile, FileMode.Create))
                {
                    fs.Write(templateBytes, 0, templateBytes.Length);
                    fs.Close();
                }
            }

            // Проверяем наличие файла-шаблона отчёта по фактическим тратам по смете
            if (!File.Exists(ContractSpendingTemplateFile))
            {
                byte[] templateBytes = Properties.Resources.contracts_spendings;
                using (FileStream fs = new FileStream(ContractSpendingTemplateFile, FileMode.Create))
                {
                    fs.Write(templateBytes, 0, templateBytes.Length);
                    fs.Close();
                }
            }

            // Проверяем наличие файла-шаблона годового плана
            if (!File.Exists(YearPlanTemplateFile))
            {
                byte[] templateBytes = Properties.Resources.full_yearplanbycontacts_template;
                using (FileStream fs = new FileStream(YearPlanTemplateFile, FileMode.Create))
                {
                    fs.Write(templateBytes, 0, templateBytes.Length);
                    fs.Close();
                }
            }

            // Проверяем наличие файла-шаблона истории изменения записей годового плана
            if (!File.Exists(YearPlanChangesHistoryTemplateFile))
            {
                byte[] templateBytes = Properties.Resources.yearplan_changes_history;
                using (FileStream fs = new FileStream(YearPlanChangesHistoryTemplateFile, FileMode.Create))
                {
                    fs.Write(templateBytes, 0, templateBytes.Length);
                    fs.Close();
                }
            }

            // Проверяем наличие файла-шаблона отчёта по текущим остаткам и новым счетам
            if (!File.Exists(CurrentMoneyRemainAndNewInvoicesTemplate))
            {
                byte[] templateBytes = Properties.Resources.current_remains_and_new_invoices;
                using (FileStream fs = new FileStream(CurrentMoneyRemainAndNewInvoicesTemplate, FileMode.Create))
                {
                    fs.Write(templateBytes, 0, templateBytes.Length);
                    fs.Close();
                }
            }
        }

        // Путь к каталогу с шаблонами
        public static string TemplateFilesDir
        {
            get { return Path.Combine(GetApplicationDirectory(), FileManager.templateFilesDir); }
        }

        // Путь к каталогу, в которых сохраняються отчёты
        public static string ReportDirectoryFullPath
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
        }

        // Путь к файлу-шаблону годовых поступлений по смете
        public static string FullEstimateTemplateFile
        {
            get
            {
                return System.IO.Path.Combine(FileManager.TemplateFilesDir, "full_estimate_template.xls");
            }
        }

        // Путь к файлу-шаблону отчёта затрат по договорам
        public static string ContractSpendingTemplateFile
        {
            get
            {
                return System.IO.Path.Combine(FileManager.TemplateFilesDir, "contracts_spendings.xls");
            }
        }

        // Путь к файлу-шаблону годового плана с фактическими тратами
        public static string EstimateMonthSpendingTemplateFile
        {
            get
            {
                return System.IO.Path.Combine(FileManager.TemplateFilesDir, "estimate_month_spending_template.xls");
            }
        }

        // Путь к файлу-шаблону годовых затрат по смете
        public static string YearPlanTemplateFile
        {
            get
            {
                return System.IO.Path.Combine(FileManager.TemplateFilesDir, "full_yearplanbycontacts_template.xls");
            }
        }

        // Путь к файлу-шаблону истории изменений годового плана
        public static string YearPlanChangesHistoryTemplateFile
        {
            get
            {
                return System.IO.Path.Combine(FileManager.TemplateFilesDir, "yearplan_changes_history.xls");
            }
        }

        // Путь к файлу-шаблону отчёта по текущим остаткам и новым счетам
        public static string CurrentMoneyRemainAndNewInvoicesTemplate
        {
            get
            {
                return System.IO.Path.Combine(FileManager.TemplateFilesDir, "current_remains_and_new_invoices.xls");
            }
        }

        public static string GetApplicationDirectory()
        {
            return Application.StartupPath;
        }

        // Загружен ли файл в специальный каталог приложения
        public static bool WasFileUploaded(UploadedFile file)
        {
            if (string.IsNullOrWhiteSpace(Path.GetDirectoryName(file.PhisicalName)))
            {
                return File.Exists(Path.Combine(UploadFilesDir, file.PhisicalName));
            }
            else
            {
                return false;
            }
        }
        public static bool WasFileUploaded(string filePath)
        {
            return WasFileUploaded(new UploadedFile { PhisicalName = filePath });
        }

        // Чистим имя файла от недопустимых символов
        public static string ClearIllegalFileNameSymbols(string fileName)
        {
            string newFileName = fileName;
            string illegalFilenameChars = new string(Path.GetInvalidFileNameChars());
            Regex regTEmplate = new Regex(string.Format("[{0}]", Regex.Escape(illegalFilenameChars)));
            newFileName = regTEmplate.Replace(fileName, "");

            return newFileName;
        }

        // Загрузка файла в каталог с приложением
        public static void UploadFile(UploadedFile fileToUpload)
        {
            if (!WasFileUploaded(fileToUpload))
            {
                FileInfo fInfo = new FileInfo(fileToUpload.PhisicalName);

                string uploadFileExt = fInfo.Extension;

                // Генерируем уникальное имя файла
                string newFileName = Guid.NewGuid().ToString() + fInfo.Extension;
                while (File.Exists(Path.Combine(UploadFilesDir, newFileName)))
                {
                    newFileName = Guid.NewGuid().ToString() + fInfo.Extension;
                }
                fileToUpload.PhisicalName = newFileName;

                if(!Directory.Exists(UploadFilesDir))
                {
                    Directory.CreateDirectory(UploadFilesDir);
                }
                fInfo.CopyTo(Path.Combine(UploadFilesDir, newFileName));
            }
        }

        // Скачивание файла
        public static void DownloadFile(UploadedFile file)
        {
            string ext = Path.GetExtension(file.PhisicalName);

            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = string.Format("Файл {0}|*{0}", ext);
            sf.FileName = file.PublicName;

            if (sf.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Если файл загружен в директорию с приложением
                    if (WasFileUploaded(file))
                    {
                        string sourceFile = Path.Combine(UploadFilesDir, file.PhisicalName);
                        File.Copy(sourceFile, sf.FileName);
                    }
                    else
                    {
                        File.Copy(file.PhisicalName, sf.FileName);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // Обновление списка связаных с сущностью файлов
        public static void UpdateRelatedFilesOfEntity(TenderContext tc,
            ICollection<UploadedFile> entityFilesCollection,
            ICollection<UploadedFile> filesList,
            ICollection<UploadedFile> deletingFilesList)
        {

            entityFilesCollection.Clear();
            tc.SaveChanges();
            foreach (var file in filesList)
            {
                if (file.Id > 0)
                {
                    tc.UploadedFiles.Attach(file);
                    tc.Entry<UploadedFile>(file).State = System.Data.Entity.EntityState.Modified;
                }
                entityFilesCollection.Add(file);
                FileManager.UploadFile(file);
            }

            foreach (var file in deletingFilesList)
            {
                if(file.Id > 0)
                {
                    tc.UploadedFiles.Attach(file);
                    tc.UploadedFiles.Remove(file);
                }
                try
                {
                    File.Delete(Path.Combine(UploadFilesDir, file.PhisicalName));
                }
                catch (Exception ex) { }
            }

            tc.SaveChanges();
        }
    }
}
