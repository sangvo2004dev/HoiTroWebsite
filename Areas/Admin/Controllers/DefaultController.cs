using HoiTroWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HoiTroWebsite.Areas.Admin.Controllers
{
    public class DefaultController : Controller
    {
        private HoiTroEntities db = new HoiTroEntities();
        // GET: Admin/Default
        public ActionResult AdminIndex()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public JsonResult Login(string Email, string Password)
        {
            if (ModelState.IsValid)
            {
                var user = db.AdminAccounts.SingleOrDefault(a => a.email == Email && a.password == Password);
                if (user != null)
                {
                    Session["Admin"] = user; // Lưu thông tin admin vào Session
                    return Json(new {statusCode = 200,message = "Đăng nhập thành công!",redirectUrl = Url.Action("AdminIndex", "Default")});
                }
                else
                {
                    return Json(new {statusCode = 417,message = "Tài khoản hoặc mật khẩu không đúng!"});
                }
            }
            else
            {
                return Json(new {statusCode = 500,message = "Đã xảy ra lỗi, vui lòng thử lại sau!"});
            }
        }
        [Route("dang-xuat-admin")]
        public ActionResult Logout()
        {
            Session["Admin"] = null;
            return Redirect("/Admin/Default/Login");
        }

    }
}