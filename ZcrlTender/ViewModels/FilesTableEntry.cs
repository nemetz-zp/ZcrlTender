using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZcrlTender.ViewModels
{
    public class FilesTableEntry
    {
        public string FileName { get; set; }
        public string FileExt
        {
            get
            {
                if(string.IsNullOrWhiteSpace(FileName))
                {
                    return "";
                }

                try
                {
                    string result = string.Format("*{0}", new System.IO.FileInfo(FileName).Extension);
                    return result;
                }
                catch(Exception ex)
                {
                    return "";
                }
            }
        }
    }
}
