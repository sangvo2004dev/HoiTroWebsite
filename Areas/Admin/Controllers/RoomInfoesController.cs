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
    public class RoomInfoesController : Controller
    {
        private HoiTroEntities db = new HoiTroEntities();

        // GET: Admin/RoomInfoes
        public ActionResult Index()
        {
            var roomInfoes = db.RoomInfoes.Include(r => r.RoomType).Include(r => r.Account);
            return View(roomInfoes.ToList());
        }

        // GET: Admin/RoomInfoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomInfo roomInfo = db.RoomInfoes.Find(id);
            if (roomInfo == null)
            {
                return HttpNotFound();
            }
            return View(roomInfo);
        }

        // GET: Admin/RoomInfoes/Create
        public ActionResult Create()
        {
            ViewBag.roomTypeId = new SelectList(db.RoomTypes, "id", "title");
            ViewBag.accountId = new SelectList(db.Accounts, "id", "name");
            return View();
        }

        // POST: Admin/RoomInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,title,brief_description,detail_description,price,acreage,area,location,tenant,meta,hide,order,datebegin,roomTypeId,accountId")] RoomInfo roomInfo)
        {
            if (ModelState.IsValid)
            {
                db.RoomInfoes.Add(roomInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.roomTypeId = new SelectList(db.RoomTypes, "id", "title", roomInfo.roomTypeId);
            ViewBag.accountId = new SelectList(db.Accounts, "id", "name", roomInfo.accountId);
            return View(roomInfo);
        }

        // GET: Admin/RoomInfoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomInfo roomInfo = db.RoomInfoes.Find(id);
            if (roomInfo == null)
            {
                return HttpNotFound();
            }
            ViewBag.roomTypeId = new SelectList(db.RoomTypes, "id", "title", roomInfo.roomTypeId);
            ViewBag.accountId = new SelectList(db.Accounts, "id", "name", roomInfo.accountId);
            return View(roomInfo);
        }

        // POST: Admin/RoomInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,title,brief_description,detail_description,price,acreage,area,location,tenant,meta,hide,order,datebegin,roomTypeId,accountId")] RoomInfo roomInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roomInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.roomTypeId = new SelectList(db.RoomTypes, "id", "title", roomInfo.roomTypeId);
            ViewBag.accountId = new SelectList(db.Accounts, "id", "name", roomInfo.accountId);
            return View(roomInfo);
        }

        // GET: Admin/RoomInfoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomInfo roomInfo = db.RoomInfoes.Find(id);
            if (roomInfo == null)
            {
                return HttpNotFound();
            }
            return View(roomInfo);
        }

        // POST: Admin/RoomInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RoomInfo roomInfo = db.RoomInfoes.Find(id);
            db.RoomInfoes.Remove(roomInfo);
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
