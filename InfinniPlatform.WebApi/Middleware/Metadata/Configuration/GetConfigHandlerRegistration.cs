﻿using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Owin.Middleware;
using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware.Metadata.Configuration
{
    public class GetConfigHandlerRegistration : ConfigHandlerRegistration
    {
        public GetConfigHandlerRegistration()
            : base("GET")
        {
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            var reader = ManagerFactoryConfiguration.BuildConfigurationMetadataReader(routeDictionary["versionMetadata"],true);

            if (routeDictionary["instanceId"].ToLowerInvariant() != "unknown")
            {
                return new ValueRequestHandlerResult(reader.GetItem(routeDictionary["instanceId"]));
            }
            return new ValueRequestHandlerResult(reader.GetItems());

        }
    }
}