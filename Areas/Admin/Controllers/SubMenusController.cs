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
    public class SubMenusController : Controller
    {
        private HoiTroEntities db = new HoiTroEntities();


        // GET: Admin/SubMenus/Create
        public ActionResult Create(int? menuId)
        {
            ViewBag.menuId = new SelectList(db.Menus, "id", "name", menuId);
            return View(new SubMenu { menuId = menuId }); // Truyền menuId vào SubMenu khi render View
        }

        // POST: Admin/SubMenus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,link,meta,hide,order,datebegin,menuId")] SubMenu subMenu, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                db.SubMenus.Add(subMenu);
                db.SaveChanges();
                return Redirect(returnUrl);
            }

            ViewBag.menuId = new SelectList(db.Menus, "id", "name", subMenu.menuId);
            return View(subMenu);
        }

        // GET: Admin/SubMenus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) 
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubMenu subMenu = db.SubMenus.Find(id);
            if (subMenu == null)
            {
                return HttpNotFound();
            }
            ViewBag.menuId = new SelectList(db.Menus, "id", "name", subMenu.menuId);
            return View(subMenu);
        }

        // POST: Admin/SubMenus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,link,meta,hide,order,datebegin,menuId")] SubMenu subMenu, string returnUrl)
        {
            SubMenu temp = getById(subMenu.id);
            if (ModelState.IsValid)
            {
                temp.name = subMenu.name;
                temp.link = subMenu.link;
                temp.meta = subMenu.meta;
                temp.hide = subMenu.hide;
                temp.order = subMenu.order;

                db.Entry(temp).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect(returnUrl);
            }
            ViewBag.menuId = new SelectList(db.Menus, "id", "name", subMenu.menuId);
            return View(subMenu);
        }
        public SubMenu getById(long id)
        {
            return db.SubMenus.Where(x => x.id == id).FirstOrDefault();
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
