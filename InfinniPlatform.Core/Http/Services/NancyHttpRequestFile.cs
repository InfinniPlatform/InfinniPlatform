using System.IO;

using Nancy;

namespace InfinniPlatform.Http.Services
{
    /// <summary>
    /// Реализация <see cref="IHttpRequestFile"/> на базе Nancy.
    /// </summary>
    internal class NancyHttpRequestFile : IHttpRequestFile
    {
        public NancyHttpRequestFile(HttpFile nancyFile)
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