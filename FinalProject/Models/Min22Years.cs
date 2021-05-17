using AutoMapper;
using FinalProject.DTOs;
using System;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class Min22YearsIfADocotor : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var user = (SignUpDoctor)validationContext.ObjectInstance;
            //var user =  Mapper.Map<object,User>(validationContext.ObjectInstance) ;

            if (user.UserTypeId == 10)
                return ValidationResult.Success;

            var age = DateTime.Today.Year - user.Birth.Year;
            return (age >= 22) ? ValidationResult.Success : new ValidationResult("Doctor shuold be at least 22 years old");
        }
    }
}