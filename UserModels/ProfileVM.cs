using System.ComponentModel.DataAnnotations;

namespace HoiTroWebsite.UserModels
{
    public class ProfileVM
    {
        [Required(ErrorMessage = "Tiên liên hệ không được để trống.")]
        public string Ten_lien_he { get; set; }

        public string Facebook_link { get; set; }
        public string Email {  get; set; }
    }
}