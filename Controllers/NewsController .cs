using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HoiTroWebsite.Models;

namespace HoiTroWebsite.Controllers
{
    public class NewsController : Controller
    {
        // Khai báo
        HoiTroEntities _db = new HoiTroEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult getNews()
        {
            var v = from t in _db.News
                    where t.hide == true
                    orderby t.datebegin descending
                    select t;
            return PartialView(v.ToList());
        }
    }
}