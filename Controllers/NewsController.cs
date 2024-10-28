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
        private readonly HoiTroEntities _db = new HoiTroEntities();

        public ActionResult Index()
        {
            var v = (from t in _db.News
                     orderby t.order
                     orderby t.datebegin
                     select t).AsNoTracking();

            return View(v.ToList());
        }

        // lấy các tin tức theo loại
        public ActionResult GetNewsFollowType(string meta)
        {
            ViewBag.title = (from t in _db.NewsTypes
                            where t.meta == meta
                            select t.title).First();

            var v = (from t in _db.News
                     join s in _db.NewsTypes on t.newsTypeId equals s.id
                     where t.hide == true && s.meta == meta
                     orderby t.order descending
                     orderby t.datebegin
                     select t).AsNoTracking();

            return View(v.ToList());
        }

        // chi tiết tin trên
        public ActionResult NewsDetail(long id)
        {
            var models = (from t in _db.News
                    join s in _db.NewsTypes on t.newsTypeId equals s.id
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