using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Helpers;
using System.Web.Mvc;
using HoiTroWebsite.Models;
using HoiTroWebsite.Models2;
using Microsoft.Ajax.Utilities;
//dùng MailHelper
using Common;
using System.Web;
using System.IO;
using HoiTroWebsite.HTLibraries;

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
            db.Entry(room).Collection(r => r.RoomImages).Load();
            int dienTich = 0;
            int.TryParse(room.acreage, out dienTich);

            // sắp xếp lại RoomImages theo order
            room.RoomImages = room.RoomImages.OrderBy(i => i.order).ToList();

            PostRoomVM postRoomVM = new PostRoomVM()
            {
                id = room.id,
                dia_chi = room.location,
                tieu_de = room.title,
                tieu_de_meta = room.meta,
                noi_dung = room.detail_description,
                gia = room.price,
                dien_tich = dienTich,
                loai_chuyen_muc = room.roomTypeId,
                doi_tuong = room.tenant,
            };

            ViewBag.loai_chuyen_muc = new SelectList(db.RoomTypes.OrderBy(r => r.order), "id", "title", postRoomVM.loai_chuyen_muc);

            TempData["id"] = room.id;
            ViewBag.RoomImages = room.RoomImages;
            ViewBag.ten_lien_he = room.Account.name;
            ViewBag.phone = room.Account.phoneNum;

            return View(postRoomVM);
        }


        // cho user cập nhật thông tin cá nhân
        [CustomAuthenticationFilter]
        [Route("quan-ly/cap-nhat-thong-tin-tai-khoan")]
        [HttpGet]
        public ActionResult UpdateProfile()
        {
            var account = Session["User"] as Account;
            ProfileVM profileVM = new ProfileVM
            {
                Ten_lien_he = account.name,
                Facebook_link = account.FBlink,
                Email = account.email,
                So_dien_thoai = account.phoneNum,
            };
            ViewBag.Account = account;
            return View(profileVM);
        }

        [CustomAuthenticationFilter]
        [Route("quan-ly/cap-nhat-thong-tin")]
        [HttpPost]
        public ActionResult UpdatePersonalInfo(ProfileVM model, HttpPostedFileBase avatar)
        {
            var account = Session["User"] as Account;
            string filename = "";
            string path = "";

            var acc = (from t in db.Accounts where t.id == account.id select t).FirstOrDefault();
            acc.name = model.Ten_lien_he;
            acc.FBlink = model.Facebook_link;
            acc.email = model.Email;
            if (avatar != null)
            {

                HandleUrlFile.SaveFile(avatar, "/Data/avatar/", acc);
            }
            try
            {
                                  // Cập nhật lại thông tin session
                                  // Cập nhật thông tin tài khoản
                //account.name = model.Ten_lien_he;
                //account.FBlink = model.Facebook_link;
                //account.email = model.Email;
                db.SaveChanges(); // Gọi SaveChanges để lưu thay đổi
                Session["User"] = acc;


                return Json(new { success = true, message = "Cập nhật thông tin thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi khi lưu vào cơ sở dữ liệu: " + ex.Message });
            }
        }

        [CustomAuthenticationFilter]
        [Route("quan-ly/thay-doi-so-dien-thoai")]
        [HttpPost]
        public ActionResult UpdatePhoneNumber(string oldNumberPhone, string newNumberPhone, string maXacThuc)
        {
            var account = Session["User"] as Account;

            if (account.phoneNum != oldNumberPhone)
            {
                return Json(new { success = false, message = "Số điện thoại cũ không đúng!", oldNumberPhone, account.phoneNum });
            }

            // Kiểm tra mã xác thực
            string sessionVerificationCode = Session["VerificationCode"] as string;
            if (string.IsNullOrEmpty(sessionVerificationCode) || maXacThuc != sessionVerificationCode)
            {
                return Json(new { success = false, message = "Mã xác thực không đúng!" });
            }
            // Cập nhật số điện thoại

            var acc = (from t in db.Accounts where t.id == account.id select t).FirstOrDefault();
            acc.phoneNum = newNumberPhone;
            try
            {
                db.SaveChanges(); // Gọi SaveChanges để lưu thay đổi
                                  // Cập nhật lại thông tin session
                account.phoneNum = newNumberPhone;
                Session["User"] = account;

                return Json(new { success = true, message = "Cập nhật số điện thoại thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi khi lưu vào cơ sở dữ liệu: " + ex.Message });
            }
        }
        [CustomAuthenticationFilter]
        [Route("quan-ly/thay-doi-so-dien-thoai/laysodemaxacthuc")]
        [HttpPost]
        public ActionResult GetVerificationCode(string oldNumberPhone, string newNumberPhone)
        {
            var account = Session["User"] as Account;

            // Kiểm tra nếu số điện thoại cũ đúng
            if (account.phoneNum != oldNumberPhone)
            {
                return Json(new { success = false, message = "Số điện thoại cũ không đúng!", oldNumberPhone, account.phoneNum });
            }

            // Tạo mã xác thực ngẫu nhiên 6 chữ số
            string verificationCode = GenerateVerificationCode();
            Session["VerificationCode"] = verificationCode;

            // Gửi mã xác thực qua email 

            if (account.email != null)
            {
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Areas/Admin/Content/assets/template/Authenticate.html"));
                content = content.Replace("{{maxacthuc}}", verificationCode);
                new MailHelper().SendMail(account.email, "Hỏi Trọ Website thông báo đến người dùng", content);
            }

            return Json(new { success = true, message = "Mã xác thực đã được gửi!" });
        }

        public string GenerateVerificationCode()
        {
            Random random = new Random();
            int verificationCode = random.Next(100000, 999999); // Tạo số ngẫu nhiên 6 chữ số
            return verificationCode.ToString();
        }

        [CustomAuthenticationFilter]
        [Route("quan-ly/thay-doi-mat-khau")]
        [HttpPost]
        public ActionResult ChangePassword(string oldPassword, string newPassword)
        {
            var account = Session["User"] as Account;

            // Kiểm tra mật khẩu cũ
            if (account.password != oldPassword) 
            {
                return Json(new { success = false, message = "Mật khẩu cũ không đúng!" });
            }

            // Lưu thông tin vào database
            var acc = (from t in db.Accounts where t.id == account.id select t).FirstOrDefault();
            acc.password = newPassword;
            try
            {
                db.SaveChanges(); // Gọi SaveChanges để lưu thay đổi
                                  // Cập nhật lại thông tin session
                account.password = newPassword;
                Session["User"] = account;

                return Json(new { success = true, message = "Đổi mật khẩu thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi khi lưu vào cơ sở dữ liệu: " + ex.Message });
            }
        }
        [CustomAuthenticationFilter]
        [Route("quan-ly/quen-mat-khau/gui-lai-mat-khau")]
        [HttpPost]
        public ActionResult SendPassword()
        {
            var account = Session["User"] as Account;

            // Gửi mã OTP qua email
            string content = System.IO.File.ReadAllText(Server.MapPath("~/Areas/Admin/Content/assets/template/SendPass.html"));
            content = content.Replace("{{password}}", account.password);
            try
            {
                new MailHelper().SendMail(account.email, "Khôi Phục Mật Khẩu", content);
                return Json(new { success = true, message = "Mật khẩu đã được gửi đến email của bạn." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi khi gửi email: " + ex.Message });
            }
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
            return View(listRoom);
        }
    }
}