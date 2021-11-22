using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace AfricaRentCar.Controllers
{
    public class MailController : Controller
    {
        // GET: Mail
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string usermail, string sujet, string objet)
        {
            WebMail.Send(usermail, sujet, objet, null, null, null, true, null, null, null, null, null, null);
            ViewBag.msg = "email was sent successfully...";
            return View();
        }
    }
}