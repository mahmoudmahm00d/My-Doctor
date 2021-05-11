using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using FinalProject.Models;

namespace FinalProject.Controllers.api
{
    public class MedicineTypesController : ApiController
    {
        private MyAppContext db = new MyAppContext();

        // GET: api/MedicineTypes
        
        public IHttpActionResult GetMedicineTypes()
        {
            return Ok(db.MedicineTypes.ToList());
        }

        // GET: api/MedicineTypes/5
        [ResponseType(typeof(MedicineType))]
        public async Task<IHttpActionResult> GetMedicineType(byte id)
        {
            MedicineType medicineType = await db.MedicineTypes.FindAsync(id);
            if (medicineType == null)
            {
                return NotFound();
            }

            return Ok(medicineType);
        }

        // PUT: api/MedicineTypes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMedicineType(byte id, MedicineType medicineType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var type = db.MedicineTypes.FirstOrDefault(t => t.MedicineTypeId == id);
            if (type == null)
                return NotFound();

            if (id != medicineType.MedicineTypeId)
            {
                return BadRequest();
            }

            type.IsActiveMedicineType = medicineType.IsActiveMedicineType;
            type.MedicineTypeName = medicineType.MedicineTypeName;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/MedicineTypes
        [ResponseType(typeof(MedicineType))]
        public async Task<IHttpActionResult> PostMedicineType(MedicineType medicineType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MedicineTypes.Add(medicineType);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (db.MedicineTypes.Count(e => e.MedicineTypeId == medicineType.MedicineTypeId) > 0)
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = medicineType.MedicineTypeId }, medicineType);
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