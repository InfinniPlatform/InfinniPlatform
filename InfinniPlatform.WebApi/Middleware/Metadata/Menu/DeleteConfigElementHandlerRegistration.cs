using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.WebApi.Middleware.Metadata.Configuration;
using Microsoft.Owin;
using InfinniPlatform.Api.Properties;

namespace InfinniPlatform.WebApi.Middleware.Metadata.Menu
{
    public class DeleteConfigElementHandlerRegistration : ConfigElementHandlerRegistration
    {
        public DeleteConfigElementHandlerRegistration()
            : base("DELETE")
        {
        }


        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            var managerConfigElement = new ManagerFactoryConfiguration(routeDictionary["version"],
                                                                       routeDictionary["configuration"]);

            IDataManager manager = managerConfigElement.BuildManagerByType(routeDictionary["metadataType"]);

            dynamic metadataElement = managerConfigElement.BuildMetadataReaderByType(routeDictionary["metadataType"]).GetItem(routeDictionary["instanceId"]);

            if (metadataElement != null)
            {
                manager.DeleteItem(metadataElement);
                return new EmptyRequestHandlerResult();
            }

            return new ErrorRequestHandlerResult(string.Format(Resources.MetadataElementNotFound, routeDictionary["instanceId"], routeDictionary["version"]));
        }
    }
}
