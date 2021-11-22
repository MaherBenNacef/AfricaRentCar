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
    public class paniersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: paniers
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            List<panier> ligneCommandes = db.paniers.Include(l => l.voiture).Include(l => l.user).ToList();
            List<panier> ligneCommandesUser = new List<panier>();
            foreach (var item in ligneCommandes)
            {
                if (item.user != null && item.user.Id.Equals(userId))
                {
                    ligneCommandesUser.Add(item);
                }
            }
            return View(ligneCommandesUser);
        }
        // GET: paniers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            panier panier = db.paniers.Find(id);
            if (panier == null)
            {
                return HttpNotFound();
            }
            return View(panier);
        }

        // GET: paniers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: paniers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre_jours")] panier panier)
        {
            if (ModelState.IsValid)
            {
                db.paniers.Add(panier);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(panier);
        }

        // GET: paniers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            panier panier = db.paniers.Find(id);
            if (panier == null)
            {
                return HttpNotFound();
            }
            return View(panier);
        }

        // POST: paniers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre_jours")] panier panier)
        {
            if (ModelState.IsValid)
            {
                db.Entry(panier).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(panier);
        }

        // GET: paniers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            panier panier = db.paniers.Find(id);
            if (panier == null)
            {
                return HttpNotFound();
            }
            return View(panier);
        }

        // POST: paniers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            panier panier = db.paniers.Find(id);
            db.paniers.Remove(panier);
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
