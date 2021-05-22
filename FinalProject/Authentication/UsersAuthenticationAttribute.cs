using FinalProject.Controllers.api;
using FinalProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace FinalProject.Models
{
    public class UsersAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request
                    .CreateResponse(System.Net.HttpStatusCode.Unauthorized);
            }
            else
            {
                string authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
                string decodedAuthentication = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
                string[] emailPassword = decodedAuthentication.Split(':');
                string email = emailPassword[0];
                string password = emailPassword[1];
                if (Login(email, password))
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(email), null);
                }
                else
                {
                    actionContext.Response = actionContext.Request
                        .CreateResponse(System.Net.HttpStatusCode.Unauthorized);
                }
            }
            base.OnAuthorization(actionContext);
        }

        public static bool Login(string email, string password)
        {
            using (MyAppContext db = new MyAppContext())
            {
                var user = db.Users.Where(u => u.Locked == false)
                    .FirstOrDefault(u => u.UserEmail == email && u.UserTypeId == 10);
                if (user == null)
                    return false;
                return AppServices.VerifayPasswrod(password, user.UserPassword);
            }
        }
    }
}