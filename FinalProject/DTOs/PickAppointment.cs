using System;

namespace FinalProject.DTOs
{
    public class PickAppointment
    {
        public int? clinicId { get; set; }
        public int? userId { get; set; }
        public DateTime date { get; set; }
        public string time { get; set; }
    }
}
