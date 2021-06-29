using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTOs
{
    public class AppointmentDTO
    {
        public int AppointmentId { get; set; }

        public UserDTO User { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string Date { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public string Time { get; set; }

        public bool Confirmed { get; set; }

        [Required]
        [StringLength(255)]
        public string Symptoms { get; set; }

        [StringLength(255)]
        public string Remarks { get; set; }
    }
}