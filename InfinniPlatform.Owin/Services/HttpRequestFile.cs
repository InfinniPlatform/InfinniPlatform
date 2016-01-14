using System.IO;

using InfinniPlatform.Sdk.Services;

using Nancy;

namespace InfinniPlatform.Owin.Services
{
    /// <summary>
    /// Информация о файле запроса.
    /// </summary>
    internal sealed class HttpRequestFile : IHttpRequestFile
    {
        public HttpRequestFile(HttpFile nancyFile)
        {
            _nancyFile = nancyFile;
        }


        private readonly HttpFile _nancyFile;


        public string ContentType => _nancyFile.ContentType;

        public string Name => _nancyFile.Name;

        public string Key => _nancyFile.Key;

        public Stream Value => _nancyFile.Value;
    }
}