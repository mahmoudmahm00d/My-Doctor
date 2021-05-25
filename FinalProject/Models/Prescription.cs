using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class Prescription
    {
        public Appointment Appointment { get; set; }
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AppointmentId { get; set; }

        public Medicine Medicine { get; set; }
        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MedicineId { get; set; }
        public byte Dosage { get; set; }
        public TimeSpans Every { get; set; }
        public byte For { get; set; }
        public TimeSpans TimeSpan { get; set; }
        public bool Completed { get; set; }
    }

    public enum TimeSpans
    {
        Day = 0, Week, Month, Year
    }
}