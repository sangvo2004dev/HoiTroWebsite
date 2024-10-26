using HoiTroWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HoiTroWebsite.Controllers
{
    public class ManageAccountController : Controller
    {
        HoiTroEntities _db = new HoiTroEntities();
        public ActionResult Index(long id)
        {
            var account = _db.Accounts.FirstOrDefault(a => a.id == id);

            if (account == null)
            {
                return HttpNotFound();
            }
            return PartialView(account);
        }
        public ActionResult editInfor(long id)
        {
            var account = _db.Accounts.FirstOrDefault(a => a.id == id);

            if (account == null)
            {
                return HttpNotFound();
            }

            return PartialView(account);
        }
        public ActionResult managePosted(long id)
        {
            var account = _db.Accounts.FirstOrDefault(a => a.id == id);

            if (account == null)
            {
                return HttpNotFound();
            }

            return PartialView(account);
        }
        public ActionResult listPosted(long id)
        {
            var v = from n in _db.RoomInfoes
                    join r in _db.Accounts on n.accountId equals r.id
                    where n.hide == true && r.id == id
                    orderby n.datebegin descending
                    select n;
            return PartialView(v.ToList());
        }
    }
}