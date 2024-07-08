using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNoiBai.Common;

namespace WebNoiBai.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return View();
            }
            if(username == "admin")
            {
                Session[AppConst.UserSession] = username;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Tài khoản hoặc mật khẩu không chính xác";
                return View();
            }
        }

        public ActionResult Logout() { 
            Session.Remove(AppConst.UserSession);
            return View("Index");
        }
    }
}