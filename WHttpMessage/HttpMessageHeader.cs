using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebNoiBai.WHttpMessage
{
    public class HttpMessageHeader
    {
        public string AppName { get; set; }

        public string AppVersion { get; set; }

        public string ResponseID { get; set; }

        public string ResponseDate { get; set; }

        public HttpMessageHeader()
        {
            ResponseID = Guid.NewGuid().ToString();
            ResponseDate = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss");
        }

        public void SetAppInfo(string appName, string appVersion)
        {
            AppName = appName;
            AppVersion = appVersion;
        }
    }
}