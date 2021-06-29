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

        public double Longtude { get; set; }
        public double Latitude { get; set; }

        public bool IsActivePharmacy { get; set; }
    }
}