using AutoMapper;
using FinalProject.DTOs;
using FinalProject.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace FinalProject.Controllers.api
{
    public class PharmaciesController : ApiController
    {
        private MyAppContext db = new MyAppContext();

        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(db.Pharmacies.Include(c => c.ForUser).ToList().Select(Mapper.Map<Pharmacy, PharmacyDTO>));
        }

        [HttpGet]
        [Route("api/pharmaciesMedicines/{id}")]//PhramacyId
        public IHttpActionResult Get(int id)
        {
            var query = from pm in db.PharmacyMedicines
                        where pm.PharmacyId == id
                        join m in db.Medicines
                        on pm.MedicineId equals m.MedicineId
                        select new { m.MedicineId, m.NameAR, m.NameEN, pm.Available };

            return Ok(query);
        }

        [HttpPost]
        public IHttpActionResult Create(PharmacyDTO pharmacy)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Clinic Properties");

            int userId = db.Pharmacies.Select(c => c.UserId == pharmacy.ForUser.UserId).Count();

            if (userId > 1)
                return BadRequest();

            db.Pharmacies.Add(Mapper.Map<PharmacyDTO, Pharmacy>(pharmacy));
            return Created(Request.RequestUri.ToString(), pharmacy);
        }

        [HttpPut]
        public IHttpActionResult Edit(int id, PharmacyOnlyDTO pharmacy)
        {
            if (!ModelState.IsValid || pharmacy.PharmacyId != id)
                return BadRequest("Invalid Clinic Properties");

            var pharmacyInDb = db.Pharmacies.Find(id);

            if (pharmacyInDb == null)
                return NotFound();

            Mapper.Map(pharmacy, pharmacyInDb);
            db.SaveChanges();
            return Ok(pharmacy);
        }
    }
}
