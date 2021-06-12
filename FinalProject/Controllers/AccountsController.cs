using AutoMapper;
using FinalProject.DTOs;
using FinalProject.Models;
using FinalProject.Services;
using FinalProject.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace FinalProject.Controllers
{
    public class AccountsController : Controller
    {
        private MyAppContext db = new MyAppContext();

        public int UserId => int.Parse(Session["UserId"].ToString());
        public bool SessionIsNull => Session["UserId"] == null;

        // GET: Accounts
        public ActionResult SignUp()
        {
            var userTypes = db.Usertypes.Where(u => u.UserTypeName != "PublicUser").Select(u => u);

            SignUpDoctorViewModel signUp = new SignUpDoctorViewModel { UserTypes = userTypes };
            return View(signUp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(SignUpDoctor doctor)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("SignUp");

            if (AppServices.CheckEmailIfExist(doctor.UserEmail))
            {
                ModelState.AddModelError("", "Invalid Email");
                return RedirectToAction("SignUp");
            }
            if (doctor.UserTypeId != 20 && doctor.UserTypeId != 30)
            {
                ModelState.AddModelError("", "Invalid User Type");
                return RedirectToAction("SignUp");
            }
            doctor.UserPassword = AppServices.HashPassword(doctor.UserPassword);
            doctor = AppServices.TrimStringProperties(doctor);
            doctor.UserEmail = doctor.UserEmail.ToLower();
            var userInDb = Mapper.Map<SignUpDoctor, User>(doctor);
            userInDb.VerCode = AppServices.GenerateRandomNumber();
            userInDb.Locked = true;

            db.Users.Add(userInDb);
            db.SaveChanges();
            doctor.UserId = userInDb.UserId;

            AppServices.SendConfirmEmail(userInDb.UserEmail, userInDb.VerCode, userInDb.UserId);

            return RedirectToAction("ConfirmUser", new { id = doctor.UserId });
        }

        public ActionResult ConfirmUser(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction("SignIn");

            var user = db.Users.Where(u => u.Locked == true).FirstOrDefault(u => u.UserId == id);
            if (user == null)
                return HttpNotFound();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmUser(int? id, string code)
        {
            if (!id.HasValue || code.Length < 6)
                return RedirectToAction("SignIn");

            var user = db.Users.Where(u => u.Locked == true).FirstOrDefault(u => u.UserId == id);

            if (user == null)
                return HttpNotFound();

            if (user.VerCode == code.Trim())
            {
                user.Locked = false;
                user.VerCode = string.Empty;
                db.SaveChanges();
                if (user.UserTypeId == 10)
                    return new EmptyResult();
                return RedirectToAction("SignIn");
            }
            //ToDo
            //Do more security things
            ModelState.AddModelError("", "Code not match");
            return View();
        }

        public ActionResult SignIn()
        {
            if (Session["UserId"] != null)
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

            var userInDb = db.Users.Where(u => u.Locked == false)
                .FirstOrDefault(u => u.UserEmail == user.Email
                && (u.UserTypeId != 20 || u.UserTypeId != 30));

            if (userInDb == null)
            {
                ModelState.AddModelError("", "Incorrect Email Or Password");
                return View();
            }

            bool passwordVerified = AppServices.VerifayPasswrod(user.Password, userInDb.UserPassword);

            if (!passwordVerified)
            {
                ModelState.AddModelError("", "Incorrect Email Or Password");
                return View();
            }

            Session["UserId"] = userInDb.UserId;
            Session["UserTypeId"] = userInDb.UserTypeId;

            if (userInDb.UserTypeId == 20)
                return RedirectToAction("Index", "Clinics");
            return RedirectToAction("Index", "Pharmacies");
        }

        //GET: Accounts/EditPassword
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

            int userId = UserId;
            var user = db.Users.Find(userId);
            if (user == null)
                return RedirectToAction("SignIn");

            bool verfied = AppServices.VerifayPasswrod(edit.OldPassword, user.UserPassword);
            if (!verfied)
            {
                ModelState.AddModelError("", "Incorrect password");
                return View();
            }

            user.UserPassword = AppServices.HashPassword(edit.NewPassword);
            db.SaveChanges();
            return View();
        }

        //GET: Accounts/EditEmail
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

            int userId = UserId;
            var user = db.Users.Find(userId);
            if (user == null)
                return RedirectToAction("SignIn");

            bool verfied = AppServices.VerifayPasswrod(edit.Password, user.UserPassword);
            if (!verfied)
            {
                ModelState.AddModelError("", "Incorrect password");
                return View();
            }

            user.UserEmail = edit.Email.Trim().ToLower();
            db.SaveChanges();
            return View();
        }

        public ActionResult SignOut()
        {
            Session.Clear();
            return RedirectToAction("SignIn");
        }
    }
}