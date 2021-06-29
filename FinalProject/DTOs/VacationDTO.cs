using System;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTOs
{
    public class VacationDTO
    {
        public int VacationId { get; set; }

        public int ClinicId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FromDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ToDate { get; set; }

        [StringLength(255)]
        public string Statue { get; set; }
    }
}