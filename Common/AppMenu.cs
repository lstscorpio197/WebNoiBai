using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace WebNoiBai.Common
{
    public static class AppMenu
    {
        public static List<Menu> ListMenu = new List<Menu>
        {
            new Menu(1,null,"QT","Quản trị",0,"fa fa-cogs"),
            new Menu(0,1,"SPhongBan","Quản lý phòng ban"),
            new Menu(0,1,"SUser","Quản lý tài khoản"),
            new Menu(0,1,"SRole","Quản lý nhóm quyền"),
            new Menu(0,1,"SPermission","Quản lý quyền"),
            new Menu(2,null,"DM","Danh mục",0, "fa fa-list"),
            new Menu(0,2,"SCangHangKhong","Danh sách cảng hàng không"),
            new Menu(0,2,"SHangHangKhong","Danh sách hãng hàng không"),
            new Menu(0,2,"SChuyenBay","Danh mục chuyến bay"),
            new Menu(0,2,"SNuocRuiRo","Danh sách các nước có rủi ro cao"),
            new Menu(0,2,"SHanhKhachDiLaiNhieu","Hành khách đi lại nhiều"),
            new Menu(0,2,"SHanhKhachVIP","Hành khách VIP"),
            new Menu(0,2,"SDoiTuongTrongDiem","Đối tượng trọng điểm"),
            new Menu(0,2,"STheoDoiDacBiet","Đối tượng theo dõi đặc biệt"),
            new Menu(0,2,"SDoiTuongDaKT","Đối tượng đã kiểm tra"),
            new Menu(3,null,"HK","Quản lý hành khách",0,"fa fa-users"),
            new Menu(0,3,"DHanhKhach","Danh sách hành khách"),
            new Menu(0,3,"DHKHoChieuNuocNgoai","Hành khách HC nước ngoài"),
            new Menu(0,3,"DDatCho","Thông tin đặt chỗ"),
            new Menu(0,3,"DNoiChuyen","Hành khách nối chuyến"),
            new Menu(4,null,"BC","Báo cáo thống kê",0,"fa fa-dashboard"),
        };

        public static List<Action> ListActionDefault = new List<Action>
        {
            new Action("view","Xem"),
            new Action("create","Thêm"),
            new Action("update","Sửa"),
            new Action("delete","Xóa"),
            new Action("import","Import"),
            new Action("export","Export"),
        };
    }

    public class Menu
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Controller { get; set; }
        public string ControllerName { get; set; }

        /// <summary>
        /// Loại menu:0-Menu cha; 1-Menu
        /// </summary>
        public int Type {  get; set; }

        public string Icon {  get; set; }

        public Menu(int id, int? parentId, string controller, string name, int type = 1, string icon = "fas fa-tags")
        {
            Id = id;
            ParentId = parentId;
            Controller = controller;
            ControllerName = name;
            Type = type;
            Icon = icon;
        }
    }

    public class Action
    {
        public string TypeHandle { get; set; }
        public string Name { get; set; }
        public Action(string typeHandle, string name)
        {
            TypeHandle = typeHandle;
            Name = name;
        }
    }
}