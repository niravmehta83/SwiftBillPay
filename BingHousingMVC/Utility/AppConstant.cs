using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace BingHousingMVC.Utility
{
    public class AppConstant
    {
        static string filename =  HttpContext.Current.Server.MapPath(String.Format("{0}{1}.txt",ConfigurationManager.AppSettings["logFileName"] , DateTime.Now.ToString("yyyyMMdd")));
        static string _apppath = HttpContext.Current.Server.MapPath("~/");
        public static string AppPath
        {
            get { return _apppath; }
            
        }

        public static string AppLogPath
        {
            get { return filename; }

        }
    }

}