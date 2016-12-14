using System.Text;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.Session;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Metadata;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Session;
using InfinniPlatform.Sdk.Settings;

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

            // Metadata

            builder.RegisterFactory(r => r.Resolve<IAppConfiguration>().GetSection<MetadataSettings>(MetadataSettings.SectionName))
                   .As<MetadataSettings>()
                   .SingleInstance();

            builder.RegisterType<JsonDocumentMetadataSource>()
                   .As<IDocumentMetadataSource>()
                   .SingleInstance();

            // Session

            builder.RegisterType<TenantScopeProvider>()
                   .As<ITenantScopeProvider>()
                   .SingleInstance();

            builder.RegisterType<TenantProvider>()
                   .As<ITenantProvider>()
                   .SingleInstance();
        }
    }
}