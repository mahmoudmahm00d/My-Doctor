using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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
        public IHttpActionResult SignUp()
        {
            return Ok();
        }
    }
}
