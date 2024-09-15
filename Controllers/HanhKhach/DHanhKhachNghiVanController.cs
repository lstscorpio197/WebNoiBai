using GemBox.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebNoiBai.Authorize;
using WebNoiBai.Common;
using WebNoiBai.Dto.HanhKhach;
using WebNoiBai.Models;
using WebNoiBai.WHttpMessage;

namespace WebNoiBai.Controllers.HanhKhach
{
    public class DHanhKhachNghiVanController : BaseController
    {
        // GET: DHanhKhachNghiVan
        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetTable(DHanhKhachSearchDto itemSearch)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            try
            {
                var query = await GetQuery(itemSearch);
                var result = query.Skip(itemSearch.Skip).Take(itemSearch.PageSize).ToList();
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

        public JsonResult GetItem(string sogiayto, long idchuyenbay)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            try
            {
                var item = dbXNC.chuyenbay_hanhkhach.FirstOrDefault(x => x.SOGIAYTO == sogiayto && x.IDCHUYENBAY == idchuyenbay);
                if (item == null)
                {
                    httpMessage.IsOk = false;
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Không tìm thấy thông tin");
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }

                httpMessage.Body.Data = new { HoTen = (item.HO + " " + item.TENDEM + " " + item.TEN).Trim(), SoGiayTo = item.SOGIAYTO, LoaiGiayTo = item.LOAIGIAYTO, NgaySinh = item.NGAYSINH, GioiTinh = item.GIOITINH, QuocTich = item.QUOCTICH };
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                httpMessage.IsOk = false;
                httpMessage.Body.MsgNoti = new HttpMessageNoti("500", null, ex.Message);
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetItemBySoGiayTo(string sogiayto)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            try
            {
                var item = dbXNC.chuyenbay_hanhkhach.FirstOrDefault(x => x.SOGIAYTO == sogiayto);
                if (item == null)
                {
                    httpMessage.IsOk = false;
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Không tìm thấy thông tin");
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }

                httpMessage.Body.Data = new { HoTen = (item.HO + " " + item.TENDEM + " " + item.TEN).Trim(), SoGiayTo = item.SOGIAYTO, LoaiGiayTo = item.LOAIGIAYTO, NgaySinh = item.NGAYSINH, GioiTinh = item.GIOITINH, QuocTich = item.QUOCTICH };
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
        public async Task<JsonResult> ExportExcel(DHanhKhachSearchDto itemSearch)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            try
            {
                SpreadsheetInfo.SetLicense(AppConst.KeyGemBoxSpreadsheet);
                var workbook = ExcelFile.Load(Server.MapPath("/FileTemp/DS_HanhKhach.xlsx"));
                var workSheet = workbook.Worksheets[0];

                var query = await GetQuery(itemSearch);
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
                        workSheet.Cells["O" + row].SetValue(item.SoKien);
                        workSheet.Cells["P" + row].SetValue(item.HANHLY);
                        workSheet.Cells["Q" + row].SetValue(item.NgayDiGanNhat_TXT);
                        workSheet.Cells["R" + row].SetValue(item.SoNguoiDiCung?.ToString());

                        stt++;
                        row++;
                    }
                }

                var range = workSheet.Cells.GetSubrange("A4", "R" + (row - 1));
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


        private async Task<IEnumerable<DHanhKhachViewDto>> GetQuery(DHanhKhachSearchDto itemSearch)
        {
            DateTime startDate = itemSearch.StartDate.HasValue ? itemSearch.StartDate.Value.AddDays(-1) : DateTime.Today.AddDays(-1);
            DateTime endDate = itemSearch.StartDate.HasValue ? itemSearch.StartDate.Value.AddDays(1) : DateTime.Today.AddDays(1);
            DateTime flightDate = itemSearch.StartDate.HasValue ? itemSearch.StartDate.Value : DateTime.Today;
            var query = dbXNC.chuyenbay_hanhkhach.AsNoTracking().Where(x => x.FLIGHTDATE == flightDate);
            if (itemSearch.LstSoGiayTo.Any())
            {
                query = query.Where(x => itemSearch.LstSoGiayTo.Contains(x.SOGIAYTO));
            }
            if (itemSearch.LstSoHieu.Any())
            {
                query = query.Where(x => itemSearch.LstSoHieu.Contains(x.SOHIEU));
            }
            if (!string.IsNullOrEmpty(itemSearch.HoTen))
            {
                var arrHoTen = itemSearch.HoTen.Split(' ');
                foreach (var item in arrHoTen)
                {
                    query = query.Where(x => x.HO.Contains(item) || x.TENDEM.Contains(item) || x.TEN.Contains(item));
                }
            }
            if (itemSearch.IsDiTuNuocRuiRo)
            {
                var lstRuiRo = dbXNC.SNuocRuiRoes.Select(x => x.MaSanBay);
                query = query.Where(x => lstRuiRo.Contains(x.NOIDI) || lstRuiRo.Contains(x.MANOIDI));
            }
            if (!string.IsNullOrEmpty(itemSearch.NoiDi))
            {
                query = query.Where(x => x.NOIDI == itemSearch.NoiDi);
            }
            if (!string.IsNullOrEmpty(itemSearch.NoiDen))
            {
                query = query.Where(x => x.NOIDEN == itemSearch.NoiDen);
            }
            if (!string.IsNullOrEmpty(itemSearch.MaNoiDi))
            {
                query = query.Where(x => x.MANOIDI == itemSearch.MaNoiDi);
            }
            if (!string.IsNullOrEmpty(itemSearch.MaNoiDen))
            {
                query = query.Where(x => x.MANOIDEN == itemSearch.MaNoiDen);
            }
            if (!string.IsNullOrEmpty(itemSearch.QuocTich))
            {
                query = query.Where(x => x.QUOCTICH == itemSearch.QuocTich);
            }

            var lstHK = query.Select(x => new DHanhKhachViewDto
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
            }).ToList();

            var lstHKTD = await dbXNC.SHanhKhachDiLaiNhieux.AsNoTracking().Select(x => new DoiTuongCompareDto
            {
                HOTEN = x.HoTen,
                GIOITINH = x.GioiTinh,
                NGAYSINH = x.NgaySinh,
                SOGIAYTO = x.SoGiayTo
            }).ToListAsync();

            var xxx = from x in lstHK
                      join y in lstHKTD on new {x.NGAYSINH, x.GIOITINH, x.NameArray } equals new { y.NGAYSINH, y.GIOITINH, y.NameArray }
                      where x.SOGIAYTO != y.SOGIAYTO
                      select x;
            return xxx;
        }
    }
}