using FinalProject.Models;
using System.Collections.Generic;

namespace FinalProject.DTOs
{
    public class SignUpDoctor: SignUpUser
    {
        public byte UserTypeId { get; set; }

        public IEnumerable<Certifcate> Certifcates { get; set; }
    }
}