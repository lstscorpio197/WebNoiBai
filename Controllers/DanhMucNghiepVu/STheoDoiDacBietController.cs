using GemBox.Spreadsheet;
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
    public class STheoDoiDacBietController : BaseController
    {
        // GET: STheoDoiDacBiet
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
                var query = dbXNC.STheoDoiDacBiets.AsNoTracking().Where(x => true);
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
                var item = dbXNC.STheoDoiDacBiets.Find(id);
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
        public JsonResult Create(STheoDoiDacBiet item)
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
                dbXNC.STheoDoiDacBiets.Add(item);
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
        public JsonResult Update(STheoDoiDacBiet item)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            try
            {
                var exist = dbXNC.STheoDoiDacBiets.Find(item.Id);
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
                var item = dbXNC.STheoDoiDacBiets.Find(id);
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
                dbXNC.STheoDoiDacBiets.Remove(item);
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

        [AuthorizeAccessRole(TypeHandle = "import")]
        public JsonResult ImportExcel()
        {
            HttpMessage httpMessage = new HttpMessage(true);
            try
            {
                var files = Request.Files;
                if (files.Count == 0)
                {
                    httpMessage.IsOk = false;
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("500", null, "Vui lòng chọn file");
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }
                var file = files[0];
                SpreadsheetInfo.SetLicense(AppConst.KeyGemBoxSpreadsheet);

                var workbook = new ExcelFile();
                string extFile = System.IO.Path.GetExtension(file.FileName).Substring(1).ToLower();
                if (extFile == "csv")
                {
                    workbook = ExcelFile.Load(file.InputStream, LoadOptions.CsvDefault);
                }
                else if (extFile == "xlsx")
                {
                    workbook = ExcelFile.Load(file.InputStream, LoadOptions.XlsxDefault);
                }
                else
                {
                    workbook = ExcelFile.Load(file.InputStream, LoadOptions.XlsDefault);
                }

                var workSheet = workbook.Worksheets[0];
                int rows = workSheet.Rows.Count;
                if (rows < 2)
                {
                    httpMessage.IsOk = false;
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "File tải lên không có dữ liệu");
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }
                var lstSoGiayTo = new List<string>();
                for (int i = 2; i <= rows; i++)
                {
                    string value = workSheet.Cells["A" + i].Value?.ToString();
                    if (string.IsNullOrEmpty(value))
                    {
                        continue;
                    }
                    value = value.Replace(";", "");
                    lstSoGiayTo.Add(value);
                }
                lstSoGiayTo = lstSoGiayTo.GroupBy(x => x).Select(x => x.Key).ToList();
                var lstSoGiayToNew = from a in lstSoGiayTo
                                     join b in dbXNC.STheoDoiDacBiets.Select(x => x.SoGiayTo) on a equals b into c
                                     from d in c.DefaultIfEmpty()
                                     where d == null
                                     select a;
                var queryHK = dbXNC.chuyenbay_hanhkhach.Where(x => lstSoGiayToNew.Contains(x.SOGIAYTO));
                var lstItem = (from a in lstSoGiayToNew
                               join b in queryHK on a equals b.SOGIAYTO
                               select new STheoDoiDacBiet
                               {
                                   HoTen = string.Format("{0} {1} {2}", b.HO, b.TENDEM, b.TEN),
                                   GioiTinh = b.GIOITINH,
                                   LoaiGiayTo = b.LOAIGIAYTO,
                                   NgaySinh = b.NGAYSINH,
                                   SoGiayTo = b.SOGIAYTO,
                                   QuocTich = b.QUOCTICH,
                                   NgayTao = DateTime.Now,
                                   NguoiTao = us.Username
                               }).GroupBy(x => x.SoGiayTo)
              .Select(g => g.First())
              .ToList();

                dbXNC.STheoDoiDacBiets.AddRange(lstItem);
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

        private HttpMessage CheckValid(STheoDoiDacBiet item)
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
                var exist = dbXNC.STheoDoiDacBiets.AsNoTracking().FirstOrDefault(x => x.Id != item.Id && x.SoGiayTo == item.SoGiayTo);
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