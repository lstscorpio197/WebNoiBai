using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebNoiBai.Common
{
    public class SFile
    {
        public int LimitSizeFile = 4; //MB
        public long MaxLengFileAccept = 1 * 1024 * 1024; //1MB

        public SFile()
        {
            LimitSizeFile = 4;
            MaxLengFileAccept = LimitSizeFile * 1024 * 1024;
        }

        public SFile(int _limitSizeFile)
        {
            LimitSizeFile = _limitSizeFile;
            this.MaxLengFileAccept = _limitSizeFile * 1024 * 1024;
        }

        public bool IsValidFileLenght(long FileLenght = 0)
        {
            FileLenght = (FileLenght < 0 ? 0 : FileLenght);
            if (FileLenght > 0 && FileLenght <= MaxLengFileAccept)
            {
                return true;
            }
            return false;
        }

        public bool IsValidFileAccept(string FileName)
        {
            try
            {
                string extFile = System.IO.Path.GetExtension(FileName).Substring(1).ToLower();
                List<string> lstExtensionFileExce = new List<string>()
                {
                    "pdf", "doc","docx", "xls", "xlsx","ppt", "pptx", "txt", "png", "jpeg","icon"
                };
                return lstExtensionFileExce.Contains(extFile);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsValidationExcelFile(string FileName, string FileContentType)
        {
            string extFile = System.IO.Path.GetExtension(FileName).Substring(1).ToLower();
            List<string> lstExtensionFileExce = new List<string>() { "application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" };
            if ((extFile == "xls" || extFile == "xlsx" || extFile == "csv") && lstExtensionFileExce.Contains(FileContentType))
            {
                return true;
            }
            return false;
        }
    }
}