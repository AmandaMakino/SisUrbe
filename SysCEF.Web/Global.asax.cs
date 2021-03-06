﻿using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Core;
using SysCEF.Common;
using SysCEF.Common.Implementacao;
using SysCEF.Common.Interface;
using System;
using System.Web;
using System.Web.Security;

namespace SysCEF.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute
            {
                ExceptionType = typeof(InvalidOperationException),
                // Error.cshtml is a view in the Shared folder.
                View = "Error",
                Order = 1
            });

            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("{*staticfile}", new { staticfile = @".*\.(bmp|png|gif|ico|jpg|jpeg|css|js)(/.*)?" });

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Login", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            ObjectContainer.AddPrototypeDefinition(typeof(ISysCEFWorkLifetimeManager), typeof(SysCEFPerRequestWorkLifetimeManager));
            
            SpringControllerFactory.Init(ObjectContainer.Context);
            SpringControllerFactory.RegisterControllerPath(Assembly.GetExecutingAssembly(), "SysCEF.Web.Controllers");
            ControllerBuilder.Current.SetControllerFactory(new SpringControllerFactory());

            DataAccess.DependencyConfigurator.ConfigureDependencies();
            DAO.DependencyConfigurator.ConfigureSqlRepositories();

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            /* we guess at this point session is not already retrieved by application so we recreate cookie with the session id... */
            try
            {
                string session_param_name = "ASPSESSID";
                string session_cookie_name = "ASP.NET_SessionId";

                if (HttpContext.Current.Request.Form[session_param_name] != null)
                {
                    UpdateCookie(session_cookie_name, HttpContext.Current.Request.Form[session_param_name]);
                }
                else if (HttpContext.Current.Request.QueryString[session_param_name] != null)
                {
                    UpdateCookie(session_cookie_name, HttpContext.Current.Request.QueryString[session_param_name]);
                }
            }
            catch
            {
            }

            try
            {
                string auth_param_name = "AUTHID";
                string auth_cookie_name = FormsAuthentication.FormsCookieName;

                if (HttpContext.Current.Request.Form[auth_param_name] != null)
                {
                    UpdateCookie(auth_cookie_name, HttpContext.Current.Request.Form[auth_param_name]);
                }
                else if (HttpContext.Current.Request.QueryString[auth_param_name] != null)
                {
                    UpdateCookie(auth_cookie_name, HttpContext.Current.Request.QueryString[auth_param_name]);
                }

            }
            catch
            {
            }
        }

        private void UpdateCookie(string cookie_name, string cookie_value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(cookie_name);
            if (null == cookie)
            {
                cookie = new HttpCookie(cookie_name);
            }
            cookie.Value = cookie_value;
            HttpContext.Current.Request.Cookies.Set(cookie);
        }
    }
}