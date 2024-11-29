using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using HoiTroWebsite.Models;
//dùng MailHelper
using Common;
using HoiTroWebsite.Hubs;
using Microsoft.AspNet.SignalR;
using HoiTroWebsite.Models2;
using HoiTroWebsite.HTLibraries;

namespace HoiTroWebsite.Areas.Admin.Controllers
{
    public class RoomInfoesController : Controller
    {
        private readonly HoiTroEntities db = new HoiTroEntities();

        // GET: Admin/RoomInfoes
        public ActionResult Index(long? id = null)
        {
            var roomInfoes = db.RoomInfoes.Include(r => r.RoomType).Include(r => r.Account);
            Response.StatusCode = 200;
            getRoomType(id);
            
            return View();
        }

        public ActionResult CheckPendingPosts()
        {
            var pendingPosts = db.RoomInfoes.Where(r => r.isApproved == false).ToList(); // Ví dụ: Lọc bài chưa duyệt
            int count = pendingPosts.Count;

            if (count > 0)
            {
                var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                context.Clients.All.receiveNotification($"Có {count} bài đăng chưa được duyệt.");
            }

            return Json(new { code = 200, count = count }, JsonRequestBehavior.AllowGet);
        }

        public string getTitleRoomType(int? id)
        {
            if (!id.HasValue)
            {
                // Kiểm tra nếu id là null
                throw new ArgumentNullException(nameof(id), "ID không hợp lệ.");
            }
            // Truy vấn và lấy title của loại phòng có id khớp với id truyền vào
            var title = (from t in db.RoomTypes
                         where t.id == id
                         select t.title).FirstOrDefault();

            if (title == null)
            {
                // Kiểm tra nếu không tìm thấy title
                throw new InvalidOperationException($"Không tìm thấy title cho tài khoản có ID {id}");
            }

            return title; // Trả về title
        }
        public string GetEmail(int? id)
        {
            if (!id.HasValue)
            {
                // Kiểm tra nếu id là null
                throw new ArgumentNullException(nameof(id), "ID không hợp lệ.");
            }

            // Truy vấn và lấy email của tài khoản có id khớp với id truyền vào
            var email = (from t in db.Accounts
                         where t.id == id
                         select t.email).FirstOrDefault();
            return email; // Trả về email
        }
        public ActionResult CheckConfig()
        {
            Console.WriteLine("One");
            try
            {
                Console.WriteLine(getTitleRoomType(1))                ;
            }
            catch (Exception ex)
            {
                // Log lỗi hoặc hiển thị thông báo lỗi
                Console.WriteLine($"Lỗi: {ex.Message}");
            }
            return View();
        }
        public void TestMail(int? id)
        {
            string content = System.IO.File.ReadAllText(Server.MapPath("~/Areas/Admin/Content/assets/template/CreatePost.html"));
            content = content.Replace("{{Title}}", "Cho Thuê trọ Q10");
            content = content.Replace("{{BriefDescription}}", "Trọ rẻ");
            content = content.Replace("{{DetailDescription}}", "   szvdscsgvbdbdzbdfzvzd");
            content = content.Replace("{{RoomType}}", "Phòng trọ cho thuê");
            content = content.Replace("{{Tenant}}", "Tất cả");
            content = content.Replace("{{Price}}", "4");
            content = content.Replace("{{Acreage}}", "9.9");
            content = content.Replace("{{Area}}", "Q10, tpHCM");
            content = content.Replace("{{Location}}", "Hẻm 458/20 đg 3/2 phường 12 quận 10");
            content = content.Replace("{{Author}}", "Admin:Phước Phùng");// thêm Admin: Session.name

            Console.WriteLine(content);

            string email = GetEmail(id);
            new MailHelper().SendMail(email, "Thông báo đến người dùng", content);


        }
        public void getRoomType(long? selectedId = null)
        {
            ViewBag.RoomType = new SelectList(db.RoomTypes.Where(x => x.hide == true).OrderBy(x => x.order), "id", "title", selectedId);
        }
        [HttpGet]

        public JsonResult getRoom(int? id)
        {
            try
            {
                if (id == null)
                {
                    var room1 = (from t in db.RoomInfoes.OrderBy(x => x.order)
                                 select new
                                 {
                                     Id = t.id,
                                     Name = t.title,
                                     Price = t.price,
                                     Acreage = t.acreage,
                                     Address = t.location,
                                     Meta = t.meta,
                                     Hide = t.hide,
                                     Order = t.order,
                                     DateBegin = t.datebegin,
                                     RoomType = t.RoomType.title,
                                     Account = t.Account.name
                                 }).ToList();
                    return Json(new { code = 200, room = room1, msg = "Lấy Room thành công" }, JsonRequestBehavior.AllowGet);
                }
                var room2 = (from t in db.RoomInfoes.Where(x => x.roomTypeId == id).OrderBy(x => x.order)
                             select new
                             {
                                 Id = t.id,
                                 Name = t.title,
                                 Price = t.price,
                                 Acreage = t.acreage,
                                 Address = t.location,
                                 Meta = t.meta,
                                 Hide = t.hide,
                                 Order = t.order,
                                 DateBegin = t.datebegin,
                                 RoomType = t.RoomType.title
                             }).ToList();
                return Json(new { code = 200, room = room2, msg = "Lấy Room thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy Room thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Admin/RoomInfoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomInfo roomInfo = db.RoomInfoes.Find(id);
            if (roomInfo == null)
            {
                return HttpNotFound();
            }
            return View(roomInfo);
        }

        public int getMaxOrder(long roomTypeId)
        {
            return db.RoomInfoes.Where(x => x.roomTypeId == roomTypeId).Count();
        }
        // GET: Admin/RoomInfoes/Create
        public ActionResult Create()
        {
            ViewBag.roomTypeId = new SelectList(db.RoomTypes, "id", "title");
            return View();
        }

        // POST: Admin/RoomInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "id,title,detail_description,price,acreage,location,tenant,meta,hide,order,datebegin,roomTypeId,accountId")] RoomInfo roomInfo)
        {
            try
            {
                ViewBag.roomTypeId = new SelectList(db.RoomTypes, "id", "title", roomInfo.roomTypeId);
                if (!ModelState.IsValid)
                {
                    Response.StatusCode = 400;
                    return View();
                }
                roomInfo.datebegin = DateTime.Now.Date;
                roomInfo.order = getMaxOrder((long)roomInfo.roomTypeId);
                db.RoomInfoes.Add(roomInfo);
                db.SaveChanges();

                string email = GetEmail(roomInfo.accountId);
                if (email != null)
                {
                    //Gửi mail đến người dùng
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/Areas/Admin/Content/assets/template/CreatePost.html"));
                    // chọn loại Email gửi đến người dùng
                    // thêm thông tin phù hợp
                    content = content.Replace("{{Title}}", roomInfo.title);
                    content = content.Replace("{{DetailDescription}}", roomInfo.detail_description);
                    content = content.Replace("{{RoomType}}", getTitleRoomType(roomInfo.roomTypeId));
                    content = content.Replace("{{Tenant}}", roomInfo.tenant);
                    content = content.Replace("{{Price}}", roomInfo.price);
                    content = content.Replace("{{Acreage}}", (roomInfo.acreage).ToString());
                    content = content.Replace("{{Location}}", roomInfo.location);
                    content = content.Replace("{{Author}}", "....");// thêm Admin: Session.name or User: Account.name
                    new MailHelper().SendMail(email, "Hỏi Trọ Website thông báo đến người dùng", content);
                }
                return Json(new { code = 200, msg = "Room created successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult SangCreate()
        {
            ViewBag.loai_chuyen_muc = new SelectList(db.RoomTypes.OrderBy(r => r.order), "id", "title");
            PostRoomAdvandceVM postRoomVM = new PostRoomAdvandceVM
            {
                doi_tuong = "",
            };

            return View(postRoomVM);
        }

        [HttpPost]
        public ActionResult SangCreate(PostRoomAdvandceVM postRoomVM, string[] file_name_list)
        {
            if (ModelState.IsValid)
            {
                RoomInfo roomInfo = new RoomInfo
                {
                    title = postRoomVM.tieu_de,
                    meta = postRoomVM.tieu_de_meta,
                    roomTypeId = postRoomVM.loai_chuyen_muc,
                    price = postRoomVM.gia,
                    acreage = postRoomVM.dien_tich.ToString(),
                    location = postRoomVM.dia_chi,
                    detail_description = postRoomVM.noi_dung,
                    datebegin = DateTime.Now,
                    hide = true,
                    isApproved = false,
                    tenant = postRoomVM.doi_tuong.Replace("-", "").Trim(),
                    accountId = int.Parse(postRoomVM.User_id),
                };
                try
                {
                    db.RoomInfoes.Add(roomInfo);
                    db.SaveChanges();

                    string email = GetEmail(roomInfo.accountId);
                    if (email != null)
                    {
                        //Gửi mail đến người dùng
                        string content = System.IO.File.ReadAllText(Server.MapPath("~/Areas/Admin/Content/assets/template/CreatePost.html"));
                        // chọn loại Email gửi đến người dùng
                        // thêm thông tin phù hợp
                        content = content.Replace("{{Title}}", roomInfo.title);
                        content = content.Replace("{{DetailDescription}}", roomInfo.detail_description);
                        content = content.Replace("{{RoomType}}", getTitleRoomType(roomInfo.roomTypeId));
                        content = content.Replace("{{Tenant}}", roomInfo.tenant);
                        content = content.Replace("{{Price}}", roomInfo.price);
                        content = content.Replace("{{Acreage}}", (roomInfo.acreage).ToString());
                        content = content.Replace("{{Location}}", roomInfo.location);
                        content = content.Replace("{{Author}}", "....");// thêm Admin: Session.name or User: Account.name
                        new MailHelper().SendMail(email, "Hỏi Trọ Website thông báo đến người dùng", content);
                    }

                     HandleUrlFile.SaveFileUrl(file_name_list, roomInfo.id);
                    Response.StatusCode = 200;
                    return Json(new { statusCode = Response.StatusCode, message = "Đăng tin đăng thành công" });
                }
                catch (Exception ex)
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Json(new { statusCode = Response.StatusCode, message = "Vui lòng thử lại sau", error = ex.ToString() });
                }
            }
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return Json(new { statusCode = Response.StatusCode, message = "Vui lòng thử lại sau" });
        }

        public RoomInfo getById(long id)
        {
            return db.RoomInfoes.Where(x => x.id == id).FirstOrDefault();
        }
        // GET: Admin/RoomInfoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomInfo roomInfo = db.RoomInfoes.Find(id);
            if (roomInfo == null)
            {
                return HttpNotFound();
            }
            ViewBag.roomTypeId = new SelectList(db.RoomTypes, "id", "title", roomInfo.roomTypeId);
            return View(roomInfo);
        }

        // POST: Admin/RoomInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "id,title,detail_description,price,acreage,location,tenant,isApproved,meta,hide,order,datebegin,roomTypeId,accountId")] RoomInfo roomInfo)
        {
            RoomInfo temp = getById(roomInfo.id); // Tìm phòng theo ID
            if (ModelState.IsValid)
            {
                // Cập nhật thông tin phòng
                roomInfo.datebegin = DateTime.Now.Date;
                roomInfo.order = getMaxOrder((long)roomInfo.roomTypeId);

                // Cập nhật các thuộc tính của phòng từ form
                temp.title = roomInfo.title;
                temp.detail_description = roomInfo.detail_description;
                temp.price = roomInfo.price;
                temp.acreage = roomInfo.acreage;
                temp.location = roomInfo.location;
                temp.tenant = roomInfo.tenant;
                temp.meta = roomInfo.meta;
                temp.hide = roomInfo.hide;
                temp.order = roomInfo.order;
                temp.roomTypeId = roomInfo.roomTypeId;
                temp.accountId = roomInfo.accountId;
                temp.isApproved = roomInfo.isApproved;

                // Cập nhật phòng trong database
                db.Entry(temp).State = EntityState.Modified;
                db.SaveChanges();

                string email = GetEmail(roomInfo.accountId);
                if (email != null)
                {
                    //Gửi mail đến người dùng thông báo chỉnh sửa
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/Areas/Admin/Content/assets/template/EditPost.html"));
                    // chọn loại Email gửi đến người dùng

                    content = content.Replace("{{Title}}", roomInfo.title);
                    content = content.Replace("{{DetailDescription}}", roomInfo.detail_description);
                    content = content.Replace("{{RoomType}}", getTitleRoomType(roomInfo.roomTypeId));
                    content = content.Replace("{{Tenant}}", roomInfo.tenant);
                    content = content.Replace("{{Price}}", roomInfo.price);
                    content = content.Replace("{{Acreage}}", (roomInfo.acreage).ToString());
                    content = content.Replace("{{Location}}", roomInfo.location);
                    content = content.Replace("{{Author}}", "....");// thêm Admin: Session.name or User: Account.name
                                                                    // thêm thông tin sau khi Edit
                    content = content.Replace("{{NewTitle}}", temp.title);
                    content = content.Replace("{{NewDetailDescription}}", temp.detail_description);
                    content = content.Replace("{{NewRoomType}}", getTitleRoomType(temp.roomTypeId));
                    content = content.Replace("{{NewTenant}}", temp.tenant);
                    content = content.Replace("{{NewPrice}}", temp.price);
                    content = content.Replace("{{NewAcreage}}", (temp.acreage).ToString());
                    content = content.Replace("{{NewLocation}}", temp.location);
                    content = content.Replace("{{NewAuthor}}", "....");// thêm Admin: Session.name or User: Account.name
                                                                       //Editor
                    content = content.Replace("{{Editor}}", "....");// thêm Admin: Session.name or User: Account.name
                    new MailHelper().SendMail(email, "Hỏi Trọ Website thông báo đến người dùng", content);
                }

                return Json(new { code = 200, msg = "Cập nhật thành công" }); // Trả về thông báo thành công
            }
            ViewBag.roomTypeId = new SelectList(db.RoomTypes, "id", "title", temp.roomTypeId);
            return Json(new { code = 400, msg = "Cập nhật thất bại" }); // Trả về thông báo thất bại nếu dữ liệu không hợp lệ
        }

        // GET: Admin/RoomInfoes/Delete/5

        [HttpGet]
        public ActionResult SangEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomInfo roomInfo = db.RoomInfoes.SingleOrDefault(r => r.id == id);
            if (roomInfo == null)
            {
                return HttpNotFound();
            }

            // load và lấy theo thứ tự
            db.Entry(roomInfo).Collection(r => r.RoomImages).Query().OrderBy(i => i.order).Load();
            ViewBag.RoomImages = roomInfo.RoomImages;
            ViewBag.ten_lien_he = roomInfo.Account.name;
            ViewBag.phone = roomInfo.Account.phoneNum;
            int dienTich = 0;
            int.TryParse(roomInfo.acreage, out dienTich);

            PostRoomVM postRoomVM = new PostRoomVM()
            {
                id = roomInfo.id,
                dia_chi = roomInfo.location,
                tieu_de = roomInfo.title,
                tieu_de_meta = roomInfo.meta,
                noi_dung = roomInfo.detail_description,
                gia = roomInfo.price,
                dien_tich = dienTich,
                loai_chuyen_muc = roomInfo.roomTypeId,
                doi_tuong = roomInfo.tenant,
            };

            ViewBag.loai_chuyen_muc = new SelectList(db.RoomTypes.OrderBy(r => r.order), "id", "title", postRoomVM.loai_chuyen_muc);
            ViewBag.RoomImages = roomInfo.RoomImages;
            return View(postRoomVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SangEdit(PostRoomVM postRoomVM, string[] file_name_list, string[] file_delete_list)
        {
            try
            {
                var temp = db.RoomInfoes.SingleOrDefault(r => r.id == postRoomVM.id);
                var roomInfo = new RoomInfo
                {
                    title = temp.title,
                    detail_description = temp.detail_description,
                    roomTypeId = temp.roomTypeId,
                    tenant = temp.tenant,
                    price = temp.price,
                    acreage = temp.acreage,
                    location = temp.location,
                };


                if (temp != null)
                {
                    temp.title = postRoomVM.tieu_de;
                    temp.meta = postRoomVM.tieu_de_meta;
                    temp.roomTypeId = postRoomVM.loai_chuyen_muc;
                    temp.price = postRoomVM.gia;
                    temp.acreage = postRoomVM.dien_tich.ToString();
                    temp.location = postRoomVM.dia_chi;
                    temp.detail_description = postRoomVM.noi_dung;
                    temp.datebegin = DateTime.Now;
                    temp.tenant = postRoomVM.doi_tuong.Replace("-", "").Trim();
                    db.SaveChanges();
                }

                HandleUrlFile.SaveFileUrl(file_name_list, postRoomVM.id);
                HandleUrlFile.DeleteFileUrl(file_delete_list, postRoomVM.id);

                string email = GetEmail(temp.accountId);
                if (email != null)
                {
                    //Gửi mail đến người dùng thông báo chỉnh sửa
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/Areas/Admin/Content/assets/template/EditPost.html"));
                    // chọn loại Email gửi đến người dùng

                    content = content.Replace("{{Title}}", roomInfo.title);
                    content = content.Replace("{{DetailDescription}}", roomInfo.detail_description);
                    content = content.Replace("{{RoomType}}", getTitleRoomType(roomInfo.roomTypeId));
                    content = content.Replace("{{Tenant}}", roomInfo.tenant);
                    content = content.Replace("{{Price}}", roomInfo.price);
                    content = content.Replace("{{Acreage}}", (roomInfo.acreage).ToString());
                    content = content.Replace("{{Location}}", roomInfo.location);
                    content = content.Replace("{{Author}}", "....");// thêm Admin: Session.name or User: Account.name
                                                                    // thêm thông tin sau khi Edit
                    content = content.Replace("{{NewTitle}}", temp.title);
                    content = content.Replace("{{NewDetailDescription}}", temp.detail_description);
                    content = content.Replace("{{NewRoomType}}", getTitleRoomType(temp.roomTypeId));
                    content = content.Replace("{{NewTenant}}", temp.tenant);
                    content = content.Replace("{{NewPrice}}", temp.price);
                    content = content.Replace("{{NewAcreage}}", (temp.acreage).ToString());
                    content = content.Replace("{{NewLocation}}", temp.location);
                    content = content.Replace("{{NewAuthor}}", "....");// thêm Admin: Session.name or User: Account.name
                                                                       //Editor
                    content = content.Replace("{{Editor}}", "....");// thêm Admin: Session.name or User: Account.name
                    new MailHelper().SendMail(email, "Hỏi Trọ Website thông báo đến người dùng", content);
                }

                Response.StatusCode = 200;
                return Json(new { statusCode = Response.StatusCode, message = "Cập nhật tin đăng thành công" });
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { statusCode = Response.StatusCode, message = "Vui lòng thử lại sau", error = ex.ToString() });
                //return View();
            }
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomInfo roomInfo = db.RoomInfoes.Find(id);
            if (roomInfo == null)
            {
                return HttpNotFound();
            }
            return View(roomInfo);
        }

        // POST: Admin/RoomInfoes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            var room = db.RoomInfoes.Find(id);
            if (room == null)
            {
                return Json(new { code = 404, msg = "Room không tồn tại" });
            }

            try
            {
                DateTime expiryDate = DateTime.UtcNow;
                if (room.datebegin != null)
                {
                    expiryDate = room.datebegin.Value.AddDays(30);
                }

                string email = GetEmail(room.accountId);
                if (email != null)
                {
                    
                    //Gửi mail đến người dùng
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/Areas/Admin/Content/assets/template/DeletePost.html"));
                    // chọn loại Email gửi đến người dùng
                    // thêm thông tin phù hợp
                    content = content.Replace("{{Title}}", room.title);
                    content = content.Replace("{{PostDate}}", (room.datebegin).ToString());
                    content = content.Replace("{{ExpiryDate}}", expiryDate.ToString("dd/MM/yyyy"));
                    new MailHelper().SendMail(email, "Hỏi Trọ Website thông báo đến người dùng", content);
                }

                db.RoomInfoes.Remove(room);
                db.SaveChanges();

                return Json(new { code = 200, msg = "Xóa Room thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Có lỗi xảy ra khi xóa Room: " + ex.Message });
            }
        }


        [HttpGet]
        public ActionResult DuyetBai()
        {
            var rooms = db.RoomInfoes.ToList();
            return View(rooms);
        }

        public ActionResult GetRoomInfoes(string action)
        {
            var rooms = db.RoomInfoes.ToList();
            return PartialView();
        }

        [HttpGet]
        public ActionResult DuyetBaiDang(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var room = db.RoomInfoes.FirstOrDefault(r => r.id == id);
            if (room == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            try
            {
                room.isApproved = true;
                db.SaveChanges();
                Response.StatusCode = 200;
                return Json(new { message = "Duyệt bài thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) 
            { 
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { message = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
