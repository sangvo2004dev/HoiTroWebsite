using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using HoiTroWebsite.Models;
using HoiTroWebsite.UserModels;
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
        public ActionResult PostRoom(PostRoomVM postRoomVM, string[] file_name_list)
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
        //[Route("quan-ly/sua-bai-dang/{id}")]
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
                noi_dung = room.detail_description,
                gia = room.price,
                dien_tich = Convert.ToInt32(room.area),
                loai_chuyen_muc = room.roomTypeId,
            };

            ViewBag.loai_chuyen_muc = new SelectList(db.RoomTypes.OrderBy(r => r.order), "id", "title", postRoomVM.loai_chuyen_muc);
            TempData["id"] = room.id;
            ViewBag.RoomImgs = room.RoomImgs;

            return View(postRoomVM);
        }

        [HttpPost]
        public ActionResult EditPostRoom(PostRoomVM postRoomVM, string[] file_name_list, string[] file_delete_list)
        {
            int id = int.Parse((TempData["id"]).ToString());
            var room = db.RoomInfoes.SingleOrDefault(r => r.id == id);

            if (room != null)
            {
                room.title = postRoomVM.tieu_de;
                room.brief_description = "a";
                room.price = postRoomVM.gia;
                room.area = postRoomVM.dien_tich.ToString();
                room.location = postRoomVM.dia_chi;
                room.detail_description = postRoomVM.noi_dung;
                room.datebegin = DateTime.Now;
                db.SaveChanges();
            }

            SaveFileUrl(file_name_list, id);
            DeleteFileUrl(file_delete_list, id);

            return RedirectToAction("Index", "HomePage");
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
            return View();
        }

        // lưu tên file và đường dẫn lên database
        private void SaveFileUrl(string[] file_name_list, int roomInfoId)
        {
            if (file_name_list == null) { return; }
            int count = 0;
            foreach (var file_name in file_name_list)
            {
                var f = db.RoomImgs.SingleOrDefault(ri => ri.fileName == file_name);
                if (f == null)
                {
                    // thêm đường dẫn file vào database
                    db.RoomImgs.Add(new RoomImg
                    {
                        postRoomId = roomInfoId,
                        folder = "/Data/user/",
                        fileName = file_name,
                        hide = true,
                        order = count,
                        datebegin = DateTime.Now,
                    });
                }
                else
                {
                    f.order = count;
                }
                count++;
            }
            db.SaveChanges();
        }

        // xóa đường dẫn file trên database
        private void DeleteFileUrl(string[] file_delete_list, int roomInfoId)
        {
            if (file_delete_list == null) { return; }

            file_delete_list.ForEach(f =>
            {
                var roomImg = db.RoomImgs.SingleOrDefault(ri => ri.postRoomId == roomInfoId && ri.fileName == f);
                if (roomImg != null)
                {
                    db.RoomImgs.Remove(roomImg);
                }
            });
            db.SaveChanges();
        }
    }
}