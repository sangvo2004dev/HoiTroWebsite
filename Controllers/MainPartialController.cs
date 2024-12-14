using HoiTroWebsite.Models;
using HoiTroWebsite.Models2;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HoiTroWebsite.Controllers
{
    public class MainPartialController : Controller
    {
        // khai báo
        private readonly HoiTroEntities db = new HoiTroEntities();

        // GET: MainPartial
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetMenuBar()
        {
            var v = from t in db.Menus
                    where t.hide == true
                    orderby t.order ascending
                    select t;
            //List<Menu> menus = db.Menus.ToList();
            //menus.ForEach(m => db.Entry(m).Collection(p => p.SubMenus).Load());

            return PartialView(v.ToList());
        }

        public ActionResult GetSubMenu(int mainMenuID, string mainMenuMeta)
        {
            ViewBag.mainMenuMeta = mainMenuMeta;
            var v = from t in db.SubMenus
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
            var rooms = db.RoomInfoes.Where(r => r.hide == true && r.isApproved == true)
                .Include(r => r.RoomImages)
                .OrderByDescending(r => r.datebegin)
                .Select(r => new LatestRoomsModel
                {
                    title = r.title,
                    area = r.acreage,
                    price = r.price,
                    dateTime = r.datebegin,
                    pathImg = r.RoomImages.OrderBy(ri => ri.order).FirstOrDefault().imagePath,
                }).Take(10);

            return PartialView(rooms.ToList());
        }
        
        // hỗ trợ người dùng
        public ActionResult GetSupport()
        {
            var v = from t in db.Mentors
                    where t.hide == true
                    orderby t.order ascending
                    select t;

            return PartialView(v.ToList());
        }
    }
}
