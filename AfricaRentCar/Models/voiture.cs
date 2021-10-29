using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.ComponentModel;
namespace AfricaRentCar.Models
{
    public class voiture
    {
        public int id { get; set; }
        public string marque { get; set; }
        public string modele { get; set; }
        public string type { get; set; }
        public int puissance_fiscale { get; set; }
        public int puissance_chdin { get; set; }
        public string energie { get; set; }
        public string boite_vitesse { get; set; }
        public int nombre_rapport { get; set; }
        public int nombre_cylindres { get; set; }
        public ApplicationUser user_Id { get; set; }
        public Boolean disponibilite { get; set; }
        public float prix { get; set; }
        public string image { get; set; }
        [DisplayName("Upload image")]
        public string url_image { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
        public string description { get; set; }
       
    }
}