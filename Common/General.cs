using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;

namespace WebNoiBai.Common
{
    public class General
    {
        public static void WriteLog(Exception ex)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ": " + ex.Source + "; " + ex.Message);
                sw.Flush();
                sw.Close();
            }
            catch
            {
                // ignored
            }
        }
        public static void WriteLog(string message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ": " + message);
                sw.Flush();
                sw.Close();
            }
            catch
            {
                // ignored
            }
        }

        public static DateTime? ConvertStringToDateTime(string strDate, bool IsDdMmyyyy = true)
        {
            if (string.IsNullOrEmpty(strDate))
            {
                return null;
            }
            try
            {
                if (IsDdMmyyyy)
                {
                    return Convert.ToDateTime(strDate, new System.Globalization.CultureInfo("vi-VN").DateTimeFormat);
                }
                strDate = strDate.Replace("-", "/");
                DateTime startDate;
                string[] formats = { "MM/dd/yyyy", "MM/dd/yyyy HH:mm:ss", "yyyy/MM/dd", "yyyy/MM/dd HH:mm:ss" };
                DateTime.TryParseExact(strDate, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out startDate);
                return startDate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static void WriteLog(string action, Exception ex)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ": " + action + "---" + ex.Source + "; " + ex.Message);
                sw.Flush();
                sw.Close();
            }
            catch
            {
                // ignored
            }
        }

    }
}