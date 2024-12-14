using System.Globalization;
using System;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace HoiTroWebsite.HTLibraries
{
    public static class StringUI
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

    public static class Time
    {
        /// <summary>
        /// DateTime now = new DateTime(2022, 3, 23); // current time
        /// string output = VietnamNumber.Time.TimeAgo(now, from: new DateTime(2022, 3, 25));
        /// or
        /// output = now.TimeAgo(new DateTime(2022, 3, 25));
        /// Output: 2 ngày trước
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="from"></param>
        /// <returns></returns>


        public static string TimeAgo(this DateTime dateTime, DateTime? from = null)
        {
            string result = string.Empty;
            var now = from ?? DateTime.Now;
            var timeSpan = now.Subtract(dateTime);

            if (timeSpan <= TimeSpan.FromSeconds(3))
            {
                result = string.Format("bây giờ", timeSpan.Seconds);
            }
            else if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                result = string.Format("{0} giây trước", timeSpan.Seconds);
            }
            else if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                result = string.Format("{0} phút trước", timeSpan.Minutes);
            }
            else if (timeSpan <= TimeSpan.FromHours(24))
            {
                result = string.Format("{0} giờ trước", timeSpan.Hours);
            }
            else if (timeSpan <= TimeSpan.FromDays(30))
            {
                result = string.Format("{0} ngày trước", timeSpan.Days);
            }
            else if (timeSpan <= TimeSpan.FromDays(365))
            {
                result = string.Format("{0} tháng trước", timeSpan.Days / 30);
            }
            else
            {
                result = string.Format("{0} năm trước", timeSpan.Days / 365);
            }

            return result;
        }

    }
}