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
            ///menu-tin tức
            routes.MapRoute("News", "{type}",
               new { controller = "News", action = "getNewsType", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    {"type","tin-tuc" }
               },
               new[] { "HoiTroWebsite.Controllers" });
            /////phân loại phòng
            routes.MapRoute("News_Thay", "{type}",
               new { controller = "HomePage", action = "getNewsType", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    {"type","tin-tuc" }
               },
               new[] { "HoiTroWebsite.Controllers" });
                    ///hiển thị tất cả phòng thuộc loại đã chọn
            routes.MapRoute("News_Thay2", "{type}/{meta}",
               new { controller = "News", action = "Index", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    {"type","tin-tuc" }
               },
               new[] { "HoiTroWebsite.Controllers" });
                    ///chi tiết tin
            routes.MapRoute("NewsDetail", "{type}/{meta}/{id}",
               new { controller = "News", action = "NewsDetail", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    {"type","tin-tuc" }
               },
               new[] { "HoiTroWebsite.Controllers" });
            /////
            ///menu-phòng trọ
            routes.MapRoute("Rooms", "{type}",
               new { controller = "RoomInfo", action = "getRoomType", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    {"type","phong-tro" }
               },
               new[] { "HoiTroWebsite.Controllers" });

            // menu-trang chủ
            routes.MapRoute("Home", "{type}",
               new { controller = "HomePage", action = "Index", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    {"type","trang-chu" }
               },
               new[] { "HoiTroWebsite.Controllers" });

            // phân loại phòng
            routes.MapRoute("Rooms_Thay", "{type}",
               new { controller = "HomePage", action = "getRoomType", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    {"type","phong-tro" }
               },
               new[] { "HoiTroWebsite.Controllers" });
            ///hiển thị tất cả phòng thuộc loại đã chọn
            routes.MapRoute("Rooms_Thay2", "{type}/{meta}",
               new { controller = "RoomInfo", action = "Index", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    {"type","phong-tro" }
               },
               new[] { "HoiTroWebsite.Controllers" });
            ///chi tiết phòng
            routes.MapRoute("RoomDetail", "{type}/{meta}/{id}",
               new { controller = "RoomInfo", action = "RoomDetail", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    {"type","phong-tro" }
               },
               new[] { "HoiTroWebsite.Controllers" });

            //Default
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "HomePage", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
