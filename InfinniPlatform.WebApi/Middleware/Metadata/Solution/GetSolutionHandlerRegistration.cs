using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Owin.Middleware;
using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware.Metadata.Solution
{
    public class GetSolutionHandlerRegistration : SolutionHandlerRegistration
    {
        public GetSolutionHandlerRegistration() : base("GET")
        {
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            var solutionReader = ManagerFactorySolution.BuildSolutionReader(routeDictionary["versionMetadata"]);

            if (routeDictionary["instanceId"].ToLowerInvariant() != "unknown")
            {
                return new ValueRequestHandlerResult(solutionReader.GetItem(routeDictionary["instanceId"]));
            }
            return new ValueRequestHandlerResult(solutionReader.GetItems());
        }
    }
}
