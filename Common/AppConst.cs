﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebNoiBai.Common
{
    public static class AppConst
    {
        public static string UserSession = "USER_SESSION";
    }

    public class ChucVu
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ChucVu(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
    public static class LstChucVu
    {
        public static List<ChucVu> Init = new List<ChucVu>
        {
            new ChucVu(UserLevel.Admin,"Admin"),
            new ChucVu(UserLevel.ChiCucTruong,"Chi cục trưởng"),
            new ChucVu(UserLevel.PhoChiCucTruong,"Phó chi cục trưởng"),
            new ChucVu(UserLevel.DoiTruong,"Đội trưởng"),
            new ChucVu(UserLevel.DoiPho,"Đội phó"),
            new ChucVu(UserLevel.CongChuc,"Công chức")
        };
    }

    public static class UserLevel
    {
        public static int Admin = 0;
        public static int ChiCucTruong = 1;
        public static int PhoChiCucTruong = 2;
        public static int DoiTruong = 3;
        public static int DoiPho = 4;
        public static int CongChuc = 5;
    }
}