﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HoiTroWebsite.Models;

namespace HoiTroWebsite.Controllers
{
    public class ButtonController : Controller
    {
        // Khai báo
        HoiTroEntities _db = new HoiTroEntities();

        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult getPost(string name, string phoneNum)
        {
            return View();
        }

        public ActionResult getRegister()
        {
            return PartialView();
        }

        public ActionResult getLognIn()
        {
            return PartialView();
        }

        public ActionResult manageAccount(long id)
        {
            
            return PartialView();
        }
    }
}