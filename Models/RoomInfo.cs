﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HoiTroWebsite.Models
{
    using System;
    using System.Collections.Generic;
    //them xu ly not null
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    
    public partial class RoomInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RoomInfo()
        {
            this.RoomImages = new HashSet<RoomImage>();
        }
    
        public int id { get; set; }
        [Required(ErrorMessage = "Tiêu đề không được để trống.")]
        public string title { get; set; }
        [Required(ErrorMessage = "Chi tiết không được để trống.")]
        public string detail_description { get; set; }
        [Required(ErrorMessage = "Giá cho thuê không được để trống.")]
        public string price { get; set; }
        public string acreage { get; set; }
        public string location { get; set; }
        public string tenant { get; set; }
        public string nameInfor { get; set; }
        public string phoneInfor { get; set; }
        public string zaloInfor { get; set; }
        public Nullable<bool> isApproved { get; set; }
        public string meta { get; set; }
        public Nullable<bool> hide { get; set; }
        public Nullable<int> order { get; set; }
        public Nullable<System.DateTime> datebegin { get; set; }
        public Nullable<int> roomTypeId { get; set; }
        public Nullable<int> accountId { get; set; }
    
        public virtual RoomType RoomType { get; set; }
        public virtual Account Account { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoomImage> RoomImages { get; set; }
    }
}
