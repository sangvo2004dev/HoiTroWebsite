using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HoiTroWebsite.Models;

namespace HoiTroWebsite.Controllers
{
    public class NewsController : Controller
    {
        // Khai báo
        private readonly HoiTroEntities db = new HoiTroEntities();

        public ActionResult Index()
        {
            var news = db.News.Where(n => n.hide == true).ToList();    

            return View(news);
        }

        // lấy các tin tức theo loại
        public ActionResult GetNewsFollowType(string meta)
        {
            ViewBag.title = (from t in db.NewsTypes
                            where t.meta == meta
                            select t.title).First();

            var v = (from t in db.News
                     join s in db.NewsTypes on t.newsTypeId equals s.id
                     where t.hide == true && s.meta == meta
                     orderby t.order descending
                     orderby t.datebegin
                     select t).AsNoTracking();

            return View(v.ToList());
        }

        // chi tiết tin trên
        public ActionResult NewsDetail(long id)
        {
            var models = (from t in db.News
                    join s in db.NewsTypes on t.newsTypeId equals s.id
                    where t.id == id
                    select new
                    {
                        newModel = t,
                        newTypeModel = s
                    }).First();

            ViewBag.metaNewsType = models.newTypeModel.meta;
            ViewBag.titleNewsType = models.newTypeModel.title;

            return View(models.newModel);
        }

    }
}