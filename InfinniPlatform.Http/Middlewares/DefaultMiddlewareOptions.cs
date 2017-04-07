using System;
using Microsoft.AspNetCore.Builder;

namespace InfinniPlatform.Http.Middlewares
{
    public class DefaultMiddlewareOptions : IMiddlewareOptions
    {
        public Action<IApplicationBuilder> Configure => app => { };
    }
}