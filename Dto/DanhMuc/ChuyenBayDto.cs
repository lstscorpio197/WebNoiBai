using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebNoiBai.Dto.DanhMuc
{
    public class ChuyenBayDto
    {
        public int Id {  get; set; }
        public string ChuyenBay {  get; set; }
        public int GioKhoiHanh_Gio {  get; set; }
        public int? GioKhoiHanh_Phut {  get; set; }
        public int GioKetThuc_Gio {  get; set; }
        public int? GioKetThuc_Phut { get; set; }

        public string GioKhoiHanh => GioKhoiHanh_Gio.ToString("D2") + ":"+GioKhoiHanh_Phut?.ToString("D2") ?? "00";
        public string GioKetThuc => GioKetThuc_Gio.ToString("D2") + ":"+ GioKetThuc_Phut?.ToString("D2") ?? "00";
    }
}