using AutoMapper;
using FinalProject.Authentication;
using FinalProject.DTOs;
using FinalProject.Models;
using FinalProject.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace FinalProject.Controllers.api
{
    public class PharmaciesController : ApiController
    {
        private MyAppContext db = new MyAppContext();

        [HttpGet]
        [Route("api/pharmacies/Medicines/{id}")]//PhramacyId
        [PharmacyAuthentication]
        public IHttpActionResult Get(int id)
        {
            if (!IsPharmacyIdBelongsToToken(id))
                return BadRequest();

            var medicines = db.PharmacyMedicines.Include(p => p.MedicineFrom).Include(m => m.MedicineFrom.MedicineType).Where(p => p.PharmacyId == id).Select(p => p).ToList();
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

        [HttpPost]
        [Route("api/pharmacies/addMedicine/{id}")]//PhramacyId
        [PharmacyAuthentication]
        public IHttpActionResult AddMedicines(int id, MedicinesId data)
        {
            if (!IsPharmacyIdBelongsToToken(id))
                return BadRequest();

            var pharmacy = db.Pharmacies.Find(id);
            if (pharmacy == null || data == null || data.medicineIds.Count() == 0)
                return BadRequest();

            data.medicineIds = data.medicineIds.Distinct();

            foreach (var item in data.medicineIds)
            {
                try
                {
                    int medicineId = Convert.ToInt32(item);
                    db.PharmacyMedicines.Add(new PharmacyMedicines { PharmacyId = id, MedicineId = medicineId });
                }
                catch
                {

                }
            }
            db.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("api/pharmacies/Medicines/{pharmacyId}/{id}")]//PhramacyId , MedicineId
        [PharmacyAuthentication]
        public IHttpActionResult DeleteMedicine(int id, int pharmacyId)
        {
            if (!IsPharmacyIdBelongsToToken(pharmacyId))
                return BadRequest();

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

        public bool IsPharmacyIdBelongsToToken(int? pharmacyId)
        {
            int? tokenPharmacyId = AppServices.GetPharmacyIdFromToken(GetToken);

            if (tokenPharmacyId != pharmacyId)
                return false;
            return true;
        }

        [HttpGet]
        [Route("api/pharmacies/Medicines")]//PhramacyId
        [UsersAuthentication]
        public IHttpActionResult GetPharmacyHaveMedicines(MedicinesId medicinesId)
        {

            if (medicinesId.medicineIds.Count() == 0)
                return BadRequest();
            
            //All Pharmacies Ids That Have Any Of These medicines 
            var medicines = db.PharmacyMedicines
                .Include(p => p.MedicineFrom)
                .Include(p => p.PharmacyFrom)
                .Include(m => m.MedicineFrom.MedicineType)
                .ToList()
                .Where(pm => medicinesId.medicineIds.Contains(pm.MedicineId))
                .Select(p => p).ToList();

            if (medicines.Count < 1)
                return Ok(medicines);//Empty list

            Dictionary<int, List<MedicineToAppDTO>> PharmacyMedicines = new Dictionary<int, List<MedicineToAppDTO>>();

            for (int i = 0; i < medicines.Count; i++)
            {
                int pharcmacyId = medicines[i].PharmacyId;
                if (!PharmacyMedicines.ContainsKey(pharcmacyId))//no pharmacy id like this
                {
                    PharmacyMedicines.Add(pharcmacyId, new List<MedicineToAppDTO>());
                }
                PharmacyMedicines[pharcmacyId]
                        .Add(new MedicineToAppDTO
                        {
                            MedicineId = medicines[i].MedicineId,
                            NameAR = medicines[i].MedicineFrom.NameAR,
                            NameEN = medicines[i].MedicineFrom.NameEN,
                            MedicineType = medicines[i].MedicineFrom.MedicineType.MedicineTypeName,
                        });

            }

            return Ok(PharmacyMedicines.ToList());
        }

        public string GetToken => Request.Headers.Authorization.Parameter;
    }




    public class MedicinesId
    {
        public IEnumerable<int> medicineIds { get; set; }
    }
}
