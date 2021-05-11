using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using FinalProject.DTOs;
using FinalProject.App_Start;

namespace FinalProject
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configure(WebApiConfig.Register);

            Mapper.Initialize(c => c.AddProfile<MapperProfile>());

            //Default Values
            MyAppContext db = new MyAppContext();
            int count = db.Managers.Count();
            if (count == 0)
            {
                Manager manager = new Manager { ManagerEmail = "mydocmanager@gmail.com", ManagerPassword = "P@ssw0rd" };
                db.Managers.Add(manager);
                db.SaveChanges();
            }
            count = db.Usertypes.Count();
            if (count == 0)
            {
                UserType[] userTypes = new UserType[] { new UserType { UserTypeName = "PublicUser" }
                    ,new UserType { UserTypeName = "Doctor" }
                    ,new UserType { UserTypeName = "Pahrmacist" }};
                db.Usertypes.AddRange(userTypes);
                db.SaveChanges();
                Application.Add("PublicUser", userTypes[0].UserTypeId);
                Application.Add("Doctor", userTypes[1].UserTypeId);
                Application.Add("Pharmacist", userTypes[2].UserTypeId);
            }
        }
    }
}
