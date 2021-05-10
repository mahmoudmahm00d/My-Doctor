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
using Graduate.Models;

namespace Graduate.Controllers.api
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
        [ResponseType(typeof(UserType))]
        public async Task<IHttpActionResult> GetUserType(byte id)
        {
            UserType UserType = await db.Usertypes.FindAsync(id);
            if (UserType == null)
            {
                return NotFound();
            }

            return Ok(UserType);
        }

        // PUT: api/UserTypes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUserType(byte id, UserType userType)
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
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        //ToDo
        /*
        // POST: api/UserTypes
        [ResponseType(typeof(UserType))]
        public async Task<IHttpActionResult> PostUserType(UserType UserType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Usertypes.Add(UserType);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (db.Usertypes.Count(e => e.UserTypeId == UserType.UserTypeId) > 0)
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = UserType.UserTypeId }, UserType);
        }
        */

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