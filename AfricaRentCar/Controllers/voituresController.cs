using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AfricaRentCar.Models;
using Microsoft.AspNet.Identity;

namespace AfricaRentCar.Controllers
{
    public class voituresController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: voitures
        public ActionResult Index()
        {
            voiture voiture;
            foreach (var item in db.paniers.Include(c=>c.voiture).ToList())
            {
                if ((item.date_location.AddDays(item.nombre_jours).CompareTo(DateTime.Now))>=0)
                {
                    voiture = db.voitures.Find(item.voiture.id);
                    voiture.disponibilite = true;
                    db.Entry(voiture).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            List<voiture> voitures = new List<voiture>();
            foreach (var item in db.voitures.ToList())
            {
                if (item.disponibilite)
                {
                    voitures.Add(item);
                }
            }
            return View(voitures);
        }

        // GET: voitures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            voiture voiture = db.voitures.Find(id);
            if (voiture == null)
            {
                return HttpNotFound();
            }
            return View(voiture);
        }

        [Authorize(Roles = "responsable , admin")]
        // GET: voitures/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: voitures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "responsable , admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(voiture voiture)
        {
            var userId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                string filename = Path.GetFileNameWithoutExtension(voiture.ImageFile.FileName);
                string extension = Path.GetExtension(voiture.ImageFile.FileName);
                filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                voiture.url_image = "/Image/" + filename;
                filename = Path.Combine(Server.MapPath("/Image/"), filename);
                voiture.ImageFile.SaveAs(filename);
                voiture.user_Id = db.Users.Find(userId);
                db.voitures.Add(voiture);
                db.SaveChanges();
                ModelState.Clear();
                return RedirectToAction("Index");
            }

            return View(voiture);
        }
        [Authorize(Roles = "responsable , admin")]
        // GET: voitures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            voiture voiture = db.voitures.Find(id);
            if (voiture == null)
            {
                return HttpNotFound();
            }
            return View(voiture);
        }

        // POST: voitures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "responsable , admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,marque,modele,type,puissance_fiscale,puissance_chdin,energie,boite_vitesse,nombre_rapport,nombre_cylindres,disponibilite,prix,image,url_image,description")] voiture voiture)
        {
            if (ModelState.IsValid)
            {
                db.Entry(voiture).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(voiture);
        }
        [Authorize(Roles = "responsable , admin")]
        // GET: voitures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            voiture voiture = db.voitures.Find(id);
            if (voiture == null)
            {
                return HttpNotFound();
            }
            return View(voiture);
        }

        // POST: voitures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            voiture voiture = db.voitures.Find(id);
            db.voitures.Remove(voiture);
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
        public ActionResult add(int? id)
        {
            var produit = db.voitures.Find(id);
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            if (User.Identity.IsAuthenticated)
            {
                if (user.EmailConfirmed)
                {
                    panier ligneCommande = new panier();
                    ligneCommande.user = db.Users.Find(userId);
                    ligneCommande.voiture = db.voitures.Find(id);
                    produit.disponibilite = false;
                    ligneCommande.nombre_jours = 1;
                    db.paniers.Add(ligneCommande);
                    db.Entry(produit).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.messager = "Car add to cart successfully !";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View("ErrorConfirmation");
                }
            }

            else
            {
                return View("CreateAccount");
            }



        }
        [Authorize(Roles = "responsable")]
        public ActionResult stat()
        {
            int nbVente = 0;
            var userId = User.Identity.GetUserId();
            var nbProduits = db.voitures.Count();
            //int nbMyProduct = 0;
            List<panier> ligneCommandes = db.paniers.Include(l => l.user).Include(l => l.voiture).ToList();
            List<voiture> produits = db.voitures.Include(l => l.user_Id).ToList();
            List<voiture> produitsResponsable = new List<voiture>();
            foreach (var item in produits)
            {
                if (item.user_Id != null)
                {
                    if (item.user_Id.Id == userId)
                    {
                        produitsResponsable.Add(item);
                    }
                }
            }
            foreach (var item in ligneCommandes)
            {
                foreach (var i in produitsResponsable)
                {
                    if (item.voiture.id == i.id)
                    {
                        nbVente += 1;
                    }
                }
            }
            ViewBag.nbMyProduct = produitsResponsable.Count();
            ViewBag.nbProduits = nbProduits;
            ViewBag.nbVente = nbVente;

            return View();

        }
        [Authorize(Roles = "responsable")]
        public ActionResult MyCars()
        {
            List<voiture> produits = db.voitures.Include(l => l.user_Id).ToList();
            List<voiture> mesProduits = new List<voiture>();
            var userId = User.Identity.GetUserId();
            foreach (var item in produits)
            {
                if (item.user_Id != null)
                {
                    if (item.user_Id.Id.Equals(userId))
                    {
                        mesProduits.Add(item);
                    }
                }

            }
            return View(mesProduits);
        }
     
   
   
    }
}
