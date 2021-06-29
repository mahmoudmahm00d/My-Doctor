using FinalProject.Models;

namespace FinalProject.DTOs
{
    public class PrescriptionDTO
    {
        public string CompositId { get; set; }
        public string MedicineNameAr { get; set; }
        public string MedicineNameEn { get; set; }
        public string MedicineType { get; set; }
        public byte Dosage { get; set; }
        public TimeSpans Every { get; set; }
        public byte For { get; set; }
        public TimeSpans Timespan { get; set; }
    }
}