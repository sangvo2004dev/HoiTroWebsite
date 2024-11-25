using HoiTroWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace HoiTroWebsite.Controllers
{
    public class AccountController : Controller
    {
        HoiTroEntities _db = new HoiTroEntities();

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _db.Accounts.SingleOrDefault(a => a.phoneNum == model.PhoneNum && a.password == model.Password);
                if (user != null)
                {
                    Session.Add("User", user);
                    ViewBag.Message = "Đăng nhập thành công.";
                    return RedirectToAction("Index", "HomePage");
                }
                else
                {
                    ViewBag.Message = "Tài khoản hoặc mật khẩu không đúng.";
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }
    }
}