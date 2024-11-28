using System.ComponentModel.DataAnnotations;

namespace HoiTroWebsite.Models2
{
    public class ProfileVM
    {
        [Required(ErrorMessage = "Tiên liên hệ không được để trống.")]
        public string Ten_lien_he { get; set; }

        public string So_dien_thoai { get; set; }
        public string Facebook_link { get; set; }
        public string Email {  get; set; }
    }
}