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
    public class MenusController : Controller
    {
        private HoiTroEntities db = new HoiTroEntities();

        // GET: Admin/Menus
        public ActionResult Index()
        {
            var menus = db.Menus.ToList();
            foreach (var menu in menus)
            {
                db.Entry(menu).Collection(i => i.SubMenus).Load();
                menu.SubMenus.Where(s => s.hide == true).OrderBy(s => s.order).ToList();
            }

            return View();
        }

        // GET: Admin/Menus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var menu = db.Menus.Include(m => m.SubMenus)
                               .FirstOrDefault(s => s.hide == true && s.id == id);
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
            if (ModelState.IsValid)
            {
                menu.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                db.Menus.Add(menu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(menu);
        }
        // Edit xóa menucon
        public JsonResult DeleteSubMenu(long id)
        {
            var subMenu = db.SubMenus.Find(id);
            if (subMenu != null)
            {
                db.SubMenus.Remove(subMenu);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        // GET: Admin/Menus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var menu = db.Menus.Include(m => m.SubMenus)
                               .FirstOrDefault(s => s.hide == true && s.id == id);

            if (menu == null)
            {
                return HttpNotFound();
            }

            //Response.StatusCode = 200; // 200
            //return Json( new { data = "test json"}, JsonRequestBehavior.AllowGet);
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

                // Thêm lại các SubMenus mới
                if (menu.SubMenus != null)
                {
                    foreach (var subMenu in menu.SubMenus)
                    {
                        // Thêm SubMenu vào temp và đặt trạng thái là Added
                        temp.SubMenus.Add(subMenu);
                        db.Entry(subMenu).State = EntityState.Added;
                    }
                }

                db.Entry(temp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.menuId = new SelectList(db.Menus, "id", "name", menu.id);
            return View(menu);
        }

        public Menu getById(long id)
        {
            var menu = db.Menus.Include(m => m.SubMenus) // Load SubMenus khi truy vấn menu
                .Where(m => m.id == id && m.hide == true)
                .FirstOrDefault();

            // Sắp xếp lại SubMenus
            if (menu != null)
            {
                menu.SubMenus = menu.SubMenus.Where(s => s.hide == true).OrderBy(s => s.order).ToList();
            }
            return menu;
        }

        // GET: Admin/Menus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var menu = db.Menus.Include(m => m.SubMenus)
                               .FirstOrDefault(s => s.hide == true && s.id == id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // POST: Admin/Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Menu menu = db.Menus.Find(id);
            db.Menus.Remove(menu);
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
