using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebNoiBai.Common
{
    public class ExcelImport
    {
        public ExcelImport(string _MaColumn, string _TenColumn, string _ExcelColumn, bool _IsBatBuoc = false, string _TypeColumn = "string", string _DefaultValue = "", bool _IsColumnJoinUpdate = false, bool _IsColumnUpdate = false)
        {
            MaColumn = _MaColumn;
            TenColumn = _TenColumn;
            ExcelColumn = _ExcelColumn;
            IsBatBuoc = _IsBatBuoc;
            TypeColumn = _TypeColumn;
            DefaultValue = _DefaultValue;
        }

        public string MaColumn { get; set; }
        public string TenColumn { get; set; }
        public string TypeColumn { get; set; }
        public bool IsBatBuoc { get; set; }
        public string ExcelColumn { get; set; }
        public string DefaultValue { get; set; }
    }
}