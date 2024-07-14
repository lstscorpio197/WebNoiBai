using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebNoiBai.WHttpMessage
{
    public class HttpMessageNoti
    {
        public string Code { get; set; } = "000";


        public string Name { get; set; }

        public string Description { get; set; }

        public List<object> Errors { get; set; }

        public HttpMessageNoti()
        {
            Code = "000";
        }

        public HttpMessageNoti(string notiCode, string name = null, string description = null)
        {
            Code = notiCode;
            Name = name;
            Description = description;
        }
    }
}