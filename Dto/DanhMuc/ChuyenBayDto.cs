using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebNoiBai.Dto.DanhMuc
{
    public class ChuyenBayDto
    {
        public int Id {  get; set; }
        public DateTime Ngay { get; set; }
        public string ChuyenBay {  get; set; }
        public int? SOBT {  get; set; }
        public int? EOBT {  get; set; }
        public string ChangBay {  get; set; }
        public string DaoHanhLy {  get; set; }
        public string CuaSo {  get; set; }

        public string NgayBay => Ngay.ToString("dd/MM/yyyy");
        public string SOBT_TXT => SOBT?.ToString("D4");
        public string EOBT_TXT => SOBT?.ToString("D4");
    }
}