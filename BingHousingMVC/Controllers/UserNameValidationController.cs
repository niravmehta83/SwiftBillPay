using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Web.UI;
using BingHousing_BO;
using BingHousingMVC_DAL;

namespace BingHousingMVC.Controllers
{
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class UserNameValidationController : Controller
    {
        public JsonResult IsUserNameAvailable(string Username)//class to check username avalability
        {
            IBHDbase dbase = new BHDbase();
            List<string> lstUserName = dbase.GetallUserName().ToList<string>(); //creating username list.

            
            string UserName = Username.ToLower();
            if (lstUserName.Contains(UserName)) //checking whether user name is available on the table or not
            {
                string suggestedUID = SuggestionName(Username,lstUserName);//calling the class suggestion name to give suggetion on username when username not available ...
                return Json(suggestedUID, JsonRequestBehavior.AllowGet);//returning the suggestion name to usernameIsAvailable class
            }

            return Json(true, JsonRequestBehavior.AllowGet);

        }

        public static string SuggestionName(string UserName, List<string> lstUserName)//suggestion name class
        {
                        
            string suggestedUID = String.Format(CultureInfo.InvariantCulture,
                "{0} is not available.", UserName);
            string altUsername = UserName;
            for (int i = 1; i < 100; i++)
            {

                if (lstUserName.Contains(altUsername.ToLower()))
                {
                    altUsername = UserName + i.ToString();//creating suggestion name from user name 
                    suggestedUID = String.Format(CultureInfo.InvariantCulture,
                   "{0} is not available. Try {1}.", UserName, altUsername);

                }
                else
                {
                    break;
                }
            }

            return suggestedUID;

        }

    }
}
