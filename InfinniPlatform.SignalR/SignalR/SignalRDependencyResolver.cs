﻿using System;
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
            return services.Any() ? services : base.GetServices(serviceType);
        }
    }
}