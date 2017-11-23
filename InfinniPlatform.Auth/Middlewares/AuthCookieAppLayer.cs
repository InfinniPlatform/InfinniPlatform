using System;

using InfinniPlatform.Http.Middlewares;

using Microsoft.AspNetCore.Builder;

namespace InfinniPlatform.Auth.Middlewares
{
    /// <summary>
    /// Hosting layer for user authentication via cookies.
    /// </summary>
    [Obsolete]
    public class AuthCookieAppLayer : IAuthenticationBarrierAppLayer, IDefaultAppLayer
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseAuthentication();
        }
    }
}