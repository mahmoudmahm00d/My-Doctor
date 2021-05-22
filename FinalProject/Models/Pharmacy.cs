
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class Pharmacy
    {
        public int PharmacyId { get; set; }

        [Required]
        [StringLength(255)]
        public string PharmacyName { get; set; }

        public User ForUser { get; set; }
        public int UserId { get; set; }

        //[Required]
        //[StringLength(255)]
        //public string PharmacyLocation { get; set; }

        public double Langtude { get; set; }
        public double Latitude { get; set; }

        public string Certificate { get; set; }

        public bool IsActivePharmacy { get; set; }

        public IEnumerable<PharmacyMedicines> PharmacyMedicines { get; set; }
    }
}