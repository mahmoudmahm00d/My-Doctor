using FinalProject.DTOs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace FinalProject.ViewModels
{
    public class CreateClinicViewModel
    {
        public ClinicOnlyDTO Clinic { get; set; }

        public IEnumerable<ClinicTypeDTO> Clinics { get; set; }

        [Required]
        public HttpPostedFileBase Certificate { get; set; }
    }
}