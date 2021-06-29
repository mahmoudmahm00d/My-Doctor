using System;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTOs
{
    public class VisitListItemDTO
    {
        public int ClinicId { get; set; }
        public string ClinicName { get; set; }
        public string Doctor { get; set; }
        public string Symptoms { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}