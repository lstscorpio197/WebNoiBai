using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebNoiBai.Common;

namespace WebNoiBai.Dto.User
{
    public class SUserViewDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string HoTen {  get; set; }
        public string Email { get; set; }
        public string PhongBan { get; set; }
        public int ChucVu {  get; set; }
        public int? IsActived {  get; set; }
        public string ChucVuTxt => LstChucVu.Init.FirstOrDefault(y => y.Id == ChucVu)?.Name;
    }
}