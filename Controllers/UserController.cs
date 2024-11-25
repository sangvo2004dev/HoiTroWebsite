using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using HoiTroWebsite.Models;
using HoiTroWebsite.Models2;
using Microsoft.Ajax.Utilities;

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

        [Route("dang-ky-tai-khoan")]
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [Route("dang-ky-a", Name = "UserLogout")]
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
            return RedirectToAction("Login", "User");
        }

        // GET
        [Route("dang-nhap-tai-khoan")]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // POST
        [Route("dang-nhap", Name = "UserLogin")]
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

        [Route("dang-xuat-tai-khoan")]
        public ActionResult Logout()
        {
            Session["User"] = null;
            return RedirectToAction("Index", "HomePage");
        }

        [HttpGet]
        [Route("quan-ly/dang-tin-moi")]
        public ActionResult PostRoom()
        {
            ViewBag.loai_chuyen_muc = new SelectList(db.RoomTypes.OrderBy(r => r.order), "id", "title");
            return View();
        }        

        [Route("quan-ly/sua-bai-dang/{id}", Name = "editPost")]
        [HttpGet]
        // edit bài đăng từ của user
        public ActionResult EditPostRoom(int? id) // id bài post
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            RoomInfo room = db.RoomInfoes.SingleOrDefault(r => r.id == id);
            db.Entry(room).Collection(r => r.RoomImgs).Load();

            // sắp xếp lại roomImgs theo order
            room.RoomImgs = room.RoomImgs.OrderBy(i => i.order).ToList();

            PostRoomVM postRoomVM = new PostRoomVM()
            {
                dia_chi = room.location,
                tieu_de = room.title,
                tieu_de_meta = room.meta,
                noi_dung = room.detail_description,
                gia = room.price,
                dien_tich = Convert.ToInt32(room.area),
                loai_chuyen_muc = room.roomTypeId,
                doi_tuong = room.tenant,
            };

            ViewBag.loai_chuyen_muc = new SelectList(db.RoomTypes.OrderBy(r => r.order), "id", "title", postRoomVM.loai_chuyen_muc);
            TempData["id"] = room.id;
            ViewBag.RoomImgs = room.RoomImgs;

            return View(postRoomVM);
        }


        // cho user cập nhật thông tin cá nhân
        [Route("quan-ly/cap-nhat-thong-tin-tai-khoan")]
        [HttpGet]
        public ActionResult UpdateProfile()
        {
            if (Session["User"] == null)
            {
                Session["User"] = db.Accounts.SingleOrDefault(a => a.id == 11);
            }
            var account = Session["User"] as Account;

            ViewBag.id = account.id;
            return View(account);
        }

        [Route("quan-ly/tin-dang")]
        [HttpGet]
        public ActionResult ManagePostRooms()
        {
            ViewBag.tat_ca_count = 10;
            ViewBag.tin_an_count = 0;
            ViewBag.duoc_duyet_count = 2;
            return View();
        }
    }
}