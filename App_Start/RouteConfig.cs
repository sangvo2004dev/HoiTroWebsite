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
            routes.MapMvcAttributeRoutes();

            // menu-tin tức
            routes.MapRoute("News", "tin-tuc",
               new { controller = "News", action = "Index" },

               new[] { "HoiTroWebsite.Controllers" });

            // tin tức theo loại
            routes.MapRoute("NewsType", "tin-tuc/{meta}",
                new { controller = "News", action = "GetNewsFollowType", meta = UrlParameter.Optional },
                new[] { "HoiTroWebsite.Controllers" });

            // chi tiết tin
            routes.MapRoute("NewsDetail", "tin-tuc/{meta}/{id}",
               new { controller = "News", action = "NewsDetail", meta = UrlParameter.Optional, id = UrlParameter.Optional },
               new[] { "HoiTroWebsite.Controllers" });

            // hiển thị tất cả phòng khi vào trang chủ cho thuê
            routes.MapRoute("ListRoom", "phong-tro",
                new { controller = "RoomInfo", action = "Index" },
                new[] { "HoiTroWebsite.Controllers" });

            // hiển thị tất cả phòng thuộc loại đã chọn
            routes.MapRoute("ListRoomFollowType", "{type}/{roomTypeMeta}",
               new { controller = "RoomInfo", action = "GetListRoomInfoFollowType", roomTypeMeta = UrlParameter.Optional },
               constraints: new { type = @"^(phong-tro|ky-tuc-xa|nha-cho-thue)$" },
               new[] { "HoiTroWebsite.Controllers" });

            // chi tiết phòng
            routes.MapRoute("RoomDetail", "{metaRoomType}/{metaRoomInfo}/{roomID}",
               new { controller = "RoomInfo", action = "RoomDetail"},
               new[] { "HoiTroWebsite.Controllers" });

            // menu-trang chủ
            routes.MapRoute("Home", "{type}",
               new { controller = "HomePage", action = "Index", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    {"type","trang-chu" }
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
               new { controller = "User", action = "Register", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    {"type","dang-ky" }
               },
               new[] { "HoiTroWebsite.Controllers" });

            // đăng nhập
            routes.MapRoute("Login", "{type}",
               new { controller = "User", action = "Login", type = UrlParameter.Optional },
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

            // quản lí tài khoản cá nhân
            routes.MapRoute("MNA", "{type}/{id}",
               new { controller = "Button", action = "editInfor", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    { "type", "quan-ly-thong-tin-tai-khoan" }
               },
               new[] { "HoiTroWebsite.Controllers" });

            // thay đổi số điện thoai
            routes.MapRoute("ALTSDT", "{type}/{id}",
               new { controller = "Button", action = "altPhoneNum", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    { "type", "thay-doi-so-dien-thoai" }
               },
               new[] { "HoiTroWebsite.Controllers" });

            // thay đổi mật khẩu
            routes.MapRoute("ALTPW", "{type}/{id}",
               new { controller = "Button", action = "altPassword", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    { "type", "thay-doi-mat-khau" }
               },
               new[] { "HoiTroWebsite.Controllers" });

            // quản lý tin đăng
            routes.MapRoute("MNP", "{type}/{id}",
               new { controller = "Button", action = "managePosted", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    { "type", "quan-ly-tin-dang" }
               },
               new[] { "HoiTroWebsite.Controllers" });

            // thông tin công ty
            routes.MapRoute("CompanyInfo", "{type}",
               new { controller = "Footer", action = "getCompanyInfo", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    { "type", "gioi-thieu-cong-ty" }
               },
               new[] { "HoiTroWebsite.Controllers" });
            // chính sách bảo mật
            routes.MapRoute("privacyPolicy", "{type}",
               new { controller = "Footer", action = "getPrivacyPolicyLink", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    { "type", "chinh-sach-bao-mat" }
               },
               new[] { "HoiTroWebsite.Controllers" });
            // chính sách bảo mật
            routes.MapRoute("TermsOfService", "{type}",
               new { controller = "Footer", action = "getTermsOfServiceLink", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    { "type", "dieu-khoan-dich-vu" }
               },
               new[] { "HoiTroWebsite.Controllers" });
            // quy định sử dụng
            routes.MapRoute("TermsOfUse", "{type}",
               new { controller = "Footer", action = "getTermsOfUseLink", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    { "type", "quy-dinh-su-dung" }
               },
               new[] { "HoiTroWebsite.Controllers" });
            // quy định đăng tin
            routes.MapRoute("TermsOfPosting", "{type}",
               new { controller = "Footer", action = "getTermsOfPostingLink", type = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    { "type", "quy-dinh-dang-tin" }
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
