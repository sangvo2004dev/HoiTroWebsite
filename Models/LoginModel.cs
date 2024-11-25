using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HoiTroWebsite.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại của bạn")]
        [Display(Name = "Số điện thoại")]
        public string PhoneNum { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}