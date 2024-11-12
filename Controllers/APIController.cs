using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
                    fileNames.ForEach(fileName => {
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
    }
}