using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CKFinder.Settings;
using HoiTroWebsite.Models;
using Newtonsoft.Json;

namespace HoiTroWebsite.Areas.Admin.Controllers
{
    public class RoomInfoesController : Controller
    {
        private HoiTroEntities db = new HoiTroEntities();

        // GET: Admin/RoomInfoes
        public ActionResult Index(long? id = null)
        {
            var roomInfoes = db.RoomInfoes.Include(r => r.RoomType).Include(r => r.Account);
            Response.StatusCode = 200;
            getRoomType(id);
            return View();
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
        public ActionResult Create([Bind(Include = "id,title,brief_description,detail_description,price,acreage,area,location,tenant,meta,hide,order,datebegin,roomTypeId,accountId")] RoomInfo roomInfo, List<RoomImageViewModel> Images)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.roomTypeId = new SelectList(db.RoomTypes, "id", "title", roomInfo.roomTypeId);
                    Response.StatusCode = 400;
                    return View();
                }
                roomInfo.datebegin = DateTime.Now.Date;
                roomInfo.order = getMaxOrder((long)roomInfo.roomTypeId);
                db.RoomInfoes.Add(roomInfo);
                db.SaveChanges();


                // Xử lý danh sách ảnh nếu có
                if (Images != null)
                {
                    foreach (var image in Images)
                    {
                        if (image.File != null && image.File.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(image.File.FileName);
                            var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                            image.File.SaveAs(path);

                            var roomImage = new RoomImage
                            {
                                reference_id = roomInfo.id,
                                imagePath = fileName,
                                meta = image.Meta,
                                hide = image.Hide
                            };
                            db.RoomImages.Add(roomImage);
                        }
                    }
                    db.SaveChanges();
                    return Json(new { code = 200, msg = "Room created successfully" }, JsonRequestBehavior.AllowGet);
                } // Lưu tất cả ảnh liên kết với bài đăng
                return Json(new { code = 400, msg = "Room created failed" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
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
        public ActionResult Edit([Bind(Include = "id,title,brief_description,detail_description,price,acreage,area,location,tenant,meta,hide,order,datebegin,roomTypeId,accountId")] RoomInfo roomInfo,
                         List<RoomImageViewModel> Images, string DeletedImageIds)
        {
            try
            {
                RoomInfo temp = getById(roomInfo.id); // Tìm phòng theo ID
                if (ModelState.IsValid)
                {
                    // Cập nhật thông tin phòng
                    roomInfo.datebegin = DateTime.Now.Date;
                    roomInfo.order = getMaxOrder((long)roomInfo.roomTypeId);

                    // Cập nhật các thuộc tính của phòng từ form
                    temp.title = roomInfo.title;
                    temp.brief_description = roomInfo.brief_description;
                    temp.detail_description = roomInfo.detail_description;
                    temp.price = roomInfo.price;
                    temp.acreage = roomInfo.acreage;
                    temp.area = roomInfo.area;
                    temp.location = roomInfo.location;
                    temp.tenant = roomInfo.tenant;
                    temp.meta = roomInfo.meta;
                    temp.hide = roomInfo.hide;
                    temp.order = roomInfo.order;
                    temp.roomTypeId = roomInfo.roomTypeId;
                    temp.accountId = roomInfo.accountId;

                    // Cập nhật phòng trong database
                    db.Entry(temp).State = EntityState.Modified;
                    db.SaveChanges();

                    // Cập nhật ảnh (nếu có ảnh mới được gửi lên)
                    if (Images != null)
                    {
                        foreach (var image in Images)
                        {
                            // Kiểm tra ảnh mới và thêm vào database
                            if (image.File != null && image.File.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(image.File.FileName);
                                var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                                image.File.SaveAs(path); // Lưu ảnh vào thư mục

                                var roomImage = new RoomImage
                                {
                                    reference_id = roomInfo.id,
                                    imagePath = fileName,
                                    meta = image.Meta,
                                    hide = image.Hide
                                };
                                db.RoomImages.Add(roomImage); // Thêm ảnh vào database
                            }
                        }
                    }

                    // Xóa các ảnh không còn tồn tại trên view
                    if (!string.IsNullOrEmpty(DeletedImageIds))
                    {
                        var deletedIds = JsonConvert.DeserializeObject<List<int>>(DeletedImageIds); // Deserialize danh sách ID ảnh đã xóa
                        foreach (var imageId in deletedIds)
                        {
                            var roomImage = db.RoomImages.Find(imageId); // Lấy ảnh từ CSDL theo ID
                            if (roomImage != null)
                            {
                                db.RoomImages.Remove(roomImage); // Xóa ảnh khỏi CSDL
                            }
                        }
                        db.SaveChanges();
                    }

                    return Json(new { code = 200, msg = "Cập nhật thành công" }); // Trả về thông báo thành công
                }

                ViewBag.roomTypeId = new SelectList(db.RoomTypes, "id", "title", roomInfo.roomTypeId);
                return Json(new { code = 400, msg = "Cập nhật thất bại" }); // Trả về thông báo thất bại nếu dữ liệu không hợp lệ
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lỗi: " + ex.Message }); // Trả về lỗi nếu có ngoại lệ xảy ra
            }
        }




        // GET: Admin/RoomInfoes/Delete/5
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
                db.RoomInfoes.Remove(room);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Xóa Room thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Có lỗi xảy ra khi xóa Room: " + ex.Message });
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
