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
                var f = db.RoomImgs.SingleOrDefault(ri => ri.fileName == file_name);
                if (f == null)
                {
                    // thêm đường dẫn file vào database
                    db.RoomImgs.Add(new RoomImg
                    {
                        postRoomId = roomInfoId,
                        folder = "/Data/user/",
                        fileName = file_name,
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
                var roomImg = db.RoomImgs.SingleOrDefault(ri => ri.postRoomId == roomInfoId && ri.fileName == f);
                if (roomImg != null)
                {
                    db.RoomImgs.Remove(roomImg);
                }
            });
            db.SaveChanges();
        }
    }
}