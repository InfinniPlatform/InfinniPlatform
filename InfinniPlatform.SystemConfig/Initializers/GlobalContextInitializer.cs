using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers;
using InfinniPlatform.Hosting;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.SystemConfig.Initializers
{
    public sealed class GlobalContextInitializer : IStartupInitializer
    {
        private readonly IGlobalContext _globalContext;

        public GlobalContextInitializer(IGlobalContext globalContext)
        {
            _globalContext = globalContext;
        }

        public void OnStart(HostingContextBuilder contextBuilder)
        {
            var systemComponent = _globalContext.GetComponent<ISystemComponent>();

            systemComponent.ManagerIdentifiers = new ManagerIdentifiersStandard();
        }
    }
}