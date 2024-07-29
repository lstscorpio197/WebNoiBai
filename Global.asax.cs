using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebNoiBai.Common;

namespace WebNoiBai
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Z.EntityFramework.Extensions.LicenseManager.AddLicense(AppConst.ZEFExtensionsName, AppConst.ZEFExtensionsKey);
            Z.EntityFramework.Extensions.LicenseManager.AddLicense(AppConst.ZBulkName, AppConst.ZBulkKey);
        }
    }
}
