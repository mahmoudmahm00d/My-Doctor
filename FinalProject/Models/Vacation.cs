using System;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class Vacation
    {
        public int VacationId { get; set; }

        public virtual Clinic ClinicFrom { get; set; }
        public int ClinicId { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        [StringLength(255)]
        public string Statue { get; set; }
    }
}