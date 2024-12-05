using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HoiTroWebsite.Models
{
    public class AdminLogin
    {
        [Required(ErrorMessage = "Vui lòng nhập Email.")]
        public string email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        public string password { get; set; }
    }
}