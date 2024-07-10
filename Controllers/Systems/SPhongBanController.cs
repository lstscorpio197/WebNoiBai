using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNoiBai.Common;
using WebNoiBai.Dto;
using WebNoiBai.Models;

namespace WebNoiBai.Controllers.Systems
{
    public class SPhongBanController : BaseController
    {
        // GET: SPhongBan
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetList(SearchDto itemSearch)
        {
            WResult msg = new WResult(true);
            try
            {
                var query = db.SPhongBans.AsNoTracking().Where(x => true);
                if (!string.IsNullOrEmpty(itemSearch.Ma))
                {
                    query = query.Where(x => x.Ma == itemSearch.Ma);
                }
                if (!string.IsNullOrEmpty(itemSearch.Ten))
                {
                    query = query.Where(x => x.Ten.Contains(itemSearch.Ten));
                }
                if (itemSearch.Enable != -1)
                {
                    query = query.Where(x => x.Enable == itemSearch.Enable);
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
                var item = db.SPhongBans.Find(id);
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
        public JsonResult AddNew(SPhongBan item)
        {
            WResult msg = new WResult(true);
            try
            {
                msg = CheckValid(item);
                if (!msg.IsOk) {
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
                db.SPhongBans.Add(item);
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
        public JsonResult Update(SPhongBan item)
        {
            WResult msg = new WResult(true);
            try
            {
                var exist = db.SPhongBans.Find(item.Id);
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
                var item = db.SPhongBans.Find(id);
                if (item == null)
                {
                    msg.IsOk = false;
                    msg.Message = "Không tìm thấy thông tin";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                db.SPhongBans.Remove(item);
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

        private WResult CheckValid(SPhongBan item)
        {
            WResult msg = new WResult(false);
            try
            {
                if (string.IsNullOrEmpty(item.Ma))
                {
                    msg.Message = "Vui lòng nhập thông tin mã phòng ban";
                    return msg;
                }
                if (string.IsNullOrEmpty(item.Ten))
                {
                    msg.Message = "Vui lòng nhập thông tin tên phòng ban";
                    return msg;
                }
                var exist = db.SPhongBans.AsNoTracking().FirstOrDefault(x => x.Id != item.Id && x.Ma == item.Ma);
                if (exist != null)
                {
                    msg.Message = "Mã phòng ban đã tồn tại";
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