using System;
using Microsoft.AspNetCore.Http;

namespace InfinniPlatform.IoC
{
    public class ServiceProviderAccessor : IServiceProviderAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IServiceProvider _rootServiceProvider;

        public ServiceProviderAccessor(IServiceProvider rootServiceProvider, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _rootServiceProvider = rootServiceProvider;
        }

        public IServiceProvider GetProvider()
        {
            return _httpContextAccessor.HttpContext?.RequestServices ?? _rootServiceProvider;
        }
    }
}