using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HoiTroWebsite.HTLibraries;
using HoiTroWebsite.Models;

namespace HoiTroWebsite.Areas.Admin.Controllers
{
    [AdminAuthenticationFilter]
    public class AccountsController : Controller
    {
        private HoiTroEntities db = new HoiTroEntities();

        // GET: Admin/Accounts
        public ActionResult Index()
        {
            Response.StatusCode = 200;
            return View();
        }
        [HttpGet]
        public JsonResult getAccount()
        {
            try
            {
                var account = (from t in db.Accounts
                              select new
                              {
                                  Id = t.id,
                                  Name = t.name,
                                  Img = t.imagePath ?? "/Content/images/default-user.jpg",
                                  PhoneNum = t.phoneNum,
                                  Meta = t.meta,
                                  Hide = t.hide,
                                  Order = t.order,
                                  DateBegin = t.datebegin
                              }).ToList();
                return Json(new { code = 200, account = account, msg = "Lấy Account thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy Account thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        // GET: Admin/Accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: Admin/Accounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "id,name,phoneNum,email,zaloNum,FBlink,file_name,password,meta,hide,order,datebegin")] Account account, HttpPostedFileBase img)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Response.StatusCode = 400;
                    return View(); // Trả lại View cùng với lỗi xác thực
                }
                
                account.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                db.Accounts.Add(account);
                if (img != null)
                {
                    HandleUrlFile.SaveFile(img, "/Data/avatar/", account);
                }

                db.SaveChanges();
                return Json(new { code = 200, msg = "Account created successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public Account getById(long id)
        {
            return db.Accounts.Where(x => x.id == id).FirstOrDefault();
        }

        // GET: Admin/Accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        private void resetPassword(int? id)
        {
            if (!id.HasValue)
            {
                // Kiểm tra nếu id là null
                throw new ArgumentNullException(nameof(id), "ID không hợp lệ.");
            }

            // Truy vấn tài khoản có id khớp với id truyền vào
            var client = (from t in db.Accounts
                         where t.id == id
                         select t).FirstOrDefault();
            client.password = client.phoneNum;
        }

        // POST: Admin/Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include = "id,name,phoneNum,email,zaloNum,FBlink,file_name,password,resetPass,meta,hide,order,datebegin")] Account account, HttpPostedFileBase img)
        {
            var path = "";
            var filename = "";
            Account temp = getById(account.id);
            if (ModelState.IsValid)
            {
                if (img != null)
                {
                    HandleUrlFile.SaveFile(img, "/Data/avatar/", account);
                }

                temp.name = account.name;
                temp.phoneNum = account.phoneNum;
                temp.email = account.email;
                temp.zaloNum = account.zaloNum;
                temp.FBlink = account.FBlink;
                temp.meta = account.meta;
                temp.hide = account.hide;
                temp.order = account.order;

                if (account.resetPass)
                {
                    resetPassword(account.id);
                }

                db.Entry(temp).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Cập nhật thành công" });
            }
            return Json(new { code = 400, msg = "Cập nhật thất bại" });
        }

        // GET: Admin/Accounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Admin/Accounts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            var account = db.Accounts.Find(id);
            if (account == null)
            {
                return Json(new { code = 404, msg = "Account không tồn tại" });
            }

            try
            {
                db.Accounts.Remove(account);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Xóa account thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Có lỗi xảy ra khi xóa account: " + ex.Message });
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
