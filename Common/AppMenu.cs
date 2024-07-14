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
            new Menu(1,null,"QT","Quản trị",0),
            new Menu(2,1,"SPhongBan","Quản lý phòng ban"),
            new Menu(3,1,"SUser","Quản lý tài khoản"),
            new Menu(4,1,"SRole","Quản lý nhóm quyền"),
            new Menu(5,1,"SPermission","Quản lý quyền"),
            new Menu(6,null,"DM","Danh mục",0),
            new Menu(7,6,"SCangHangKhong","Danh sách cảng hàng không"),
            new Menu(8,6,"SHangHangKhong","Danh sách hãng hàng không"),
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