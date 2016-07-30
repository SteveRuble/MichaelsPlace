using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MichaelsPlace.Controllers;
using MichaelsPlace.Controllers.Admin;

namespace MichaelsPlace
{
    internal static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                name: "Admin",
                url: "Admin/{controller}/{action}/{id}",
                defaults: new {controller = "Dashboard", action = "Index", id = UrlParameter.Optional},
                namespaces: new[] {typeof(AdminControllerBase).Namespace}
                );

            routes.MapRoute(
                name: "Identity",
                url: "Identity/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { typeof(AccountController).Namespace }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { typeof(HomeController).Namespace }
            );

            routes.MapMvcAttributeRoutes();
        }
    }
}
