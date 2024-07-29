using GemBox.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNoiBai.Authorize;
using WebNoiBai.Common;
using WebNoiBai.Dto.HanhKhach;
using WebNoiBai.Models;
using WebNoiBai.WHttpMessage;

namespace WebNoiBai.Controllers.HanhKhach
{
    public class DNoiChuyenController : BaseController
    {
        // GET: DNoiChuyen
        [AuthorizeAccessRole(TypeHandle = "view")]
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
                var result = query.OrderBy(x => x.FLIGHTDATE).ThenBy(x => x.IDCHUYENBAY).Skip(itemSearch.Skip).Take(itemSearch.PageSize).ToList();
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

        [AuthorizeAccessRole(TypeHandle = "export")]
        public JsonResult ExportExcel(DHanhKhachSearchDto itemSearch)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            try
            {
                SpreadsheetInfo.SetLicense(AppConst.KeyGemBoxSpreadsheet);
                var workbook = ExcelFile.Load(Server.MapPath("/FileTemp/DS_HanhKhach.xlsx"));
                var workSheet = workbook.Worksheets[0];

                var query = GetQuery(itemSearch);
                var result = query.ToList();
                int row = 5;
                int stt = 1;
                if (result.Any())
                {
                    foreach (var item in result)
                    {
                        workSheet.Cells["A" + row].SetValue(stt);
                        workSheet.Cells["B" + row].SetValue(item.FLIGHTDATE_TXT);
                        workSheet.Cells["C" + row].SetValue(item.SOHIEU);
                        workSheet.Cells["D" + row].SetValue(item.MADATCHO);
                        workSheet.Cells["E" + row].SetValue(item.HOTEN);
                        workSheet.Cells["F" + row].SetValue(item.GIOITINH_TXT);
                        workSheet.Cells["G" + row].SetValue(item.QUOCTICH);
                        workSheet.Cells["H" + row].SetValue(item.NGAYSINH_TXT);
                        workSheet.Cells["I" + row].SetValue(item.LOAIGIAYTO);
                        workSheet.Cells["J" + row].SetValue(item.SOGIAYTO);
                        workSheet.Cells["K" + row].SetValue(item.NOIDI);
                        workSheet.Cells["L" + row].SetValue(item.MANOIDI);
                        workSheet.Cells["M" + row].SetValue(item.MANOIDEN);
                        workSheet.Cells["N" + row].SetValue(item.NOIDEN);
                        workSheet.Cells["O" + row].SetValue(item.HANHLY);

                        stt++;
                        row++;
                    }
                }

                var range = workSheet.Cells.GetSubrange("A4", "O" + (row - 1));
                range.Style.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromName(ColorName.Black), LineStyle.Thin);
                // xuất tài liệu thành tệp tin
                string handle = Guid.NewGuid().ToString();
                var stream = new MemoryStream();
                workbook.Save(stream, SaveOptions.XlsxDefault);
                stream.Position = 0;
                System.Web.HttpContext.Current.Cache.Insert(handle, stream.ToArray());
                byte[] data = stream.ToArray() as byte[];

                httpMessage.Body.Data = new { FileGuid = handle, FileName = string.Format("DS_HanhKhach_{0}_{1}.xlsx", itemSearch.StartDate?.ToString("yyyyMMdd"), itemSearch.EndDate?.ToString("yyyyMMdd")) };
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                httpMessage.IsOk = false;
                httpMessage.Body.MsgNoti = new HttpMessageNoti("500", null, ex.Message);
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
        }

        private IQueryable<DHanhKhachViewDto> GetQuery(DHanhKhachSearchDto itemSearch)
        {
            DateTime startDate = itemSearch.StartDate?.AddDays(-1) ?? DateTime.Today.AddDays(-1);
            DateTime endDate = itemSearch.EndDate?.AddDays(1) ?? DateTime.Today.AddDays(1);
            var query = dbXNC.chuyenbay_hanhkhach.AsNoTracking().Where(x => x.FLIGHTDATE > startDate && x.FLIGHTDATE < endDate);
            if (!string.IsNullOrEmpty(itemSearch.NoiDi))
            {
                query = query.Where(x => x.NOIDI == itemSearch.NoiDi);
            }
            if (!string.IsNullOrEmpty(itemSearch.NoiDen))
            {
                query = query.Where(x => x.NOIDEN == itemSearch.NoiDen);
            }
            if (itemSearch.ObjectType.HasValue)
            {
                switch (itemSearch.ObjectType.Value) {
                    case 0: //Nối chuyến đến HAN
                        query = query.Where(x => x.MANOIDEN == "HAN" && x.NOIDI != x.MANOIDI);
                        break;
                    case 1://Nối chuyến qua HAN
                        query = query.Where(x => x.NOIDEN != "HAN" && x.NOIDI != "HAN");
                        break;
                    case 2://Nối chuyến từ HAN
                        query = query.Where(x => x.NOIDEN != x.MANOIDEN && x.MANOIDI == "HAN");
                        break;
                    default:
                        break;
                }
            }
            return query.GroupBy(x => new { x.FLIGHTDATE, x.SOGIAYTO }).Select(x => x.FirstOrDefault()).Select(x => new DHanhKhachViewDto
            {
                FLIGHTDATE = x.FLIGHTDATE,
                GIOITINH = x.GIOITINH,
                HANHLY = x.HANHLY,
                HO = x.HO,
                IDCHUYENBAY = x.IDCHUYENBAY,
                MADATCHO = x.MADATCHO,
                MANOIDEN = x.MANOIDEN,
                MANOIDI = x.MANOIDI,
                NGAYSINH = x.NGAYSINH,
                NOIDEN = x.NOIDEN,
                NOIDI = x.NOIDI,
                QUOCTICH = x.QUOCTICH,
                SOGIAYTO = x.SOGIAYTO,
                LOAIGIAYTO = x.LOAIGIAYTO,
                SOHIEU = x.SOHIEU,
                TEN = x.TEN,
                TENDEM = x.TENDEM
            }).OrderBy(x => x.FLIGHTDATE).ThenBy(x => x.IDCHUYENBAY);
        }
    }
}