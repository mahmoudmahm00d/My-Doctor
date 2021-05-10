using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class MedicineType
    {
        public byte MedicineTypeId { get; set; }

        [Required]
        [StringLength(100)]
        public string MedicineTypeName { get; set; }

        public bool IsActiveMedicineType { get; set; }

        IEnumerable<User> Medicines { get; set; }
    }
}