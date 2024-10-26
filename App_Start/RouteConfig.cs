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
            // menu-tin tức
            routes.MapRoute("News", "{type}",
               new { controller = "News", action = "getNewsType", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    {"type","tin-tuc" }
               },
               new[] { "HoiTroWebsite.Controllers" });

            // phân loại phòng
            routes.MapRoute("News_Thay", "{type}",
               new { controller = "HomePage", action = "getNewsType", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    {"type","tin-tuc" }
               },
               new[] { "HoiTroWebsite.Controllers" });

            // hiển thị tất cả phòng thuộc loại đã chọn
            routes.MapRoute("News_Thay2", "{type}/{meta}",
               new { controller = "News", action = "Index", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    {"type","tin-tuc" }
               },
               new[] { "HoiTroWebsite.Controllers" });

            // chi tiết tin
            routes.MapRoute("NewsDetail", "{type}/{meta}/{id}",
               new { controller = "News", action = "NewsDetail", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    {"type","tin-tuc" }
               },
               new[] { "HoiTroWebsite.Controllers" });
            
            // menu-phòng trọ
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
               new { controller = "HomePage", action = "GetRoomType", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    { "type", "phong-tro" }
               },
               new[] { "HoiTroWebsite.Controllers" });

            // hiển thị tất cả phòng thuộc loại đã chọn
            routes.MapRoute("Rooms_Thay2", "{type}/{meta}",
               new { controller = "RoomInfo", action = "Index", type = UrlParameter.Optional },
               constraints: new {type = @"^(phong-tro|ky-tuc-xa|nha-cho-thue)$" },
               new[] { "HoiTroWebsite.Controllers" });

            // chi tiết phòng
            routes.MapRoute("RoomDetail", "{type}/{meta}/{id}",
               new { controller = "RoomInfo", action = "RoomDetail", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    {"type","phong-tro" }
               },
               new[] { "HoiTroWebsite.Controllers" });

            // contact-menu
            routes.MapRoute("Contact", "{type}",
               new { controller = "Contact", action = "getContact", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    {"type","lien-he" }
               },
               new[] { "HoiTroWebsite.Controllers" });

            // đăng ký
            routes.MapRoute("Register", "{type}",
               new { controller = "Button", action = "getRegister", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    {"type","dang-ky" }
               },
               new[] { "HoiTroWebsite.Controllers" });

            // đăng nhập
            routes.MapRoute("Login", "{type}",
               new { controller = "Button", action = "getLognIn", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    {"type","dang-nhap" }
               },
               new[] { "HoiTroWebsite.Controllers" });

            // đăng tin cho thuê
            routes.MapRoute("Post", "{type}",
               new { controller = "Button", action = "getPost", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    {"type","dang-tin-moi" }
               },
               new[] { "HoiTroWebsite.Controllers" });




            // quản lí tài khoản cá nhân-Index
            routes.MapRoute("QLTK", "{type}/{id}",
               new { controller = "ManageAccount", action = "Index", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    { "type", "quan-ly-tai-khoan" }
               },
               new[] { "HoiTroWebsite.Controllers" });
            // quản lí tài khoản cá nhân
            routes.MapRoute("MNA", "{type}/{id}",
               new { controller = "ManageAccount", action = "editInfor", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    { "type", "account" }
               },
               new[] { "HoiTroWebsite.Controllers" });
            // quản lý tin đăng
            routes.MapRoute("MNP", "{type}/{id}",
               new { controller = "ManageAccount", action = "managePosted", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    { "type", "posted" }
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
