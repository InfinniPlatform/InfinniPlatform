using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Sdk.IoC;

using Microsoft.AspNet.SignalR;

namespace InfinniPlatform.PushNotification.SignalR
{
    /// <summary>
    /// Контейнер зависимостей ASP.NET SignalR.
    /// </summary>
    public class SignalRDependencyResolver : DefaultDependencyResolver
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="containerResolver">Провайдер разрешения зависимостей.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public SignalRDependencyResolver(IContainerResolver containerResolver)
        {
            if (containerResolver == null)
            {
                throw new ArgumentNullException(nameof(containerResolver));
            }

            _containerResolver = containerResolver;
        }


        private readonly IContainerResolver _containerResolver;


        /// <summary>
        /// Возвращает реализацию сервиса.
        /// </summary>
        /// <param name="serviceType">Тип сервиса.</param>
        public override object GetService(Type serviceType)
        {
            return _containerResolver.ResolveOptional(serviceType) ?? base.GetService(serviceType);
        }

        /// <summary>
        /// Возвращает список реализаций сервиса.
        /// </summary>
        /// <param name="serviceType">Тип сервиса.</param>
        public override IEnumerable<object> GetServices(Type serviceType)
        {
            var enumerableServiceType = typeof(IEnumerable<>).MakeGenericType(serviceType);
            var services = (IEnumerable<object>)_containerResolver.Resolve(enumerableServiceType);
            return services.Any()
                       ? services
                       : base.GetServices(serviceType);
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                base.Dispose(disposing);
            }
            catch (NotSupportedException e)
            {
                if (!IsSignalRMessageBusException(e))
                {
                    throw;
                }
            }
        }

        private static bool IsSignalRMessageBusException(Exception e)
        {
            //TODO Workaround. SignalR raise exception when disposing AckSubscriber due to EventList type. Check for newer version of SignalR.
            return (e.Message == "Collection was of a fixed size.")
                   && e.StackTrace.Contains("AckSubscriber");
        }
    }
}