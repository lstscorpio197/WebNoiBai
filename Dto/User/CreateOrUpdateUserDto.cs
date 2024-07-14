using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebNoiBai.Dto
{
    public class CreateOrUpdateUserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public Nullable<int> ChucVu { get; set; }
        public Nullable<int> IsActived { get; set; }
        public string SDT { get; set; }
        public int PhongBan { get; set; }
    }
}