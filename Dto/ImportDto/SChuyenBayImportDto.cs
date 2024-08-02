using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebNoiBai.Dto.ImportDto
{
    public class SChuyenBayImportDto
    {
        public DateTime Ngay { get; set; }
        public string SoHieu { get; set; }
        public string Gio { get; set; }
        public string ChangBay { get; set; }
        public string DaoHanhLy { get; set; }
        public string CuaSo { get; set; }
        public string AmPm { get; set; }

        public int? SOBT
        {
            get
            {
                if (string.IsNullOrEmpty(Gio))
                {
                    return null;
                }
                Gio = Gio.Trim().Substring(Gio.Length - 5, 5).Replace(":", "");

                int gio = int.Parse(Gio);
                if (AmPm.ToLower() == "am")
                    return gio;
                else
                    return gio % 1200 + 1200;
            }
        }

        public string ChuyenBay
        {
            get
            {
                int length = SoHieu.Length;
                if (length > 5)
                {
                    return SoHieu;
                }
                else
                {
                    bool parseSo = int.TryParse(SoHieu, out int so);
                    if (parseSo) {
                        return SoHieu.Substring(0,2) + so.ToString("D4");
                    }
                    else
                    {
                        string str = SoHieu.Substring(2);
                        if (str.Length == 1) {
                            return SoHieu.Substring(0, 2) + "000"+str;
                        }
                        if (str.Length == 2)
                        {
                            return SoHieu.Substring(0, 2) + "00" + str;
                        }
                        if (str.Length == 3)
                        {
                            return SoHieu.Substring(0, 2) + "0" + str;
                        }
                        return SoHieu;
                    }
                }
            }
        }
    }
}