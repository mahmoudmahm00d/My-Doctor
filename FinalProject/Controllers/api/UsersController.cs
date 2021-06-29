using AutoMapper;
using FinalProject.Authentication;
using FinalProject.DTOs;
using FinalProject.Models;
using FinalProject.Services;
using System;
using System.Linq;
using System.Web.Http;

namespace FinalProject.Controllers.api
{

    //[System.Web.Mvc.Route("api/users")]
    public class UsersController : ApiController
    {
        private MyAppContext db = new MyAppContext();

        #region Users
        //api/get/id
        [UsersAuthentication]
        [Route("api/users/{id}")]
        public IHttpActionResult Get(int id)
        {
            var user = db.Users
                .Where(u => u.UserTypeId == 10 && u.UserId == id)
                .Select(Mapper.Map<User, UserDTO>)
                .FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }
            
            return Ok(user);
        }
        #endregion

        [HttpPost]
        [Route("api/users/signin")]
        public IHttpActionResult SignIn([FromBody]SignInUser user)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var userInDb = db.Users.Where(u => u.Locked == false)
                .FirstOrDefault(u => u.UserEmail == user.Email && u.UserTypeId == 10);//Public User

            if (userInDb == null)
                return NotFound();

            bool passwordVerified = AppServices.VerifayPasswrod(user.Password, userInDb.UserPassword);
            if (passwordVerified)
            {
                var token = db.Tokens.FirstOrDefault(t => t.UserId == userInDb.UserId);
                string userToken = AppServices.TokenEncoding(user.Email, user.Password);
                TokenProperties tokenProperties = new TokenProperties
                {
                    UserId = userInDb.UserId,
                    Token = userToken,
                    ExpireDate = DateTime.Now.AddDays(15),
                    ObjectType ="Public User"
                };
                if (token == null)
                    db.Tokens.Add(tokenProperties);
                else
                    token.ExpireDate = tokenProperties.ExpireDate;

                db.SaveChanges();
                return Ok(new { userId = userInDb.UserId, token = userToken });
            }
            return NotFound();
        }

        [HttpPost]
        [Route("api/users/signup")]
        public IHttpActionResult SignUp([FromBody]SignUpUser user)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid properties");

            if (AppServices.CheckEmailIfExist(user.UserEmail))
                return BadRequest("Invalid Email");

            var userInDb = Mapper.Map<SignUpUser, User>(user);

            userInDb.UserPassword = AppServices.HashPassword(user.UserPassword);
            userInDb = AppServices.TrimStringProperties(userInDb);
            userInDb.UserEmail = userInDb.UserEmail.ToLower();
            userInDb.VerCode = AppServices.GenerateRandomNumber();
            userInDb.UserTypeId = 10;
            userInDb.Locked = true;

            db.Users.Add(userInDb);
            db.SaveChanges();
            AppServices.SendConfirmEmail(userInDb.UserEmail, userInDb.VerCode, userInDb.UserId);
            return Ok();
        }

        [HttpPost]
        [UsersAuthentication]
        [Route("api/users/EditPassword")]
        public IHttpActionResult ChangePassword([FromBody] ChangePasswordDTO data)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = db.Users.Where(u => u.Locked == false && u.UserId == data.UserId).FirstOrDefault();

            if (!AppServices.VerifayPasswrod(data.OldPassword, user.UserPassword))
                return BadRequest();

            user.UserPassword = AppServices.HashPassword(data.UserPassword);
            db.SaveChanges();

            return Ok("Password Reset");
        }

        [HttpPost]
        [UsersAuthentication]
        [Route("api/users/EditEmail")]
        public IHttpActionResult EditEmail([FromBody] ChangeEmailDTO data)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = db.Users.Where(u => u.Locked == false && u.UserId == data.UserId).FirstOrDefault();

            if (!AppServices.VerifayPasswrod(data.UserPassword, user.UserPassword))
                return BadRequest();

            user.UserEmail = data.Email.Trim().ToLower();
            db.SaveChanges();
            
            return Ok("Email Reset");
        }

        [HttpPost]
        [UsersAuthentication]
        [Route("api/users/EditProfile/{id}")]
        public IHttpActionResult EditProfile(int id, UserDTO user)
        {
            if (!IsSameUserId(id))//userId
                return BadRequest();

            var userdb = db.Users.Where(u => u.Locked == false).FirstOrDefault(u => u.UserId == id);
            if (userdb == null)
                return NotFound();
            userdb.FirstName = user.FirstName;
            userdb.FatherName = user.FatherName;
            userdb.LastName = user.LastName;
            userdb.Jop = user.Jop;
            db.SaveChanges();
            
            return Ok("Profile Edited");
        }

        public bool IsSameUserId(int? userId)
        {
            int? tokenUserId = AppServices.GetUserIdFromToken(GetToken);
            var user = db.Users.FirstOrDefault(a => a.UserId == userId);
            if (user == null || tokenUserId != user.UserId)
                return false;
            return true;
        }

        public string GetToken => Request.Headers.Authorization.Parameter;
    }
}
