using System;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTOs
{
    public class PickAppointment
    {
        [Required]
        public int clinicId { get; set; }
        [Required]
        public int userId { get; set; }
        [Required]
        public DateTime date { get; set; }
        [Required]
        public string time { get; set; }
    }
}
