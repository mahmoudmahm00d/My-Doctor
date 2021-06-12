using AutoMapper;
using FinalProject.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FinalProject.Filters
{
    public class PhoneNumber : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var user = Mapper.Map<object, User>(validationContext.ObjectInstance);
            var type = validationContext.ObjectInstance.GetType().ToString();

            Regex regex = new Regex("^\\+9639[0-9]{8}");
            if (user.UserPhone == null)
            {
                if (type == "FinalProject.DTOs.CreateClinicDTO")
                    return ValidationResult.Success;

                return new ValidationResult("Phone number can not be empty");
            }
            return (regex.IsMatch(user.UserPhone)) ? ValidationResult.Success : new ValidationResult("Invalid phone number");
        }
    }
}