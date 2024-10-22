//------------------------------------------------------------------------------
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
    
    public partial class RoomInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RoomInfo()
        {
            this.RoomImages = new HashSet<RoomImage>();
        }
    
        public int id { get; set; }
        public string title { get; set; }
        public string brief_description { get; set; }
        public string detail_description { get; set; }
        public string price { get; set; }
        public Nullable<double> acreage { get; set; }
        public string area { get; set; }
        public string location { get; set; }
        public string tenant { get; set; }
        public string meta { get; set; }
        public Nullable<bool> hide { get; set; }
        public Nullable<int> order { get; set; }
        public Nullable<System.DateTime> datebegin { get; set; }
        public Nullable<int> roomTypeId { get; set; }
        public Nullable<int> accountId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoomImage> RoomImages { get; set; }
        public virtual RoomType RoomType { get; set; }
        public virtual Account Account { get; set; }
    }
}
