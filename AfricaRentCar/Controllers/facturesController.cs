using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AfricaRentCar.Models;
using Microsoft.AspNet.Identity;

namespace AfricaRentCar.Controllers
{
    public class facturesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: factures
        [Authorize(Roles = "client")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var fact = db.factures
                .Include(c => c.user).Where(c => c.user.Id == userId)
               .ToList();
            return View(fact);
        }
        [Authorize(Roles = "client")]
        public ActionResult calculFacture()
        {
            facture facture = new facture();
            float somme = 0;
            var userId = User.Identity.GetUserId();
            List<panier> ligneCommandes = db.paniers.Include(l => l.voiture).Include(l => l.user).ToList();
            List<panier> ligneCommandesUser = new List<panier>();
            foreach (var item in ligneCommandes)
            {
                if (item.user != null && item.user.Id.Equals(userId))
                {
                    ligneCommandesUser.Add(item);
                }
                else
                {

                }
            }
            if (userId != null)
            {

                facture.date = DateTime.Now;
                facture.user = db.Users.Find(userId);
                foreach (var item in ligneCommandesUser)
                {
                    somme += item.Montant() + somme;
                }
                facture.somme = somme;
                db.factures.Add(facture);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        [Authorize(Roles = "client")]
        public ActionResult Pay(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            facture facture = db.factures.Find(id);
            if (facture == null)
            {
                return HttpNotFound();
            }
            facture fact = db.factures.Find(id);
            db.factures.Remove(fact);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        // GET: factures/Details/5
        [Authorize(Roles = "client")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            facture facture = db.factures.Find(id);
            if (facture == null)
            {
                return HttpNotFound();
            }
            return View(facture);
        }

        // GET: factures/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: factures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,date,somme")] facture facture)
        {
            if (ModelState.IsValid)
            {
                db.factures.Add(facture);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(facture);
        }

        // GET: factures/Edit/5
        [Authorize(Roles = "client")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            facture facture = db.factures.Find(id);
            if (facture == null)
            {
                return HttpNotFound();
            }
            return View(facture);
        }

        // POST: factures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "client")]
        public ActionResult Edit([Bind(Include = "id,date,somme")] facture facture)
        {
            if (ModelState.IsValid)
            {
                db.Entry(facture).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(facture);
        }

        // GET: factures/Delete/5
        [Authorize(Roles = "client")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            facture facture = db.factures.Find(id);
            if (facture == null)
            {
                return HttpNotFound();
            }
            return View(facture);
        }

        // POST: factures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "client")]
        public ActionResult DeleteConfirmed(int id)
        {
            facture facture = db.factures.Find(id);
            db.factures.Remove(facture);
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
