using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class Location
    {
        public virtual Clinic ForClinic{ get; set; }
        [Key]
        [ForeignKey("ForClinic")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ClinicId { get; set; }

        public City FromCity { get; set; }
        public int CityId { get; set; }

        [StringLength(255)]
        public string Area { get; set; }

        [StringLength(255)]
        public string Street { get; set; }

        public double Longtude { get; set; }
        public double Latitude { get; set; }
    }
}