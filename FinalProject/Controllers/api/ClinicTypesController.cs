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
    public class ClinicTypesController : ApiController
    {
        private MyAppContext db = new MyAppContext();

        // GET: api/ClinicTypes
        public IQueryable<ClinicType> GetClinicTypes()
        {
            return db.ClinicTypes;
        }

        // GET: api/ClinicTypes/5
        [ResponseType(typeof(ClinicType))]
        public async Task<IHttpActionResult> GetClinicType(byte id)
        {
            ClinicType clinicType = await db.ClinicTypes.FindAsync(id);
            if (clinicType == null)
            {
                return NotFound();
            }

            return Ok(clinicType);
        }

        // PUT: api/ClinicTypes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutClinicType(byte id, ClinicType clinicType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != clinicType.ClinicTypeId)
            {
                return BadRequest();
            }

            db.Entry(clinicType).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClinicTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ClinicTypes
        [ResponseType(typeof(ClinicType))]
        public async Task<IHttpActionResult> PostClinicType(ClinicType clinicType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ClinicTypes.Add(clinicType);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ClinicTypeExists(clinicType.ClinicTypeId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = clinicType.ClinicTypeId }, clinicType);
        }

        // DELETE: api/ClinicTypes/5
        [ResponseType(typeof(ClinicType))]
        public async Task<IHttpActionResult> DeleteClinicType(byte id)
        {
            ClinicType clinicType = await db.ClinicTypes.FindAsync(id);
            if (clinicType == null)
            {
                return NotFound();
            }

            db.ClinicTypes.Remove(clinicType);
            await db.SaveChangesAsync();

            return Ok(clinicType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClinicTypeExists(byte id)
        {
            return db.ClinicTypes.Count(e => e.ClinicTypeId == id) > 0;
        }
    }
}