using InfinniPlatform.Api.Actions;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Hosting.Implementation.ExtensionPointHandling;
using InfinniPlatform.Hosting.Implementation.Modules;
using InfinniPlatform.Metadata;

namespace InfinniPlatform.WebApi.Tests.TestConfiguration
{
    public sealed class QueryExecutorTestInstaller : MetadataConfigurationInstaller
    {
        public QueryExecutorTestInstaller(IMetadataConfigurationProvider metadataConfigurationProvider,
                                          IScriptConfiguration actionConfiguration)
            : base(metadataConfigurationProvider, actionConfiguration)
        {
        }


        protected override void RegisterConfiguration(IMetadataConfiguration metadataConfiguration)
        {
        }

        protected override void RegisterServices(IServiceRegistrationContainer servicesConfiguration)
        {
            servicesConfiguration.AddRegistration("patient", "Search");
            servicesConfiguration.AddRegistration("patient", "ApplyEvents", reg => reg
                                                                                       .RegisterHandlerInstance(
                                                                                           "Publish")
                                                                                       .SetResultHandler(
                                                                                           HttpResultHandlerType
                                                                                               .BadRequest));
        }
    }
}