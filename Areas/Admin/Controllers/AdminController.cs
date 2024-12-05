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
using HoiTroWebsite.Models2;

namespace HoiTroWebsite.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin/Admin
        private HoiTroEntities db = new HoiTroEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var user = db.Accounts.SingleOrDefault(a => a.phoneNum == loginVM.phoneNumber && a.password == loginVM.password);
                if (user != null)
                {
                    Session.Add("User", user);
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json(new { statusCode = Response.StatusCode, message = "Đăng nhập thành công", redirectUrl = Url.Action("Index", "HomePage") });
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                    return Json(new { statusCode = Response.StatusCode, message = "Tài khoản hoặc mật khẩu không đúng" });
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { message = "Đã xảy ra lỗi vui lòng thử lại sau!" });
            }
        }
    }
}