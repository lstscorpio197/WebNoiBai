using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNoiBai.Models;

namespace WebNoiBai.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        public SystemEntities db = new SystemEntities();
        public USER us = new USER();
    }
}