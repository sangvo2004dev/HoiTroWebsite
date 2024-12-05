using System;
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
        HoiTroEntities db = new HoiTroEntities();

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

        public ActionResult editInfor(long id)
        {
            var account = db.Accounts.FirstOrDefault(a => a.id == id);

            if (account == null)
            {
                return HttpNotFound();
            }

            return PartialView(account);
        }
        public ActionResult managePosted(long id)
        {
            var account = db.Accounts.FirstOrDefault(a => a.id == id);

            if (account == null)
            {
                return HttpNotFound();
            }

            return PartialView(account);
        }
        public ActionResult listPosted(long id)
        {
            var v = from n in db.RoomInfoes
                    join r in db.Accounts on n.accountId equals r.id
                    where n.hide == true && r.id == id
                    orderby n.datebegin descending
                    select n;
            return PartialView(v.ToList());
        }
        public ActionResult altPhoneNum(long id)
        {
            var account = db.Accounts.FirstOrDefault(a => a.id == id);

            if (account == null)
            {
                return HttpNotFound();
            }

            return PartialView(account);
        }
        public ActionResult altPassword(long id)
        {
            var account = db.Accounts.FirstOrDefault(a => a.id == id);

            if (account == null)
            {
                return HttpNotFound();
            }

            return PartialView(account);
        }
    }
}