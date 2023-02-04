﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace ControleEstoque.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError(); 

            if(ex is HttpRequestValidationException)
            {
                Response.Clear();
                Response.StatusCode = 200; 
                Response.ContentType = "application/json";
                Response.Write("{\"Resultado\":\"AVISO\",\"Mensagens\":[\"Não são permitidos caracteres especiais!\"],\"IdSalvo\":\"\"}"); 
                Response.End();
            }
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie cookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName]; 

            if(cookie != null && cookie.Value != string.Empty)
            {
                FormsAuthenticationTicket ticket;
                try
                {
                    ticket = FormsAuthentication.Decrypt(cookie.Value);
                }
                catch (Exception)
                {
                    return; 
                }
                string[] perfis = ticket.UserData.Split(';'); 

                if(Context.User != null)
                {
                    Context.User = new GenericPrincipal(Context.User.Identity, perfis); 
                }
            }
        }
    }
}
