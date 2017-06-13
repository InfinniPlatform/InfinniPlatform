using InfinniPlatform.Http.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace InfinniPlatform.ServiceHost
{
    public class HttpService
    {
        
    }

    public class FacebookAuthAppLayer : IExternalAuthenticationAppLayer
    {
        public void Configure(IApplicationBuilder app)
        {
            var facebookOptions = new FacebookOptions
            {
                AppId = "199994547162009",
                AppSecret = "ffd317eb16b31540f42c3bbc406bedfa"
            };

            app.UseFacebookAuthentication(facebookOptions);
        }
    }
}