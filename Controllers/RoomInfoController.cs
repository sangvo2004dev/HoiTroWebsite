using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HoiTroWebsite.Models;

namespace HoiTroWebsite.Controllers
{
    public class RoomInfoController : Controller
    {
        // Khai báo
        HoiTroEntities _db = new HoiTroEntities();
        //xem tất cả phòng theo 1 loại
        public ActionResult Index(string meta)
        {
            var v = from t in _db.RoomTypes
                    where t.meta == meta
                    select t;
            return View(v.FirstOrDefault());
        }
        ///chi tiết tin trên HomePage-Menu
        public ActionResult RoomDetail(long id)
        {
            var v = from t in _db.RoomInfoes
                    where t.id == id
                    select t;
            return PartialView(v.FirstOrDefault());
        }
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
                        Price = n.price + "VNĐ/tháng",
                        Acreage = n.acreage + "m^2",
                        BriefDescription = n.brief_description,
                        Area = n.area,
                        ImagePath = (from i in _db.NewsImages
                                     where i.reference_id == n.id
                                     orderby i.datebegin descending
                                     select i.imagePath).FirstOrDefault()
                    };
            return PartialView(v.ToList());
        }
    }
}