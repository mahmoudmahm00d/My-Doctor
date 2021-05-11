using FinalProject.Models;
using System.Collections.Generic;

namespace FinalProject.DTOs
{
    public class SignUpDoctor: SignUpUser
    {
        public IEnumerable<Certifcate> Certifcates { get; set; }
    }
}