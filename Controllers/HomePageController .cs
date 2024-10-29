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
        HoiTroEntities _db = new HoiTroEntities();

        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetNews(long? id, string metatitle)
        {
            if ((id == null) && (metatitle == null))
            {
                ViewBag.meta = "tin-tuc";
                var _7TinTuc = from t in _db.News
                        where t.hide == true
                        orderby t.order
                        orderby t.datebegin
                        select t;

                return PartialView(_7TinTuc.Take(7).ToList());
            }
            else
            {
                ViewBag.meta = metatitle;
                var v = from n in _db.News
                        join t in _db.NewsTypes on n.newsTypeId equals t.id
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
            var v = from t in _db.RoomTypes
                    where t.hide == true
                    orderby t.order ascending
                    select t;

            return PartialView(v.ToList());
        }
        // lấy thông tin phòng trọ
        public ActionResult HomePageGetRoomInfo(long roomTypeID, string metatitle)
        {
            ViewBag.meta = metatitle;
            var v = from n in _db.RoomInfoes
                    join t in _db.RoomTypes on n.roomTypeId equals t.id
                    join r in _db.Accounts on n.accountId equals r.id
                    where n.hide == true && n.roomTypeId == roomTypeID
                    orderby n.datebegin descending
                    select new RoomInfoViewModel
                    {
                        RoomInfo = n,
                        Account = r,
                        ImagePath = (from i in _db.RoomImages
                                     where i.reference_id == n.id
                                     orderby i.datebegin descending
                                     select i.imagePath).FirstOrDefault()
                    };
            return PartialView(v.ToList());
        }
        //contact
        public ActionResult getContact()
        {
            return PartialView();
        }
        //banner
        public ActionResult getBanner()
        {
            return PartialView();
        }
    }
}
