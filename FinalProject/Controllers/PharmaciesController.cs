using AutoMapper;
using FinalProject.DTOs;
using FinalProject.Models;
using FinalProject.Services;
using FinalProject.ViewModels;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinalProject.Controllers
{
    public class PharmaciesController : Controller
    {
        private MyAppContext db = new MyAppContext();

        // GET: Pharmacies
        public ActionResult Index()
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            int userId = int.Parse(Session["UserId"].ToString());
            var pharmacy = db.Pharmacies.FirstOrDefault(c => c.UserId == userId);
            if (pharmacy == null)
                return RedirectToAction("Create");

            Session["PharmacyId"] = pharmacy.PharmacyId;
            return View(pharmacy.IsActivePharmacy);
        }

        public ActionResult Location(string errorMessage = null)
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            int? pharmacyId = GetPharmacyId();
            var pharmacy = db.Pharmacies.FirstOrDefault(c => c.PharmacyId == pharmacyId.Value);

            if (pharmacy == null)
                return RedirectToAction("Create");

            var cities = db.Cities.Select(Mapper.Map<City, CityDTO>);
            CreatePharamcyViewModel model = new CreatePharamcyViewModel
            {
                Cities = cities,
                Pharmacy = Mapper.Map<Pharmacy, CreatePharmacyDTO>(pharmacy)
            };

            if (errorMessage != null)
            {
                ModelState.AddModelError("", errorMessage);
            }

            return View(model);
        }

        public ActionResult Location(CreatePharmacyDTO pharmacy)
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Location", "Longtude and latitude are requierd");
            }

            int? pharmacyId = GetPharmacyId();
            var pharmacyInDb = db.Pharmacies.FirstOrDefault(c => c.PharmacyId == pharmacyId.Value);
            if (pharmacyInDb == null)
                return RedirectToAction("Create");

            pharmacyInDb = Mapper.Map<CreatePharmacyDTO, Pharmacy>(pharmacy);
            pharmacyInDb.PharmacyId = pharmacyId.Value;
            return RedirectToAction("Management");
        }

        [HttpGet]
        public ActionResult Create(string errorMessage = null)
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            int userId = GetUserId();
            var pharmacy = db.Pharmacies.FirstOrDefault(c => c.UserId == userId);

            if (pharmacy != null)//Have pharmacy
                return RedirectToAction("Index");

            if (errorMessage != null)
            {
                ModelState.AddModelError("", errorMessage);
            }

            var cities = db.Cities.Select(Mapper.Map<City, CityDTO>);
            CreatePharamcyViewModel model = new CreatePharamcyViewModel { Cities = cities };
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreatePharmacyDTO Pharmacy, HttpPostedFileBase certificate)
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            if (!ModelState.IsValid || certificate == null || (certificate.ContentLength == 0 && certificate.ContentLength > 204800))//200KB
            {
                return RedirectToAction("Create", new { errorMessage = "Invalid properties" });
            }

            string extention = Path.GetExtension(certificate.FileName);
            if (extention != ".jpg" && extention != ".png")
            {
                return RedirectToAction("Create", new { errorMessage = "Invalid file type" });
            }

            var pharmacyInDB = Mapper.Map<CreatePharmacyDTO, Pharmacy>(Pharmacy);
            string path = Path.Combine(Server.MapPath("~/Certificates"), certificate.FileName);
            pharmacyInDB.UserId = int.Parse(Session["UserId"].ToString());
            certificate.SaveAs(path);
            pharmacyInDB.Certificate = path;
            pharmacyInDB.IsActivePharmacy = false;

            db.Pharmacies.Add(pharmacyInDB);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET: Pharmacies/AddMedicine
        public ActionResult AddMedicine(string errorMessage = null)
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            int? pharmacyId = GetPharmacyId();
            if (!pharmacyId.HasValue)
                return RedirectToAction("Create");

            var medincinesTypes = db.MedicineTypes.Select(Mapper.Map<MedicineType, MedicineTypeDTO>);
            CreateMedicineViewModel model = new CreateMedicineViewModel
            {
                MedicineTypes = medincinesTypes
            };

            if (errorMessage != null)
            {
                ModelState.AddModelError("", errorMessage);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult AddMedicine(MedicineDTO medicine)
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            int? pharmacyId = GetPharmacyId();
            if (!pharmacyId.HasValue)
                return RedirectToAction("Create");

            if (!ModelState.IsValid)
            {
                return RedirectToAction("AddMedicine", new { errorMessage = "Invalid Properties" });
            }

            medicine = AppServices.TrimStringProperties(medicine);
            var medicineToDb = Mapper.Map<MedicineDTO, Medicine>(medicine);
            db.Medicines.Add(medicineToDb);
            PharmacyMedicines pharmacyMedicines = new PharmacyMedicines
            {
                MedicineId = medicineToDb.MedicineId,
                PharmacyId = pharmacyId.Value,
                Available = true
            };
            db.PharmacyMedicines.Add(pharmacyMedicines);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AddMedicines(string errorMessage = null)
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            int? pharmacyId = GetPharmacyId();
            if (!pharmacyId.HasValue)
                return RedirectToAction("Create");

            var medincinesTypes = db.MedicineTypes.Select(Mapper.Map<MedicineType, MedicineTypeDTO>);
            CreateMedicineViewModel model = new CreateMedicineViewModel
            {
                MedicineTypes = medincinesTypes
            };

            if (errorMessage == null)
            {
                ModelState.AddModelError("", errorMessage);
            }

            return View(model);
        }

        //ToDo
        //AddMedicine Implemntation

        public ActionResult EditMedicine(int? id, string errorMessage = null)
        {
            if (!id.HasValue)
                return RedirectToAction("Edit", new { errorMessage = "Bad Request" });

            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            int? pharmacyId = GetPharmacyId();
            if (!pharmacyId.HasValue)
                return RedirectToAction("Create");

            var medicineInDb = db.Medicines.Find(id);
            if (medicineInDb == null)
                return HttpNotFound();

            var medincinesTypes = db.MedicineTypes.Select(Mapper.Map<MedicineType, MedicineTypeDTO>);
            CreateMedicineViewModel model = new CreateMedicineViewModel
            {
                MedicineTypes = medincinesTypes,
                Medicine = Mapper.Map<Medicine, MedicineDTO>(medicineInDb)
            };

            if (errorMessage != null)
            {
                ModelState.AddModelError("", errorMessage);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult EditMedicine(int? id, MedicineDTO medicine)
        {
            if (!id.HasValue)
                return RedirectToAction("Index");

            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            if (!ModelState.IsValid)
            {
                return RedirectToAction("AddMedicine", new { errorMessage = "Invalid Properties" });
            }

            int? pharmacyId = GetPharmacyId();
            if (!pharmacyId.HasValue)
                return RedirectToAction("Create");

            var medicineInDb = db.Medicines.FirstOrDefault(m => m.MedicineId == id);
            if (medicineInDb == null)
                return HttpNotFound();

            medicineInDb.NameAR = medicine.NameAR;
            medicineInDb.NameEN = medicine.NameEN;
            medicineInDb.MedicineTypeId = medicine.MedicineTypeId;
            medicineInDb.IsActiveMedicine = true;
            var pharmacyMedicine = db.PharmacyMedicines.FirstOrDefault(m => m.MedicineId == id && m.PharmacyId == pharmacyId.Value);
            pharmacyMedicine.Available = medicine.IsActiveMedicine;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Pharmacies
        public ActionResult Management()
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            int userId = GetUserId();
            int? pharmacyId = GetPharmacyId();

            var pharmacy = db.Pharmacies
                .Where(c => c.PharmacyId == pharmacyId)
                .Select(Mapper.Map<Pharmacy, PharmacyOnlyDTO>)
                .FirstOrDefault();

            if (pharmacy == null)
                return RedirectToAction("Create");

            var doctor = db.Users
                .Where(u => u.UserId == userId)
                .Select(Mapper.Map<User, Doctor>)
                .FirstOrDefault();

            var certificates = db.Certifcates.Where(c => c.UserId == userId).Select(c => c);

            doctor.Certificates = certificates;//All Certificates

            PharmacyManagementViewModel model = new PharmacyManagementViewModel
            {
                Doctor = doctor,
                Pharmacy = pharmacy
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Management(PharmacyManagementViewModel model)
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            int? clinicId = GetPharmacyId();
            if (!clinicId.HasValue)
                return RedirectToAction("Create");

            var pharmacy = db.Clinics
                .Include("ClinicType")
                .Where(c => c.ClinicId == clinicId)
                .FirstOrDefault();

            int doctorId = int.Parse(Session["UserId"].ToString());
            var doctor = db.Users
                .Where(u => u.UserId == doctorId)
                .FirstOrDefault();

            model.Doctor = AppServices.TrimStringProperties(model.Doctor);
            model.Pharmacy = AppServices.TrimStringProperties(model.Pharmacy);
            if (ModelState.IsValid)
            {
                doctor.FirstName = model.Doctor.FirstName;
                doctor.LastName = model.Doctor.LastName;
                doctor.FatherName = model.Doctor.FatherName;
                pharmacy.ClinicName = model.Pharmacy.PharmacyName;
                db.SaveChanges();
            }

            return RedirectToAction("Management");
        }

        public ActionResult AddCertificate()
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            int? pharmacyId = GetPharmacyId();
            if (!pharmacyId.HasValue)
                return RedirectToAction("Create");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCertificate(CertificateDTO certificate)
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            int? pharmacyId = GetPharmacyId();
            if (!pharmacyId.HasValue)
                return RedirectToAction("Create");

            if (!ModelState.IsValid && string.IsNullOrWhiteSpace(certificate.CertifcateDescription))
            {
                ModelState.AddModelError("", "Not valid");
                return View();
            }

            int doctorId = GetUserId();
            var certificteToDb = Mapper.Map<CertificateDTO, Certifcate>(certificate);
            certificteToDb.UserId = doctorId;
            db.Certifcates.Add(certificteToDb);
            db.SaveChanges();
            return RedirectToAction("Management");
        }


        //Get: Clinics/EditCertification/{CertificationId}
        public ActionResult EditCertificate(int? id)
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            int? pharmacyId = GetPharmacyId();
            if (!pharmacyId.HasValue)
                return RedirectToAction("Create");

            if (!id.HasValue)
                return RedirectToAction("Management");

            var certificate = db.Certifcates.Select(Mapper.Map<Certifcate, CertificateDTO>).FirstOrDefault(c => c.CertifcateID == id);
            if (certificate == null)
                return HttpNotFound();

            return View(certificate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCertificate(int? id, CertificateDTO certificate)
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            int? pharmacyId = GetPharmacyId();
            if (!pharmacyId.HasValue)
                return RedirectToAction("Create");

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Not valid");
                return View();
            }

            if (!id.HasValue)
                return RedirectToAction("Management");

            var certificateInDb = db.Certifcates.FirstOrDefault(c => c.CertifcateID == id);
            if (certificateInDb == null)
            {
                return HttpNotFound();
            }
            int doctorId = GetUserId();
            certificateInDb.CertifcateDescription = certificate.CertifcateDescription;
            certificateInDb.UserId = doctorId;
            db.SaveChanges();
            return RedirectToAction("Management");
        }

        public ActionResult EditLocation()
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            int? pharmacyId = GetPharmacyId();
            if (!pharmacyId.HasValue)
                return RedirectToAction("Create");

            var pharmacy = db.Pharmacies.FirstOrDefault(p => p.PharmacyId == pharmacyId);
            if (pharmacy == null)
            {
                return HttpNotFound();
            }

            var cities = db.Cities.Where(c => c.IsActiveCity == true).Select(Mapper.Map<City, CityDTO>);
            CreatePharamcyViewModel model = new CreatePharamcyViewModel
            {
                Cities = cities,
                Pharmacy = Mapper.Map<Pharmacy, CreatePharmacyDTO>(pharmacy)
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult EditLocation(CreatePharmacyDTO pharmacy)
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            int? pharmacyId = GetPharmacyId();
            if (!pharmacyId.HasValue)
                return RedirectToAction("Create");

            if (pharmacy.Longtude == 0 || pharmacy.Latitude ==0)
            {
                ModelState.AddModelError("", "Invalid Location");
                return RedirectToAction("Location");
            }

            var pharmacyInDb = db.Pharmacies.FirstOrDefault(l => l.PharmacyId == pharmacyId);
            pharmacyInDb.Latitude = pharmacy.Latitude;
            pharmacyInDb.Longtude = pharmacy.Longtude;
            pharmacyInDb.PharmacyId = pharmacyId.Value;
            if (pharmacyInDb == null)
            {
                return HttpNotFound();
            }
            db.SaveChanges();
            return RedirectToAction("Management");
        }

        //Utilties
        private bool SessionIsNull() => Session["UserId"] == null;

        private int GetUserId() => int.Parse(Session["UserId"].ToString());

        private int? GetPharmacyId() => int.Parse(Session["PharmacyId"].ToString());
    }
}