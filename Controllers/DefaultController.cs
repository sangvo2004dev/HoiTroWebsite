using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HoiTroWebsite.Models;

namespace HoiTroWebsite.Controllers
{
    public class DefaultController : Controller
    {
        // Khai báo
        HoiTroEntities _db = new HoiTroEntities();

        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult getMenu()
        {
            var v = from t in _db.Menus
                    where t.hide == false
                    orderby t.order ascending
                    select t;

            return PartialView(v.ToList());
        }
    }
}