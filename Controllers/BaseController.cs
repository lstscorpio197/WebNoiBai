using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNoiBai.Authorize;
using WebNoiBai.Common;
using WebNoiBai.Models;

namespace WebNoiBai.Controllers
{
    [AuthorizeAccessRole]
    public class BaseController : Controller
    {
        // GET: Base
        public SystemEntities db = new SystemEntities();
        public DataXNCEntities dbXNC = new DataXNCEntities();
        public USER us = (USER)System.Web.HttpContext.Current.Session[AppConst.UserSession];

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                dbXNC.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Download(string fileGuid, string fileName)
        {
            var dataCache = System.Web.HttpContext.Current.Cache.Get(fileGuid);
            if (dataCache != null)
            {
                byte[] data = dataCache as byte[];
                System.Web.HttpContext.Current.Cache.Remove(fileGuid);

                return File(data, "application/octet-stream", fileName);
            }
            else
            {
                return new EmptyResult();
            }
        }
    }
}