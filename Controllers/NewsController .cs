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
        //xem tất cả tin tức theo 1 loại
        public ActionResult Index(string meta)
        {
            var v = from t in _db.NewsTypes
                    where t.meta == meta
                    select t;
            return View(v.FirstOrDefault());
        }
        ///chi tiết tin trên HomePage-Menu
        public ActionResult NewsDetail(long id)
        {
            var v = from t in _db.News
                    where t.id == id
                    select t;
            return View(v.FirstOrDefault());
        }
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
                    select n;
            return PartialView(v.ToList());
        }


    }
}