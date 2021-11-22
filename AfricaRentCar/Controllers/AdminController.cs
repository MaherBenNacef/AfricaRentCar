using AfricaRentCar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AfricaRentCar.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "admin")]
        public ActionResult AdminLayout()
        {
            var users = db.Users.Count();
            var produits = db.voitures.Count();
            var commande = db.paniers.Count();
            //var factures = db.factures.Count();
            var gaint2020 = 20000;
            var gaint2021 = 22000;
            ViewBag.users = users;
            ViewBag.produits = produits;
            ViewBag.commande = commande;
            ViewBag.factures = 17000;
            ViewBag.gaitn2020 = gaint2020;
            ViewBag.gaint2021 = gaint2021;
            return View();
        }
    }
}