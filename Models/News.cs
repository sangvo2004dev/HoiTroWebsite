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
    
    public partial class News
    {
        public int id { get; set; }
        public string title { get; set; }
        public string file_name { get; set; }
        public string imagePath { get; set; }
        public string brief_description { get; set; }
        public string detail_description { get; set; }
        public string author { get; set; }
        public string meta { get; set; }
        public Nullable<bool> hide { get; set; }
        public Nullable<int> order { get; set; }
        public Nullable<System.DateTime> datebegin { get; set; }
        public Nullable<int> newsTypeId { get; set; }
    
        public virtual NewsType NewsType { get; set; }
    }
}
