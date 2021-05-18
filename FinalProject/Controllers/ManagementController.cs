using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinalProject.Controllers
{
    public class ManagementController : Controller
    {
        // GET: Management
        public ActionResult Index()
        {
            return View();
        }

        // GET: Pharmacies
        public ActionResult Pharmacies()
        {
            return View();
        }

        // GET: Clinics
        public ActionResult Clinics()
        {
            return View();
        }

        // GET: SignUpRequest
        public ActionResult SignUpRequest()
        {
            return View();
        }
    }
}