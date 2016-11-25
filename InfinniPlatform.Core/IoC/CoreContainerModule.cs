using System.Text;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Sdk.Documents.Metadata;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Settings;
using InfinniPlatform.Sdk.ViewEngine;

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
        }
    }
}