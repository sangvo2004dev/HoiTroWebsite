using HoiTroWebsite.HTLibraries;
using HoiTroWebsite.Models;
using HoiTroWebsite.Models2;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace HoiTroWebsite.Controllers
{
    public class APIController : Controller
    {
        private readonly HoiTroEntities db = new HoiTroEntities();

        // GET: API
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// List<HttpPostedFileBase> file phải giống với tham số trong dropzone 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        // POST:
        [Route("api/upload")]
        [HttpPost]
        public ActionResult UploadImgFile(HttpPostedFileBase file)
        {
            try
            {
                var imgFile = file;

                long unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                //Console.WriteLine("Unix Timestamp (seconds): " + unixTimestamp);
                string fileName = string.Join("_", Path.GetFileNameWithoutExtension(imgFile.FileName), unixTimestamp) + Path.GetExtension(imgFile.FileName);
                string path = Path.Combine(Server.MapPath("/Data/user/"), fileName);
                imgFile.SaveAs(path);

                var responseData = new { success = true, file_name = fileName };

                Response.StatusCode = 200; // Mã trạng thái HTTP
                return Json(responseData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed, ex.Message);
            }
        }

        // POST:
        [Route("api/delete")]
        [HttpPost]
        public ActionResult DeleteImgFile(string fileName)
        {
            try
            {
                string path = Path.Combine(Server.MapPath("/Data/user/"), fileName);
                var f = new FileInfo(path);
                if (f.Exists)
                {
                    f.Delete();
                }
                var responseData = new { success = true, file_name = fileName };

                Response.StatusCode = 200; // Mã trạng thái HTTP
                return Json(responseData);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed, ex.Message);
            }
        }

        /// <summary>
        /// window.sendBaecon thì ở server phải nhận bằng StreamReader, mới xử lý được
        /// </summary>
        /// <returns></returns>
        // POST:
        [Route("api/delete-multiple")]
        [HttpPost]
        public ActionResult DeleteImgFiles()
        {
            try
            {
                string jsonString;
                using (var reader = new StreamReader(Request.InputStream))
                {
                    jsonString = reader.ReadToEnd();
                }

                // Parse JSON string to JObject
                JObject jsonObject = JObject.Parse(jsonString);

                // Get the array from the "list_file_delete" key
                string[] fileNames = jsonObject.Properties().First().Value.ToObject<string[]>();
                if (fileNames != null && fileNames.Length > 0)
                {
                    fileNames.ForEach(fileName =>
                    {
                        string path = Path.Combine(Server.MapPath("/Data/user/"), fileName);
                        var f = new FileInfo(path);
                        if (f.Exists)
                        {
                            f.Delete();
                        }
                    });
                }

                var responseData = new { success = true };

                Response.StatusCode = 200; // Mã trạng thái HTTP
                return Json(responseData);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed, ex.Message);
            }
        }

        [HttpPost]
        [Route("api/post/dang-tin", Name = "post/dang-tin")]
        public ActionResult PostRoom(PostRoomVM postRoomVM, string[] file_name_list)
        {
            if (ModelState.IsValid)
            {
                int userID = (Session["USer"] as Account).id;
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
                    accountId = userID,
                };
                try
                {
                    db.RoomInfoes.Add(roomInfo);
                    db.SaveChanges();
                    HandleUrlFile.SaveFileUrl(file_name_list, roomInfo.id);
                }
                catch (Exception e)
                {
                    ViewBag.Message = e.ToString();
                }

                return RedirectToAction("ManagePostRooms", "User");
            }

            return View();
        }

        [HttpGet]
        [Route("api/post/an-hien")]
        public ActionResult SwitchStatusPost(string action, int id)
        {
            
            if (action == "an-tin")
            {
                
            }
            else
            {

            }
            return RedirectToAction("ManagePostRooms", "User");
        }

        [HttpPost]
        [Route("api/post/sua-tin", Name = "post/sua-tin")]
        public ActionResult EditPostRoom(PostRoomVM postRoomVM, string[] file_name_list, string[] file_delete_list)
        {
            try
            {
                int id = int.Parse((TempData["id"]).ToString());
                var room = db.RoomInfoes.SingleOrDefault(r => r.id == id);

                if (room != null)
                {
                    room.title = postRoomVM.tieu_de;
                    room.meta = postRoomVM.tieu_de_meta;
                    room.price = postRoomVM.gia;
                    room.acreage = postRoomVM.dien_tich.ToString();
                    room.location = postRoomVM.dia_chi;
                    room.detail_description = postRoomVM.noi_dung;
                    room.datebegin = DateTime.Now;
                    room.tenant = postRoomVM.doi_tuong.Replace("-", "").Trim();
                    db.SaveChanges();
                }

                HandleUrlFile.SaveFileUrl(file_name_list, id);
                HandleUrlFile.DeleteFileUrl(file_delete_list, id);
                Response.StatusCode = 200;
                Thread.Sleep(2000);

                return RedirectToAction("ManagePostRooms", "User");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Thread.Sleep(2000);
                return RedirectToAction("ManagePostRooms", "User");
            }
        }

        [HttpPost]
        [Route("api/post/get-all", Name = "post/get-all")]
        public ActionResult GetUserPostRooms(int pg = 0, bool chonDaDuyet = false, bool chonDaAn = false)
        {
            try
            {
                int userID = (Session["USer"] as Account).id;
                List<RoomInfo> rooms = db.RoomInfoes.Where(r => r.accountId == userID)
                    .Include(ri => ri.RoomImages)
                    .ToList();
                if (chonDaDuyet == true)
                {
                    rooms = rooms.FindAll(r => r.isApproved ==  true);
                }
                if (chonDaAn == true)
                {
                    rooms = rooms.FindAll(r => r.hide == false);
                }

                const int pageSize = 5; //10 items/trang

                if (pg < 1) //page < 1;
                {
                    pg = 1; //page ==1
                }
                int recsCount = rooms.Count(); //33 items;

                var pager = new Paginate(recsCount, pg, pageSize);

                int recSkip = (pg - 1) * pageSize; //(3 - 1) * 10; 

                //category.Skip(20).Take(10).ToList()

                var rooms2 = rooms.Skip(recSkip).Take(pager.PageSize).ToList();
                //if (rooms2.Count != 0)
                //{
                //    rooms2.ForEach(room =>
                //    {
                //        db.Entry(room).Reference(r => r.RoomType).Load();
                //        db.Entry(room).Collection(r => r.RoomImages).Query().OrderBy(rm => rm.order).Load();
                //    });
                //}

                ViewBag.Pager = pager;

                Response.StatusCode = (int)HttpStatusCode.OK;
                //PartialView("~/Views/Shared/_Paging.cshtml", pager)
                //    PartialView("~/Views/Api_/GetUserPostRooms.cshtml", rooms2)
                return Json(new { list_post = RenderPartialViewToString("~/Views/Api_/GetUserPostRooms.cshtml", rooms2), paging = RenderPartialViewToString("~/Views/Shared/_Paging.cshtml", pager) });
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(new { statusCode = Response.StatusCode, data = ex.ToString() }, JsonRequestBehavior.AllowGet);

            }
        }

        private string RenderPartialViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}