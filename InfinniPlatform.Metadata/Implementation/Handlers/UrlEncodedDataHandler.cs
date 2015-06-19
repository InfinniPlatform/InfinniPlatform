using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Hosting;
using InfinniPlatform.Sdk.Application.Contracts;
using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.Metadata.Implementation.Handlers
{
    public sealed class UrlEncodedDataHandler : IWebRoutingHandler
    {
        private readonly IGlobalContext _globalContext;

        public UrlEncodedDataHandler(IGlobalContext globalContext)
        {
            _globalContext = globalContext;
        }

        public IConfigRequestProvider ConfigRequestProvider { get; set; }

        public dynamic Process(dynamic parameters)
        {
            var config = ConfigRequestProvider.GetConfiguration();
            var metadata = ConfigRequestProvider.GetMetadataIdentifier();
            var version = ConfigRequestProvider.GetVersion();

            var target = new UrlEncodedDataContext
            {
                IsValid = true,
                FormData = parameters,
                Configuration = config,
                Metadata = metadata,
                Context = _globalContext,
                Version = version
            };

            var metadataConfig =
                _globalContext.GetComponent<IMetadataConfigurationProvider>(ConfigRequestProvider.GetVersion())
                    .GetMetadataConfiguration(ConfigRequestProvider.GetVersion(),
                        ConfigRequestProvider.GetConfiguration());

            metadataConfig.MoveWorkflow(metadata,
                metadataConfig.GetExtensionPointValue(ConfigRequestProvider, "ProcessUrlEncodedData"), target);

            if (target.IsValid)
            {
                dynamic item = new DynamicWrapper();
                item.IsValid = true;
                item.ValidationMessage = Resources.UrlEncodedDataProcessingComplete;


                dynamic result = new DynamicWrapper();
                if (target.Result != null)
                {
                    result.Data = target.Result.Data;
                    result.Info = target.Result.Info;
                }
                else
                {
                    item.ValidationMessage = "Content not found.";
                    item.IsValid = false;
                }

                item.Result = result;
                return AggregateExtensions.PrepareResultAggregate(item);
            }
            return AggregateExtensions.PrepareInvalidResultAggregate(target);
        }
    }
}