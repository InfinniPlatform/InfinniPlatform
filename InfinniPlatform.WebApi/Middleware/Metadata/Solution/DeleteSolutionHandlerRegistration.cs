using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers;
using InfinniPlatform.Owin.Middleware;
using Microsoft.Owin;
using InfinniPlatform.Api.Properties;

namespace InfinniPlatform.WebApi.Middleware.Metadata.Solution
{
    public class DeleteSolutionHandlerRegistration : SolutionHandlerRegistration
    {
        public DeleteSolutionHandlerRegistration() : base("DELETE")
        {
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            MetadataManagerSolution managerSolution = ManagerFactorySolution.BuildSolutionManager(routeDictionary["versionMetadata"]);

            dynamic solution = ManagerFactorySolution.BuildSolutionReader(routeDictionary["versionMetadata"]).GetItem(routeDictionary["instanceId"]);

            if (solution != null)
            {
                managerSolution.DeleteItem(solution);
                return new EmptyRequestHandlerResult();
            }

            return new ErrorRequestHandlerResult(string.Format(Resources.SolutionNotFound, routeDictionary["instanceId"], routeDictionary["versionMetadata"]));
        }
    }
}
