using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTOs
{
    public class ClinicTypeDTO
    {
        public byte ClinicTypeId { get; set; }

        [Required]
        [StringLength(255)]
        public string ClinicTypeName { get; set; }

        public bool IsActiveClinicType { get; set; }
    }
}