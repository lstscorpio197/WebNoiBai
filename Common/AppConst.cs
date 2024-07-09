using System;
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
            new ChucVu(0,"Admin"),
            new ChucVu(1,"Chi cục trưởng"),
            new ChucVu(2,"Phó chi cục trưởng"),
            new ChucVu(3,"Đội trưởng"),
            new ChucVu(4,"Đội phó"),
            new ChucVu(5,"Công chức")
        };
    }
}