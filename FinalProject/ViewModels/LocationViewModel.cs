using FinalProject.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject.ViewModels
{
    public class LocationViewModel
    {
        public LocationDTO Location { get; set; }
        public IEnumerable<CityDTO> Cities { get; set; }
    }
}