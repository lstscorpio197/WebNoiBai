﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DataXNCEntities : DbContext
    {
        public DataXNCEntities()
            : base("name=DataXNCEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<cang_hang_khong> cang_hang_khong { get; set; }
        public virtual DbSet<canhbao_hanhkhach> canhbao_hanhkhach { get; set; }
        public virtual DbSet<chuyenbay_hanhkhach> chuyenbay_hanhkhach { get; set; }
        public virtual DbSet<hang_hang_khong> hang_hang_khong { get; set; }
        public virtual DbSet<mien_thue> mien_thue { get; set; }
        public virtual DbSet<warning_history> warning_history { get; set; }
        public virtual DbSet<SHanhKhachDiLaiNhieu> SHanhKhachDiLaiNhieux { get; set; }
        public virtual DbSet<SHanhKhachVIP> SHanhKhachVIPs { get; set; }
        public virtual DbSet<SDoiTuongTrongDiem> SDoiTuongTrongDiems { get; set; }
        public virtual DbSet<SDoiTuongDaKT> SDoiTuongDaKTs { get; set; }
        public virtual DbSet<SNuocRuiRo> SNuocRuiRoes { get; set; }
        public virtual DbSet<STheoDoiDacBiet> STheoDoiDacBiets { get; set; }
        public virtual DbSet<STheoDoi> STheoDois { get; set; }
        public virtual DbSet<SHuongDanVien> SHuongDanViens { get; set; }
        public virtual DbSet<SChuyenBay> SChuyenBays { get; set; }
        public virtual DbSet<chuyenbay_hh> chuyenbay_hh { get; set; }
        public virtual DbSet<chuyenbay_pnr> chuyenbay_pnr { get; set; }
        public virtual DbSet<chuyenbay_tobay> chuyenbay_tobay { get; set; }
        public virtual DbSet<chuyenbay_vandon> chuyenbay_vandon { get; set; }
        public virtual DbSet<SDauHieuRuiRo> SDauHieuRuiRoes { get; set; }
    }
}
