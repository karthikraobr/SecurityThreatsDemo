using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcMusicStore
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {


            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        //protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        //{
        //    // Remove the "Server" HTTP Header from response
        //    var app = sender as HttpApplication;
        //    if (null != app && null != app.Context && null != app.Context.Response)
        //    {
        //        NameValueCollection headers = app.Context.Response.Headers;
        //        if (null != headers)
        //        {
        //            headers.Remove("Server");
        //            headers.Remove("X-AspNet-Version");
        //            headers.Remove("X-AspNetMvc-Version");
        //            headers.Remove("X-Powered-By");
        //        }
        //    }
        //}
    }
}