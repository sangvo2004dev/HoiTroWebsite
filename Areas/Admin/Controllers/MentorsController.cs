﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using HoiTroWebsite.Models;

namespace HoiTroWebsite.Areas.Admin.Controllers
{
    public class MentorsController : Controller
    {
        private HoiTroEntities db = new HoiTroEntities();

        // GET: Admin/Mentors
        public ActionResult Index()
        {
            return View(db.Mentors.ToList());
        }

        // GET: Admin/Mentors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mentor mentor = db.Mentors.Find(id);
            if (mentor == null)
            {
                return HttpNotFound();
            }
            return View(mentor);
        }

        // GET: Admin/Mentors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Mentors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "id,name,avtImage,email,phoneNum,FBlink,zaloNum,supportTask,meta,hide,order,datebegin")] Mentor mentor, HttpPostedFileBase img)
        {
            if (ModelState.IsValid)
            {
                var path = "";
                var filename = "";
                if (ModelState.IsValid)
                {
                    if (img != null)
                    {
                        //file =  Guid = Guid.newGuid().toString() + img.FileName;
                        filename = Path.GetFileName(img.FileName);  // Chỉ lấy phần tên file

                        path = Path.Combine(Server.MapPath("~/Content/images"), filename);
                        img.SaveAs(path);
                        mentor.avtImage = filename; //Lưu ý
                    }
                    else
                    {
                        mentor.avtImage = "logo.png";
                    }
                    mentor.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    db.Mentors.Add(mentor);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(mentor);
        }

        // GET: Admin/Mentors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mentor mentor = db.Mentors.Find(id);
            if (mentor == null)
            {
                return HttpNotFound();
            }
            return View(mentor);
        }

        // POST: Admin/Mentors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "id,name,avtImage,email,phoneNum,FBlink,zaloNum,supportTask,meta,hide,order,datebegin")] Mentor mentor, HttpPostedFileBase img)
        {
            var path = "";
            var filename = "";
            Mentor temp = getById(mentor.id);

            if (ModelState.IsValid)
            {
                if (img != null)
                {
                    // filename = Guid.NewGuid().ToString() + img.FileName;
                    //filename = DateTime.Now.ToString("dd-MM-yy-hh-mm-ss-") + img.FileName;
                    filename = Path.GetFileName(img.FileName);  // Chỉ lấy phần tên file
                    path = Path.Combine(Server.MapPath("~/Content/images"), filename);
                    img.SaveAs(path);
                    temp.avtImage = filename; // Lưu ý
                }
                temp.name = mentor.name;
                temp.email = mentor.email;
                temp.phoneNum = mentor.phoneNum;
                temp.FBlink = mentor.FBlink;
                temp.zaloNum = mentor.zaloNum;
                temp.supportTask = mentor.supportTask;
                temp.meta = mentor.meta;
                temp.hide = mentor.hide;
                temp.order = mentor.order;
                
                db.Entry(temp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mentor);
        }
        public Mentor getById(long id)
        {
            return db.Mentors.Where(x => x.id == id).FirstOrDefault();
        }

        // GET: Admin/Mentors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mentor mentor = db.Mentors.Find(id);
            if (mentor == null)
            {
                return HttpNotFound();
            }
            return View(mentor);
        }

        // POST: Admin/Mentors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Mentor mentor = db.Mentors.Find(id);
            db.Mentors.Remove(mentor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
