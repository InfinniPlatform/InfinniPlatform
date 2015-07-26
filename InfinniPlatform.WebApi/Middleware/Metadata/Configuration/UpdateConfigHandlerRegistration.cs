using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.Sdk.Dynamic;
using Microsoft.Owin;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.WebApi.Middleware.Metadata.Configuration
{
    public class UpdateConfigHandlerRegistration : ConfigHandlerRegistration
    {
        public UpdateConfigHandlerRegistration()
            : base("POST")
        {
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            dynamic body = JObject.Parse(RoutingOwinMiddleware.ReadRequestBody(context).ToString());

            if (body.Version == null || body.Name == null || body.Id == null)
            {
                return new ErrorRequestHandlerResult(Resources.NotAllRequestParamsAreFiled);
            }

            MetadataManagerConfiguration managerConfig = ManagerFactoryConfiguration.BuildConfigurationManager(body.Version.ToString());

            managerConfig.MergeItem(DynamicWrapperExtensions.ToDynamic(body));

            return new EmptyRequestHandlerResult();
        }
    }
}
