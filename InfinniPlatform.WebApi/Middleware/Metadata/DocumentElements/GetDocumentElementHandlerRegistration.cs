using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Owin.Middleware;
using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware.Metadata.DocumentElements
{
    public class GetDocumentElementHandlerRegistration : DocumentElementHandlerRegistration
    {
        public GetDocumentElementHandlerRegistration()
            : base("GET")
        {
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            var managerConfigElement = new ManagerFactoryDocument(routeDictionary["versionMetadata"],
                                                                       routeDictionary["configuration"],
                                                                       routeDictionary["document"]);

            if (routeDictionary["instanceId"].ToLowerInvariant() != "list")
            {
                return new ValueRequestHandlerResult(managerConfigElement.BuildMetadataReaderByType(routeDictionary["metadataType"]).GetItem(routeDictionary["instanceId"]));
            }
            return new ValueRequestHandlerResult(managerConfigElement.BuildMetadataReaderByType(routeDictionary["metadataType"]).GetItems());

        }
    }
}
