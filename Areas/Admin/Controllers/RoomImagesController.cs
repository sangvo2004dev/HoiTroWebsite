using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HoiTroWebsite.Models;

namespace HoiTroWebsite.Areas.Admin.Controllers
{
    public class RoomImagesController : Controller
    {
        private HoiTroEntities db = new HoiTroEntities();

        // GET: Admin/RoomImages
        public ActionResult Index()
        {
            var roomImages = db.RoomImages.Include(r => r.RoomInfo);
            return View(roomImages.ToList());
        }

        // GET: Admin/RoomImages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomImage roomImage = db.RoomImages.Find(id);
            if (roomImage == null)
            {
                return HttpNotFound();
            }
            return View(roomImage);
        }

        // GET: Admin/RoomImages/Create
        public ActionResult Create()
        {
            ViewBag.reference_id = new SelectList(db.RoomInfoes, "id", "title");
            return View();
        }

        // POST: Admin/RoomImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,reference_id,imagePath,meta,hide,order,datebegin")] RoomImage roomImage)
        {
            if (ModelState.IsValid)
            {
                db.RoomImages.Add(roomImage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.reference_id = new SelectList(db.RoomInfoes, "id", "title", roomImage.reference_id);
            return View(roomImage);
        }

        // GET: Admin/RoomImages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomImage roomImage = db.RoomImages.Find(id);
            if (roomImage == null)
            {
                return HttpNotFound();
            }
            ViewBag.reference_id = new SelectList(db.RoomInfoes, "id", "title", roomImage.reference_id);
            return View(roomImage);
        }

        // POST: Admin/RoomImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,reference_id,imagePath,meta,hide,order,datebegin")] RoomImage roomImage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roomImage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.reference_id = new SelectList(db.RoomInfoes, "id", "title", roomImage.reference_id);
            return View(roomImage);
        }

        // GET: Admin/RoomImages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomImage roomImage = db.RoomImages.Find(id);
            if (roomImage == null)
            {
                return HttpNotFound();
            }
            return View(roomImage);
        }

        // POST: Admin/RoomImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RoomImage roomImage = db.RoomImages.Find(id);
            db.RoomImages.Remove(roomImage);
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
