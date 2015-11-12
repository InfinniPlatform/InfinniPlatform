using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.Sdk.Dynamic;
using Microsoft.Owin;
using Newtonsoft.Json.Linq;
using InfinniPlatform.Api.Properties;

namespace InfinniPlatform.WebApi.Middleware.Metadata.Solution
{
    public class InsertSolutionHandlerRegistration : SolutionHandlerRegistration
    {
        public InsertSolutionHandlerRegistration()
            : base("PUT")
        {
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {         
            dynamic body = JObject.Parse(RoutingOwinMiddleware.ReadRequestBody(context).ToString());

            if (body.Version == null || body.Name == null || body.Id == null)
            {
                return new ErrorRequestHandlerResult(Resources.NotAllRequestParamsAreFiled);
            }

            MetadataManagerSolution managerSolution = ManagerFactorySolution.BuildSolutionManager();

            managerSolution.InsertItem(DynamicWrapperExtensions.ToDynamic(body));

            return new EmptyRequestHandlerResult();
        }
    }
}
