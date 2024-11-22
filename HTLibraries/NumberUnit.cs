using System;

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
                throw new ArgumentException("Chuỗi không hợp lệ. Vui lòng nhập một chuỗi số hợp lệ.");
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
    }
}