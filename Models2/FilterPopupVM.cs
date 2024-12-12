using System.ComponentModel.DataAnnotations;

namespace HoiTroWebsite.Models2
{
    public class FilterPopupVM
    {
        [Required]
        public string danh_muc {  get; set; }

        [Required]
        public string tinhthanhpho { get; set; }

        [Required]
        public string quanhuyen { get; set; }

        [Required]
        public string phuongxa { get; set; }

        [Required]
        public string min_price { get; set; }

        [Required]
        public string max_price { get; set; }

        [Required]
        public string min_area { get; set; }

        [Required]
        public string max_area { get; set; }
    }
}