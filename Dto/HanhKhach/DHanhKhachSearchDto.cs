using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebNoiBai.Dto.HanhKhach
{
    public class DHanhKhachSearchDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string SoGiayTo { get; set; }
        public string SoHieu { get; set; }
        public string NoiDi {  get; set; }
        public string NoiDen {  get; set; }
        public int? ObjectType {  get; set; }

        public int PageNum { get; set; } = 1;
        public int PageSize { get; set; } = 100;

        public int Skip => (PageNum - 1) * PageSize;
        public List<string> LstSoGiayTo => SoGiayTo?.Replace(";",",").Split(',').ToList() ?? new List<string>();
        public List<string> LstSoHieu => SoHieu?.Replace(";",",").Split(',').ToList() ?? new List<string>();

    }
}