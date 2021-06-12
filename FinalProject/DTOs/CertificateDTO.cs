using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTOs
{
    public class CertificateDTO
    {
        public int CertifcateID { get; set; }

        public int UserId { get; set; }

        [Required]
        public string CertifcateDescription { get; set; }
    }
}