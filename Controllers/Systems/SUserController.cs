using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNoiBai.Common;
using WebNoiBai.Dto;
using WebNoiBai.Models;

namespace WebNoiBai.Controllers.Systems
{
    public class SUserController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetList(SearchDto itemSearch)
        {
            WResult msg = new WResult(true);
            try
            {
                var query = db.SUsers.AsNoTracking().Where(x => true);
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

                msg.TotalRowData = query.Count();
                msg.Data = query.OrderBy(x => x.Id).Skip(itemSearch.Skip).Take(itemSearch.PageSize).ToList();
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                msg.IsOk = false;
                msg.Message = ex.Message;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetItem(int id)
        {
            WResult msg = new WResult(true);
            try
            {
                var item = db.SUsers.Find(id);
                if (item == null)
                {
                    msg.IsOk = false;
                    msg.Message = "Không tìm thấy thông tin";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                msg.Data = item;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                msg.IsOk = false;
                msg.Message = ex.Message;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AddNew(SUser item)
        {
            WResult msg = new WResult(true);
            try
            {
                msg = CheckValid(item);
                if (!msg.IsOk)
                {
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                db.SUsers.Add(item);
                db.SaveChanges();
                msg.Message = "Thêm mới thành công";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                msg.IsOk = false;
                msg.Message = ex.Message;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Update(SUser item)
        {
            WResult msg = new WResult(true);
            try
            {
                var exist = db.SUsers.Find(item.Id);
                if (exist == null)
                {
                    msg.IsOk = false;
                    msg.Message = "Không tìm thấy thông tin";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                db.Entry(exist).State = EntityState.Detached;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                msg.Message = "Cập nhập thông tin thành công";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                msg.IsOk = false;
                msg.Message = ex.Message;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            WResult msg = new WResult(true);
            try
            {
                var item = db.SUsers.Find(id);
                if (item == null)
                {
                    msg.IsOk = false;
                    msg.Message = "Không tìm thấy thông tin";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                db.SUsers.Remove(item);
                db.SaveChanges();
                msg.Message = "Xóa thành công";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                msg.IsOk = false;
                msg.Message = ex.Message;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        private WResult CheckValid(SUser item)
        {
            WResult msg = new WResult(false);
            try
            {
                if (string.IsNullOrEmpty(item.Username))
                {
                    msg.Message = "Vui lòng nhập thông tin tên đăng nhập";
                    return msg;
                }
                if (string.IsNullOrEmpty(item.Password))
                {
                    msg.Message = "Vui lòng nhập thông tin mật khẩu";
                    return msg;
                }
                if (string.IsNullOrEmpty(item.Email))
                {
                    msg.Message = "Vui lòng nhập thông tin email";
                    return msg;
                }
                if (string.IsNullOrEmpty(item.HoTen))
                {
                    msg.Message = "Vui lòng nhập thông tin họ và tên";
                    return msg;
                }
                if (item.PhongBan == null || item.PhongBan == 0)
                {
                    msg.Message = "Vui lòng chọn phòng ban";
                    return msg;
                }
                var exist = db.SUsers.AsNoTracking().FirstOrDefault(x => x.Id != item.Id && (x.Username == item.Username || x.Email == item.Email));
                if (exist != null)
                {
                    msg.Message = "Tên đăng nhập hoặc email này đã tồn tại";
                    return msg;
                }
                msg.IsOk = true;
                return msg;
            }
            catch (Exception ex)
            {
                msg.IsOk = false;
                msg.Message = ex.Message;
                return msg;
            }
        }
    }
}