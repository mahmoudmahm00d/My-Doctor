using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FinalProject.Models
{
    public class PharmacyMedicines
    {
        public Pharmacy PharmacyFrom { get; set; }
        [Key]
        [Column(Order = 1)]
        public int PharmacyId { get; set; }

        public Medicine MedicineFrom { get; set; }
        [Key]
        [Column(Order = 2)]
        public int MedicineId { get; set; }

        public bool Available { get; set; }
    }
}