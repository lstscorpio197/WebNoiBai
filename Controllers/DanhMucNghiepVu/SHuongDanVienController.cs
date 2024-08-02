using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNoiBai.Authorize;
using WebNoiBai.Common;
using WebNoiBai.Dto;
using WebNoiBai.Models;
using WebNoiBai.WHttpMessage;

namespace WebNoiBai.Controllers.DanhMucNghiepVu
{
    public class SHuongDanVienController : BaseController
    {
        // GET: SHuongDanVien
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
                var query = dbXNC.SHuongDanViens.AsNoTracking().Where(x => true);
                if (!string.IsNullOrEmpty(itemSearch.Ma))
                {
                    query = query.Where(x => x.SoGiayTo == itemSearch.Ma);
                }
                if (!string.IsNullOrEmpty(itemSearch.Ten))
                {
                    query = query.Where(x => x.HoTen.Contains(itemSearch.Ten));
                }
                var result = query.OrderBy(x => x.Id).Skip(itemSearch.Skip).Take(itemSearch.PageSize).ToList();
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
                var item = dbXNC.SHuongDanViens.Find(id);
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
        public JsonResult Create(SHuongDanVien item)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            try
            {
                httpMessage = CheckValid(item);
                if (!httpMessage.IsOk)
                {
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }
                item.NgayTao = DateTime.Now;
                item.NguoiTao = us.Username;
                dbXNC.SHuongDanViens.Add(item);
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
        public JsonResult Update(SHuongDanVien item)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            try
            {
                var exist = dbXNC.SHuongDanViens.Find(item.Id);
                if (exist == null)
                {
                    httpMessage.IsOk = false;
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Không tìm thấy thông tin");
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }
                if (exist.NguoiTao != us.Username && us.ChucVu != UserLevel.Admin)
                {
                    httpMessage.IsOk = false;
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Chỉ người tạo hoặc admin mới được sửa thông tin");
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }
                item.NguoiTao = exist.NguoiTao;
                item.NgayTao = exist.NgayTao;
                item.NgaySua = DateTime.Now;
                item.NguoiSua = us.Username;
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
                var item = dbXNC.SHuongDanViens.Find(id);
                if (item == null)
                {
                    httpMessage.IsOk = false;
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Không tìm thấy thông tin");
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }
                if (item.NguoiTao != us.Username && us.ChucVu != UserLevel.Admin)
                {
                    httpMessage.IsOk = false;
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Chỉ người tạo hoặc admin mới được xóa thông tin");
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }
                dbXNC.SHuongDanViens.Remove(item);
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

        private HttpMessage CheckValid(SHuongDanVien item)
        {
            HttpMessage httpMessage = new HttpMessage(false);
            try
            {
                if (string.IsNullOrEmpty(item.HoTen))
                {
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Vui lòng nhập họ tên");
                    return httpMessage;
                }
                if (string.IsNullOrEmpty(item.SoGiayTo))
                {
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Vui lòng nhập số giấy tờ");
                    return httpMessage;
                }
                if (string.IsNullOrEmpty(item.LoaiGiayTo))
                {
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Vui lòng nhập loại giấy tờ");
                    return httpMessage;
                }
                var exist = dbXNC.SHuongDanViens.AsNoTracking().FirstOrDefault(x => x.Id != item.Id && x.SoGiayTo == item.SoGiayTo);
                if (exist != null)
                {
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Số giấy tờ này đã tồn tại");
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