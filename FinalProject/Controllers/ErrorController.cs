using System.Web.Mvc;

namespace FinalProject.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult NotFound404()
        {
            return View();
        }
    }
}