using System.Web.Mvc;

namespace HoiTroWebsite.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            // Default
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Default", action = "AdminIndex" , id = UrlParameter.Optional }
            );

            // Menu
            context.MapRoute(
                "Menu",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Menus", action = "Index", id = UrlParameter.Optional }
            );
            // Banner
            context.MapRoute(
                "Banner",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Banners", action = "Index", id = UrlParameter.Optional }
            );
            // Account
            context.MapRoute(
                "Account",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Accounts", action = "Index", id = UrlParameter.Optional }
            );
            // Footer
            context.MapRoute(
                "Footer",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Footers", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}