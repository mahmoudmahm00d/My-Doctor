using FinalProject.Filters;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace FinalProject.DTOs
{
    public class CreateClinicDTO
    {
        [Required]
        public byte ClinicTypeId { get; set; }

        [Required]
        [MaxLength(200)]
        public string ClinicName { get; set; }

        [PhoneNumber]
        public string ClinicPhone { get; set; }

        [EmailAddress]
        public string ClinicEmail { get; set; }
    }
}