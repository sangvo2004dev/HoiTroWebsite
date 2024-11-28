using System;
using System.Linq;
using System.Net;
using System.Threading;
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

        [Route("dang-ky", Name = "UserRegister")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                var temp = db.Accounts.Where(a => a.phoneNum == registerVM.PhoneNumber).FirstOrDefault();
                if (temp != null) // số điện thoại đã tồn tại trong database
                {
                    Response.StatusCode = (int)HttpStatusCode.Conflict;
                    return Json(new { statusCode = Response.StatusCode, message = "Số điện thoại đã được sử dụng!" });
                }
                try
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
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json(new { statusCode = Response.StatusCode , redirectUrl = Url.Action("Login", "User") });
                }
                catch (Exception ex)
                {
                    Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                    return Json(new { message = "Đã xảy ra lỗi vui lòng thử lại sau!", error = ex.ToString() });
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(new { message = "Đã xảy ra lỗi vui lòng thử lại sau!" });
            }
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

        [Route("dang-xuat-tai-khoan")]
        public ActionResult Logout()
        {
            Session["User"] = null;
            return RedirectToAction("Index", "HomePage");
        }

        [CustomAuthenticationFilter]
        [HttpGet]
        [Route("quan-ly/dang-tin-moi")]
        public ActionResult PostRoom()
        {
            ViewBag.loai_chuyen_muc = new SelectList(db.RoomTypes.OrderBy(r => r.order), "id", "title");
            return View();
        }

        [CustomAuthenticationFilter]
        [Route("quan-ly/sua-bai-dang", Name = "editPost")]
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

        [CustomAuthenticationFilter]
        [Route("quan-ly/tin-dang")]
        [HttpGet]
        public ActionResult ManagePostRooms()
        {
            int userID = (Session["User"] as Account).id;
            var listRoom = db.RoomInfoes.Where(ri => ri.accountId == userID).ToList();
            ViewBag.tat_ca_count = listRoom.Count;
            ViewBag.tin_an_count = listRoom.Where(ri => ri.hide == false).Count();
            ViewBag.duoc_duyet_count = listRoom.Where(ri => ri.isApproved == true).Count();         
            return View();
        }
    }
}