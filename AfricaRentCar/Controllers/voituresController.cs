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
            return View(db.voitures.ToList());
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

        // GET: voitures/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: voitures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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
    }
}
