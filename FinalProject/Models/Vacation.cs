﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class Vacation
    {
        public virtual Clinic ClinicFrom { get; set; }
        [Key]
        [Column(Order = 1)]
        public int ClinicId { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        [StringLength(255)]
        public string Statue { get; set; }
    }
}