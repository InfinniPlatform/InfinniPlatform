using System;

using Microsoft.AspNetCore.Http;

namespace InfinniPlatform.IoC
{
    /// <inheritdoc />
    public class ServiceProviderAccessor : IServiceProviderAccessor
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ServiceProviderAccessor" />.
        /// </summary>
        /// <param name="rootServiceProvider">ASP.NET root service provider.</param>
        /// <param name="httpContextAccessor">Current request <see cref="HttpContext"/> provider.</param>
        public ServiceProviderAccessor(IServiceProvider rootServiceProvider, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _rootServiceProvider = rootServiceProvider;
        }


        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IServiceProvider _rootServiceProvider;


        /// <inheritdoc />
        public IServiceProvider GetProvider()
        {
            return _httpContextAccessor.HttpContext?.RequestServices ?? _rootServiceProvider;
        }
    }
}