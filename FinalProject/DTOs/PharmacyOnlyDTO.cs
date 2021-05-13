using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinalProject.DTOs
{
    public class PharmacyOnlyDTO
    {
        public int PharmacyId { get; set; }

        [Required]
        [StringLength(255)]
        public string PharmacyName { get; set; }

        [Required]
        [StringLength(255)]
        public string PharmacyLocation { get; set; }

        public bool IsActivePharmacy { get; set; }
    }
}