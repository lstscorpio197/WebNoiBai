﻿using GemBox.Spreadsheet;
using Newtonsoft.Json;
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
    public class DLocTheoTanSuatController : BaseController
    {
        // GET: DLocTheoTanSuat
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
                var workbook = ExcelFile.Load(Server.MapPath("/FileTemp/ExportAdvancedSearch.xlsx"));
                var workSheet = workbook.Worksheets[0];

                var query = GetQuery(itemSearch);
                var result = query.ToList();
                int row = 4;
                int stt = 1;
                if (result.Any())
                {
                    foreach (var item in result)
                    {
                        workSheet.Cells["A" + row].SetValue(stt);
                        workSheet.Cells["B" + row].SetValue(item.HOTEN);
                        workSheet.Cells["C" + row].SetValue(item.GIOITINH_TXT);
                        workSheet.Cells["D" + row].SetValue(item.QUOCTICH);
                        workSheet.Cells["E" + row].SetValue(item.SOGIAYTO);
                        workSheet.Cells["F" + row].SetValue(item.LOAIGIAYTO);
                        workSheet.Cells["G" + row].SetValue(item.NGAYSINH_TXT);
                        workSheet.Cells["H" + row].SetValue(item.SOLAN);
                        workSheet.Cells["I" + row].SetValue(item.SOHIEU);
                        workSheet.Cells["J" + row].SetValue(item.HANHLY);
                        stt++;
                        row++;
                    }
                }

                var range = workSheet.Cells.GetSubrange("A4", "J" + (row - 1));
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
            itemSearch.SoLan = itemSearch.SoLan ?? 1;
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

            var queryRes = query.GroupBy(x => x.SOGIAYTO).Where(x => x.Count() > itemSearch.SoLan).Select(x => new DHanhKhachViewDto
            {
                SOHIEU = x.OrderByDescending(y => y.FLIGHTDATE).FirstOrDefault().SOHIEU,
                FLIGHTDATE = x.OrderByDescending(y => y.FLIGHTDATE).FirstOrDefault().FLIGHTDATE,
                GIOITINH = x.OrderByDescending(y => y.FLIGHTDATE).FirstOrDefault().GIOITINH,
                HANHLY = x.OrderByDescending(y => y.FLIGHTDATE).FirstOrDefault().HANHLY,
                HO = x.OrderByDescending(y => y.FLIGHTDATE).FirstOrDefault().HO,
                TENDEM = x.OrderByDescending(y => y.FLIGHTDATE).FirstOrDefault().TENDEM,
                TEN = x.OrderByDescending(y => y.FLIGHTDATE).FirstOrDefault().TEN,
                IDCHUYENBAY = x.OrderByDescending(y => y.FLIGHTDATE).FirstOrDefault().IDCHUYENBAY,
                MANOIDEN = x.OrderByDescending(y => y.FLIGHTDATE).FirstOrDefault().MANOIDEN,
                MANOIDI = x.OrderByDescending(y => y.FLIGHTDATE).FirstOrDefault().MANOIDI,
                NOIDEN = x.OrderByDescending(y => y.FLIGHTDATE).FirstOrDefault().NOIDEN,
                NOIDI = x.OrderByDescending(y => y.FLIGHTDATE).FirstOrDefault().NOIDI,
                NGAYSINH = x.OrderByDescending(y => y.FLIGHTDATE).FirstOrDefault().NGAYSINH,
                QUOCTICH = x.OrderByDescending(y => y.FLIGHTDATE).FirstOrDefault().QUOCTICH,
                SOGIAYTO = x.OrderByDescending(y => y.FLIGHTDATE).FirstOrDefault().SOGIAYTO,
                LOAIGIAYTO = x.OrderByDescending(y => y.FLIGHTDATE).FirstOrDefault().LOAIGIAYTO,
                SOLAN = x.Count(),
            });
            return queryRes.OrderByDescending(x => x.SOLAN).ThenBy(x => x.IDCHUYENBAY);
        }

        public JsonResult InsertToHKDiLaiNhieu(DHanhKhachSearchDto itemSearch, string strSoGiayTo, bool isAll = false)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            try
            {
                if (isAll)
                {
                    var lstItem = GetListHKDiLaiNhieu(itemSearch);
                    lstItem = (from a in lstItem
                               join b in dbXNC.SHanhKhachDiLaiNhieux.Select(x => x.SoGiayTo) on a.SoGiayTo equals b into c
                               from d in c.DefaultIfEmpty()
                               where d == null
                               select a).ToList();
                    dbXNC.SHanhKhachDiLaiNhieux.AddRange(lstItem);
                    dbXNC.SaveChanges();
                }
                else
                {
                    var lstSoGiayTo = JsonConvert.DeserializeObject<List<string>>(strSoGiayTo);
                    lstSoGiayTo = lstSoGiayTo.GroupBy(x => x).Select(x => x.Key).ToList();
                    var lstSoGiayToNew = from a in lstSoGiayTo
                                         join b in dbXNC.SHanhKhachDiLaiNhieux.Select(x => x.SoGiayTo) on a equals b into c
                                         from d in c.DefaultIfEmpty()
                                         where d == null
                                         select a;

                    DateTime startDate = itemSearch.StartDate?.AddDays(-1) ?? DateTime.Today.AddDays(-1);
                    DateTime endDate = itemSearch.EndDate?.AddDays(1) ?? DateTime.Today.AddDays(1);
                    var queryHK = dbXNC.chuyenbay_hanhkhach.Where(x => x.FLIGHTDATE > startDate && x.FLIGHTDATE < endDate && lstSoGiayToNew.Contains(x.SOGIAYTO));
                    var lstItem = (from a in lstSoGiayToNew
                                   join b in queryHK on a equals b.SOGIAYTO
                                   select new SHanhKhachDiLaiNhieu
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

                    dbXNC.SHanhKhachDiLaiNhieux.AddRange(lstItem);
                    dbXNC.SaveChanges();
                }
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                httpMessage.IsOk = false;
                httpMessage.Body.MsgNoti = new HttpMessageNoti("500", null, ex.Message);
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
        }

        private List<SHanhKhachDiLaiNhieu> GetListHKDiLaiNhieu(DHanhKhachSearchDto itemSearch)
        {
            DateTime startDate = itemSearch.StartDate?.AddDays(-1) ?? DateTime.Today.AddDays(-1);
            DateTime endDate = itemSearch.EndDate?.AddDays(1) ?? DateTime.Today.AddDays(1);
            itemSearch.SoLan = itemSearch.SoLan ?? 1;
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

            var queryRes = query.GroupBy(x => x.SOGIAYTO).Where(x => x.Count() > itemSearch.SoLan).Select(x => x.FirstOrDefault()).ToList().Select(b => new SHanhKhachDiLaiNhieu
            {
                HoTen = string.Format("{0} {1} {2}", b.HO, b.TENDEM, b.TEN),
                GioiTinh = b.GIOITINH,
                LoaiGiayTo = b.LOAIGIAYTO,
                NgaySinh = b.NGAYSINH,
                SoGiayTo = b.SOGIAYTO,
                QuocTich = b.QUOCTICH,
                NgayTao = DateTime.Now,
                NguoiTao = us.Username
            });
            return queryRes.ToList();
        }
    }
}