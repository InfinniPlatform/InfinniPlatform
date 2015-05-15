using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers;
using InfinniPlatform.Hosting;

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
			systemComponent.ConfigurationReader = new JsonConfigReaderStandard();
			systemComponent.ManagerIdentifiers = new ManagerIdentifiersStandard();
        }
    }
}
