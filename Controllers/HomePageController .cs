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
        public ActionResult getNews()
        {
            var v = from n in _db.News
                    join i in _db.NewsImages on n.id equals i.reference_id
                    where i.hide == true
                    orderby i.datebegin descending
                    select new NewsViewModel
                    {
                        Title = n.title,
                        Author = "Tác giả: " + n.author,
                        BriefDescription = n.brief_description,
                        ImagePath = i.imagePath
                    };
            return PartialView(v.ToList());
        }
        public ActionResult getRoomType()
        {
            var v = from t in _db.RoomTypes
                    where t.hide == true
                    orderby t.order ascending
                    select t;
            return PartialView(v.ToList());
        }

        public ActionResult getInfor(long id)
        {
            var v = from n in _db.RoomInfoes
                    join i in _db.RoomImages on n.id equals i.reference_id
                    join t in _db.RoomTypes on n.roomTypeId equals t.id
                    where n.hide == true && n.roomTypeId == id
                    orderby n.datebegin descending
                    select new RoomInfoViewModel
                    {
                        Title = n.title,
                        Price = n.price + "VNĐ/tháng",
                        Acreage = n.acreage + "m^2",
                        BriefDescription = n.brief_description,
                        Area = n.area,
                        ImagePath = i.imagePath
                    };
            return PartialView(v.ToList());
        }
    }
}