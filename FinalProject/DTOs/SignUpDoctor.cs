using FinalProject.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTOs
{
    public class SignUpDoctor: SignUpUser
    {

        public IEnumerable<Certifcate> Certifcates { get; set; }
    }
}