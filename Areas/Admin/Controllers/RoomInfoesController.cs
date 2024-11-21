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
        public ActionResult Index(long? id = null)
        {
            var roomInfoes = db.RoomInfoes.Include(r => r.RoomType).Include(r => r.Account);
            //return View(roomInfoes.ToList());
            getRoomType(id);
            return View();
        }

        public void getRoomType(long? selectedId = null)
        {
            ViewBag.RoomType = new SelectList(db.RoomTypes.Where(x => x.hide == true).OrderBy(x => x.order), "id", "title", selectedId);
        }

        public ActionResult getRoom(long? id)
        {
            if (id == null)
            {
                var v = db.RoomInfoes.OrderBy(x => x.order).ToList();
                return PartialView(v);
            }
            var m = db.RoomInfoes.Where(x => x.roomTypeId == id).OrderBy(x => x.order).ToList();
            return PartialView(m);
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

        public int getMaxOrder(long roomTypeId)
        {
            return db.RoomInfoes.Where(x => x.roomTypeId == roomTypeId).Count();
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
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "id,title,brief_description,detail_description,price,acreage,area,location,tenant,meta,hide,order,datebegin,roomTypeId,accountId")] RoomInfo roomInfo)
        {
            if (ModelState.IsValid)
            {
                roomInfo.datebegin = DateTime.Now.Date;
                roomInfo.order = getMaxOrder((long)roomInfo.roomTypeId);
                db.RoomInfoes.Add(roomInfo);
                db.SaveChanges();
                return RedirectToAction("Index", "RoomInfoes", new { roomTypeId = roomInfo.roomTypeId, accountId = roomInfo.accountId });
            }

            ViewBag.roomTypeId = new SelectList(db.RoomTypes, "id", "title", roomInfo.roomTypeId);
            ViewBag.accountId = new SelectList(db.Accounts, "id", "name", roomInfo.accountId);
            return View(roomInfo);
        }

        public RoomInfo getById(long id)
        {
            return db.RoomInfoes.Where(x => x.id == id).FirstOrDefault();
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
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "id,title,brief_description,detail_description,price,acreage,area,location,tenant,meta,hide,order,datebegin,roomTypeId,accountId")] RoomInfo roomInfo)
        {
            RoomInfo temp = getById(roomInfo.id);
            if (ModelState.IsValid)
            {
                roomInfo.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                temp.title = roomInfo.title;
                temp.brief_description = roomInfo.brief_description;
                temp.detail_description = roomInfo.detail_description;
                temp.price = roomInfo.price;
                temp.acreage = roomInfo.acreage;
                temp.area = roomInfo.area;
                temp.location = roomInfo.location;
                temp.tenant = roomInfo.tenant;
                temp.meta = roomInfo.meta;
                temp.hide = roomInfo.hide;
                temp.order = roomInfo.order;
                temp.roomTypeId = roomInfo.roomTypeId;
                roomInfo.order = getMaxOrder((long)roomInfo.roomTypeId);
                db.Entry(temp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "RoomInfoes", new { roomTypeId = roomInfo.roomTypeId, accountId = roomInfo.accountId });
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
