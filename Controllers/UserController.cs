﻿using System;
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
        public ActionResult PostRoom(PostRoomVM postRoomVM, List<HttpPostedFileBase> imgFiles)
        {
            if (ModelState.IsValid)
            {
                RoomInfo roomInfo = new RoomInfo
                {
                    title = postRoomVM.tieu_de,
                    brief_description = "a",
                    price = "a",
                    detail_description = "a",
                    hide = false
                };
                try
                {
                    db.RoomInfoes.Add(roomInfo);
                    db.SaveChanges();
                    SaveFileToServer(imgFiles, roomInfo);
                }
                catch (Exception e)
                {
                    ViewBag.Message = e.ToString();
                }

            }

            return View();
        }

        // edit bài đăng từ trang user
        // GET 
        [HttpGet]
        public ActionResult EditPostRoom(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var room = db.RoomInfoes.SingleOrDefault(r => r.id == id);
            db.Entry(room).Collection(r => r.RoomImgs).Load();
            // sắp xếp lại roomImgs theo order
            room.RoomImgs = room.RoomImgs.OrderBy(i => i.order).ToList();
            return View(room);
        }

        [HttpPost]
        public ActionResult EditPostRoom(string titlea, List<HttpPostedFileBase> imgFiles, HttpPostedFileBase testa)
        {
            SaveFileToServer(imgFiles);
            return View();
        }

        // lưu file lên thư mục ở server
        private bool SaveFileToServer(List<HttpPostedFileBase> imgFiles, RoomInfo roomInfo = null)
        {
            try
            {
                // các biến temp
                //var roomInfo.meta = "lam-toi-chet";

                //int roomId = roomInfo.id; // lấy id của post vừa mới đăng
                int roomId = 5;
                //long userId = (Session["User"] as Account).id
                int userId = 10;
                int count = 0;
                foreach (var file in imgFiles)
                {
                    var severFolder = "/Data/";
                    var folder = string.Concat("user/", userId, "/");
                    var fileName = string.Join("_", roomId, roomInfo.meta, count) + Path.GetExtension(file.FileName);
                    count++;
                    var path = Path.Combine(Server.MapPath(severFolder + folder), fileName);
                    file.SaveAs(path);

                    // thêm đường dẫn file vào database
                    db.RoomImgs.Add(new RoomImg
                    {
                        postRoomId = roomId,
                        serverFolder = severFolder,
                        folder = folder,
                        fileName = fileName,
                        hide = true,
                        order = count                       
                    });
                }
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}