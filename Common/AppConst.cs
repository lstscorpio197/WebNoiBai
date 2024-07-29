using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebNoiBai.Common
{
    public static class AppConst
    {
        public static string UserSession = "USER_SESSION";


        public static string KeyGemBoxDocument = "DOVJ-6B74-HTFU-1VVQ";
        public static string KeyGemBoxSpreadsheet = "ETZX-IT28-33Q6-1HA2";

        public static string ZEFExtensionsName = "311;101-AJDHNCJ";
        public static string ZEFExtensionsKey = "5010A0A-13BF275-DDBF6C2-80CDD0A-4E10";

        public static string ZBulkName = "116;301-DGDSGSR";
        public static string ZBulkKey = "ADA3D50-121A4FA-B2A4464-306B22D-2B98";
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
        public const int Admin = 0;
        public const int ChiCucTruong = 1;
        public const int PhoChiCucTruong = 2;
        public const int DoiTruong = 3;
        public const int DoiPho = 4;
        public const int CongChuc = 5;
    }
}