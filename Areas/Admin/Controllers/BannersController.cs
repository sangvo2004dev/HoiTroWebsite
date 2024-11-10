using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HoiTroWebsite.Models;

namespace HoiTroWebsite.Areas.Admin.Controllers
{
    public class BannersController : Controller
    {
        private HoiTroEntities db = new HoiTroEntities();

        // GET: Admin/Banners
        public ActionResult Index()
        {
            return View(db.Banners.ToList());
        }

        // GET: Admin/Banners/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner banner = db.Banners.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }

        // GET: Admin/Banners/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Banners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "id,name,imgData,meta,hide,order,datebegin")] Banner banner, HttpPostedFileBase img)
        {
            var path = "";
            var filename = "";
            if (ModelState.IsValid)
            {
                if (img != null)
                {
                    //file =  Guid = Guid.newGuid().toString() + img.FileName;
                    filename = Path.GetFileName(img.FileName);  // Chỉ lấy phần tên file

                    path = Path.Combine(Server.MapPath("~/Content/images"), filename);
                    img.SaveAs(path);
                    banner.img = filename; //Lưu ý
                }
                else
                {
                    banner.img = "logo.png";
                }
                db.Banners.Add(banner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(banner);
        }

        // GET: Admin/Banners/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner banner = db.Banners.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }

        // POST: Admin/Banners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "id,name,imgData,meta,hide,order,datebegin")] Banner banner, HttpPostedFileBase img)
        {
            var path = "";
            var filename = "";
            Banner temp = getById(banner.id);
            if (ModelState.IsValid)
            {
                if (img != null)
                {
                    filename = Path.GetFileName(img.FileName);  // Chỉ lấy phần tên file
                    path = Path.Combine(Server.MapPath("~/Content/images"), filename);
                    img.SaveAs(path);
                    temp.img = filename; // Lưu ý
                }
                temp.name = banner.name;
                temp.meta = banner.meta;
                temp.hide = banner.hide;
                temp.order = banner.order;

                db.Entry(temp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(banner);
        }
        public Banner getById(long id)
        {
            return db.Banners.Where(x => x.id == id).FirstOrDefault();
        }

        // GET: Admin/Banners/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner banner = db.Banners.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }

        // POST: Admin/Banners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Banner banner = db.Banners.Find(id);
            db.Banners.Remove(banner);
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
