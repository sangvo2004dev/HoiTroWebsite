using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoiTroWebsite.Models2
{
    public class PostVM
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public List<String> Images { get; set; }
    }
}