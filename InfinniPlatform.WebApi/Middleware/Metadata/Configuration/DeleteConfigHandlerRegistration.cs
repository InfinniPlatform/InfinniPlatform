using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers;
using InfinniPlatform.Owin.Middleware;
using Microsoft.Owin;
using InfinniPlatform.Api.Properties;

namespace InfinniPlatform.WebApi.Middleware.Metadata.Configuration
{
    public class DeleteConfigHandlerRegistration : ConfigHandlerRegistration
    {
        public DeleteConfigHandlerRegistration() : base("DELETE")
        {
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            var routeDictionary = RouteFormatter.GetRouteDictionary(context);
           
            MetadataManagerConfiguration manager = ManagerFactoryConfiguration.BuildConfigurationManager(routeDictionary["versionMetadata"]);

            dynamic config = ManagerFactoryConfiguration.BuildConfigurationMetadataReader(routeDictionary["versionMetadata"]).GetItem(routeDictionary["instanceId"]);

            if (config != null)
            {
                manager.DeleteItem(config);
                return new EmptyRequestHandlerResult();
            }

            return new ErrorRequestHandlerResult(string.Format(Resources.ApplicationNotFound, routeDictionary["instanceId"], routeDictionary["versionMetadata"]));
        }
    }
}
