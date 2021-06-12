using FinalProject.DTOs;
using System.Collections.Generic;

namespace FinalProject.ViewModels
{
    public class CreateMedicineViewModel
    {
        public MedicineDTO Medicine { get; set; }
        public IEnumerable<MedicineTypeDTO> MedicineTypes { get; set; }
    }
}