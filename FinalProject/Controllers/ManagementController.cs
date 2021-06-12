using AutoMapper;
using FinalProject.DTOs;
using FinalProject.Models;
using FinalProject.Services;
using FinalProject.ViewModels;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace FinalProject.Controllers
{
    public class ManagementController : Controller
    {
        private MyAppContext db = new MyAppContext();

        public ActionResult DownloadImage(string id)
        {
            if (SessionIsNull)
                return RedirectToAction("SignIn", "Management");

            id += ".png";
            string path = Path.Combine(Server.MapPath("~/Certificates/"), id);
            Image img;
            try
            {
                img = Image.FromFile(path);
            }
            catch
            {
                return HttpNotFound();
            }
            img.Dispose();
            return File(path, "Image/Png");
        }

        // GET: Management
        public ActionResult Index()//Sign Up Request By Default
        {
            if (SessionIsNull)
                return RedirectToAction("SignIn", "Management");

            return View();
        }


        // GET: Pharmacies
        public ActionResult Pharmacies()
        {
            if (SessionIsNull)
                return RedirectToAction("SignIn", "Management");

            return View();
        }

        // GET: Clinics
        public ActionResult Clinics()
        {
            if (SessionIsNull)
                return RedirectToAction("SignIn", "Management");

            return View();
        }

        public ActionResult DoctorDetails(int? id)
        {
            if (SessionIsNull)
                return RedirectToAction("SignIn", "Management");

            if (!id.HasValue)
                return RedirectToAction("Clinics");

            var doctor = db.Users.Find(id);
            if (doctor == null)
                return HttpNotFound();

            var certificates = db.Certifcates.Where(c => c.UserId == id.Value).Select(c => c);
            var model = Mapper.Map<User, Doctor>(doctor);
            model.Certificates = certificates;
            return View(model);
        }

        public ActionResult ClinicLocation(int? id)
        {
            if (SessionIsNull)
                return RedirectToAction("SignIn", "Management");

            if (!id.HasValue)
                return RedirectToAction("Clinics");

            var location = db.Locations.Include(l => l.FromCity).FirstOrDefault(l => l.ClinicId == id);
            if (location == null)
                return View(location);

            locationManagementViewModel model = new locationManagementViewModel
            {
                Location = Mapper.Map<Location, LocationDTO>(location),
                CityName = location.FromCity.CityName
            };

            return View(model);
        }

        #region Cities

        public ActionResult Cities()
        {
            if (SessionIsNull)
                return RedirectToAction("SignIn", "Management");

            return View();
        }

        public ActionResult AddCity()
        {
            if (SessionIsNull)
                return RedirectToAction("SignIn", "Management");

            return View();
        }

        [HttpPost]
        public ActionResult AddCity(CityDTO city)
        {
            if (SessionIsNull)
                return RedirectToAction("SignIn", "Management");

            if (!ModelState.IsValid)
                return View();

            var cityToDb = Mapper.Map<CityDTO, City>(city);
            db.Cities.Add(cityToDb);
            db.SaveChanges();
            return RedirectToAction("Cities");
        }

        public ActionResult EditCity(int? id)
        {
            if (SessionIsNull)
                return RedirectToAction("SignIn", "Management");

            if (!id.HasValue)
                return RedirectToAction("Cities");

            var city = db.Cities.Find(id);
            if (city == null)
                return HttpNotFound();

            return View(Mapper.Map<City, CityDTO>(city));
        }

        [HttpPost]
        public ActionResult EditCity(CityDTO city)
        {
            if (SessionIsNull)
                return RedirectToAction("SignIn", "Management");

            if (!ModelState.IsValid)
                return View();

            var cityInDb = db.Cities.Find(city.CityId);
            if (cityInDb == null)
                return HttpNotFound();

            cityInDb.CityName = city.CityName;
            cityInDb.IsActiveCity = city.IsActiveCity;
            db.SaveChanges();
            return RedirectToAction("Cities");
        }

        #endregion

        #region Medicines

        public ActionResult Medicines()
        {
            if (SessionIsNull)
                return RedirectToAction("SignIn", "Management");

            return View();
        }

        public ActionResult AddMedicine(string errorMessage = null)
        {
            if (SessionIsNull)
                return RedirectToAction("SignIn", "Management");


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
            if (SessionIsNull)
                return RedirectToAction("SignIn", "Management");

            if (!ModelState.IsValid)
            {
                return RedirectToAction("AddMedicine", new { errorMessage = "Invalid Properties" });
            }

            medicine = AppServices.TrimStringProperties(medicine);
            var medicineToDb = Mapper.Map<MedicineDTO, Medicine>(medicine);
            db.Medicines.Add(medicineToDb);
            db.SaveChanges();
            return RedirectToAction("Medicines");
        }

        public ActionResult EditMedicine(int? id, string errorMessage = null)
        {
            if (!id.HasValue)
                return RedirectToAction("Edit", new { errorMessage = "Bad Request" });

            if (SessionIsNull)
                return RedirectToAction("SignIn");


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

            if (SessionIsNull)
                return RedirectToAction("SignIn");

            if (!ModelState.IsValid)
            {
                return RedirectToAction("AddMedicine", new { errorMessage = "Invalid Properties" });
            }

            var medicineInDb = db.Medicines.FirstOrDefault(m => m.MedicineId == id);
            if (medicineInDb == null)
                return HttpNotFound();

            medicineInDb.NameAR = medicine.NameAR;
            medicineInDb.NameEN = medicine.NameEN;
            medicineInDb.MedicineTypeId = medicine.MedicineTypeId;
            medicineInDb.IsActiveMedicine = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion

        #region Account
        public ActionResult SignIn()
        {
            if (!SessionIsNull)
                return HttpNotFound();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(SignInUser user)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Incorrect Email Or Password");
                return View();
            }

            user.Email = user.Email.Trim().ToLower();

            var managerInDb = db.Managers.FirstOrDefault(m => m.ManagerEmail == user.Email);

            if (managerInDb == null)
            {
                ModelState.AddModelError("", "Incorrect Email Or Password");
                return View();
            }

            bool passwordVerified = AppServices.VerifayPasswrod(user.Password, managerInDb.ManagerPassword);

            if (!passwordVerified)
            {
                ModelState.AddModelError("", "Incorrect Email Or Password");
                return View();
            }

            Session["ManagerId"] = managerInDb.ManagerId;

            return RedirectToAction("Index");
        }

        //GET: Managements/EditPassword
        public ActionResult EditPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditPassword(EditPasswordDTO edit)
        {
            if (SessionIsNull)
                return RedirectToAction("SignIn", "Management");

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invaild Properties");
                return View();
            }

            int managerId = ManagerId;
            var manager = db.Managers.Find(managerId);
            if (manager == null)
                return RedirectToAction("SignIn");

            bool verfied = AppServices.VerifayPasswrod(edit.OldPassword,manager.ManagerPassword);
            if (!verfied)
            {
                ModelState.AddModelError("", "Incorrect password");
                return View();
            }

            manager.ManagerPassword = AppServices.HashPassword(edit.NewPassword);
            db.SaveChanges();
            return View();
        }

        //GET: Management/EditEmail
        public ActionResult EditEmail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditEmail(EditEmailDTO edit)
        {
            if (SessionIsNull)
                return RedirectToAction("SignIn", "Management");

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invaild Properties");
                return View();
            }

            int managerId = ManagerId;
            var manager = db.Managers.Find(managerId);
            if (manager == null)
                return RedirectToAction("SignIn");

            bool verfied = AppServices.VerifayPasswrod(edit.Password, manager.ManagerPassword);
            if (!verfied)
            {
                ModelState.AddModelError("", "Incorrect password");
                return View();
            }

            manager.ManagerEmail = edit.Email.Trim().ToLower();
            db.SaveChanges();
            return View();
        }

        public ActionResult SignOut()
        {
            Session.Clear();
            return RedirectToAction("SignIn");
        }

        #endregion

        #region Users


        public ActionResult Users()
        {
            if (SessionIsNull)
                return RedirectToAction("SignIn", "Management");

            return View();
        }

        #endregion

        private bool SessionIsNull => Session["ManagerId"] == null;
        private int ManagerId => int.Parse(Session["ManagerId"].ToString());
    }
}