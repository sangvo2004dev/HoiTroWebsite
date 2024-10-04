using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoiTroWebsite.Models
{
    public class RoomInfoViewModel
    {
        public string Title { get; set; }
        public string Price { get; set; }

        public string Acreage { get; set; }

        public string Area { get; set; }

        public string BriefDescription { get; set; }
        public string ImagePath { get; set; }  // Path to the image
    }
}