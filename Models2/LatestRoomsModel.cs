using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoiTroWebsite.Models2
{
    public class LatestRoomsModel
    {
        public string title { get; set; }
        public string area { get; set; }
        public string price { get; set; }
        public DateTime? dateTime { get; set; }
        public string pathImg { get; set; }
    }
}