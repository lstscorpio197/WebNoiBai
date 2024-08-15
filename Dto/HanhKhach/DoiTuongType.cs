using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebNoiBai.Dto.HanhKhach
{
    public static class DoiTuongType
    {
        public const int DiLaiNhieu = 0;
        public const int TrongDiem = 1;
        public const int DaKiemTra = 2;
        public const int HanhKhachVIP = 3;
        public const int HuongDanVien = 4;
        public const int TheoDoi = 5;
        public const int TheoDoiDacBiet = 6;
    }

    public class DoiTuongType_Sheet
    {
        public int Type { get; set; }
        public int Sheet { get; set; }
        public DoiTuongType_Sheet(int type, int sheet)
        {
            Type = type;
            Sheet = sheet;
        }
    }
}