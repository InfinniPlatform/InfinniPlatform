using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace InfinniPlatform.SignalR.Modules
{
    internal static class SignalRGlobalHost
    {
        static SignalRGlobalHost()
        {
            Resolver = new DefaultDependencyResolver();
            RegisterHub<ClientNotificationServiceHub>();
            var hubDescriptorProvider = new SignalRHubDescriptorProvider(Hubs);
            Resolver.Register(typeof(IHubDescriptorProvider), () => hubDescriptorProvider);
        }


        public static IDependencyResolver Resolver { get; }

        public static IConnectionManager ConnectionManager => Resolver.Resolve<IConnectionManager>();

        private static readonly Dictionary<string, HubDescriptor> Hubs = new Dictionary<string, HubDescriptor>(StringComparer.OrdinalIgnoreCase);


        private static void RegisterHub<T>() where T : Hub
        {
            var hubType = typeof(T);

            Hubs.Add(hubType.Name, new HubDescriptor
            {
                HubType = hubType,
                Name = hubType.Name,
                NameSpecified = false
            });
        }


        private sealed class SignalRHubDescriptorProvider : IHubDescriptorProvider
        {
            public SignalRHubDescriptorProvider(Dictionary<string, HubDescriptor> hubs)
            {
                _hubs = hubs;
            }

            private readonly Dictionary<string, HubDescriptor> _hubs;

            public IList<HubDescriptor> GetHubs()
            {
                return _hubs.Values.ToArray();
            }

            public bool TryGetHub(string hubName, out HubDescriptor descriptor)
            {
                return _hubs.TryGetValue(hubName, out descriptor);
            }
        }
    }
}