using HoiTroWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HoiTroWebsite.Controllers
{
    public class MainPartialController : Controller
    {
        // khai báo
        private readonly HoiTroEntities _db = new HoiTroEntities();

        // GET: MainPartial
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetMenuBar()
        {
            var v = from t in _db.Menus
                    where t.hide == true
                    orderby t.order ascending
                    select t;

            return PartialView(v.ToList());
        }

        public ActionResult GetSubMenu(int mainMenuID, string mainMenuMeta)
        {
            ViewBag.mainMenuMeta = mainMenuMeta;
            var v = from t in _db.SubMenus
                    where (t.hide == true) && (t.menuId == mainMenuID)
                    orderby t.order ascending
                    select t;

            return PartialView(v.ToList());
        }

        // thanh tìm kiếm
        public ActionResult GetSearchBar()
        {
            return PartialView();
        }

        public ActionResult GetMiniNews()
        {
            return PartialView();
        }

        // lấy các bài viết phòng mới đăng
        public ActionResult GetLatestRoomList()
        {
            return PartialView();
        }
    }
}
