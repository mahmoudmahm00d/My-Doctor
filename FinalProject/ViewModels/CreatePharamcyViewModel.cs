using FinalProject.DTOs;
using System.Collections.Generic;

namespace FinalProject.ViewModels
{
    public class CreatePharamcyViewModel
    {
        public CreatePharmacyDTO Pharmacy { get; set; }
        public IEnumerable<CityDTO> Cities { get; set; }
    }
}