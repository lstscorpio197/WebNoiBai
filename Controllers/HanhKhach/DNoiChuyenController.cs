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
    public class DNoiChuyenController : BaseController
    {
        // GET: DNoiChuyen
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
            query = query.GroupBy(x => new { x.FLIGHTDATE, x.SOGIAYTO }).Select(x => x.FirstOrDefault());
            return query;
        }
    }
}