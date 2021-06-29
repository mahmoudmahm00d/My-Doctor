using AutoMapper;
using FinalProject.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FinalProject.Filters
{
    public class StrongPassword : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var user = Mapper.Map<object, User>(validationContext.ObjectInstance);

            try
            {
                var password = validationContext.ObjectInstance.GetType().GetProperty("UserPassword").GetValue(validationContext.ObjectInstance).ToString();
                Regex r1 = new Regex("[0-9]");
                Regex r2 = new Regex("[A-Z]");

                return (r1.IsMatch(password) && r2.IsMatch(password))
                    ? ValidationResult.Success
                    : new ValidationResult("Password should contain at least 1 capital letter and 1 digit");
            }
            catch
            {
                return new ValidationResult("Password is requierd");
            }

            //if(string.IsNullOrEmpty(user.UserPassword))
            //    return new ValidationResult("Password is requierd");

            //Regex r1 = new Regex("[0-9]");
            //Regex r2 = new Regex("[A-Z]");

            //return (r1.IsMatch(user.UserPassword) && r2.IsMatch(user.UserPassword))
            //    ?ValidationResult.Success
            //    :new ValidationResult("Password should contain at least 1 capital letter and 1 digit");
        }
    }
}