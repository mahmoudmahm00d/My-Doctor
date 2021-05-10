using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class ClinicType
    {
        public byte ClinicTypeId { get; set; }

        [Required]
        [StringLength(255)]
        public string ClinicTypeName { get; set; }

        public bool IsActiveClinicType { get; set; }

        IEnumerable<Clinic> Clinics { get; set; }
    }
}