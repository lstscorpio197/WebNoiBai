using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebNoiBai.WHttpMessage
{
    public class HttpMessageBody
    {
        public object Data { get; set; }

        public string Description { get; set; }

        public HttpMessagePagination Pagination { get; set; }

        public HttpMessageNoti MsgNoti { get; set; }

        public HttpMessageBody()
        {
            MsgNoti = new HttpMessageNoti();
        }

        public HttpMessageBody(int numberRowsOnPage)
        {
            Pagination = new HttpMessagePagination(numberRowsOnPage);
            MsgNoti = new HttpMessageNoti();
        }
    }
}