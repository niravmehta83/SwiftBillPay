using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Logging;
using System.Configuration;
using BingHousingMVC.Utility;
using System.Threading;
using BingHousingMVC.Models;
using WebMatrix.WebData;
using System.IO;
using System.Net;

namespace BingHousingMVC
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //AreaRegistration.RegisterAllAreas();
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; // This is equivalent to SecurityProtocolType.Tls12

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            AutoMapperConfigurator.Configure();
            if (!WebSecurity.Initialized)
            {
                WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            }
        }

        //protected void Application_Error(object sender, EventArgs e)
        //{

        //    Exception excep = Server.GetLastError();
        //    Response.Redirect("/Home/Error");
        //} 

    }
}