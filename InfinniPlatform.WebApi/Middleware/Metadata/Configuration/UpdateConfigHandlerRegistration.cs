using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.Sdk.Dynamic;

using Microsoft.Owin;

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
            dynamic body = RoutingOwinMiddleware.ReadRequestBody(context);

            if (body != null)
            {
                if (body.Version == null || body.Name == null || body.Id == null)
                {
                    return new ErrorRequestHandlerResult(Resources.NotAllRequestParamsAreFiled);
                }

                var managerConfig = ManagerFactoryConfiguration.BuildConfigurationManager();

                managerConfig.MergeItem(DynamicWrapperExtensions.ToDynamic(body));
            }

            return new ValueRequestHandlerResult(ManagerFactoryConfiguration.BuildConfigurationManager().CreateItem("NewConfig"));
        }
    }
}