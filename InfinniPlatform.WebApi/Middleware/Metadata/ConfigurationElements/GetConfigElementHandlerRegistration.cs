using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Owin.Middleware;
using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware.Metadata.ConfigurationElements
{
    public class GetConfigElementHandlerRegistration : ConfigElementHandlerRegistration
    {
        public GetConfigElementHandlerRegistration()
            : base("GET")
        {
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            var factoryConfiguration = new ManagerFactoryConfiguration(routeDictionary["configuration"]);

            if (routeDictionary["instanceId"].ToLowerInvariant() != "unknown")
            {
                return new ValueRequestHandlerResult(factoryConfiguration.BuildMetadataReaderByType(routeDictionary["metadataType"]).GetItem(routeDictionary["instanceId"]));
            }
            return new ValueRequestHandlerResult(factoryConfiguration.BuildMetadataReaderByType(routeDictionary["metadataType"]).GetItems());

        }
    }
}
