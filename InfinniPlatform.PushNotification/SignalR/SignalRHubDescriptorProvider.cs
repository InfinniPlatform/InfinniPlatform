using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.IoC;

using Microsoft.AspNet.SignalR.Hubs;

namespace InfinniPlatform.PushNotification.SignalR
{
    /// <summary>
    /// Провайдер точек обмена ASP.NET SignalR.
    /// </summary>
    public class SignalRHubDescriptorProvider : IHubDescriptorProvider
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="containerResolver">Провайдер разрешения зависимостей.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public SignalRHubDescriptorProvider(IContainerResolver containerResolver)
        {
            if (containerResolver == null)
            {
                throw new ArgumentNullException(nameof(containerResolver));
            }

            _hubs = new Lazy<IDictionary<string, HubDescriptor>>(() => BuildHubsCache(containerResolver));
        }


        private readonly Lazy<IDictionary<string, HubDescriptor>> _hubs;


        /// <summary>
        /// Возвращает информацию о зарегистрированных точках обмена.
        /// </summary>
        public IList<HubDescriptor> GetHubs()
        {
            return _hubs.Value.Values.ToList();
        }

        /// <summary>
        /// Возвращает информацию о точке обмена с заданным именем.
        /// </summary>
        /// <param name="hubName">Имя точки обмена.</param>
        /// <param name="descriptor">Информация точки обмена.</param>
        public bool TryGetHub(string hubName, out HubDescriptor descriptor)
        {
            return _hubs.Value.TryGetValue(hubName, out descriptor);
        }


        private static IDictionary<string, HubDescriptor> BuildHubsCache(IContainerResolver containerResolver)
        {
            // Список зарегистрированных реализаций IHub
            var hubTypes = containerResolver.Services.Where(IsHubType);

            // Список с информацией по каждой реализации IHub
            var hubDescriptors = hubTypes
                .Select(type => new HubDescriptor
                {
                    NameSpecified = false,
                    Name = GetHubName(type),
                    HubType = type
                });

            var hubs = new Dictionary<string, HubDescriptor>(StringComparer.OrdinalIgnoreCase);

            foreach (var hubDescriptor in hubDescriptors)
            {
                hubs[hubDescriptor.Name] = hubDescriptor;
            }

            return hubs;
        }


        private static bool IsHubType(Type type)
        {
            try
            {
                return type.IsClass
                       && !type.IsAbstract
                       && !type.IsGenericType
                       && typeof(IHub).IsAssignableFrom(type);
            }
            catch
            {
                return false;
            }
        }

        private static string GetHubName(Type type)
        {
            return type.GetAttributeValue<HubNameAttribute, string>(i => i.HubName, type.NameOf());
        }
    }
}