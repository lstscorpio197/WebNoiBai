using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNoiBai.Common;
using WebNoiBai.Dto;
using WebNoiBai.Dto.User;
using WebNoiBai.Models;
using WebNoiBai.WHttpMessage;

namespace WebNoiBai.Controllers.Systems
{
    public class SUserController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetTable(SearchDto itemSearch)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            try
            {
                var query = db.SUsers.AsNoTracking().Include(x => x.SPhongBan).Where(x => true);
                if (!string.IsNullOrEmpty(itemSearch.Ma))
                {
                    query = query.Where(x => x.Username.Contains(itemSearch.Ma) || x.Email.Contains(itemSearch.Ma));
                }
                if (!string.IsNullOrEmpty(itemSearch.Ten))
                {
                    query = query.Where(x => x.HoTen.Contains(itemSearch.Ten));
                }
                if (itemSearch.Enable != -1)
                {
                    query = query.Where(x => x.IsActived == itemSearch.Enable);
                }

                var result = query.Select(x => new SUserViewDto { Id = x.Id, Username = x.Username, HoTen = x.HoTen, Email = x.Email, PhongBan = x.SPhongBan.Ten, ChucVu = x.ChucVu.Value, IsActived = x.IsActived }).OrderBy(x => x.Id).Skip(itemSearch.Skip).Take(itemSearch.PageSize).ToList();

                httpMessage.Body.Data = result;
                httpMessage.Body.Pagination = new HttpMessagePagination
                {
                    NumberRowsOnPage = itemSearch.PageSize,
                    PageNumber = itemSearch.PageNum,
                    TotalRowsOnPage = result.Count(),
                    TotalRows = query.Count()
                };
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                httpMessage.IsOk = false;
                httpMessage.Body.MsgNoti = new HttpMessageNoti("500", null, ex.Message);
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetItem(int id)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            try
            {
                var item = db.SUsers.Find(id);
                if (item == null)
                {
                    httpMessage.IsOk = false;
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Không tìm thấy thông tin");
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }
                item.Password = "";
                httpMessage.Body.Data = item;
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                httpMessage.IsOk = false;
                httpMessage.Body.MsgNoti = new HttpMessageNoti("500", null, ex.Message);
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Create(CreateOrUpdateUserDto item)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            try
            {
                httpMessage = CheckValid(item);
                if (!httpMessage.IsOk)
                {
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }
                SUser us = new SUser
                {
                    Password = BCrypt.Net.BCrypt.HashPassword(item.Password),
                    Username = item.Username,
                    HoTen = item.HoTen,
                    Id = item.Id,
                    ChucVu = item.ChucVu,
                    Email = item.Email,
                    GioiTinh = item.GioiTinh,
                    IsActived = item.IsActived,
                    NgaySinh = item.NgaySinh,
                    PhongBan = item.PhongBan,
                    SDT = item.SDT
                };
                db.SUsers.Add(us);
                db.SaveChanges();
                httpMessage.Body.MsgNoti = new HttpMessageNoti("200", null, "Thêm mới thành công");
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                httpMessage.IsOk = false;
                httpMessage.Body.MsgNoti = new HttpMessageNoti("500", null, ex.Message);
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Update(CreateOrUpdateUserDto item)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            try
            {
                var exist = db.SUsers.Find(item.Id);
                if (exist == null)
                {
                    httpMessage.IsOk = false;
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Không tìm thấy thông tin");
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }

                exist.HoTen = item.HoTen;
                exist.Id = item.Id;
                exist.ChucVu = item.ChucVu;
                exist.Email = item.Email;
                exist.GioiTinh = item.GioiTinh;
                exist.IsActived = item.IsActived;
                exist.NgaySinh = item.NgaySinh;
                exist.PhongBan = item.PhongBan;
                exist.SDT = item.SDT;
                db.Entry(exist).State = EntityState.Modified;
                db.SaveChanges();
                httpMessage.Body.MsgNoti = new HttpMessageNoti("200", null, "Cập nhập thông tin thành công");
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                httpMessage.IsOk = false;
                httpMessage.Body.MsgNoti = new HttpMessageNoti("500", null, ex.Message);
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            try
            {
                var item = db.SUsers.Find(id);
                if (item == null)
                {
                    httpMessage.IsOk = false;
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Không tìm thấy thông tin");
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }
                db.SUsers.Remove(item);
                db.SaveChanges();
                httpMessage.Body.MsgNoti = new HttpMessageNoti("200", null, "Xóa thành công");
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                httpMessage.IsOk = false;
                httpMessage.Body.MsgNoti = new HttpMessageNoti("500", null, ex.Message);
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetRole(int id)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            try
            {
                var lstRoleId = db.SUserRoles.AsNoTracking().Where(x => x.UserId == id).Select(x => x.RoleId).ToList();

                httpMessage.Body.Data = new {Id = id, LstRoleId = lstRoleId };
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                httpMessage.IsOk = false;
                httpMessage.Body.MsgNoti = new HttpMessageNoti("500", null, ex.Message);
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetPermission(string strRoleId, int id)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            try
            {
                List<int> lstRoleId = JsonConvert.DeserializeObject<List<int>>(strRoleId);
                var lstPermission = db.SRolePermissions.AsNoTracking().Include(x => x.SPermission)
                    .Where(x => (lstRoleId.Contains(x.RoleId.Value) && !x.UserId.HasValue) || x.UserId == id)
                    .Select(x => new UserPermissionDto
                    {
                        PermissionId = x.SPermission.Id,
                        Action = x.SPermission.Action,
                        ActionName = x.SPermission.ActionName,
                        Controller = x.SPermission.Controller,
                        ControllerName = x.SPermission.ControllerName,
                        IsGranted = x.IsGranted
                    }).GroupBy(x=> new {x.Controller, x.ControllerName})
                    .Select(x=> new UserPermissionViewDto
                    {
                        Controller = x.Key.Controller,
                        ControllerName= x.Key.ControllerName,
                        LstPermission = x.ToList()
                    }).ToList();

                httpMessage.Body.Data = lstPermission;
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                httpMessage.IsOk = false;
                httpMessage.Body.MsgNoti = new HttpMessageNoti("500", null, ex.Message);
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
        }

        private HttpMessage CheckValid(CreateOrUpdateUserDto item)
        {
            HttpMessage httpMessage = new HttpMessage(false);
            try
            {
                if (string.IsNullOrEmpty(item.Username))
                {
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Vui lòng nhập thông tin tên đăng nhập");
                    return httpMessage;
                }
                if (item.Id == 0)
                {
                    if (string.IsNullOrEmpty(item.Password))
                    {
                        httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Vui lòng nhập thông tin mật khẩu");
                        return httpMessage;
                    }
                    if (string.IsNullOrEmpty(item.ConfirmPassword))
                    {
                        httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Vui lòng nhập thông tin mật khẩu");
                        return httpMessage;
                    }
                    if (item.Password != item.ConfirmPassword)
                    {
                        httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Xác nhận mật khẩu không trùng khớp");
                        return httpMessage;
                    }
                }

                if (string.IsNullOrEmpty(item.Email))
                {
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Vui lòng nhập thông tin email");
                    return httpMessage;
                }
                if (string.IsNullOrEmpty(item.HoTen))
                {
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Vui lòng nhập thông tin họ và tên");
                    return httpMessage;
                }
                if (item.PhongBan == null || item.PhongBan == 0)
                {
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Vui lòng chọn phòng ban");
                    return httpMessage;
                }
                var exist = db.SUsers.AsNoTracking().FirstOrDefault(x => x.Id != item.Id && (x.Username == item.Username || x.Email == item.Email));
                if (exist != null)
                {
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Tên đăng nhập hoặc email này đã tồn tại");
                    return httpMessage;
                }
                httpMessage.IsOk = true;
                return httpMessage;
            }
            catch (Exception ex)
            {
                httpMessage.IsOk = false;
                httpMessage.Body.MsgNoti = new HttpMessageNoti("500", null, ex.Message);
                return httpMessage;
            }
        }
    }
}