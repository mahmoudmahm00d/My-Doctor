using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTOs
{
    public class CityDTO
    {
        public int CityId { get; set; }

        [Required]
        [StringLength(50)]
        public string CityName { get; set; }

        public bool IsActiveCity { get; set; }
    }
}