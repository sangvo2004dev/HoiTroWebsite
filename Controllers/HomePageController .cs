using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HoiTroWebsite.Models;

namespace HoiTroWebsite.Controllers
{
    public class HomePageController : Controller
    {
        // Khai báo
        private readonly HoiTroEntities db = new HoiTroEntities();

        // GET: Default
        public ActionResult Index()
        {
            // lấy list loại phòng
            ViewBag.ListRoomType = db.RoomTypes.Where(rt => rt.hide == true).OrderBy(rt => rt.order).ToList();
            return View();
        }

        public ActionResult GetNews(long? id, string metatitle)
        {
            if ((id == null) && (metatitle == null))
            {
                ViewBag.meta = "tin-tuc";
                var _7TinTuc = db.News.Where(n => n.hide == true).OrderBy(n => n.order).Take(7).ToList();

                return PartialView(_7TinTuc);
            }
            else
            {
                ViewBag.meta = metatitle;
                var v = from n in db.News
                        join t in db.NewsTypes on n.newsTypeId equals t.id
                        where n.hide == true && n.newsTypeId == id
                        orderby n.datebegin descending
                        select n;

                return PartialView(v.ToList());
            }            
        }
        // loại phòng trọ
        public ActionResult GetRoomType()
        {
            ViewBag.meta = "phong-tro";
            var v = from t in db.RoomTypes
                    where t.hide == true
                    orderby t.order ascending
                    select t;

            return PartialView(v.ToList());
        }
        // lấy thông tin phòng trọ
        public ActionResult HomePageGetRoomInfo(int roomTypeID, string metatitle)
        {
            ViewBag.meta = metatitle;
            var listRoomInfo = db.RoomInfoes.Where(ri => ri.roomTypeId == roomTypeID && ri.isApproved == true)
                .Include(ri => ri.Account)
                .Include(ri => ri.RoomImages)
                .ToList();
            return PartialView(listRoomInfo);
        }
        // contact
        public ActionResult getContact()
        {
            return PartialView();
        }
        // banner
        public ActionResult getBanner()
        {
            var v = from i in db.Banners
                    where i.hide == true
                    orderby i.order ascending
                    select i;
            return PartialView(v.ToList());
        }
    }
}
