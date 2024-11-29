using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HoiTroWebsite.Models;

namespace HoiTroWebsite.Controllers
{
    public class ContactController : Controller
    {
        // Khai báo
        HoiTroEntities db = new HoiTroEntities();

        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult getContact()
        {
            return PartialView();
        }
    }
}