using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.Sdk.Dynamic;

using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware.Metadata.DocumentElements
{
    public class UpdateDocumentElementHandlerRegistration : DocumentElementHandlerRegistration
    {
        public UpdateDocumentElementHandlerRegistration()
            : base("POST")
        {
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            dynamic body = RoutingOwinMiddleware.ReadRequestBody(context);

            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            var managerFactoryDocument = new ManagerFactoryDocument(routeDictionary["configuration"], routeDictionary["document"]);

            IDataManager manager = managerFactoryDocument.BuildManagerByType(routeDictionary["metadataType"]);

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