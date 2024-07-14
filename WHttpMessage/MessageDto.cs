using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebNoiBai.WHttpMessage
{
    public class MessageDto
    {
        public bool IsOk { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public object Data { get; set; }

        public object SubData { get; set; }

        public List<object> Errors { get; set; }

        public MessageDto()
        {
            IsOk = false;
        }

        public MessageDto(bool isOk)
        {
            IsOk = isOk;
        }
    }
}