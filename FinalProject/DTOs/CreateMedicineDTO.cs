using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTOs
{
    public class MedicineDTO
    {
        public int MedicineId { get; set; }
        [Required]
        [StringLength(255)]
        public string NameAR { get; set; }

        [Required]
        [StringLength(255)]
        public string NameEN { get; set; }

        public byte MedicineTypeId { get; set; }

        public bool IsActiveMedicine { get; set; }
    }
}