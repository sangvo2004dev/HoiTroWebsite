﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoiTroWebsite.Models
{
    public class NewsViewModel
    {
        public int ID { get; set; }
        public string Meta { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string BriefDescription { get; set; }
        public string ImagePath { get; set; }  // Path to the image
    }

}