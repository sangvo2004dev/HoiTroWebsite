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
        private readonly HoiTroEntities db = new HoiTroEntities();

        // GET: Admin/News
        public ActionResult Index()
        {
            var news = db.News.Include(n => n.NewsType);
            getNewType();
            Response.StatusCode = 200;
            return View();
        }
        public void getNewType(long? selectedId = null)
        {
            ViewBag.NewsType = new SelectList(db.NewsTypes.Where(x => x.hide == true).OrderBy(x => x.order), "id", "title", selectedId);
        }
        [HttpGet]

        public JsonResult getNews(int? id)
        {
            try
            {
                if (id == null)
                {
                    var news1 = (from t in db.News.OrderBy(x => x.order)
                                    select new
                                    {
                                        Id = t.id,
                                        Name = t.title,
                                        Img = t.file_name,
                                        Author = t.author, 
                                        Meta = t.meta,
                                        Hide = t.hide,
                                        Order = t.order,
                                        DateBegin = t.datebegin,
                                        NewsType = t.NewsType.title
                                    }).ToList();
                    return Json(new { code = 200, news = news1, msg = "Lấy News thành công" }, JsonRequestBehavior.AllowGet);
                }
                var news2 = (from t in db.News.Where(x => x.newsTypeId == id).OrderBy(x => x.order)
                                select new
                                {
                                    Id = t.id,
                                    Name = t.title,
                                    Img = t.file_name,
                                    Author = t.author,
                                    Meta = t.meta,
                                    Hide = t.hide,
                                    Order = t.order,
                                    DateBegin = t.datebegin,
                                    NewsType = t.NewsType.title
                                }).ToList();
                return Json(new { code = 200, news = news2, msg = "Lấy News thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
        {
                return Json(new { code = 500, msg = "Lấy SubMenu thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
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
        public int getMaxOrder(long newsTypeId)
        {
            return db.News.Where(x => x.newsTypeId == newsTypeId).Count();
        }

        // POST: Admin/News/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "id,title,author,meta,hide,order,datebegin,brief_description,detail_description,newsTypeId,file_name")] News news, HttpPostedFileBase img)
        {
            try
            {
                ViewBag.newsTypeId = new SelectList(db.NewsTypes, "id", "title", news.newsTypeId);
                if (!ModelState.IsValid)
                {
                    Response.StatusCode = 400;
                    return View();
                }
                var path = "";
                var filename = "";
                if (img != null)
                {
                    filename = img.FileName;  // Chỉ lấy phần tên file

                    path = Path.Combine(Server.MapPath("~/Content/images"), filename);
                    img.SaveAs(path);
                    news.file_name = filename; //Lưu ý
                }
                else
                {
                    news.file_name = "logo.png";
                }
                news.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                db.News.Add(news);
                db.SaveChanges();
                return Json(new { code = 200, msg = "News created successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) 
            {
                return Json(new { code = 500, msg = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
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
        public ActionResult Edit([Bind(Include = "id,title,author,meta,hide,order,datebegin,brief_description,detail_description,newsTypeId,file_name")] News news, HttpPostedFileBase img)
        {
            var path = "";
            var filename = "";
            News temp = getById(news.id);

            if (ModelState.IsValid)
            {
                if (img != null)
                {
                    filename = DateTime.Now.ToString("dd-MM-yy-hh-mm-ss-") + img.FileName;
                    path = Path.Combine(Server.MapPath("~/Content/images"), filename);
                    img.SaveAs(path);
                    temp.file_name = filename; // Lưu ý
                }
                temp.title = news.title;
                temp.brief_description = news.brief_description;
                temp.detail_description = news.detail_description;
                temp.meta = news.meta;
                temp.hide = news.hide;
                temp.order = news.order;
                db.Entry(temp).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Cập nhật thành công" }); // Trả về JSON để AJAX xử lý
            }
            ViewBag.newsTypeId = new SelectList(db.NewsTypes, "id", "title", news.newsTypeId);
            return Json(new { code = 400, msg = "Cập nhật thất bại" });
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            var news = db.News.Find(id);
            if (news == null)
            {
                return Json(new { code = 404, msg = "News không tồn tại" });
            }

            try
        {
            db.News.Remove(news);
            db.SaveChanges();
                return Json(new { code = 200, msg = "Xóa News thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Có lỗi xảy ra khi xóa News: " + ex.Message });
            }
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
