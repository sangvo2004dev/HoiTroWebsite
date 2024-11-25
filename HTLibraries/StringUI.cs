using System.Globalization;
using System;

namespace HoiTroWebsite.HTLibraries
{
    public class StringUI
    {
        public static string Convert2DateTimeVietnamese(string dateString)
        {
            // Định dạng của chuỗi ngày giờ
            string format = "MM/dd/yyyy h:mm:ss tt";

            // Chuyển đổi chuỗi thành DateTime
            DateTime date = DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);

            // Tạo CultureInfo cho Việt Nam
            var vietnamCulture = new CultureInfo("vi-VN");

            // Định dạng ngày giờ theo kiểu "Thứ, ngày/tháng/năm giờ:phút:giây"
            string formattedDate = date.ToString("dddd, HH:mm dd/MM/yyyy", vietnamCulture);

            return formattedDate;
        }
    }
}