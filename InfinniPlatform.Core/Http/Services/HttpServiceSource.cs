using System.Collections.Generic;

namespace InfinniPlatform.Core.Http.Services
{
    /// <summary>
    /// Источник зарегистрированных сервисов.
    /// </summary>
    internal class HttpServiceSource : IHttpServiceSource
    {
        public HttpServiceSource(IEnumerable<IHttpService> httpServices)
        {
            _httpServices = httpServices;
        }


        private readonly IEnumerable<IHttpService> _httpServices;


        public IEnumerable<IHttpService> GetServices()
        {
            return _httpServices;
        }
    }
}