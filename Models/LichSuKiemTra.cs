//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebNoiBai.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class LichSuKiemTra
    {
        public decimal Id { get; set; }
        public decimal DoiTuongId { get; set; }
        public string KetQuaKT { get; set; }
        public Nullable<System.DateTime> NgayKT { get; set; }
        public string NguoiTao { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
    
        public virtual SDoiTuongDaKT SDoiTuongDaKT { get; set; }
    }
}
