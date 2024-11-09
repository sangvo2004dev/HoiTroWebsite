using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HoiTroWebsite.Controllers
{
    public class APIController : Controller
    {
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
                string fileName = string.Join("_", Path.GetFileNameWithoutExtension(imgFile.FileName), unixTimestamp) +  Path.GetExtension(imgFile.FileName);
                string path = Path.Combine(Server.MapPath("/Data/user/"), fileName);
                imgFile.SaveAs(path);

                var responseData = new { success = true, file_name = fileName };

                Response.StatusCode = 200; // Mã trạng thái HTTP
                return Json(responseData, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
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

        // POST:
        [Route("api/delete-multiple")]
        [HttpPost]
        public ActionResult DeleteImgFiles(List<string> fileNameList)
        {
            try
            {
                var list = new List<string>();
                fileNameList.ForEach(fileName =>
                {
                    string path = Path.Combine(Server.MapPath("/Data/user/"), fileName);
                    var f = new FileInfo(path);
                    if (f.Exists)
                    {
                        f.Delete();
                    }
                });
                var responseData = new { success = true };

                Response.StatusCode = 200; // Mã trạng thái HTTP
                return Json(responseData);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed, ex.Message);
            }
        }
    }
}