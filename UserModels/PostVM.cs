using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoiTroWebsite.UserModels
{
    public class PostVM
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public List<String> Images { get; set; }
    }
}