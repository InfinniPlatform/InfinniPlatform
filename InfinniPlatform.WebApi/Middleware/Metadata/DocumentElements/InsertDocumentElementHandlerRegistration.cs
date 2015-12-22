using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.Sdk.Dynamic;

using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware.Metadata.DocumentElements
{
    public class InsertDocumentElementHandlerRegistration : DocumentElementHandlerRegistration
    {
        public InsertDocumentElementHandlerRegistration()
            : base("PUT")
        {
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            dynamic body = RoutingOwinMiddleware.ReadRequestBody(context);

            if (body.Name == null || body.Id == null)
            {
                return new ErrorRequestHandlerResult(Resources.NotAllRequestParamsAreFiled);
            }

            var managerDocumentElement = new ManagerFactoryDocument(routeDictionary["configuration"], routeDictionary["document"]);

            IDataManager manager = managerDocumentElement.BuildManagerByType(routeDictionary["metadataType"]);

            manager.InsertItem(DynamicWrapperExtensions.ToDynamic(body));

            return new EmptyRequestHandlerResult();
        }
    }
}