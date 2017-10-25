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
    public class FileManager
    {
        private static string uploadFilesDir = "files";
        private static string templateFilesDir = "templates";

        public static string UploadFilesDir
        {
            get { return Path.Combine(GetApplicationDirectory(), FileManager.uploadFilesDir); }
        }

        public static string TemplateFilesDir
        {
            get { return Path.Combine(GetApplicationDirectory(), FileManager.templateFilesDir); }
        }

        public static string GetApplicationDirectory()
        {
            return Application.StartupPath;
        }

        public static bool WasFileUploaded(UploadedFile file)
        {
            return File.Exists(Path.Combine(UploadFilesDir, file.PhisicalName));
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

        public static void UploadFile(UploadedFile fileToUpload)
        {
            FileInfo fInfo = new FileInfo(fileToUpload.PhisicalName);

            string uploadFileExt = fInfo.Extension;

            if (!WasFileUploaded(fileToUpload))
            {
                // Генерируем уникальное имя файла
                string newFileName = Guid.NewGuid().ToString() + fInfo.Extension;
                while (File.Exists(Path.Combine(UploadFilesDir, newFileName)))
                {
                    newFileName = Guid.NewGuid().ToString() + fInfo.Extension;
                }
                fileToUpload.PhisicalName = newFileName;

                fInfo.CopyTo(Path.Combine(UploadFilesDir, newFileName));
            }
        }
    }
}
