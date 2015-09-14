using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Owin.Middleware;
using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware.Metadata.DocumentElements
{
    public class DeleteDocumentElementHandlerRegistration : DocumentElementHandlerRegistration
    {
        public DeleteDocumentElementHandlerRegistration()
            : base("DELETE")
        {
        }


        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            var managerFactoryDocument = new ManagerFactoryDocument(routeDictionary["versionMetadata"],
                                                                       routeDictionary["configuration"], routeDictionary["document"]);

            IDataManager manager = managerFactoryDocument.BuildManagerByType(routeDictionary["metadataType"]);

            dynamic metadataElement = managerFactoryDocument.BuildMetadataReaderByType(routeDictionary["metadataType"]).GetItem(routeDictionary["instanceId"]);

            if (metadataElement != null)
            {
                manager.DeleteItem(metadataElement);
                return new EmptyRequestHandlerResult();
            }

            return new ErrorRequestHandlerResult(string.Format(Resources.MetadataElementNotFound, routeDictionary["instanceId"], routeDictionary["versionMetadata"]));
        }
    }
}
