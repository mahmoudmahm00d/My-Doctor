using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTOs
{
    public class LocationDTO
    {

        public int CityId { get; set; }

        [StringLength(255)]
        public string Area { get; set; }

        [StringLength(255)]
        public string Street { get; set; }

        public double Langtude { get; set; }
        public double Latitude { get; set; }
    }
}