using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BingHousingMVC.Filters
{
    [AttributeUsage (AttributeTargets.Class | AttributeTargets.Method,Inherited=true,AllowMultiple=false)]
    public sealed class DenyAccessToUserAttribute : FilterAttribute, IAuthorizationFilter
    {
        //Parameter
        public string Roles { get; set; }

        //Constructor
        public DenyAccessToUserAttribute()
        {

        }

        //IAuthorizationFilter Implemetation
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            bool res = filterContext.HttpContext.Request.IsAuthenticated ? (filterContext.HttpContext.User.IsInRole(Roles) ? false : true) : false;
            if (!res)
            {
                // Returns HTTP 401 - see comment in HttpUnauthorizedResult.cs.
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
            }
            else
            {

            }
        }

        

    }


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class CustomerAuthorize : FilterAttribute, IAuthorizationFilter
    {
        //Parameter
        //public string Roles { get; set; }

        //Constructor
        public CustomerAuthorize()
        {

        }

        //IAuthorizationFilter Implemetation
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            bool res = filterContext.HttpContext.Request.IsAuthenticated ? (filterContext.HttpContext.User.IsInRole("Customer") ? true : false) : false;
            if (!res)
            {
                // Returns HTTP 401 - see comment in HttpUnauthorizedResult.cs.
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Cart", action = "CheckOutLogin" }));
            }
            else
            {

            }
        }



    }
}