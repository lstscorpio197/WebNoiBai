using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebNoiBai.WHttpMessage
{
    public class HttpMessage
    {
        public bool IsOk { get; set; }

        public HttpMessageHeader Header { get; set; }

        public HttpMessageBody Body { get; set; }

        public HttpMessageFooter Footer { get; set; }

        public HttpMessage()
        {
            IsOk = false;
            Header = new HttpMessageHeader();
            Body = new HttpMessageBody();
            Footer = new HttpMessageFooter();
        }

        public HttpMessage(bool isOk)
        {
            IsOk = isOk;
            Header = new HttpMessageHeader();
            Body = new HttpMessageBody();
            Footer = new HttpMessageFooter();
        }

        public void CreateMessage_ClientError(string msgContent, string code = "")
        {
            HttpMessageNoti httpMessageNoti = new HttpMessageNoti("400");
            httpMessageNoti.Name = "ClientError";
            httpMessageNoti.Description = msgContent;
            httpMessageNoti.Code = code;
            Body.MsgNoti = httpMessageNoti;
        }

        public void CreateMessage_ClientError(MessageDto msgDto)
        {
            IsOk = msgDto.IsOk;
            HttpMessageNoti httpMessageNoti = new HttpMessageNoti("400");
            httpMessageNoti.Code = msgDto.Code;
            httpMessageNoti.Name = msgDto.Name;
            httpMessageNoti.Description = msgDto.Description ?? "Đã có lỗi xảy ra, vui lòng kiểm tra lại.";
            httpMessageNoti.Errors = msgDto.Errors;
            Body.MsgNoti = httpMessageNoti;
        }

        public void CreateMessage_ServerError(Exception ex)
        {
            HttpMessageNoti httpMessageNoti = new HttpMessageNoti("500");
            httpMessageNoti.Name = "ServerError";
            httpMessageNoti.Description = ex.Message;
            Body.MsgNoti = httpMessageNoti;
        }
    }
}