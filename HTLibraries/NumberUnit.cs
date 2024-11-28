using System;
using System.Globalization;

namespace HoiTroWebsite.HTLibraries
{
    public class NumberUnit
    {
        private readonly long _number;

        // Constructor: Chuyển chuỗi số thành kiểu long khi khởi tạo đối tượng
        public NumberUnit(string numberString)
        {
            if (!long.TryParse(numberString, out _number))
            {
                // chuyển đổi không thành công thì chuyển thành 0
                _number = 0;
            }
        }

        // Phương thức xác định đơn vị lớn nhất
        public string GetLargestUnit()
        {
            if (_number >= 1_000_000_000)
                return "tỷ";
            else if (_number >= 1_000_000)
                return "triệu";
            else if (_number >= 1_000)
                return "nghìn";
            else
                return "đồng";
        }

        public string RoundWithLargestUnit()
        {
            if (_number >= 1_000_000_000)
                return (_number * 1.0 / 1_000_000_000).ToString("0.#####") + " " + "tỷ";
            else if (_number >= 1_000_000)
                return (_number * 1.0 / 1_000_000).ToString("0.#####") + " " + "triệu";
            else if (_number >= 1_000)
                return (_number * 1.0 / 1_000).ToString("0.#####") + " " + "nghìn";
            else
                return  _number.ToString() + " " + "đồng";
        }

        public string GetDateTimeVietnamese(string dateString)
        {
            // Định dạng của chuỗi ngày giờ
            string format = "MM/dd/yyyy h:mm:ss tt";

            // Chuyển đổi chuỗi thành DateTime
            DateTime date = DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);

            // Tạo CultureInfo cho Việt Nam
            var vietnamCulture = new CultureInfo("vi-VN");

            // Định dạng ngày giờ theo kiểu "Thứ, ngày/tháng/năm giờ:phút:giây"
            string formattedDate = date.ToString("dddd, HH:mm tt dd/MM/yyyy", vietnamCulture);

            return formattedDate;
        }
    }
}