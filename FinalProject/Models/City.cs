using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class City
    {
        public int CityId { get; set; }

        [Required]
        [StringLength(50)]
        public string CityName { get; set; }
        
        public bool IsActiveCity { get; set; }

        public IEnumerable<Location> Locations { get; set; }
    }
}
