using FinalProject.Models;

namespace FinalProject.DTOs
{
    public class FormPrescritpionDTO
    {
        public int AppointmentId { get; set; }
        public int MedicineId { get; set; }
        public byte Dosage { get; set; }
        public TimeSpans Every { get; set; }
        public byte For { get; set; }
        public TimeSpans TimeSpan { get; set; }
    }
}