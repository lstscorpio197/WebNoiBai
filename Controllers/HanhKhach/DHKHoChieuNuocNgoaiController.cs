using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNoiBai.Dto.HanhKhach;
using WebNoiBai.Models;
using WebNoiBai.WHttpMessage;

namespace WebNoiBai.Controllers.HanhKhach
{
    public class DHKHoChieuNuocNgoaiController : BaseController
    {
        // GET: DHKHoChieuNuocNgoai
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

        private IQueryable<chuyenbay_hanhkhach> GetQuery(DHanhKhachSearchDto itemSearch)
        {
            DateTime startDate = itemSearch.StartDate?.AddDays(-1) ?? DateTime.Today.AddDays(-1);
            DateTime endDate = itemSearch.EndDate?.AddDays(1) ?? DateTime.Today.AddDays(1);
            List<string> lstHo = new List<string> 
            { 
                "NGUYEN", "TRAN", "LE", "PHAM", "HOANG", "HUYNH", "PHAN", "VU", "VO", "DANG", "BUI", "DO", "HO", "NGO", "DUONG", "LY", "PHUNG","MAI","AU",
                "NONG","DINH","TAT","CHU","LUU","ONG","VUONG","TRIEU","LUONG","TRINH","VAN","TA","NHAM","QUACH","SAM","QUE","DOAN","DUONG","DANH","LAM",
                "TONG","CU","THI","TRUONG","DOAN","NGHIEM","DICH","LOI","HA","LUC","LOAN","CAO","PHO","GIAP","VI","KHUAT","TANG","CHAU","TO","DONG"};
            var query = dbXNC.chuyenbay_hanhkhach.AsNoTracking().Where(x => x.FLIGHTDATE > startDate && x.FLIGHTDATE < endDate && lstHo.Contains(x.HO.Contains(" ") ?
                        x.HO.Substring(0, x.HO.IndexOf(" ")) :
                        x.HO) && x.QUOCTICH != "VNM");
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

        private string GetNamePart(string ho)
        {
            if (string.IsNullOrWhiteSpace(ho))
                return string.Empty;

            int spaceIndex = ho.IndexOf(' ');
            return spaceIndex == -1 ? ho : ho.Substring(0, spaceIndex);
        }
    }
}