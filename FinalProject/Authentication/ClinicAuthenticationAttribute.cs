﻿using FinalProject.Models;
using FinalProject.Services;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace FinalProject.Authentication
{
    public class ClinicAuthenticationAttribute : AuthorizationFilterAttribute
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
                var managerToken = db.Tokens.FirstOrDefault(t => t.Token == token && t.ObjectType == "Clinic" && DateTime.Now < t.ExpireDate);
                if (managerToken == null)
                    return false;

                AppServices.UpdateWebsiteTokenExpireDate(token);
                return true;
            }
        }
    }
}