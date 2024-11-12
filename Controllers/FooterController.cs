using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HoiTroWebsite.Models;

namespace HoiTroWebsite.Controllers
{
    public class FooterController : Controller
    {
        // Khai báo db
        HoiTroEntities _db = new HoiTroEntities();
        // GET: Footer
        public ActionResult Index()
        {
            return View();
        }
        // footer
        public ActionResult getFooter()
        {
            var v = from i in _db.Footers
                    where i.hide == true
                    orderby i.order ascending
                    select i;
            return PartialView(v.ToList().FirstOrDefault());
        }
        public ActionResult getCompanyInfo()
        {
            var v = _db.Footers.FirstOrDefault();

            if (v == null)
            {
                return HttpNotFound("Không tìm thấy Footer nào.");
            }
            return PartialView(v);
        }
        public ActionResult getPrivacyPolicyLink()
        {
            var v = _db.Footers.FirstOrDefault();

            if (v == null)
            {
                return HttpNotFound("Không tìm thấy Footer nào.");
            }
            return PartialView(v);
        }
        public ActionResult getTermsOfServiceLink()
        {
            var v = _db.Footers.FirstOrDefault();

            if (v == null)
            {
                return HttpNotFound("Không tìm thấy Footer nào.");
            }
            return PartialView(v);
        }
        public ActionResult getTermsOfUseLink()
        {
            var v = _db.Footers.FirstOrDefault();

            if (v == null)
            {
                return HttpNotFound("Không tìm thấy Footer nào.");
            }
            return PartialView(v);
        }
        public ActionResult getTermsOfPostingLink()
        {
            var v = _db.Footers.FirstOrDefault();

            if (v == null)
            {
                return HttpNotFound("Không tìm thấy Footer nào.");
            }
            return PartialView(v);
        }
    }
}