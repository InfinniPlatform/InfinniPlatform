using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.SignalR.Modules;

namespace InfinniPlatform.SignalR.IoC
{
    internal sealed class SignalRContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<SignalROwinHostingModule>()
                   .As<IOwinHostingModule>()
                   .SingleInstance();
        }
    }
}