using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.WebPages;
using HoiTroWebsite.Models;
using Newtonsoft.Json.Linq;

namespace HoiTroWebsite.Controllers
{
    public class RoomInfoController : Controller
    {
        // Khai báo
        private readonly HoiTroEntities db = new HoiTroEntities();

        // xem tất cả phòng theo 1 loại
        public ActionResult Index(int page = 1, int pagesize = 10)
        {
            var rooms = db.RoomInfoes.Where(ri => ri.hide == true && ri.isApproved == true)
                .Include(ri => ri.Account)
                .Include(ri => ri.RoomImages)
                .OrderBy(r => r.order).AsQueryable();

            ViewBag.pagesize = pagesize;
            ViewBag.page = page;
            ViewBag.recsCount = rooms.Count();
            ViewBag.url = Request.Url.ToString();

            int recSkip = (page - 1) * pagesize; //(3 - 1) * 10;

            rooms = rooms.Skip(recSkip).Take(pagesize);

            return View(rooms.ToList());
        }

        // chi tiết tin trên HomePage-Menu
        public ActionResult RoomDetail(long roomID)
        {
            RoomInfo roomDetail = (from t in db.RoomInfoes
                                   where t.id == roomID
                                   select t).FirstOrDefault();
            db.Entry(roomDetail).Collection(ri => ri.RoomImages).Query().OrderBy(img => img.order).Load();
            return PartialView(roomDetail);
        }

        // lấy thông tin danh sách phòng theo loại
        public ActionResult GetListRoomInfoFollowType(string roomTypeMeta, int page = 1, int pagesize = 10)
        {
            var v = db.RoomTypes.Where(rt => rt.meta == roomTypeMeta).FirstOrDefault();
            var rooms = db.RoomInfoes.Where(ri => ri.roomTypeId == v.id && ri.isApproved == true && ri.hide == true).OrderBy(r => r.order).AsQueryable();

            ViewBag.meta = v.meta;
            ViewBag.pagesize = pagesize;
            ViewBag.title = v.title;
            ViewBag.page = page;
            ViewBag.recsCount = rooms.Count();
            ViewBag.url = Request.Url.ToString();

            int recSkip = (page - 1) * pagesize; //(3 - 1) * 10;

            rooms = rooms.Skip(recSkip).Take(pagesize);

            return View(rooms.ToList());
        }

        // list thông tin dánh sách phòng
        public ActionResult GetListRoomInfo(long roomTypeID, string metaTitle)
        {
            ViewBag.meta = metaTitle;

            var listRoomInfo = db.RoomInfoes.Where(ri => ri.roomTypeId == roomTypeID && ri.isApproved == true).ToList();

            return PartialView(listRoomInfo);
        }


        [Route("tim-kiem-{room_type_meta}")]
        [Route("tim-kiem-{room_type_meta}/dia-chi/{tinhthanhpho_slug}")]
        [Route("tim-kiem-{room_type_meta}/dia-chi/{tinhthanhpho_slug}/{quanhuyen_slug}")]
        [Route("tim-kiem-{room_type_meta}/dia-chi/{tinhthanhpho_slug}/{quanhuyen_slug}/{phuongxa_slug}")]
        public ActionResult GetRoomsBySearchValues(string room_type_meta, string tinhthanhpho_slug, string quanhuyen_slug, string phuongxa_slug,
             long? gia_den, Int16? dien_tich_den, long? gia_tu = 0, Int16? dien_tich_tu = 0, int page = 1, int pagesize = 5)
        {
            if (!room_type_meta.IsEmpty() && tinhthanhpho_slug.IsEmpty() && quanhuyen_slug.IsEmpty() && phuongxa_slug.IsEmpty() && Request.QueryString.Count == 0)
            {
                return Redirect("/phong-tro/" +  room_type_meta);
            }

            ViewBag.url = Request.Url.ToString();
            ViewBag.room_type_meta = room_type_meta;
            ViewBag.pagesize = pagesize;
            ViewBag.page = page;

            int room_type_id = db.RoomTypes.Where(rt => rt.meta == room_type_meta).Select(rt => rt.id).FirstOrDefault();

            string query = @"Select * from RoomInfo as r Where " +
                $"r.roomTypeId = {room_type_id} and r.hide = 1 and r.isApproved = 1 ";

            //var rooms = db.RoomInfoes.Where(r => r.RoomType.meta == room_type_meta && r.hide == true && r.isApproved == true);

            if (Session[tinhthanhpho_slug] == null || Session[quanhuyen_slug] == null || Session[phuongxa_slug] == null)
            {
                new APIController { ControllerContext = this.ControllerContext }.GetHanhChinhVnBySlug(tinhthanhpho_slug, quanhuyen_slug, phuongxa_slug);
            }

            //rooms = gia_den != null ?
            //    rooms.ToList().Where(r => r.price.As<long>() >= gia_tu && r.price.As<long>() <= gia_den).AsQueryable()
            //    : rooms.ToList().Where(r => r.price.As<long>() >= gia_tu).AsQueryable();

            //rooms = dien_tich_den != null ?
            //    rooms.ToList().Where(r => r.acreage.As<Int16>() >= dien_tich_tu && r.acreage.As<Int16>() <= dien_tich_den).AsQueryable()
            //        : rooms.ToList().Where(r => r.acreage.As<Int16>() >= dien_tich_tu).AsQueryable();

            query = gia_den != null ?
                query += $" and cast(convert(float, r.price) as bigint) >= {gia_tu} and cast(convert(float, r.price) as bigint) <= {gia_den} "
                    : query += $" and cast(convert(float, r.price) as bigint) >= {gia_tu} ";

            query = dien_tich_den != null ?
                query += $" and cast(convert(float, r.acreage) as int) >= {dien_tich_tu} and cast(convert(float, r.acreage) as int) <= {dien_tich_den} "
                    : query += $" and cast(convert(float, r.acreage) as int) >= {dien_tich_tu} ";

            var rooms = db.RoomInfoes.SqlQuery(query).AsQueryable();

            string hanhchinh_1 = (Session[tinhthanhpho_slug] as JToken)?["name"].ToString();
            rooms = hanhchinh_1.IsEmpty() == false ?
                rooms.Where(r => r.location.Contains(hanhchinh_1))
                : rooms;

            string hanhchinh_2 = (Session[quanhuyen_slug] as JToken)?["name_with_type"].ToString();
            rooms = hanhchinh_2.IsEmpty() == false ?
                rooms.Where(r => r.location.Contains(hanhchinh_2))
                : rooms;

            string hanhchinh_3 = (Session[phuongxa_slug] as JToken)?["name_with_type"].ToString();
            rooms = hanhchinh_3.IsEmpty() == false ?
                rooms.Where(r => r.location.Contains(hanhchinh_3))
                : rooms;

            //else if (string.IsNullOrEmpty(tinhthanhpho) == false)
            //{
            //        rooms = db.RoomInfoes.SqlQuery("SELECT * FROM RoomInfo WHERE " +
            //            "location COLLATE Latin1_General_CI_AI LIKE @p0 " +
            //            "and location COLLATE Latin1_General_CI_AI like @p1 " +
            //            "and location COLLATE Latin1_General_CI_AI like @p2 ", 
            //                            tinhthanhpho.IsEmpty() == true ?"%" : "%" + tinhthanhpho.Replace("-", " ") + "%",
            //                            quanhuyen.IsEmpty() == true ? "%" : "%" + quanhuyen.Replace("-", " ") + "%",
            //                            phuongxa.IsEmpty() == true ? "%" : "%" + phuongxa.Replace("-", " ") + "%")
            //        .AsQueryable();
            //}

            int recSkip = (page - 1) * pagesize; //(3 - 1) * 10;
            ViewBag.recsCount = rooms.Count();

            return View(rooms.Skip(recSkip).Take(pagesize).ToList());
        }
    }
}