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
    
    public partial class SDoiTuongDaKT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SDoiTuongDaKT()
        {
            this.LichSuKiemTras = new HashSet<LichSuKiemTra>();
        }
    
        public decimal Id { get; set; }
        public string HoTen { get; set; }
        public string SoGiayTo { get; set; }
        public string LoaiGiayTo { get; set; }
        public Nullable<System.DateTime> NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string QuocTich { get; set; }
        public string GhiChu { get; set; }
        public string KetQuaKT { get; set; }
        public System.DateTime NgayKT { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public Nullable<System.DateTime> NgaySua { get; set; }
        public string NguoiSua { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LichSuKiemTra> LichSuKiemTras { get; set; }
    }
}
