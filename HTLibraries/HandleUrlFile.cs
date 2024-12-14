using HoiTroWebsite.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HoiTroWebsite.HTLibraries
{
    public class HandleUrlFile : IController
    {
        private static readonly HoiTroEntities db = new HoiTroEntities();

        // lưu tên file và đường dẫn lên database
        public static void SaveFileUrl(string[] file_name_list, int roomInfoId)
        {
            if (file_name_list == null) { return; }
            int count = 0;
            foreach (var file_name in file_name_list)
            {
                var f = db.RoomImages.SingleOrDefault(ri => ri.file_name == file_name);
                if (f == null)
                {
                    // thêm đường dẫn file vào database
                    db.RoomImages.Add(new RoomImage
                    {
                        reference_id = roomInfoId,
                        imagePath = "/Data/user/" + file_name,
                        file_name = file_name,
                        hide = true,
                        order = count,
                        datebegin = DateTime.Now,
                    });
                }
                else
                {
                    f.order = count;
                }
                count++;
            }
            db.SaveChanges();
        }

        // xóa đường dẫn file trên database
        public static void DeleteFileUrl(string[] file_delete_list, int roomInfoId)
        {
            if (file_delete_list == null) { return; }

            file_delete_list.ForEach(f =>
            {
                var RoomImage = db.RoomImages.SingleOrDefault(ri => ri.reference_id == roomInfoId && ri.file_name == f);
                if (RoomImage != null)
                {
                    db.RoomImages.Remove(RoomImage);
                }
            });
            db.SaveChanges();
        }

        public static void SaveFile(HttpPostedFileBase file, string serverFolderPath, Account account)
        {
            if (file != null)
            {
                long unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                string fileName = string.Join("_", Path.GetFileNameWithoutExtension(file.FileName), unixTimestamp) + Path.GetExtension(file.FileName);
                string path = Path.Combine(HttpContext.Current.Server.MapPath(serverFolderPath), fileName);
                file.SaveAs(path);
                SaveFileUrl(serverFolderPath + fileName, fileName, account);
            }
        }

        // Luu duong dan len database
        public static void SaveFileUrl(string imgPath, string fileName, Account account)
        {
            if (account != null)
            {
                account.imagePath = imgPath;
                account.file_name = fileName;
                db.SaveChanges();
            }
        }

        public void Execute(RequestContext requestContext)
        {
            throw new NotImplementedException();
        }
    }
}