using FinalProject.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTOs
{
    public class PharmacyDTO
    {
        public int PharmacyId { get; set; }

        [Required]
        [StringLength(255)]
        public string PharmacyName { get; set; }

        public UserDTO ForUser { get; set; }

        [Required]
        [StringLength(255)]
        public string PharmacyLocation { get; set; }

        public bool IsActivePharmacy { get; set; }

        public IEnumerable<PharmacyMedicines> PharmacyMedicines { get; set; }
    }
}