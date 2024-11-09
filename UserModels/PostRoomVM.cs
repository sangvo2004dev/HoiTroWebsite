using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HoiTroWebsite.UserModels
{
    public class PostRoomVM
    {
        //[Required(ErrorMessage = "Chưa chọn khu vực đăng tin")]
        public string dia_chi { get; set; }

        [Required(ErrorMessage = "Chưa chọn loại chuyên mục")]
        public int loai_chuyen_muc { get; set; }

        public string tieu_de { get; set; }

        public string tieu_de_meta { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập nội dung")]
        [MinLength(100, ErrorMessage = "Nội dung tối thiểu 100 kí tự")]
        public string noi_dung {  get; set; }
        public string ten_lien_he { get; set; }
        public string phone { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập giá phòng")]
        [RegularExpression(@"[0-9.]+", ErrorMessage = "Giá phòng chưa đúng")]
        public string gia { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập diện tích")]
        [Range(1, int.MaxValue, ErrorMessage = "Diện tích chưa đúng")]
        public int dien_tich { get; set; }
        public string doi_tuong { get; set; }
    }
}