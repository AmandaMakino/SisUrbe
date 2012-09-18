using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Core;
using Spring.Context;
using Spring.Objects.Factory;

namespace SysCEF.Common
{
    public class SpringControllerFactory : DefaultControllerFactory
    {
        private static IObjectFactory _Factory;
        private static readonly HashSet<string> _RegisteredControllers = new HashSet<string>();

        public static void Init(IApplicationContext ctx)
        {
            _Factory = ctx;
        }

        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            try
            {
                var formattedControllerName = string.Format("{0}Controller", controllerName);
                if (_RegisteredControllers.Select(c => c.ToUpper()).Contains(formattedControllerName.ToUpper()))
                {
                    return (IController)_Factory.GetObject(_RegisteredControllers.Single(c => c.ToUpper() == formattedControllerName.ToUpper()));
                }

                return base.CreateController(requestContext, controllerName); // If the controller is not configured, fall back to base class.
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", "SpringControllerFactory.CreateController:  return (IController)_Factory.GetObject(controllerName) didn't work; attempting to return base.CreateController(requestContext, controllerName);.........." + ex, EventLogEntryType.Error, 4, 5);
                throw new HttpException(404, "Unable to create controller.", ex);
            }
        }

        public override void ReleaseController(IController controller)
        {
            if (controller is IDisposable)
            {
                (controller as IDisposable).Dispose();
            }
        }

        public static void RegisterControllerPath(Assembly assembly, string controllerNamespace)
        {
            foreach (var type in assembly.GetTypes().Where(p => p.Namespace == controllerNamespace).Where(p => p.Name.EndsWith("Controller")))
            {
                ObjectContainer.AddObjectDefinition(type.Name, type, false);
                _RegisteredControllers.Add(type.Name);
            }
        }
    }
}
