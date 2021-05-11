using AutoMapper;
using FinalProject.DTOs;
using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.Mvc;

namespace FinalProject.Controllers.api
{
    public class UsersController : ApiController
    {
        private MyAppContext db = new MyAppContext();

        //api/get
        public IHttpActionResult Get()
        {
            return Ok(db.Users.ToList());
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
        //api/getusers/usertypeid
        public IHttpActionResult GetUsers(byte userTypeId)
        {
            var users = db.Users.Select(u => u.UserTypeId == userTypeId);
            if (users == null)
                return NotFound();

            return Ok(users.ToList());
        }
        //api/userscount/
        public IHttpActionResult UsersCount()
        {
            return Ok(db.Users.Count());
        }
        //api/userscount/usertypeid
        public IHttpActionResult UsersCount(byte userTypeId)
        {
            var users = db.Users.Select(u => u.UserTypeId == userTypeId);
            if (users == null)
                return NotFound();

            return Ok(users.Count());
        }
        //api/login
        public IHttpActionResult Login(string email, string password)
        {
            var user = db.Users.FirstOrDefault(u => u.UserEmail == email && u.UserPassword == password);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        //api/signup
        public IHttpActionResult SignUp(SignUpUser user)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Format");

            if (CheckEmailIfExist(user.UserEmail))
                return BadRequest("Invalid Email");

            var application = new HttpApplication();
            var userdb = Mapper.Map<SignUpUser, User>(user);

            userdb.VerCode = GenerateRandomNumber();
            userdb.UserTypeId = (byte)application.Application["PublicUser"];

            db.Users.Add(userdb);
            db.SaveChanges();
            user.UserId = userdb.UserId;
            //ToDo
            //Send Email Code
            return Ok(user);
        }
        //api/ConfirmEmail?email=email&code=code
        public IHttpActionResult ConfirmEmail(string email, string code)
        {
            var user = db.Users.FirstOrDefault(u => u.UserEmail == email);
            if (user == null)
                return NotFound();

            if (user.VerCode == code)
            {
                user.Locked = false;
                user.VerCode = string.Empty;
                db.SaveChanges();
                return Ok("Confirmed");
            }
            return BadRequest("Code Not Match");
        }

        public IHttpActionResult ForgetPassword(string email)
        {
            var user = db.Users.FirstOrDefault(u => u.UserEmail == email);
            if (user == null)
                return NotFound();

            user.VerCode = GenerateRandomNumber();
            db.SaveChanges();
            return Ok("Email Send");
        }

        public IHttpActionResult ResetPassword(string email, string password, string confirmPassword)
        {
            if (password == confirmPassword)
                return BadRequest("Password Not Match");

            var user = db.Users.FirstOrDefault(u => u.UserEmail == email);
            if (user == null)
                return NotFound();
            user.UserPassword = password;
            user.VerCode = string.Empty;
            db.SaveChanges();
            return Ok("Password Reset");
        }

        public IHttpActionResult Editprofile([Bind(Exclude = "Gender,UserId,UserTypeId")]UserDTO user)
        {
            var userdb = db.Users.FirstOrDefault(u => u.UserId == user.UserId);
            if (userdb == null)
                return NotFound();

            userdb.FirstName = user.FirstName;
            userdb.FatherName = user.FatherName;
            userdb.LastName = user.LastName;
            userdb.Jop = user.Jop;

            db.SaveChanges();
            return Ok("Profile Edited");
        }

        public static bool CheckEmailIfExist(string email)
        {
            using (MyAppContext db = new MyAppContext())
            {
                int count = db.Users.Select(u => u.UserEmail == email).Count();
                return count == 0;
            }
        }
        public static string GenerateRandomNumber()
        {
            Random random = new Random();
            string code = "";
            for (int i = 0; i < 6; i++)
            {
                code += random.Next(0, 10).ToString();
            }
            return code;
        }
    }
}
