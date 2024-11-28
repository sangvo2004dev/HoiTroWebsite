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
    public class MentorsController : Controller
    {
        private HoiTroEntities db = new HoiTroEntities();

        // GET: Admin/Mentors
        public ActionResult Index()
        {
            Response.StatusCode = 200;
            return View();
        }
        [HttpGet]
        public JsonResult getMentor()
        {
            try
            {
                var mentor = (from t in db.Mentors
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
                return Json(new { code = 200, mentor, msg = "Lấy mentor thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy mentor thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Admin/Mentors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mentor mentor = db.Mentors.Find(id);
            if (mentor == null)
            {
                return HttpNotFound();
            }
            return View(mentor);
        }

        // GET: Admin/Mentors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Mentors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "id,name,file_name,email,phoneNum,FBlink,zaloNum,supportTask,meta,hide,order,datebegin")] Mentor mentor, HttpPostedFileBase img)
        {

                try
                {
                    var path = "";
                    var filename = "";
                    if (ModelState.IsValid)
                    {
                        if (img != null)
                        {
                            filename = img.FileName;  // Chỉ lấy phần tên file
                            path = Path.Combine(Server.MapPath("~/Content/images"), filename);
                            img.SaveAs(path);
                            mentor.file_name = filename; //Lưu ý
                        }
                        else
                        {
                            mentor.file_name = "default-user.jpg";
                        }
                        mentor.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                        db.Mentors.Add(mentor);
                        db.SaveChanges();
                        return Json(new { code = 200, msg = "Banner created successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { code = 400, msg = "Invalid data" }, JsonRequestBehavior.AllowGet);
                }
                catch(Exception ex)
                {
                    return Json(new { code = 500, msg = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }

        // GET: Admin/Mentors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mentor mentor = db.Mentors.Find(id);
            if (mentor == null)
            {
                return HttpNotFound();
            }
            return View(mentor);
        }

        // POST: Admin/Mentors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "id,name,file_name,email,phoneNum,FBlink,zaloNum,supportTask,meta,hide,order,datebegin")] Mentor mentor, HttpPostedFileBase img)
        {
            var path = "";
            var filename = "";
            Mentor temp = getById(mentor.id);

            if (ModelState.IsValid)
            {
                if (img != null)
                {
                    filename = DateTime.Now.ToString("dd-MM-yy-hh-mm-ss-") + img.FileName;
                    path = Path.Combine(Server.MapPath("~/Content/images"), filename);
                    img.SaveAs(path);
                    temp.file_name = filename; // Lưu ý
                }
                temp.name = mentor.name;
                temp.email = mentor.email;
                temp.phoneNum = mentor.phoneNum;
                temp.FBlink = mentor.FBlink;
                temp.zaloNum = mentor.zaloNum;
                temp.supportTask = mentor.supportTask;
                temp.meta = mentor.meta;
                temp.hide = mentor.hide;
                temp.order = mentor.order;
                
                db.Entry(temp).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Mentor created successfully" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { code = 400, msg = "Invalid data" }, JsonRequestBehavior.AllowGet);
        }
        public Mentor getById(long id)
        {
            return db.Mentors.Where(x => x.id == id).FirstOrDefault();
        }

        // GET: Admin/Mentors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mentor mentor = db.Mentors.Find(id);
            if (mentor == null)
            {
                return HttpNotFound();
            }
            return View(mentor);
        }

        // POST: Admin/Mentors/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            var mentor = db.Mentors.Find(id);
            if (mentor == null)
            {
                return Json(new { code = 404, msg = "Mentor không tồn tại" });
            }

            try
            {
                db.Mentors.Remove(mentor);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Xóa Mentor thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Có lỗi xảy ra khi xóa Mentor: " + ex.Message });
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
