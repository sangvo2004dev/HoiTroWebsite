using System.ComponentModel.DataAnnotations;

namespace HoiTroWebsite.Models2
{
    public class LoginVM
    {
        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        public string phoneNumber { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        public string password {  get; set; }
    }
}