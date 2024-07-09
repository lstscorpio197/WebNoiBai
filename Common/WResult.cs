using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebNoiBai.Common
{
    public class WResult
    {
        public bool IsOk {  get; set; }
        public int MsgType {  get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public object SubData { get; set; }
        public int TotalRowData {  get; set; }
        public WResult(bool isOk)
        {
            IsOk = isOk;
        }
        public WResult()
        {
                
        }
    }
}