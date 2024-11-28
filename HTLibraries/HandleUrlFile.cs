using HoiTroWebsite.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Linq;

namespace HoiTroWebsite.HTLibraries
{
    public class HandleUrlFile
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
    }
}