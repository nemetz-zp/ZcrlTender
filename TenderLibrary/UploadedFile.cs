using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenderLibrary
{
    public class UploadedFile
    {
        public long Id { get; set; }
        public string PhisicalName { get; set; }
        public string PublicName { get; set; }
        public DateTime UploadDate { get; set; }

        [NotMapped]
        public string FileExt
        {
            get
            {
                return string.Format("*{0}", (new FileInfo(PhisicalName).Extension));
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            UploadedFile castedObj = obj as UploadedFile;
            if (castedObj == null)
                return false;

            return (Id == castedObj.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

    }
}
