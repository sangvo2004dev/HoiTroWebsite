using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
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

        [HttpGet]
        public ActionResult PostRoom()
        {
            ViewBag.loai_chuyen_muc = new SelectList(db.RoomTypes.OrderBy(r => r.order), "id", "title");
            return View();
        }

        [HttpPost]
        public ActionResult PostRoom(PostRoomVM postRoomVM, List<string> file_name_list)
        {
            if (ModelState.IsValid)
            {
                RoomInfo roomInfo = new RoomInfo
                {
                    title = postRoomVM.tieu_de,
                    roomTypeId = postRoomVM.loai_chuyen_muc,
                    brief_description = "a",
                    price = postRoomVM.gia,
                    location = postRoomVM.dia_chi,
                    detail_description = postRoomVM.noi_dung,
                    datebegin = DateTime.Now,
                    hide = false
                };
                try
                {
                    db.RoomInfoes.Add(roomInfo);
                    db.SaveChanges();
                    SaveFileUrl(file_name_list, roomInfo.id);
                }
                catch (Exception e)
                {
                    ViewBag.Message = e.ToString();
                }

                return RedirectToAction("Index", "HomePage");
            }

            return View();
        }

        // GET 
        [HttpGet]
        // edit bài đăng từ của user
        public ActionResult EditPostRoom(int? id)
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
                noi_dung = room.detail_description,
                gia = room.price,
                dien_tich = Convert.ToInt32(room.area),
                loai_chuyen_muc = room.roomTypeId,
            };

            ViewBag.loai_chuyen_muc = new SelectList(db.RoomTypes.OrderBy(r => r.order), "id", "title", postRoomVM.loai_chuyen_muc);

            return View(postRoomVM);
        }

        [HttpPost]
        public ActionResult EditPostRoom(string titlea, List<string> file_name_list)
        {
            SaveFileUrl(file_name_list, 0);
            return View();
        }

        // lưu file lên thư mục ở server
        private void SaveFileUrl(List<string> file_name_list, int roomInfoId)
        {
            int count = 0;
            foreach (var file_name in file_name_list)
            {
                // thêm đường dẫn file vào database
                db.RoomImgs.Add(new RoomImg
                {
                    postRoomId = roomInfoId,
                    folder = "/Data/user",
                    fileName = file_name,
                    hide = true,
                    order = count
                });
                count++;
            }
            db.SaveChanges();
        }
    }
}