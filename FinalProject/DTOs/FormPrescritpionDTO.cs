using FinalProject.Models;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTOs
{
    public class FormPrescriptionDTO
    {
        public int AppointmentId { get; set; }
        public int MedicineId { get; set; }
        [Range(0, 255)]
        public byte Dosage { get; set; }
        public TimeSpans Every { get; set; }
        [Range(0, 255)]
        public byte For { get; set; }
        public TimeSpans TimeSpan { get; set; }
    }
}