using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNoiBai.Dto.DanhMuc;
using WebNoiBai.Dto;
using WebNoiBai.Models;
using WebNoiBai.WHttpMessage;
using WebNoiBai.Authorize;

namespace WebNoiBai.Controllers.DanhMucNghiepVu
{
    public class SCangHangKhongController : BaseController
    {
        // GET: SCangHangKhong
        [AuthorizeAccessRole(TypeHandle = "view")]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetTable(SearchDto itemSearch)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            try
            {
                var query = dbXNC.cang_hang_khong.AsNoTracking().Where(x => true);
                if (!string.IsNullOrEmpty(itemSearch.Ma))
                {
                    query = query.Where(x => x.FI_MAIATA == itemSearch.Ma);
                }
                if (!string.IsNullOrEmpty(itemSearch.Ten))
                {
                    query = query.Where(x => x.FI_TENCANG.Contains(itemSearch.Ten));
                }
                var result = query.OrderBy(x => x.FI_ID).Skip(itemSearch.Skip).Take(itemSearch.PageSize).ToList();
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
                var item = dbXNC.cang_hang_khong.Find(id);
                if (item == null)
                {
                    httpMessage.IsOk = false;
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Không tìm thấy thông tin");
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }
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
        [AuthorizeAccessRole(TypeHandle = "create")]
        public JsonResult Create(cang_hang_khong item)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            try
            {
                httpMessage = CheckValid(item);
                if (!httpMessage.IsOk)
                {
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }
                dbXNC.cang_hang_khong.Add(item);
                dbXNC.SaveChanges();
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
        [AuthorizeAccessRole(TypeHandle = "update")]
        public JsonResult Update(cang_hang_khong item)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            try
            {
                var exist = dbXNC.cang_hang_khong.Find(item.FI_ID);
                if (exist == null)
                {
                    httpMessage.IsOk = false;
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Không tìm thấy thông tin");
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }
                dbXNC.Entry(exist).State = EntityState.Detached;
                dbXNC.Entry(item).State = EntityState.Modified;
                dbXNC.SaveChanges();
                httpMessage.Body.MsgNoti = new HttpMessageNoti("200", null, "Cập nhật thông tin thành công");
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
        [AuthorizeAccessRole(TypeHandle = "delete")]
        public JsonResult Delete(int id)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            try
            {
                var item = dbXNC.cang_hang_khong.Find(id);
                if (item == null)
                {
                    httpMessage.IsOk = false;
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Không tìm thấy thông tin");
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }

                dbXNC.cang_hang_khong.Remove(item);
                dbXNC.SaveChanges();
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                httpMessage.IsOk = false;
                httpMessage.Body.MsgNoti = new HttpMessageNoti("500", null, ex.Message);
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
        }

        private HttpMessage CheckValid(cang_hang_khong item)
        {
            HttpMessage httpMessage = new HttpMessage(false);
            try
            {
                if (string.IsNullOrEmpty(item.FI_MAIATA))
                {
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Vui lòng nhập mã IATA");
                    return httpMessage;
                }
                var exist = dbXNC.cang_hang_khong.AsNoTracking().FirstOrDefault(x => x.FI_ID != item.FI_ID && x.FI_MAIATA == item.FI_MAIATA);
                if (exist != null)
                {
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Mã IATA này đã tồn tại");
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