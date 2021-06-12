using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTOs
{
    public class CreatePharmacyDTO
    {
        [Required]
        [StringLength(255)]
        public string PharmacyName { get; set; }

        public int UserId { get; set; }
        public int CityId { get; set; }
        public double Longtude { get; set; }
        public double Latitude { get; set; }
    }
}