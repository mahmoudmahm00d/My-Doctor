using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class TokenProperties
    {
        [Key]
        [Column(Order = 1)]
        public int UserId { get; set; }
        [Key]
        [Column(Order = 2)]
        public string Token { get; set; }
        public DateTime ExpireDate { get; set; }
        public int? ObjectId { get; set; }
        public string ObjectType { get; set; }
    }
}