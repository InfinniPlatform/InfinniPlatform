using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.Sdk.Dynamic;
using Microsoft.Owin;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.WebApi.Middleware.Metadata.Solution
{
    public class UpdateSolutionHandlerRegistration : SolutionHandlerRegistration
    {
        public UpdateSolutionHandlerRegistration()
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

            MetadataManagerSolution managerSolution = ManagerFactorySolution.BuildSolutionManager(body.Version.ToString());

            managerSolution.MergeItem(DynamicWrapperExtensions.ToDynamic(body));

            return new EmptyRequestHandlerResult();
        }
    }
}
