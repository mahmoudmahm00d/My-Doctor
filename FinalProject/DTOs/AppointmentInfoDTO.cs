using System.Collections.Generic;

namespace FinalProject.DTOs
{
    public class AppointmentInfoDTO
    {
        public AppointmentDTO Appointment { get; set; }
        public List<PrescriptionDTO> Prescriptions { get; set; }
    }
}