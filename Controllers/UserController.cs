using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using HoiTroWebsite.Models;
using HoiTroWebsite.UserModels;

namespace HoiTroWebsite.Controllers
{
    public class UserController : Controller
    {
        private readonly HoiTroEntities db = new HoiTroEntities();

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        // 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                Account user = new Account
                {
                    name = registerVM.Name,
                    phoneNum = registerVM.PhoneNumber,
                    password = registerVM.Password,
                    zaloNum = registerVM.PhoneNumber
                };
                db.Accounts.Add(user);
                db.SaveChanges();
                ViewBag.Message = "Đăng ký thành công";
            } 
            else 
            {
                ViewBag.Message = "Đăng ký không thành công";
            }
            return View();
        }

        // GET
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // POST
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
                    ViewBag.Message = "Đăng nhập thành công.";
                    return RedirectToAction("Index", "HomePage");
                }
                else
                {
                    ViewBag.Message = "Tài khoản hoặc mật khẩu không đúng.";
                }
            }
            return View();
        }

    }
}