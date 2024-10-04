using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HoiTroWebsite
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("News", "{type}",
               new { controller = "News", action = "getNews", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    {"type","tin-tuc" }
               },
               new[] { "HoiTroWebsite.Controllers" });

            routes.MapRoute("RoomInfo", "{type}",
               new { controller = "RoomInfo", action = "getInfor", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    {"type","chu-de" }
               },
               new[] { "HoiTroWebsite.Controllers" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
