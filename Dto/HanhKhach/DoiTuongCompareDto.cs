using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace WebNoiBai.Dto.HanhKhach
{
    public class DoiTuongCompareDto
    {
        public DateTime? NGAYSINH {  get; set; }
        public string GIOITINH {  get; set; }
        public string HOTEN { get; set; }
        public string SOGIAYTO {  get; set; }
        public string NameArray
        {
            get
            {
                List<string> list = HOTEN.Split(' ').ToList();
                list.ForEach(x=>x.Trim());
                return string.Join("_", list.OrderBy(x => x));
            }
        }
    }
}