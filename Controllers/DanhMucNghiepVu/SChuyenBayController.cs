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
using WebNoiBai.Dto.DanhMuc;
using WebNoiBai.Dto.ImportDto;
using WebNoiBai.Models;
using WebNoiBai.WHttpMessage;

namespace WebNoiBai.Controllers.DanhMucNghiepVu
{
    public class SChuyenBayController : BaseController
    {
        // GET: SChuyenBay
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
                var query = dbXNC.SChuyenBays.AsNoTracking().Where(x => true);
                if (!string.IsNullOrEmpty(itemSearch.Ma))
                {
                    query = query.Where(x => x.ChuyenBay.Contains(itemSearch.Ma));
                }
                var result = query.Select(x => new ChuyenBayDto
                {
                    Id = x.Id,
                    Ngay = x.Ngay,
                    ChangBay = x.ChangBay,
                    ChuyenBay = x.ChuyenBay,
                    SOBT = x.SOBT,
                    EOBT = x.EOBT,
                    DaoHanhLy = x.DaoHanhLy,
                    CuaSo = x.CuaSo
                }).OrderByDescending(x => x.Id).Skip(itemSearch.Skip).Take(itemSearch.PageSize).ToList();
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
                var item = dbXNC.SChuyenBays.Find(id);
                if (item == null)
                {
                    httpMessage.IsOk = false;
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Không tìm thấy thông tin");
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }
                httpMessage.Body.Data = new ChuyenBayDto
                {
                    Id = item.Id,
                    Ngay = item.Ngay,
                    ChuyenBay = item.ChuyenBay,
                    ChangBay = item.ChangBay,
                    SOBT = item.SOBT,
                    EOBT = item.EOBT,
                    DaoHanhLy = item.DaoHanhLy,
                    CuaSo = item.CuaSo
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

        [HttpPost]
        [AuthorizeAccessRole(TypeHandle = "create")]
        public JsonResult Create(SChuyenBay item)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            try
            {
                httpMessage = CheckValid(item);
                if (!httpMessage.IsOk)
                {
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }
                dbXNC.SChuyenBays.Add(item);
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
        public JsonResult Update(SChuyenBay item)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            try
            {
                var exist = dbXNC.SChuyenBays.Find(item.Id);
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
                var item = dbXNC.SChuyenBays.Find(id);
                if (item == null)
                {
                    httpMessage.IsOk = false;
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Không tìm thấy thông tin");
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }

                dbXNC.SChuyenBays.Remove(item);
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
                ExcelFiles excelFiles = new ExcelFiles();
                List<SChuyenBayImportDto> lst_cb_hk = new List<SChuyenBayImportDto>();
                httpMessage = excelFiles.ProcessFileExcel(workbook, GetListColumnImportExcel(), ref lst_cb_hk);
                if (!httpMessage.IsOk)
                {
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }
                var lst_cb = lst_cb_hk.Select(x => new SChuyenBay
                {
                    ChuyenBay = x.ChuyenBay,
                    ChangBay = x.ChangBay,
                    SOBT = x.SOBT,
                    Ngay = x.Ngay,
                    DaoHanhLy = x.DaoHanhLy,
                    CuaSo = x.CuaSo
                });
                dbXNC.SChuyenBays.AddRange(lst_cb);
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

        private List<ExcelImport> GetListColumnImportExcel()
        {
            List<ExcelImport> lstExcelImport = new List<ExcelImport>();
            lstExcelImport.Add(new ExcelImport("Ngay", "Ngày bay", "A", true, "datetime"));
            lstExcelImport.Add(new ExcelImport("SoHieu", "Chuyến bay", "B", true, "string"));
            lstExcelImport.Add(new ExcelImport("ChangBay", "Chặng bay", "D", true, "string"));
            lstExcelImport.Add(new ExcelImport("Gio", "SOBT/EOBT", "C", false, "string"));
            lstExcelImport.Add(new ExcelImport("DaoHanhLy", "Đảo hành lý", "H", false, "string"));
            lstExcelImport.Add(new ExcelImport("CuaSo", "Cửa số", "I", false, "string"));
            lstExcelImport.Add(new ExcelImport("AmPm", "AM/PM", "J", true, "string"));
            return lstExcelImport;
        }

        private HttpMessage CheckValid(SChuyenBay item)
        {
            HttpMessage httpMessage = new HttpMessage(false);
            try
            {
                if (string.IsNullOrEmpty(item.ChuyenBay))
                {
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Vui lòng nhập họ tên");
                    return httpMessage;
                }
                var exist = dbXNC.SChuyenBays.AsNoTracking().FirstOrDefault(x => x.Id != item.Id && x.ChuyenBay == item.ChuyenBay && x.Ngay == item.Ngay);
                if (exist != null)
                {
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Chuyến bay này đã tồn tại");
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