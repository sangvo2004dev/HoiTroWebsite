using System.Globalization;
using System;

namespace HoiTroWebsite.HTLibraries
{
    public class StringUI
    {
        public static string Convert2DateTimeVietnamese(string dateString)
        {
            // Định dạng của chuỗi ngày giờ
            string format = "MM/dd/yyyy HH:mm:ss tt";

            // Chuyển đổi chuỗi thành DateTime
            DateTime date = DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);

            // Tạo CultureInfo cho Việt Nam
            var vietnamCulture = new CultureInfo("vi-VN");

            // Định dạng ngày giờ theo kiểu "Thứ, ngày/tháng/năm giờ:phút:giây"
            string formattedDate = date.ToString("dddd, HH:mm dd/MM/yyyy", vietnamCulture);

            return formattedDate;
        }

        public static string Convert2DateTimeVietnamese(System.DateTime? dateTime)
        {
            // Tạo CultureInfo cho Việt Nam
            var vietnamCulture = new CultureInfo("vi-VN");
            string formattedDate = "";
            try
            {
                // Định dạng ngày giờ theo kiểu "Thứ, ngày/tháng/năm giờ:phút:giây"
                formattedDate = dateTime?.ToString("dddd, HH:mm dd/MM/yyyy", vietnamCulture);
            }
            catch
            {
                formattedDate = "Thứ 2, 12:00 01/01/2000";
            }


            return formattedDate;
        }
    }
}