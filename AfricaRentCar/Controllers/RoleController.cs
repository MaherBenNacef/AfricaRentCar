using AfricaRentCar.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AfricaRentCar.Controllers
{
        public class RoleController : Controller
        {
            ApplicationDbContext context;

            public RoleController()
            {
                context = new ApplicationDbContext();
            }
            [Authorize(Roles = "admin")]
            public ActionResult Index()
            {
                var Roles = context.Roles.ToList();
                return View(Roles);
            }
            [Authorize(Roles = "admin")]
            public ActionResult Create()
            {
                var Role = new IdentityRole();
                return View(Role);
            }
            [HttpPost]
            [Authorize(Roles = "admin")]
            public ActionResult Create(IdentityRole Role)
            {
                context.Roles.Add(Role);
                context.SaveChanges();
                return RedirectToAction("Index");
            }


        }
    
}