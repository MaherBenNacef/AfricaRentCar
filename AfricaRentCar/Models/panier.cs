using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AfricaRentCar.Models
{
    public class panier
    {
        public int id { get; set; }
        public int nombre_jours { get; set; }
        public voiture voiture { get; set; }
        public DateTime date_location = DateTime.Now;
        public ApplicationUser user { get; set; }
        public float Montant()
        {
            return nombre_jours * voiture.prix;
        }
    }
}