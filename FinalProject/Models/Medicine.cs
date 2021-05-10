using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace FinalProject.Models
{
    public class Medicine
    {
        public int MedicineId { get; set; }

        [Required]
        [StringLength(255)]
        public string NameAR { get; set; }

        [Required]
        [StringLength(255)]
        public string NameEN { get; set; }

        public MedicineType MedicineType { get; set; }
        public byte MedicineTypeId { get; set; }

        public bool IsActiveMedicine{ get; set; }

        public IEnumerable<PharmacyMedicines> PharmacyMedicines { get; set; }
        public IEnumerable<Prescription> Prescriptions { get; set; }
    }
}