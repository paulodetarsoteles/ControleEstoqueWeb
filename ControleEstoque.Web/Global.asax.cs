﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

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
                Response.Write("{\"Resultado\":\"AVISO\",\"Mensagens\":[\"Somente caracteres normais são aceitos neste campo!\"],\"IdSalvo\":\"\"}"); 
                Response.End();
            }
        }
    }
}
