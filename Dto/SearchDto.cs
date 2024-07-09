using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebNoiBai.Dto
{
    public class SearchDto
    {
        public string Ma { get; set; }
        public string Ten { get; set; }
        public int Enable { get; set; } = -1;
        public int PageNum { get; set; } = 1;
        public int PageSize { get; set; } = 100;

        public int Skip => (PageNum - 1) * PageSize;
    }
}