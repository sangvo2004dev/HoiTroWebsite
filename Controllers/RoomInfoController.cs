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
            var v = from n in _db.RoomInfoes
                    join t in _db.RoomTypes on n.roomTypeId equals t.id
                    join r in _db.Accounts on n.accountId equals r.id
                    orderby n.datebegin descending
                    select new RoomInfoViewModel
                    {
                        RoomInfo = n,
                        Account = r,
                        ImagePath = (from i in _db.RoomImages
                                     where i.reference_id == n.id
                                     orderby i.datebegin descending
                                     select i.imagePath).FirstOrDefault()
                    };

            return View(v.ToList());
        }

        // chi tiết tin trên HomePage-Menu
        public ActionResult RoomDetail(long roomID)
        {
            RoomInfo v = (from t in _db.RoomInfoes
                    where t.id == roomID
                    select t).FirstOrDefault();
            _db.Entry(v).Collection(ri => ri.RoomImgs).Query().OrderBy(img => img.order).Load();
            //v.RoomImgs = v.RoomImgs.OrderBy(ri => ri.order) as Collection<RoomImg>; 
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

            var v = from n in _db.RoomInfoes
                    join t in _db.RoomTypes on n.roomTypeId equals t.id
                    join r in _db.Accounts on n.accountId equals r.id
                    where n.hide == true && n.roomTypeId == roomTypeID
                    orderby n.datebegin descending
                    select new RoomInfoViewModel
                    {
                        RoomInfo = n,
                        Account = r,
                        ImagePath = (from i in _db.RoomImages
                                     where i.reference_id == n.id
                                     orderby i.datebegin descending
                                     select i.imagePath).FirstOrDefault()
                    };

            return PartialView(v.ToList());
        }
    }
}