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
            Response.StatusCode = 200;
            return View();
        }
        [HttpGet]
        public JsonResult getBanner()
        {
            try
            {
                var banner = (from t in db.Banners
                              select new
                              {
                                  Id = t.id,
                                  Img = t.file_name,
                                  Name = t.name,
                                  Meta = t.meta,
                                  Hide = t.hide,
                                  Order = t.order,
                                  DateBegin = t.datebegin
                              }).ToList();
                return Json(new { code = 200, banner = banner, msg = "Lấy Banner thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy Banner thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
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
        public JsonResult Create([Bind(Include = "id,name,file_name,meta,hide,order,datebegin")] Banner banner, HttpPostedFileBase img)
        {
            try
            {
                var path = "";
                var filename = "";

                if (ModelState.IsValid)
                {
                    if (img != null)
                    {
                        filename = img.FileName;
                        path = Path.Combine(Server.MapPath("~/Content/images"), filename);
                        img.SaveAs(path);
                        banner.file_name = filename;
                    }
                    else
                    {
                        banner.file_name = "logo.png";
                    }

                    banner.datebegin = DateTime.Now;
                    db.Banners.Add(banner);
                    db.SaveChanges();

                    return Json(new { code = 200, msg = "Banner created successfully" }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { code = 400, msg = "Invalid data" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
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
        public ActionResult Edit([Bind(Include = "id,name,meta,hide,order,datebegin")] Banner banner, HttpPostedFileBase img)
        {
            var path = "";
            var filename = "";
            Banner temp = getById(banner.id);

            if (ModelState.IsValid)
            {
                if (img != null)
                {
                    filename = DateTime.Now.ToString("dd-MM-yy-hh-mm-ss-") + img.FileName;  // Chỉ lấy phần tên file
                    path = Path.Combine(Server.MapPath("~/Content/images"), filename);
                    img.SaveAs(path);
                    temp.file_name = filename; // Cập nhật tên ảnh mới
                }
                temp.name = banner.name;
                temp.meta = banner.meta;
                temp.hide = banner.hide;
                temp.order = banner.order;

                db.Entry(temp).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { code = 200, msg = "Cập nhật thành công" }); // Trả về JSON để AJAX xử lý
            }

            return Json(new { code = 400, msg = "Cập nhật thất bại" });
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

        // POST: Admin/Banners/DeleteConfirmed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            var banner = db.Banners.Find(id);
            if (banner == null)
            {
                return Json(new { code = 404, msg = "Banner không tồn tại" });
            }

            try
            {
                db.Banners.Remove(banner);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Xóa banner thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Có lỗi xảy ra khi xóa banner: " + ex.Message });
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
