using System.Text;

using InfinniPlatform.Core.Abstractions.IoC;
using InfinniPlatform.Core.Abstractions.Serialization;
using InfinniPlatform.Core.Abstractions.Session;
using InfinniPlatform.Core.Abstractions.Settings;
using InfinniPlatform.Core.Serialization;
using InfinniPlatform.Core.Session;
using InfinniPlatform.Core.Settings;

namespace InfinniPlatform.Core.IoC
{
    internal class CoreContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            // Serialization

            builder.RegisterInstance(JsonObjectSerializer.DefaultEncoding)
                   .As<Encoding>()
                   .SingleInstance();

            builder.RegisterType<JsonObjectSerializer>()
                   .As<IObjectSerializer>()
                   .As<IJsonObjectSerializer>()
                   .SingleInstance();

            // Session

            builder.RegisterType<TenantScopeProvider>()
                   .As<ITenantScopeProvider>()
                   .SingleInstance();

            builder.RegisterType<TenantProvider>()
                   .As<ITenantProvider>()
                   .SingleInstance();

            // Setttings

            builder.RegisterType<AppConfiguration>()
                   .As<IAppConfiguration>()
                   .SingleInstance();

            builder.RegisterFactory(r => r.Resolve<IAppConfiguration>().GetSection<AppEnvironment>(AppEnvironment.SectionName))
                   .As<IAppEnvironment>()
                   .SingleInstance();
        }
    }
}