using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using FinalProject.Models;

namespace FinalProject.Controllers.api
{
    public class UserTypesController : ApiController
    {
        private MyAppContext db = new MyAppContext();

        // GET: api/UserTypes
        public IHttpActionResult GetUserTypes()
        {
            return Ok(db.Usertypes.ToList());
        }

        // GET: api/UserTypes/5
        public IHttpActionResult GetUserType(byte id)
        {
            UserType UserType =  db.Usertypes.Find(id);
            if (UserType == null)
            {
                return NotFound();
            }
            return Ok(UserType);
        }

        // PUT: api/UserTypes/5
        public  IHttpActionResult PutUserType(byte id, UserType userType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var type = db.Usertypes.FirstOrDefault(t => t.UserTypeId == id);
            if (type == null)
                return NotFound();

            if (id != userType.UserTypeId)
            {
                return BadRequest();
            }

            type.IsActiveUserType = userType.IsActiveUserType;
            type.UserTypeName = userType.UserTypeName;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {

            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}