using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTOs
{
    public class MedicineTypeDTO
    {
        public byte MedicineTypeId { get; set; }

        [Required]
        [StringLength(100)]
        public string MedicineTypeName { get; set; }

        public bool IsActiveMedicineType { get; set; }
    }
}