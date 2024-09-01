using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebNoiBai.Dto.HanhKhach
{
    public class DHanhKhachSearchDto : SearchDto
    {
        
        public string SoGiayTo { get; set; }
        public string SoHieu { get; set; }
        public string NoiDi {  get; set; }
        public string NoiDen {  get; set; }
        public string MaNoiDi { get; set; }
        public string MaNoiDen { get; set; }
        public int? ObjectType { get; set; }
        public string QuocTich {  get; set; }

        public int? NotInObject { get; set; }

        public bool IsDiTuNuocRuiRo {  get; set; }
        public bool IsViewSoKien {  get; set; }
        public bool IsViewDiChung {  get; set; }
        public bool IsViewNgayDiGanNhat {  get; set; }

        public string DiemNoiChuyen { get; set; } = "HAN";

        public int? SoLan { get; set; } = 1;
        public string HoTen {  get; set; }
        public List<string> LstSoGiayTo => SoGiayTo?.Replace(";",",").Split(',').ToList() ?? new List<string>();
        public List<string> LstSoHieu => SoHieu?.Replace(";",",").Split(',').ToList() ?? new List<string>();

    }
}