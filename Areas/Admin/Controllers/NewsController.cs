using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Common.EntitySql;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HoiTroWebsite.Models;

namespace HoiTroWebsite.Areas.Admin.Controllers
{
    public class NewsController : Controller
    {
        private HoiTroEntities db = new HoiTroEntities();

        // GET: Admin/News
        public ActionResult Index()
        {
            var news = db.News.Include(n => n.NewsType);
            return View(news.ToList());
        }

        // GET: Admin/News/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // GET: Admin/News/Create
        public ActionResult Create()
        {
            ViewBag.newsTypeId = new SelectList(db.NewsTypes, "id", "title");
            return View();
        }

        // POST: Admin/News/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "id,title,author,meta,hide,order,datebegin,brief_description,detail_description,newsTypeId,imagePath")] News news, HttpPostedFileBase img)
        {
            if (ModelState.IsValid)
            {
                var path = "";
                var filename = "";
                if (ModelState.IsValid)
                {
                    if (img != null)
                    {
                        //file =  Guid = Guid.newGuid().toString() + img.FileName;
                        filename = img.FileName;
                        path = Path.Combine(Server.MapPath("~/Content/images"), filename);
                        img.SaveAs(path);
                        news.imagePath = filename; //Lưu ý
                    }
                    else
                    {
                        news.imagePath = "logo.png";
                    }
                    news.datebegin =  Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    //news.meta = Functions.ConvertTopUpSign(news.title);
                    db.News.Add(news);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.newsTypeId = new SelectList(db.NewsTypes, "id", "title", news.newsTypeId);
            return View(news);
        }

        // GET: Admin/News/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            ViewBag.newsTypeId = new SelectList(db.NewsTypes, "id", "title", news.newsTypeId);
            return View(news);
        }

        // POST: Admin/News/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "id,title,author,meta,hide,order,datebegin,brief_description,detail_description,newsTypeId,imagePath")] News news, HttpPostedFileBase img)
        {
            var path = "";
            var filename = "";
            News temp = getById(news.id);

            if (ModelState.IsValid)
            {
                if (img != null)
                {
                    // filename = Guid.NewGuid().ToString() + img.FileName;
                    filename = DateTime.Now.ToString("dd-MM-yy-hh-mm-ss-") + img.FileName;
                    path = Path.Combine(Server.MapPath("~Content/images"), filename);
                    img.SaveAs(path);
                    temp.imagePath = filename; // Lưu ý
                }
                // tem.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                temp.title = news.title;
                temp.brief_description = news.brief_description;
                temp.detail_description = news.detail_description;
                temp.meta = news.meta;
                temp.hide = news.hide;
                temp.order = news.order;
                db.Entry(temp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.newsTypeId = new SelectList(db.NewsTypes, "id", "title", news.newsTypeId);
            return View(news);
        }
        public News getById(long id)
        {
            return db.News.Where(x => x.id == id).FirstOrDefault();
        }

        // GET: Admin/News/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: Admin/News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            News news = db.News.Find(id);
            db.News.Remove(news);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
