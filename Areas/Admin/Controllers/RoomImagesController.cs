using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
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
            Response.StatusCode = 200;
            return View();
        }

        //
        [HttpGet]
        public JsonResult getImages()
        {
            try
            {
                var img = (from t in db.RoomImages
                              select new
                              {
                                  Id = t.id,
                                  Reference = t.reference_id,
                                  Img = t.imagePath,
                                  Meta = t.meta,
                                  Hide = t.hide,
                                  Order = t.order,
                                  DateBegin = t.datebegin
                              }).ToList();
                return Json(new { code = 200, img = img, msg = "Lấy img thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy img thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
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
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "id,reference_id,imagePath,meta,hide,order,datebegin")] RoomImage roomImage, HttpPostedFileBase img)
        {
            try
            {
                var path = "";
                var filename = "";
                if (ModelState.IsValid)
                {
                    if (img != null)
                    {
                        filename = Path.GetFileName(img.FileName);
                        path = Path.Combine(Server.MapPath("~/Content/images"), filename);
                        img.SaveAs(path);
                        roomImage.imagePath = filename;
                    }
                    else
                    {
                        roomImage.imagePath = "logo.png";
                    }
                    roomImage.datebegin = DateTime.Now;
                    db.RoomImages.Add(roomImage);
                    db.SaveChanges();
                    return Json(new { code = 200, msg = "Img created successfully" }, JsonRequestBehavior.AllowGet);
                }

                ViewBag.reference_id = new SelectList(db.RoomInfoes, "id", "title", roomImage.reference_id);
                return Json(new { code = 400, msg = "Invalid data" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) 
            {
                return Json(new { code = 500, msg = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
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
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "id,reference_id,imagePath,meta,hide,order,datebegin")] RoomImage roomImage, HttpPostedFileBase img)
        {
            var path = "";
            var filename = "";
            RoomImage temp = getById(roomImage.id);
            if (ModelState.IsValid)
            {
                if (img != null)
                {
                    filename = Path.GetFileName(img.FileName);  // Chỉ lấy phần tên file
                    path = Path.Combine(Server.MapPath("~/Content/images"), filename);
                    img.SaveAs(path);
                    temp.imagePath = filename; // Cập nhật tên ảnh mới
                }
                temp.reference_id = roomImage.reference_id ;
                temp.meta = roomImage.meta;
                temp.hide = roomImage.hide;
                temp.order = roomImage.order;

                db.Entry(temp).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Cập nhật thành công" }); // Trả về JSON để AJAX xử lý
            }
            ViewBag.reference_id = new SelectList(db.RoomInfoes, "id", "title", roomImage.reference_id);
            return Json(new { code = 400, msg = "Cập nhật thất bại" });
        }
        public RoomImage getById(long id)
        {
            return db.RoomImages.Where(x => x.id == id).FirstOrDefault();
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            var roomImage = db.RoomImages.Find(id);
            if (roomImage == null)
            {
                return Json(new { code = 404, msg = "roomImage không tồn tại" });
            }

            try
            {
                db.RoomImages.Remove(roomImage);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Xóa roomImage thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Có lỗi xảy ra khi xóa roomImage: " + ex.Message });
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
