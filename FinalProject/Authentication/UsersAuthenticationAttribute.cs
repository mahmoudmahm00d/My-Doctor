using FinalProject.Models;
using FinalProject.Services;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace FinalProject.Authentication
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
                string authenticationToken = actionContext.Request.Headers.Authorization.Scheme;
                if (CheckToken(authenticationToken))
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(authenticationToken), null);
                }
                else
                {
                    actionContext.Response = actionContext.Request
                        .CreateResponse(System.Net.HttpStatusCode.Unauthorized);
                }
            }
        }

        public static bool CheckToken(string token)
        {
            using (MyAppContext db = new MyAppContext())
            {
                var userToken = db.Tokens.FirstOrDefault(t => t.Token == token && t.ObjectType == "Public User" && DateTime.Now < t.ExpireDate);
                if (userToken == null)
                    return false;

                AppServices.UpdateApplicationTokenExpireDate(token);
                return true;
            }
        }
    }
}