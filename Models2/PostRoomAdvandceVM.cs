using System.ComponentModel.DataAnnotations;

namespace HoiTroWebsite.Models2
{
    public class PostRoomAdvandceVM : PostRoomVM
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Mã người dùng không được để trống")]
        public string User_id { get; set; }
    }
}