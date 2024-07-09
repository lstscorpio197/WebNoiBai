using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNoiBai.Common;
using WebNoiBai.Models;

namespace WebNoiBai.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            USER us = Session[AppConst.UserSession] as USER;
            if (us == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            using (var db = new SystemEntities())
            {
                try
                {
                    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                    {
                        return View();
                    }

                    var us = db.SUsers.AsNoTracking().FirstOrDefault(x => x.Username == username);
                    if (us == null)
                    {
                        ViewBag.Error = "Tài khoản không tồn tại. Vui lòng kiểm tra lại";
                        return View();
                    }
                    bool isVerify = BCrypt.Net.BCrypt.Verify(password, us.Password);
                    if (!isVerify)
                    {
                        ViewBag.Error = "Tài khoản hoặc mật khẩu không chính xác";
                        return View();
                    }
                    if (us.IsActived != 1)
                    {
                        ViewBag.Error = "Tài khoản này đã bị khóa";
                        return View();
                    }
                    USER user = new USER(us);
                    Session[AppConst.UserSession] = user;
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    return View();
                }
            }
        }

        public ActionResult Logout()
        {
            Session.Remove(AppConst.UserSession);
            return View("Index");
        }
    }
}