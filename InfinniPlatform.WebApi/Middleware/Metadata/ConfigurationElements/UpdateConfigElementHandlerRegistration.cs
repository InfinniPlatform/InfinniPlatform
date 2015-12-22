using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.Sdk.Dynamic;

using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware.Metadata.ConfigurationElements
{
    public class UpdateConfigElementHandlerRegistration : ConfigElementHandlerRegistration
    {
        public UpdateConfigElementHandlerRegistration()
            : base("POST")
        {
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            dynamic body = RoutingOwinMiddleware.ReadRequestBody(context);

            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            var managerConfigElement = new ManagerFactoryConfiguration(routeDictionary["configuration"]);

            var manager = managerConfigElement.BuildManagerByType(routeDictionary["metadataType"]);

            if (body != null)
            {
                if (body.Name == null || body.Id == null)
                {
                    return new ErrorRequestHandlerResult(Resources.NotAllRequestParamsAreFiled);
                }

                manager.MergeItem(DynamicWrapperExtensions.ToDynamic(body));
            }

            return new ValueRequestHandlerResult(manager.CreateItem(""));
        }
    }
}