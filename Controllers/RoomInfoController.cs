using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HoiTroWebsite.Models;

namespace HoiTroWebsite.Controllers
{
    public class RoomInfoController : Controller
    {
        // Khai báo
        HoiTroEntities _db = new HoiTroEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult getInfor()
        {
            var v = from n in _db.RoomInfoes
                    join i in _db.RoomImages on n.id equals i.reference_id
                    where n.hide == true
                    orderby n.datebegin descending
                    select new RoomInfoViewModel
                    {
                        Title = n.title,
                        Price = n.price +"VNĐ/tháng",
                        Acreage =  n.acreage+"m^2",
                        BriefDescription = n.brief_description,
                        Area = n.area,
                        ImagePath = i.imagePath
                    };
            return PartialView(v.ToList());
        }
    }
}