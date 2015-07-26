using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.WebApi.Middleware.Metadata.Configuration;
using Microsoft.Owin;
using Newtonsoft.Json.Linq;
using InfinniPlatform.Api.Properties;

namespace InfinniPlatform.WebApi.Middleware.Metadata.Menu
{
    public class InsertConfigElementHandlerRegistration : ConfigElementHandlerRegistration
    {
        public InsertConfigElementHandlerRegistration()
            : base("PUT")
        {
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            dynamic body = JObject.Parse(RoutingOwinMiddleware.ReadRequestBody(context).ToString());

            if (body.Name == null || body.Id == null)
            {
                return new ErrorRequestHandlerResult(Resources.NotAllRequestParamsAreFiled);
            }

            var managerConfigElement = new ManagerFactoryConfiguration(routeDictionary["version"], routeDictionary["configuration"]);

            IDataManager manager = managerConfigElement.BuildManagerByType(routeDictionary["metadataType"]);

            manager.InsertItem(DynamicWrapperExtensions.ToDynamic(body));

            return new EmptyRequestHandlerResult();
        }
    }
}
