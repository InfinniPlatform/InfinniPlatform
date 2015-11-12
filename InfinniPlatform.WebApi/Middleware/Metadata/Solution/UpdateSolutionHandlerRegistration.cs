using System;
using System.Runtime.Remoting.Messaging;
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
            
            dynamic body = null;

            try
            {
                body = JObject.Parse(RoutingOwinMiddleware.ReadRequestBody(context).ToString());
            }
            catch (Exception e)
            {
                body = null;
            }

            if (body != null)
            {
                if (body.Version == null || body.Name == null || body.Id == null)
                {
                    return new ErrorRequestHandlerResult(Resources.NotAllRequestParamsAreFiled);
                }

                MetadataManagerSolution managerSolution =
                    ManagerFactorySolution.BuildSolutionManager();

                managerSolution.MergeItem(DynamicWrapperExtensions.ToDynamic(body));
                
                return new EmptyRequestHandlerResult();
            }
            return new ValueRequestHandlerResult(ManagerFactorySolution.BuildSolutionManager().CreateItem("NewSolution"));
        }
    }
}
