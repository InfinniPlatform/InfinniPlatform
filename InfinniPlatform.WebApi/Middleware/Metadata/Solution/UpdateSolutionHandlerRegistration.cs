using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.Sdk.Dynamic;

using Microsoft.Owin;

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
            dynamic body = RoutingOwinMiddleware.ReadRequestBody(context);

            if (body != null)
            {
                if (body.Version == null || body.Name == null || body.Id == null)
                {
                    return new ErrorRequestHandlerResult(Resources.NotAllRequestParamsAreFiled);
                }

                var managerSolution = ManagerFactorySolution.BuildSolutionManager();

                managerSolution.MergeItem(DynamicWrapperExtensions.ToDynamic(body));

                return new EmptyRequestHandlerResult();
            }
            return new ValueRequestHandlerResult(ManagerFactorySolution.BuildSolutionManager().CreateItem("NewSolution"));
        }
    }
}