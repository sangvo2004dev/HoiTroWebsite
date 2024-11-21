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
    public class FootersController : Controller
    {
        private HoiTroEntities db = new HoiTroEntities();

        // GET: Admin/Footers
        public ActionResult Index()
        {
            Response.StatusCode = 200;
            return View();
        }
        [HttpGet]
        public JsonResult getFooter()
        {
            try
            {
                var footer = (from t in db.Footers
                              select new
                              {
                                  Id = t.id,
                                  Name = t.title,
                                  Address = t.contactAddress,
                                  Email = t.contactEmail,
                                  Phone = t.contactPhone,
                                  Meta = t.meta,
                                  Hide = t.hide,
                                  Order = t.order,
                                  DateBegin = t.datebegin
                              }).ToList();
                return Json(new { code = 200, footer = footer, msg = "Lấy Footer thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy Footer thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

            // GET: Admin/Footers/Details/5
            public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Footer footer = db.Footers.Find(id);
            if (footer == null)
            {
                return HttpNotFound();
            }
            return View(footer);
        }

        // GET: Admin/Footers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Footers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "id,title,companyInfo,contactAddress,contactEmail,contactPhone,socialFacebook,socialTwitter,socialLinkedIn,socialYoutube,copyrightInfo,privacyPolicyLink,termsOfServiceLink,TermsOfUseLink,TermsOfPostingLink,meta,hide,order,datebegin")] Footer footer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    footer.datebegin = DateTime.Now.Date;
                    db.Footers.Add(footer);
                    db.SaveChanges();
                    return Json(new { code = 200, msg = "Footer created successfully" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { code = 400, msg = "Invalid data" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) 
            {
                return Json(new { code = 500, msg = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Admin/Footers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Footer footer = db.Footers.Find(id);
            if (footer == null)
            {
                return HttpNotFound();
            }
            return View(footer);
        }

        // POST: Admin/Footers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "id,title,companyInfo,contactAddress,contactEmail,contactPhone,socialFacebook,socialTwitter,socialLinkedIn,socialYoutube,copyrightInfo,privacyPolicyLink,termsOfServiceLink,TermsOfUseLink,TermsOfPostingLink,meta,hide,order,datebegin")] Footer footer)
        {
            Footer temp = getById(footer.id);
            if (ModelState.IsValid)
            {
                temp.title = footer.title;
                temp.companyInfo = footer.companyInfo;
                temp.contactAddress = footer.contactAddress;
                temp.contactEmail = footer.contactEmail;
                temp.contactPhone = footer.contactPhone;
                temp.socialFacebook = footer.socialFacebook;
                temp.socialTwitter = footer.socialTwitter;
                temp.socialLinkedIn = footer.socialLinkedIn;
                temp.socialYoutube = footer.socialYoutube;
                temp.copyrightInfo = footer.copyrightInfo;
                temp.privacyPolicyLink = footer.privacyPolicyLink;
                temp.termsOfServiceLink = footer.termsOfServiceLink;
                temp.TermsOfUseLink = footer.TermsOfUseLink;
                temp.TermsOfPostingLink = footer.TermsOfPostingLink;
                temp.meta = footer.meta;
                temp.hide = footer.hide;
                temp.order = footer.order;

                db.Entry(temp).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Cập nhật thành công" }); // Trả về JSON để AJAX xử lý
            }
            return Json(new { code = 400, msg = "Cập nhật thất bại" });
        }

        public Footer getById(long id)
        {
            return db.Footers.Where(x => x.id == id).FirstOrDefault();
        }

        // GET: Admin/Footers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Footer footer = db.Footers.Find(id);
            if (footer == null)
            {
                return HttpNotFound();
            }
            return View(footer);
        }

        // POST: Admin/Footers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            var footer = db.Footers.Find(id);
            if (footer == null)
            {
                return Json(new { code = 404, msg = "Footer không tồn tại" });
            }

            try
            {
                db.Footers.Remove(footer);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Xóa Footer thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Có lỗi xảy ra khi xóa Footer: " + ex.Message });
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
