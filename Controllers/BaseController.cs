using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNoiBai.Authorize;
using WebNoiBai.Models;

namespace WebNoiBai.Controllers
{
    [AuthorizeAccessRole]
    public class BaseController : Controller
    {
        // GET: Base
        public SystemEntities db = new SystemEntities();
        public USER us = new USER();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}