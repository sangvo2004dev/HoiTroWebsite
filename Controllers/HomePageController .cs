using System;
using System.Collections.Generic;
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
        //tin tuc
        public ActionResult getNewsType()
        {
            @ViewBag.meta = "tin-tuc";
            var v = from t in _db.NewsTypes
                    where t.hide == true
                    orderby t.order ascending
                    select t;
            return PartialView(v.ToList());
        }
        public ActionResult getNews(long id, string metatitle)
        {
            ViewBag.meta = metatitle;
            var v = from n in _db.News
                    join t in _db.NewsTypes on n.newsTypeId equals t.id
                    where n.hide == true && n.newsTypeId == id
                    orderby n.datebegin descending
                    select new NewsViewModel
                    {
                        ID = n.id,
                        Meta = n.meta,
                        Title = n.title,
                        Author = "Tác giả: " + n.author,
                        BriefDescription = n.brief_description,
                        ImagePath = (from i in _db.NewsImages
                         where i.reference_id == n.id
                         orderby i.datebegin descending
                         select i.imagePath).FirstOrDefault()
                    };
            return PartialView(v.ToList());
        }
        //phong tro
        public ActionResult getRoomType()
        {
            @ViewBag.meta = "phong-tro";
            var v = from t in _db.RoomTypes
                    where t.hide == true
                    orderby t.order ascending
                    select t;
            return PartialView(v.ToList());
        }

        public ActionResult getInfor(long id, string metatitle)
        {
            ViewBag.meta = metatitle;
            var v = from n in _db.RoomInfoes
                    join t in _db.RoomTypes on n.roomTypeId equals t.id
                    where n.hide == true && n.roomTypeId == id
                    orderby n.datebegin descending
                    select new RoomInfoViewModel
                    {
                        ID = n.id,
                        Meta = n.meta,
                        Title = n.title,
                        Price = n.price + "tr VNĐ/tháng",
                        Acreage = n.acreage + "m^2",
                        BriefDescription = n.brief_description,
                        Area = n.area,
                        ImagePath = (from i in _db.RoomImages
                                     where i.reference_id == n.id
                                     orderby i.datebegin descending
                                     select i.imagePath).FirstOrDefault()
                    };
            return PartialView(v.ToList());
        }
    }
}
