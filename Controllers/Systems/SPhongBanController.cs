using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNoiBai.Common;

namespace WebNoiBai.Controllers.Systems
{
    public class SPhongBanController : Controller
    {
        // GET: SPhongBan
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetList()
        {
            WResult msg = new WResult(true);
            try
            {
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                msg.IsOk = false;
                msg.Message = ex.Message;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetItem(int id)
        {
            WResult msg = new WResult(true);
            try
            {
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                msg.IsOk = false;
                msg.Message = ex.Message;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Add()
        {
            WResult msg = new WResult(true);
            try
            {
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                msg.IsOk = false;
                msg.Message = ex.Message;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

    }
}