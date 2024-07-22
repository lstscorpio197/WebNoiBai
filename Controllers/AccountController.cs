using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNoiBai.Common;
using WebNoiBai.Dto;
using WebNoiBai.Dto.User;
using WebNoiBai.Models;
using WebNoiBai.WHttpMessage;

namespace WebNoiBai.Controllers
{
    public class AccountController : BaseController
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult ChangePassword(ChangePasswordDto itemPass)
        {
            HttpMessage httpMessage = new HttpMessage(false);
            try
            {
                var item = db.SUsers.Find(us.Id);
                if (item == null)
                {
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Không tìm thấy thông tin user");
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }
                bool validPass = BCrypt.Net.BCrypt.Verify(itemPass.OldPassword, item.Password);
                if (!validPass)
                {
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Mật khẩu cũ không đúng");
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }
                if (itemPass.NewPassword != itemPass.ConfirmPassword)
                {
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Mật khẩu cũ không đúng");
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }
                item.Password = BCrypt.Net.BCrypt.HashPassword(itemPass.NewPassword);
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                httpMessage.IsOk = true;
                httpMessage.Body.MsgNoti = new HttpMessageNoti("200", null, "Thay đổi mật khẩu thành công");
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                httpMessage.IsOk = false;
                httpMessage.Body.MsgNoti = new HttpMessageNoti("500", null, ex.Message);
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateUserInfo(CreateOrUpdateUserDto itemUpdate)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            try
            {
                var item = db.SUsers.Find(us.Id);
                if (item == null)
                {
                    httpMessage.IsOk = false;
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Không tìm thấy thông tin user");
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }
                item.HoTen = itemUpdate.HoTen;
                item.NgaySinh = itemUpdate.NgaySinh;
                item.Email = itemUpdate.Email;
                item.SDT = itemUpdate.SDT;

                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                USER user = new USER(item);
                Session[AppConst.UserSession] = user;
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                httpMessage.IsOk = false;
                httpMessage.Body.MsgNoti = new HttpMessageNoti("500", null, ex.Message);
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUserCurrent()
        {
            HttpMessage httpMessage = new HttpMessage(true);
            try
            {
                var item = db.SUsers.Find(us.Id);
                if (item == null)
                {
                    httpMessage.IsOk = false;
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Không tìm thấy thông tin user");
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }
                httpMessage.Body.Data = new { item.Username, item.HoTen, item.NgaySinh, item.Email, item.SDT };
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                httpMessage.IsOk = false;
                httpMessage.Body.MsgNoti = new HttpMessageNoti("500", null, ex.Message);
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
        }
    }
}