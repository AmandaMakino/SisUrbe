using System;
using System.Collections.Generic;
using Spring.Aop.Framework.AutoProxy;
using Spring.Context.Support;
using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;

namespace Core
{
    public sealed class ObjectContainer : IObjectContainer
    {
        private static GenericApplicationContext context = InitializeContext();
        private static bool configured;
        private static readonly object finalizationLock = new object();

        /// <summary>
        ///   This is primarily used to inject stubs/mocks.
        /// </summary>
        private static readonly Dictionary<string, object> manuallyConfiguredSingletonObjects =
            new Dictionary<string, object>();

        private ObjectContainer()
        {
        }

        public static GenericApplicationContext Context
        {
            get { return context; }
        }

        #region IObjectContainer Members

        T IObjectContainer.Get<T>(params object[] constructorArgs)
        {
            return Get<T>(constructorArgs);
        }

        #endregion

        private static void FinalizeConfigurationIfNotFinalized()
        {
            if (!configured)
            {
                lock (finalizationLock)
                {
                    if (!configured)
                    {
                        FinalizeConfiguration();
                    }
                }
            }
        }

        public static void FinalizeConfiguration()
        {
            context.Refresh();
            configured = true;
        }

        internal static void Reset()
        {
            context = InitializeContext();
            configured = false;

            manuallyConfiguredSingletonObjects.Clear();
        }

        private static GenericApplicationContext InitializeContext()
        {
            //context = new GenericApplicationContext();
            //CoreObjectConfigurer.ConfigureObjects();
            return new GenericApplicationContext();
        }

        public static void AddPrototypeDefinition(
            Type interfaceType,
            Type implementationType)
        {
            AddObjectDefinition(interfaceType, implementationType, false);
        }

        public static void AddPrototypeDefinition(
            Type nameAndImplementationType)
        {
            AddObjectDefinition(nameAndImplementationType, nameAndImplementationType, false);
        }

        public static void AddSingletonDefinition(
            Type interfaceType,
            Type implementationType)
        {
            AddObjectDefinition(interfaceType, implementationType, true);
        }

        public static void AddSingletonDefinition(
            Type nameAndImplementationType)
        {
            AddObjectDefinition(nameAndImplementationType, nameAndImplementationType, true);
        }

        public static void AddSingletonDefinition(
            string objectName,
            Type implementationType)
        {
            AddObjectDefinition(objectName, implementationType, true);
        }

        private static void AddObjectDefinition(
            Type objectNameType,
            Type implementationType,
            bool singleton)
        {
            if (!objectNameType.IsAssignableFrom(implementationType))
            {
                throw new ArgumentException("Non matching types : " + implementationType.FullName + ", " +
                                            objectNameType.FullName);
            }
            AddObjectDefinition(objectNameType.Name, implementationType, singleton);
        }

        public static void AddObjectDefinition(
            string objectName,
            Type implementationType,
            bool singleton)
        {
            var builder = ObjectDefinitionBuilder.RootObjectDefinition(new DefaultObjectDefinitionFactory(),
                                                                       implementationType)
                .SetAutowireMode(AutoWiringMode.ByType)
                .SetSingleton(singleton);

            context.RegisterObjectDefinition(objectName, builder.ObjectDefinition);
        }

        /// <summary>
        ///   Adds a proxy creator for Spring.NET AOP.
        /// </summary>
        /// <param name = "proxyCreatorName"></param>
        /// <param name = "objectNames">Supports the '*' wildcard.</param>
        /// <param name = "interceptorNames">The object name(s) of the AOP interceptors to apply to the matched objects in objectNames.</param>
        public static void AddProxyCreator(string proxyCreatorName, IList<string> objectNames,
                                           IList<string> interceptorNames)
        {
            var builder = ObjectDefinitionBuilder.RootObjectDefinition(new DefaultObjectDefinitionFactory(),
                                                                       typeof (ObjectNameAutoProxyCreator))
                .AddPropertyValue("ObjectNames", objectNames)
                .AddPropertyValue("InterceptorNames", interceptorNames);

            context.RegisterObjectDefinition(proxyCreatorName, builder.ObjectDefinition);
        }

        public static void AddSingletonObject<T>(
            object singleton) where T : class
        {
            if ((singleton as T) == null)
            {
                throw new ArgumentException("Non matching types : " + typeof (T).FullName + ", " +
                                            singleton.GetType().FullName);
            }

            manuallyConfiguredSingletonObjects[typeof (T).Name] = singleton;
        }

        public static TObjectName Get<TObjectName>(params object[] constructorArgs) where TObjectName : class
        {
            return Get<TObjectName>(typeof (TObjectName).Name, constructorArgs);
        }

        public static TObjectName Get<TObjectName>(string objectName, params object[] constructorArgs)
            where TObjectName : class
        {
            object manuallyConfiguredSingletonObject;
            if (manuallyConfiguredSingletonObjects.TryGetValue(objectName, out manuallyConfiguredSingletonObject))
            {
                return (TObjectName) manuallyConfiguredSingletonObject;
            }

            FinalizeConfigurationIfNotFinalized();

            if (constructorArgs == null || constructorArgs.Length <= 0)
                return (TObjectName) context.GetObject(objectName);

            return (TObjectName) context.GetObject(objectName, constructorArgs);
        }
    }
}