using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HoiTroWebsite.Models;

namespace HoiTroWebsite.Controllers
{

    public class RoomInfoController : Controller
    {
        // Khai báo
        private readonly HoiTroEntities _db = new HoiTroEntities();

        // xem tất cả phòng theo 1 loại
        public ActionResult Index()
        {
            var listRoomInfo = _db.RoomInfoes.Where(ri => ri.hide == true && ri.isApproved == false)
                .Include(ri => ri.Account)
                .Include(ri => ri.RoomImages)
                .ToList();

            return View(listRoomInfo);
        }

        // chi tiết tin trên HomePage-Menu
        public ActionResult RoomDetail(long roomID)
        {
            RoomInfo v = (from t in _db.RoomInfoes
                    where t.id == roomID
                    select t).FirstOrDefault();
            _db.Entry(v).Collection(ri => ri.RoomImages).Query().OrderBy(img => img.order).Load();
            //v.RoomImages = v.RoomImages.OrderBy(ri => ri.order) as Collection<RoomImg>; 
            return PartialView(v);
        }

        // lấy thông tin danh sách phòng theo loại
        public ActionResult Room_GetListRoomInfoFollowType(string roomTypeMeta)
        {
            var v = (from t in _db.RoomTypes
                     where t.meta == roomTypeMeta
                     select t).FirstOrDefault();

            return View(v);
        }

        // list thông tin dánh sách phòng
        public ActionResult GetListRoomInfo(long roomTypeID, string metaTitle) 
        {
            ViewBag.meta = metaTitle;

            var listRoomInfo = _db.RoomInfoes.Where(ri => ri.roomTypeId == roomTypeID && ri.isApproved == true)
                .Include(ri => ri.Account)
                .Include(ri => ri.RoomImages)
                .ToList();

            return PartialView(listRoomInfo);
        }
    }
}