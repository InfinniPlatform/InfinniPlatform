using System.Text;

using InfinniPlatform.Core.Compression;
using InfinniPlatform.Core.Logging;
using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.Settings;
using InfinniPlatform.Sdk.Documents.Metadata;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.Core.IoC
{
    internal class CoreContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            // Configuration

            builder.RegisterType<AppConfiguration>()
                   .As<IAppConfiguration>()
                   .SingleInstance();

            builder.RegisterFactory(r => r.Resolve<IAppConfiguration>().GetSection<AppEnvironment>(AppEnvironment.SectionName))
                   .As<IAppEnvironment>()
                   .SingleInstance();

            builder.RegisterType<GZipDataCompressor>()
                   .As<IDataCompressor>()
                   .SingleInstance();

            // Logging

            builder.OnCreateInstance(new LogContainerParameterResolver<ILog>(LogManagerCache.GetLog));
            builder.OnActivateInstance(new LogContainerInstanceActivator<ILog>(LogManagerCache.GetLog));

            builder.OnCreateInstance(new LogContainerParameterResolver<IPerformanceLog>(LogManagerCache.GetPerformanceLog));
            builder.OnActivateInstance(new LogContainerInstanceActivator<IPerformanceLog>(LogManagerCache.GetPerformanceLog));

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

            // Hosting

            builder.RegisterHttpServices(GetType().Assembly);
        }
    }
}