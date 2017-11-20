using System.Collections.Generic;
using System.Reflection;

using InfinniPlatform.Auth.HttpService.Controllers;

using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace InfinniPlatform.Auth.HttpService
{
    public class AuthControllersFeatureProvider<TUser> : IApplicationFeatureProvider<ControllerFeature> where TUser : AppUser
    {
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            feature.Controllers.Add(typeof(AuthManagementController<>).MakeGenericType(typeof(TUser)).GetTypeInfo());
            feature.Controllers.Add(typeof(AuthInternalController<>).MakeGenericType(typeof(TUser)).GetTypeInfo());
            feature.Controllers.Add(typeof(AuthExternalController<>).MakeGenericType(typeof(TUser)).GetTypeInfo());
        }
    }
}