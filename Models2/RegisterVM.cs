using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HoiTroWebsite.Models2
{
    public class RegisterVM
    {
        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Họ tên không được để trống.")]
        public string Name { get; set; }

        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Ngày sinh")]
        [DisplayFormat(DataFormatString = "0: dd/MM/yyyy")]
        public Nullable<DateTime> DateOfBirth { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        public string Password { get; set; }

        [Display(Name = "Nhập lại mật khẩu")]
        [Required(ErrorMessage = "Nhập lại mật khẩu không được để trống.")]
        [Compare("Password", ErrorMessage = "Nhập lại mật khẩu không đúng.")]
        public string ConfirmPassword { get; set; }
    }
}