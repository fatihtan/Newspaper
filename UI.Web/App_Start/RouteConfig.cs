using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace UI.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            #region DirectAppPage
            routes.MapRoute(
                    name: "DirectAppPage0",
                    url: "{title}",
                    defaults: new { controller = "Home", action = "AppPage" }
                );

            routes.MapRoute(
                name: "DirectAppPage1",
                url: "{title}/{ID}",
                defaults: new { controller = "Home", action = "AppPage" }
            );

            routes.MapRoute(
                name: "DirectAppPage2",
                url: "{title1}/{title2}/{ID}",
                defaults: new { controller = "Home", action = "AppPage" }
            );

            routes.MapRoute(
                name: "DirectAppPage3",
                url: "{title1}/{title2}/{title3}/{ID}",
                defaults: new { controller = "Home", action = "AppPage" }
            );
            #endregion

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}