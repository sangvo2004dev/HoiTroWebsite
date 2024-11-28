using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using HoiTroWebsite.Models;

namespace HoiTroWebsite.Areas.Admin.Controllers
{
    public class SubMenusController : Controller
    {
        private HoiTroEntities db = new HoiTroEntities();

        // GET: Admin/SubMenus
        public ActionResult Index(long? id = null)
        {
            var subMenus = db.SubMenus.Include(s => s.Menu);
            getMenu();
            Response.StatusCode = 200;
            return View();
        }

        public void getMenu(long? selectedId = null)
        {
            ViewBag.Menu = new SelectList(db.Menus.Where(x => x.hide == true).OrderBy(x => x.order), "id", "name", selectedId);
        }

        // GET: Admin/SubMenus/Details/5
        public ActionResult Details(int? id)
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
            return View(subMenu);
        }
        [HttpGet]

        public JsonResult getSubMenu(int? id)
        {
            try
            {
                if (id == null)
                {
                    var submenu1 = (from t in db.SubMenus.OrderBy(x => x.order)
                                   select new
                                   {
                                       Id = t.id,
                                       Name = t.name,
                                       Link = t.link,
                                       Meta = t.meta,
                                       Hide = t.hide,
                                       Order = t.order,
                                       DateBegin = t.datebegin,
                                       Menu = t.Menu.name,
                                   }).ToList();
                    return Json(new { code = 200, submenu = submenu1, msg = "Lấy SubMenu thành công" }, JsonRequestBehavior.AllowGet);
                }
                    var submenu2 = (from t in db.SubMenus.Where(x => x.menuId == id).OrderBy(x => x.order)
                               select new
                               {
                                   Id = t.id,
                                   Name = t.name,
                                   Link = t.link,
                                   Meta = t.meta,
                                   Hide = t.hide,
                                   Order = t.order,
                                   DateBegin = t.datebegin,
                                   Menu = t.Menu.name,
                               }).ToList();
                return Json(new { code = 200, submenu = submenu2, msg = "Lấy SubMenu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy SubMenu thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        // GET: Admin/SubMenus/Create
        public ActionResult Create()
        {
            ViewBag.menuId = new SelectList(db.Menus, "id", "name");
            return View();
        }
        public int getMaxOrder(long menuId)
        {
            return db.SubMenus.Where(x => x.menuId == menuId).Count();
        }

        // POST: Admin/SubMenus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,link,meta,hide,order,datebegin,menuId")] SubMenu subMenu)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    subMenu.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    subMenu.order = getMaxOrder((long)subMenu.menuId);
                    db.SubMenus.Add(subMenu);
                    db.SaveChanges();
                    return Json(new { code = 200, msg = "Footer created successfully" }, JsonRequestBehavior.AllowGet);
                }

                ViewBag.menuId = new SelectList(db.Menus, "id", "name", subMenu.menuId);
                return Json(new { code = 400, msg = "Invalid data" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
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
        public ActionResult Edit([Bind(Include = "id,name,link,meta,hide,order,datebegin,menuId")] SubMenu subMenu)
        {
            SubMenu temp = getById(subMenu.id);
            if (temp == null)
            {
                return Json(new { code = 404, msg = "Không tìm thấy SubMenu" });
            }

            if (ModelState.IsValid)
            {
                temp.name = subMenu.name;
                temp.link = subMenu.link;
                temp.meta = subMenu.meta;
                temp.hide = subMenu.hide;
                temp.order = subMenu.order;
                temp.datebegin = subMenu.datebegin;
                temp.menuId = subMenu.menuId;

                db.Entry(temp).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Cập nhật thành công" });
            }

            ViewBag.menuId = new SelectList(db.Menus, "id", "name", subMenu.menuId);
            return Json(new { code = 400, msg = "Dữ liệu không hợp lệ" });
        }

        public SubMenu getById(long id)
        {
            return db.SubMenus.FirstOrDefault(x => x.id == id);
        }

        // GET: Admin/SubMenus/Delete/5
        public ActionResult Delete(int? id)
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
            return View(subMenu);
        }

        // POST: Admin/SubMenus/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            var submenu = db.SubMenus.Find(id);
            if (submenu == null)
            {
                return Json(new { code = 404, msg = "SubMenu không tồn tại" });
            }

            try
            {
                db.SubMenus.Remove(submenu);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Xóa SubMenu thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Có lỗi xảy ra khi xóa SubMenu: " + ex.Message });
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
