using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebNoiBai.Common;
using WebNoiBai.Models;

namespace WebNoiBai.Authorize
{
    public class AuthorizeAccessRole : AuthorizeAttribute, IAuthorizationFilter
    {
        public int Level = UserLevel.CongChuc;
        public string TypeHandle = string.Empty;

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            USER us = filterContext.HttpContext.Session[AppConst.UserSession] as USER;
            if (us == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", "Logout" }, { "controller", "Login" } });
                return;
            }
            using(var db = new SystemEntities())
            {
                var user = db.SUsers.AsNoTracking().FirstOrDefault(x=>x.Id == us.Id);
                if (user == null)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", "Logout" }, { "controller", "Login" } });
                    return;
                }
            }
            if (us.ChucVu == UserLevel.Admin) return;
            if(this.Level > UserLevel.CongChuc || (this.Level != UserLevel.CongChuc && us.ChucVu > this.Level))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", "Logout" }, { "controller", "Login" } });
                return;
            }
            
            string controller = filterContext.Controller.ToString();
            if (controller == "Home" || string.IsNullOrEmpty(TypeHandle)) { 
                return;
            }
            if(!us.LstPermission.Where(x=>x.Controller == controller && x.Action == TypeHandle).Any())
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", "Logout" }, { "controller", "Login" } });
                return;
            }
            return;
        }
    }
}