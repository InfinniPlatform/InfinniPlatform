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

            var managerConfigElement = new ManagerFactoryConfiguration(routeDictionary["versionMetadata"],
                                                                       routeDictionary["configuration"]);


            return new ValueRequestHandlerResult(managerConfigElement.BuildMetadataReaderByType(routeDictionary["metadataType"]).GetItem(routeDictionary["instanceId"]));
        }
    }
}
