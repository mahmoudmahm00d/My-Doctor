using AutoMapper;
using FinalProject.App_Start;
using FinalProject.Models;
using FinalProject.Services;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace FinalProject
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Mapper.Initialize(c => c.AddProfile<MapperProfile>());

            //Default Values
            MyAppContext db = new MyAppContext();
            int count = db.Managers.Count();
            if (count == 0)
            {
                Manager manager = new Manager
                {
                    ManagerEmail = "mydocmanager@gmail.com",
                    ManagerPassword = AppServices.HashPassword("P@ssw0rd")
                };
                db.Managers.Add(manager);
                db.SaveChanges();
            }
            count = db.Usertypes.Count();
            if (count == 0)
            {
                var user = new UserType { UserTypeId = 10, UserTypeName = "PublicUser" };
                var doctor = new UserType { UserTypeId = 20, UserTypeName = "Doctor" };
                var pharmacist = new UserType { UserTypeId = 30, UserTypeName = "Pahrmacist" };
                db.Usertypes.Add(user);
                db.Usertypes.Add(doctor);
                db.Usertypes.Add(pharmacist);
                db.SaveChanges();
            }
            //count = db.MedicineTypes.Count();
            //if (count == 0)
            //{
            //    var capsule = new MedicineType {  MedicineTypeName = "capsule" };
            //    var pills = new MedicineType {  MedicineTypeName = "pills" };
            //    var injection = new MedicineType { MedicineTypeName = "injection" };
            //    var Liquid = new MedicineType { MedicineTypeName = "Liquid" };
            //    db.MedicineTypes.Add(capsule);
            //    db.MedicineTypes.Add(pills);
            //    db.MedicineTypes.Add(injection);
            //    db.MedicineTypes.Add(Liquid);
            //    db.SaveChanges();
            //}
        }
    }
}
