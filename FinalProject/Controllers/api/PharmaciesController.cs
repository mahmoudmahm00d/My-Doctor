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
        [Route("api/pharmacies/Medicines/{id}")]//PhramacyId
        public IHttpActionResult Get(int id)
        {
            var medicines = db.PharmacyMedicines.Include(p => p.MedicineFrom).Include(m=>m.MedicineFrom.MedicineType).Where(p => p.PharmacyId == id).Select(p => p).ToList();
            if (medicines.Count < 1)
                return Ok(medicines);

            var result = medicines.Select(m => new
            {
                medicineId = m.MedicineId,
                nameAR = m.MedicineFrom.NameAR,
                nameEN = m.MedicineFrom.NameEN,
                medicineType = m.MedicineFrom.MedicineType.MedicineTypeName,
                available = m.Available
            });

            return Ok(result);
        }

        [HttpDelete]
        [Route("api/pharmacies/Medicines/{pharmacyId}/{id}")]//PhramacyId
        public IHttpActionResult DeleteMedicine(int id, int pharmacyId)
        {
            var medicineInDb = db.PharmacyMedicines.Where(p => p.PharmacyId == pharmacyId).FirstOrDefault(p => p.MedicineId == id);
            if (medicineInDb == null)
                return NotFound();

            var medicine = db.PharmacyMedicines.Where(m => m.MedicineId == id).Select(m => m);
            if (medicine.Count() < 2)
            {
                var med = db.Medicines.FirstOrDefault(m => m.MedicineId == id);
                db.Medicines.Remove(med);
                db.SaveChanges();
                return Ok();
            }

            db.PharmacyMedicines.Remove(medicineInDb);
            db.SaveChanges();
            return Ok();
        }
    }
}
