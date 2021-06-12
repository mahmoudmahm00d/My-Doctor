using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTOs
{
    public class PharmacyOnlyDTO
    {
        public int PharmacyId { get; set; }

        [Required]
        [StringLength(255)]
        public string PharmacyName { get; set; }

        public bool IsActivePharmacy { get; set; }
    }
}