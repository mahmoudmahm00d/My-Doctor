using AutoMapper;
using FinalProject.DTOs;
using FinalProject.Models;
using FinalProject.Services;
using FinalProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinalProject.Controllers
{
    public class AccountsController : Controller
    {
        private MyAppContext db = new MyAppContext();
        // GET: Accounts
        public ActionResult SignUp()
        {
            var userTypes = db.Usertypes.Where(u => u.UserTypeName != "PublicUser").Select(u => u);
            SignUpDoctorViewModel signUp = new SignUpDoctorViewModel { UserTypes = userTypes };
            return View(signUp);
        }

        [HttpPost]
        public ActionResult SignUp(SignUpDoctor doctor)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("SignUp");

            if (api.UsersController.CheckEmailIfExist(doctor.UserEmail))
            {
                ModelState.AddModelError("Email", "Invalid Email");
                return RedirectToAction("SignUp");
            }
            var doctorType = db.Usertypes.FirstOrDefault(u => u.UserTypeName == "Doctor");
            var PharmacistType = db.Usertypes.FirstOrDefault(u => u.UserTypeName == "Pharmacist");
            if (doctor.UserTypeId != doctor.UserTypeId
                && doctor.UserTypeId != PharmacistType.UserTypeId)
            {
                ModelState.AddModelError("UserType", "Invalid User Type");
                return RedirectToAction("SignUp");
            }
            doctor.UserPassword = AppServices.HashPassword(doctor.UserPassword);
            var userInDb = Mapper.Map<SignUpDoctor, User>(doctor);
            userInDb.VerCode = AppServices.GenerateRandomNumber();

            db.Users.Add(userInDb);
            db.SaveChanges();
            doctor.UserId = userInDb.UserId;
            //ToDo
            //Send Email Code
            return RedirectToAction("ConfirmUser", new { id = doctor.UserId });
        }

        public ActionResult ConfirmUser(int id)
        {
            var user = db.Users.Where(u => u.Locked == true).FirstOrDefault(u => u.UserId == id);
            if (user == null)
                return HttpNotFound();

            return View();
        }

        [HttpPost]
        public ActionResult ConfirmUser(int id, string code)
        {
            var user = db.Users.Where(u => u.Locked == true).FirstOrDefault(u => u.UserId == id);
            if (user == null)
                return HttpNotFound();

            if (user.VerCode == code)
            {
                user.Locked = false;
                RedirectToAction("SignIn");
            }
            ModelState.AddModelError("", "Code not match");
            return View();
        }

        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(SignInUser user)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Login", "Incorrect Email Or Password");
                return View();
            }

            UserType userType = db.Usertypes.FirstOrDefault(u => u.UserTypeName == "PublicUser");

            var userInDb = db.Users.Where(u => u.Locked == false).FirstOrDefault(u => u.UserEmail == user.Email
                && AppServices.VerifayPasswrod(user.Password, u.UserPassword)
                && u.UserTypeId != userType.UserTypeId);

            if (userInDb != null)
            {
                return RedirectToAction("Index", "Clinics");
            }

            ModelState.AddModelError("", "Incorrect email or password");
            return View();

        }
    }
}