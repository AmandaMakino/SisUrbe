using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Core;
using SysCEF.Common;
using SysCEF.Common.Implementacao;
using SysCEF.Common.Interface;

namespace SysCEF.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
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
    }
}