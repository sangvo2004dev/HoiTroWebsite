using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using HoiTroWebsite.Models;

namespace HoiTroWebsite.Areas.Admin.Controllers
{
    [AdminAuthenticationFilter]
    public class MenusController : Controller
    {
        private readonly HoiTroEntities db = new HoiTroEntities();

        // GET: Admin/Menus
        public ActionResult Index()
        {
            Response.StatusCode = 200;
            return View();
        }
        [HttpGet]
        public JsonResult getMenu()
        {
            try
            {
                var menus = db.Menus.ToList();
                foreach (var menu in menus)
                {
                    db.Entry(menu).Collection(i => i.SubMenus).Load();
                    menu.SubMenus.Where(s => s.hide == true).OrderBy(s => s.order).ToList();
                }
                var mn = (from t in menus
                              select new
                              {
                                  Id = t.id,
                                  Name = t.name,
                                  Link = t.link,
                                  HasSubMenu = t.hasSubMenu,
                                  Meta = t.meta,
                                  Hide = t.hide,
                                  Order = t.order,
                                  DateBegin = t.datebegin
                              }).ToList();
                return Json(new { code = 200, menu = mn, msg = "Lấy Menu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy Menu thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Admin/Menus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // GET: Admin/Menus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Menus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,link,meta,hide,order,datebegin,hasSubMenu")] Menu menu)
        {
            try
            {
            if (ModelState.IsValid)
            {
                    menu.datebegin = DateTime.Now.Date;
                db.Menus.Add(menu);
                db.SaveChanges();
                    return Json(new { code = 200, msg = "Menu created successfully" }, JsonRequestBehavior.AllowGet);
            }
                return Json(new { code = 400, msg = "Invalid data" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) 
            {
                return Json(new { code = 500, msg = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }


        // GET: Admin/Menus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Menu menu = db.Menus.Find(id);

            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }


        // POST: Admin/Menus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,link,meta,hide,order,datebegin,hasSubMenu")] Menu menu)
        {
            Menu temp = getById(menu.id);

            if (ModelState.IsValid && temp != null)
            {
                temp.name = menu.name;
                temp.link = menu.link;
                temp.meta = menu.meta;
                temp.hasSubMenu = menu.hasSubMenu;
                temp.hide = menu.hide;
                temp.order = menu.order;

                db.Entry(temp).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Cập nhật thành công" }); // Trả về JSON để AJAX xử lý
            }
            return Json(new { code = 400, msg = "Cập nhật thất bại" });
            }

        public Menu getById(long id)
        {
            return db.Menus.Where(x => x.id == id).FirstOrDefault();
        }

        // GET: Admin/Menus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // POST: Admin/Menus/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            var menu = db.Menus.Find(id);
            if (menu == null)
            {
                return Json(new { code = 404, msg = "Menu không tồn tại" });
            }

            try
            {
                db.Menus.Remove(menu);
                db.SaveChanges();
                    return Json(new { code = 200, msg = "Xóa Menu thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Có lỗi xảy ra khi xóa Menu: " + ex.Message });
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
