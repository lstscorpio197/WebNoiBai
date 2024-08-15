using GemBox.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    public class DCanhBaoController : BaseController
    {
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

        [AuthorizeAccessRole(TypeHandle = "export")]
        public JsonResult ExportExcel(DHanhKhachSearchDto itemSearch)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            try
            {
                SpreadsheetInfo.SetLicense(AppConst.KeyGemBoxSpreadsheet);
                var workbook = ExcelFile.Load(Server.MapPath("/FileTemp/DanhSachCanhBao.xlsx"));
                var query = GetQuery(itemSearch);
                var result = query.ToList();

                List<DoiTuongType_Sheet> lstDoiTuong_Sheet = new List<DoiTuongType_Sheet>
                {
                    new DoiTuongType_Sheet(DoiTuongType.DiLaiNhieu, 0),
                    new DoiTuongType_Sheet(DoiTuongType.TrongDiem, 1),
                    new DoiTuongType_Sheet(DoiTuongType.HanhKhachVIP, 2),
                    new DoiTuongType_Sheet(DoiTuongType.TheoDoi, 3),
                    new DoiTuongType_Sheet(DoiTuongType.TheoDoiDacBiet, 4),
                    new DoiTuongType_Sheet(DoiTuongType.HuongDanVien, 5),
                    new DoiTuongType_Sheet(DoiTuongType.DaKiemTra, 6),
                };

                foreach (var sheet in lstDoiTuong_Sheet)
                {
                    var lstDoiTuong = result.Where(x => x.Type == sheet.Type).ToList();
                    if (lstDoiTuong.Any())
                    {
                        var workSheet = workbook.Worksheets[sheet.Sheet];
                        int row = 3;
                        int stt = 1;
                        foreach (var item in lstDoiTuong)
                        {
                            workSheet.Cells["A" + row].SetValue(stt);
                            workSheet.Cells["B" + row].SetValue(item.SoHieu);
                            workSheet.Cells["C" + row].SetValue(item.NgayBay.ToString("dd/MM/yyyy"));
                            workSheet.Cells["D" + row].SetValue(item.SoGiayTo);
                            workSheet.Cells["E" + row].SetValue(item.HoTen);
                            workSheet.Cells["F" + row].SetValue(item.GioiTinh);
                            workSheet.Cells["G" + row].SetValue(item.QuocTich);
                            workSheet.Cells["H" + row].SetValue(item.NgaySinh?.ToString("dd/MM/yyyy"));
                            workSheet.Cells["I" + row].SetValue(item.HanhLy);

                            stt++;
                            row++;
                        }
                        var range = workSheet.Cells.GetSubrange("A2", "I" + (row - 1));
                        range.Style.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromName(ColorName.Black), LineStyle.Thin);
                    }
                }
                // xuất tài liệu thành tệp tin
                string handle = Guid.NewGuid().ToString();
                var stream = new MemoryStream();
                workbook.Save(stream, SaveOptions.XlsxDefault);
                stream.Position = 0;
                System.Web.HttpContext.Current.Cache.Insert(handle, stream.ToArray());
                byte[] data = stream.ToArray() as byte[];

                httpMessage.Body.Data = new { FileGuid = handle, FileName = string.Format("DS_CanhBao_{0}_{1}.xlsx", itemSearch.StartDate?.ToString("yyyyMMdd"), itemSearch.EndDate?.ToString("yyyyMMdd")) };
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                httpMessage.IsOk = false;
                httpMessage.Body.MsgNoti = new HttpMessageNoti("500", null, ex.Message);
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
        }

        private IQueryable<DCanhBao> GetQuery(DHanhKhachSearchDto itemSearch)
        {
            DateTime startDate = itemSearch.StartDate?.AddDays(-1) ?? DateTime.Today.AddDays(-1);
            DateTime endDate = itemSearch.EndDate?.AddDays(1) ?? DateTime.Today.AddDays(1);
            var query = dbXNC.DCanhBaos.AsNoTracking().Where(x => x.NgayBay > startDate && x.NgayBay < endDate);
            if (itemSearch.LstSoGiayTo.Any())
            {
                query = query.Where(x => itemSearch.LstSoGiayTo.Contains(x.SoGiayTo));
            }
            if (itemSearch.LstSoHieu.Any())
            {
                query = query.Where(x => itemSearch.LstSoHieu.Contains(x.SoHieu));
            }
            if (!string.IsNullOrEmpty(itemSearch.HoTen))
            {
                query = query.Where(x => x.HoTen.Contains(itemSearch.HoTen));
            }
            if (!string.IsNullOrEmpty(itemSearch.NoiDi))
            {
                query = query.Where(x => x.NoiDi == itemSearch.NoiDi);
            }
            if (!string.IsNullOrEmpty(itemSearch.NoiDen))
            {
                query = query.Where(x => x.NoiDen == itemSearch.NoiDen);
            }
            if (!string.IsNullOrEmpty(itemSearch.MaNoiDi))
            {
                query = query.Where(x => x.MaNoiDi == itemSearch.MaNoiDi);
            }
            if (!string.IsNullOrEmpty(itemSearch.MaNoiDen))
            {
                query = query.Where(x => x.MaNoiDen == itemSearch.MaNoiDen);
            }
            if (!string.IsNullOrEmpty(itemSearch.QuocTich))
            {
                query = query.Where(x => x.QuocTich == itemSearch.QuocTich);
            }
            if (itemSearch.ObjectType.HasValue)
            {
                query = query.Where(x=>x.Type == itemSearch.ObjectType);
            }
            return query.OrderBy(x => x.NgayBay).ThenBy(x => x.SoHieu).ThenBy(x=>x.HoTen);
        }
    }
}