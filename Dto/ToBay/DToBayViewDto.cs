using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebNoiBai.Dto.ToBay
{
    public class DToBayViewDto
    {
        public string SOHIEU { get; set; }
        public Nullable<System.DateTime> FLIGHTDATE { get; set; }
        public long IDCHUYENBAY { get; set; }
        public string GIOITINH { get; set; }
        public string HO { get; set; }
        public string TENDEM { get; set; }
        public string TEN { get; set; }
        public string QUOCTICH { get; set; }
        public string SOGIAYTO { get; set; }
        public string LOAIGIAYTO { get; set; }
        public string NOICAP { get; set; }
        public Nullable<System.DateTime> NGAYSINH { get; set; }

        public string HOTEN => string.Format("{0} {1} {2}", HO, TENDEM, TEN);
        public string FLIGHTDATE_TXT => FLIGHTDATE?.ToString("dd/MM/yyyy");
        public string NGAYSINH_TXT => NGAYSINH?.ToString("dd/MM/yyyy");
        public string GIOITINH_TXT => GIOITINH == "M" ? "Nam" : (GIOITINH == "F" ? "Nữ" : "");
    }
}