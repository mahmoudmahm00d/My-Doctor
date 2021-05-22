using AutoMapper;
using FinalProject.DTOs;
using FinalProject.Services;
using FinalProject.Models;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.Mvc;
using FinalProject.ViewModels;

namespace FinalProject.Controllers.api
{

    [System.Web.Mvc.Route("api/users")]
    public class UsersController : ApiController
    {
        private MyAppContext db = new MyAppContext();

        //api/get
        public IHttpActionResult Get()
        {
            return Ok(db.Users.Include(u => u.UserType).Select(Mapper.Map<User,UsersManageViewModel>));
        }
        //api/get/id
        public IHttpActionResult Get(int id)
        {
            var user = db.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        public IHttpActionResult Doctors()
        {
            return Ok(db.Users.Include(u => u.UserType).Select(Mapper.Map<User,UsersManageViewModel>));
        }

        public IHttpActionResult Doctors(int id)
        {
            var user = db.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        //api/userscount/
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/users/count")]
        public IHttpActionResult UsersCount()
        {
            return Ok(db.Users.Count());
        }
        //api/userscount/usertypeid
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/users/count/{id}")]
        public IHttpActionResult UsersCount(byte Id)
        {
            return Ok(db.Users.Where(u => u.UserTypeId == Id).Count());
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/users/signin")]
        public IHttpActionResult SignIn([FromBody]SignInUser user)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var userInDb = db.Users.Where(u => u.Locked == false)
                .FirstOrDefault(u => u.UserEmail == user.Email && u.UserTypeId == 10);//Public User

            if (userInDb == null)
                return NotFound();

            bool passwordVerified = AppServices.VerifayPasswrod(user.Password, userInDb.UserPassword);
            //ToDo
            //Send Token
            if (passwordVerified)
            {
                string userToken = AppServices.TokenEncoding(user.Email, user.Password);
                return Ok(new { userId = userInDb.UserId, token = userToken });
            }
            return NotFound();
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/users/signup")]
        public IHttpActionResult SignUp([FromBody]SignUpUser user)
        {
            if (!ModelState.IsValid || user.UserTypeId != 10)
                return BadRequest("Invalid properties");

            if (AppServices.CheckEmailIfExist(user.UserEmail))
                return BadRequest("Invalid Email");

            var userInDb = Mapper.Map<SignUpUser, User>(user);

            userInDb.UserPassword = AppServices.HashPassword(user.UserPassword);
            userInDb.VerCode = AppServices.GenerateRandomNumber();
            userInDb.UserTypeId = 10;
            userInDb.Locked = true;

            db.Users.Add(userInDb);
            db.SaveChanges();

            //ToDo
            //Send Email Code
            return Ok();
        }

        // For security
        


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/users/ForgetPassword")]
        public IHttpActionResult ForgetPassword(string email)
        {
            var user = db.Users.Where(u => u.Locked == false).FirstOrDefault(u => u.UserEmail == email);
            if (user == null)
                return NotFound();

            user.VerCode = AppServices.GenerateRandomNumber();
            db.SaveChanges();

            //ToDo
            //Add Email Service
            return Ok("Email Sent");
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/users/ResetPassword")]
        public IHttpActionResult ResetPassword(string email, string password, string confirmPassword)
        {
            if (password == confirmPassword)
                return BadRequest("Password Not Match");

            var user = db.Users.Where(u => u.Locked == false).FirstOrDefault(u => u.UserEmail == email);

            if (user == null)
                return NotFound();

            user.UserPassword = password;
            user.VerCode = string.Empty;
            db.SaveChanges();

            //ToDo
            //Send email

            return Ok("Password Reset");
        }

        [System.Web.Http.HttpPost]
        [UsersAuthentication]
        [System.Web.Http.Route("api/users/EditProfile")]
        public IHttpActionResult EditProfile([FromBody, Bind(Exclude = "Gender,UserId,UserTypeId")]UserDTO user)
        {
            var userdb = db.Users.Where(u => u.Locked == false).FirstOrDefault(u => u.UserId == user.UserId);
            if (userdb == null)
                return NotFound();

            userdb.FirstName = user.FirstName;
            userdb.FatherName = user.FatherName;
            userdb.LastName = user.LastName;
            userdb.Jop = user.Jop;

            db.SaveChanges();
            return Ok("Profile Edited");
        }
    }
}
