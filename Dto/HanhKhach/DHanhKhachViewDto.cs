﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;

namespace WebNoiBai.Dto.HanhKhach
{
    public class DHanhKhachViewDto
    {
        public string SOHIEU { get; set; }
        public Nullable<System.DateTime> FLIGHTDATE { get; set; }
        public string MANOIDI { get; set; }
        public string MANOIDEN { get; set; }
        public long IDCHUYENBAY { get; set; }
        public string MADATCHO { get; set; }
        public string GIOITINH { get; set; }
        public string HO { get; set; }
        public string TENDEM { get; set; }
        public string TEN { get; set; }
        public string QUOCTICH { get; set; }
        public string SOGIAYTO { get; set; }
        public string LOAIGIAYTO { get; set; }
        public string NOICAP { get; set; }
        public Nullable<System.DateTime> NGAYSINH { get; set; }
        public string NOIDI { get; set; }
        public string NOIDEN { get; set; }
        public string HANHLY { get; set; }

        public int SOLAN { get; set; }
        public int? SOBT { get; set; }

        public int? SoNguoiDiCung { get; set; } = null;
        public DateTime? NgayDiGanNhat { get; set; } = null;
        public string SoKien { get; set; } = null;
        public string GhiChu { get; set; } = null;

        public string HOTEN => string.Format("{0} {1} {2}", HO, TENDEM, TEN);
        public string FLIGHTDATE_TXT => FLIGHTDATE?.ToString("dd/MM/yyyy");
        public string NGAYSINH_TXT => NGAYSINH?.ToString("dd/MM/yyyy");
        public string GIOITINH_TXT => GIOITINH == "M" ? "Nam" : (GIOITINH == "F" ? "Nữ" : "");
        public string NgayDiGanNhat_TXT => NgayDiGanNhat?.ToString("dd/MM/yyyy") ?? "";
        public string NameArray
        {
            get
            {
                List<string> list = new List<string>();
                if (!string.IsNullOrEmpty(HO))
                {
                    list.AddRange(HO.Trim().Split(' '));
                }
                if (!string.IsNullOrEmpty(TENDEM))
                {
                    list.AddRange(TENDEM.Trim().Split(' '));
                }
                if (!string.IsNullOrEmpty(TEN))
                {
                    list.AddRange(TEN.Trim().Split(' '));
                }
                return string.Join("_", list.OrderBy(x=>x));
            }
        }
    }
    public class DHanhKhachQueryDto
    {
        public string SOHIEU { get; set; }
        public Nullable<System.DateTime> FLIGHTDATE { get; set; }
        public string MANOIDI { get; set; }
        public string MANOIDEN { get; set; }
        public long IDCHUYENBAY { get; set; }
        public string MADATCHO { get; set; }
        public string GIOITINH { get; set; }
        public string HO { get; set; }
        public string TENDEM { get; set; }
        public string TEN { get; set; }
        public string QUOCTICH { get; set; }
        public string SOGIAYTO { get; set; }
        public string LOAIGIAYTO { get; set; }
        public string NOICAP { get; set; }
        public Nullable<System.DateTime> NGAYSINH { get; set; }
        public string NOIDI { get; set; }
        public string NOIDEN { get; set; }
        public string HANHLY { get; set; }

        public int SOLAN { get; set; }
        public int? SOBT { get; set; }

        public int? SoNguoiDiCung { get; set; } = null;
        public DateTime? NgayDiGanNhat { get; set; } = null;
        public string SoKien { get; set; } = null;

    }
}