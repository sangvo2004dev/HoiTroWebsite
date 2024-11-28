using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using HoiTroWebsite.Models;

namespace HoiTroWebsite.Controllers
{

    public class RoomInfoController : Controller
    {
        // Khai báo
        private readonly HoiTroEntities db = new HoiTroEntities();

        // xem tất cả phòng theo 1 loại
        public ActionResult Index()
        {
            var listRoomInfo = db.RoomInfoes.Where(ri => ri.hide == true && ri.isApproved == true)
                .Include(ri => ri.Account)
                .Include(ri => ri.RoomImages)
                .ToList();

            return View(listRoomInfo);
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
        public ActionResult GetListRoomInfoFollowType(string roomTypeMeta)
        {
            var v = db.RoomTypes.Where(rt => rt.meta ==  roomTypeMeta).FirstOrDefault();
            return View(v);
        }

        // list thông tin dánh sách phòng
        public ActionResult GetListRoomInfo(long roomTypeID, string metaTitle) 
        {
            ViewBag.meta = metaTitle;

            var listRoomInfo = db.RoomInfoes.Where(ri => ri.roomTypeId == roomTypeID && ri.isApproved == true).ToList();                

            return PartialView(listRoomInfo);
        }
    }
}