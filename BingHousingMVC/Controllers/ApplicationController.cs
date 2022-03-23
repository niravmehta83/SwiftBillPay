using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace BingHousingMVC.Controllers
{
    public class ApplicationController : Controller
    {
        //
        // GET: /Application/

        public string Stop()
        {
            string path = String.Concat(Server.MapPath("~"), "config.txt");
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            return "success";
        }

    }
}
