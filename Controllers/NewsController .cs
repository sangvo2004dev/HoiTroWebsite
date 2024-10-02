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
            var v = from n in _db.News
                    join i in _db.NewsImages on n.id equals i.reference_id
                    where i.hide == true
                    orderby i.datebegin descending
                    select new NewsViewModel
                    {
                        Title = n.title,
                        Author = n.author,
                        BriefDescription = n.brief_description,
                        ImagePath = i.imagePath
                    };
            return PartialView(v.ToList());
        }
    }
}