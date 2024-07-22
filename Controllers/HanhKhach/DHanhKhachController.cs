using GemBox.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebNoiBai.Common;
using WebNoiBai.Dto.HanhKhach;
using WebNoiBai.Models;
using WebNoiBai.WHttpMessage;

namespace WebNoiBai.Controllers.HanhKhach
{
    public class DHanhKhachController : BaseController
    {
        // GET: DHanhKhach
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetTable(DHanhKhachSearchDto itemSearch)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            try
            {
                var query = GetQuery(itemSearch);
                var result = query.OrderBy(x => x.FLIGHTDATE).ThenBy(x=>x.IDCHUYENBAY).Skip(itemSearch.Skip).Take(itemSearch.PageSize).ToList();
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

        public async Task<JsonResult> ImportExcel()
        {
            HttpMessage httpMessage = new HttpMessage(true);
            using (var trans = dbXNC.Database.BeginTransaction())
            {
                try
                {
                    var files = Request.Files;
                    if(files.Count == 0)
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
                    List<chuyenbay_hanhkhach> lst_cb_hk = new List<chuyenbay_hanhkhach>();
                    httpMessage = excelFiles.ProcessFileExcel(workbook, GetListColumnImportExcel(), ref lst_cb_hk, false);
                    if (!httpMessage.IsOk)
                    {
                        return Json(httpMessage, JsonRequestBehavior.AllowGet);
                    }
                    lst_cb_hk.ForEach(x => { x.SYS_DATE = DateTime.Now; x.SOGIAYTO = x.SOGIAYTO.EndsWith(";") ? x.SOGIAYTO.Replace(";", "") : x.SOGIAYTO; x.LOAIGIAYTO = x.LOAIGIAYTO.EndsWith(";") ? x.LOAIGIAYTO.Replace(";", "") : x.LOAIGIAYTO; x.HANHLY = x.HANHLY.EndsWith(";") ? x.HANHLY.Substring(0, x.HANHLY.Length - 1) : x.HANHLY; });
                    lst_cb_hk = lst_cb_hk.GroupBy(x => new { x.SOGIAYTO, x.IDCHUYENBAY }).Select(y => y.OrderByDescending(z => z.HK_VERSION).First()).ToList();
                    lst_cb_hk = lst_cb_hk.Where(x => x.IDCHUYENBAY > 0 && !string.IsNullOrEmpty(x.SOGIAYTO)).ToList();

                    await dbXNC.chuyenbay_hanhkhach.BulkMergeAsync(lst_cb_hk);
                    trans.Commit();

                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    httpMessage.IsOk = false;
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("500", null, ex.Message);
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                } 
            }
        }

        public JsonResult ExportExcel(DHanhKhachSearchDto itemSearch)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            try
            {

                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                httpMessage.IsOk = false;
                httpMessage.Body.MsgNoti = new HttpMessageNoti("500", null, ex.Message);
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AddWarning()
        {

            HttpMessage httpMessage = new HttpMessage(true);
            try
            {

                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                httpMessage.IsOk = false;
                httpMessage.Body.MsgNoti = new HttpMessageNoti("500", null, ex.Message);
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
        }

        private IQueryable<chuyenbay_hanhkhach> GetQuery(DHanhKhachSearchDto itemSearch)
        {
            DateTime startDate = itemSearch.StartDate?.AddDays(-1) ?? DateTime.Today.AddDays(-1);
            DateTime endDate = itemSearch.EndDate?.AddDays(1) ?? DateTime.Today.AddDays(1);
            var query = dbXNC.chuyenbay_hanhkhach.AsNoTracking().Where(x => x.FLIGHTDATE > startDate && x.FLIGHTDATE < endDate);
            if (itemSearch.LstSoGiayTo.Any())
            {
                query = query.Where(x => itemSearch.LstSoGiayTo.Contains(x.SOGIAYTO));
            }
            if (itemSearch.LstSoHieu.Any())
            {
                query = query.Where(x => itemSearch.LstSoHieu.Contains(x.SOHIEU));
            }
            if (!string.IsNullOrEmpty(itemSearch.NoiDi))
            {
                query = query.Where(x => x.NOIDI == itemSearch.NoiDi);
            }
            if (!string.IsNullOrEmpty(itemSearch.NoiDen))
            {
                query = query.Where(x => x.NOIDEN == itemSearch.NoiDen);
            }
            return query;
        }

        private List<ExcelImport> GetListColumnImportExcel()
        {
            List<ExcelImport> lstExcelImport = new List<ExcelImport>();
            lstExcelImport.Add(new ExcelImport("ID_CHUYENBAY", "ID_CHUYENBAY", "A", true, "long"));
            lstExcelImport.Add(new ExcelImport("SOHIEU", "SOHIEU", "B", false, "string"));
            lstExcelImport.Add(new ExcelImport("FLIGHTDATE", "FLIGHTDATE", "C", false, "datetime"));
            lstExcelImport.Add(new ExcelImport("CARRIERCODE", "CARRIERCODE", "D", false, "string"));
            lstExcelImport.Add(new ExcelImport("CARRIERNAME", "CARRIERNAME", "E", false, "string"));
            lstExcelImport.Add(new ExcelImport("MANOIDI", "MANOIDI", "F", false, "string"));
            lstExcelImport.Add(new ExcelImport("TENNOIDI", "TENNOIDI", "G", false, "string"));
            lstExcelImport.Add(new ExcelImport("MANOIDEN", "MANOIDEN", "H", false, "string"));
            lstExcelImport.Add(new ExcelImport("TENNOIDEN", "TENNOIDEN", "I", false, "string"));
            lstExcelImport.Add(new ExcelImport("IDHANHKHACH", "IDHANHKHACH", "J", false, "long"));
            lstExcelImport.Add(new ExcelImport("IDCHUYENBAY", "IDCHUYENBAY", "K", false, "long"));
            lstExcelImport.Add(new ExcelImport("MADATCHO", "MADATCHO", "L", false, "string"));
            lstExcelImport.Add(new ExcelImport("GIOITINH", "GIOITINH", "M", false, "string"));
            lstExcelImport.Add(new ExcelImport("HO", "HO", "N", false, "string"));
            lstExcelImport.Add(new ExcelImport("TENDEM", "TENDEM", "O", false, "string"));
            lstExcelImport.Add(new ExcelImport("TEN", "TEN", "P", false, "string"));
            lstExcelImport.Add(new ExcelImport("QUOCTICH", "QUOCTICH", "Q", false, "string"));
            lstExcelImport.Add(new ExcelImport("SOGIAYTO", "SOGIAYTO", "R", false, "string"));
            lstExcelImport.Add(new ExcelImport("LOAIGIAYTO", "LOAIGIAYTO", "S", false, "string"));
            lstExcelImport.Add(new ExcelImport("NOICAP", "NOICAP", "T", false, "string"));
            lstExcelImport.Add(new ExcelImport("NGAYHETHAN", "NGAYHETHAN", "U", false, "datetime"));
            lstExcelImport.Add(new ExcelImport("NGAYSINH", "NGAYSINH", "V", false, "datetime"));
            lstExcelImport.Add(new ExcelImport("NOIDI", "NOIDI", "W", true, "string"));
            lstExcelImport.Add(new ExcelImport("NOIDEN", "NOIDEN", "X", true, "string"));
            lstExcelImport.Add(new ExcelImport("HANHLY", "HANHLY", "Y", false, "string"));
            lstExcelImport.Add(new ExcelImport("HK_VERSION", "HK_VERSION", "Z", false, "int"));
            return lstExcelImport;
        }
    }
}