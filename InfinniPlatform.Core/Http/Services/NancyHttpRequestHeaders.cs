using System.Collections;
using System.Collections.Generic;

using InfinniPlatform.Sdk.Http.Services;

using Nancy;

namespace InfinniPlatform.Core.Http.Services
{
    /// <summary>
    /// Реализация <see cref="IHttpRequestHeaders"/> на базе Nancy.
    /// </summary>
    internal class NancyHttpRequestHeaders : IHttpRequestHeaders
    {
        public NancyHttpRequestHeaders(RequestHeaders nancyHeaders)
        {
            _nancyHeaders = nancyHeaders;
        }


        private readonly RequestHeaders _nancyHeaders;


        public IEnumerable<string> Keys => _nancyHeaders.Keys;

        public IEnumerable<IEnumerable<string>> Values => _nancyHeaders.Values;

        public IEnumerable<string> this[string key] => _nancyHeaders[key];


        public string UserAgent => _nancyHeaders.UserAgent;

        public string ContentType => _nancyHeaders.ContentType;

        public long ContentLength => _nancyHeaders.ContentLength;


        IEnumerator<KeyValuePair<string, IEnumerable<string>>> IEnumerable<KeyValuePair<string, IEnumerable<string>>>.GetEnumerator()
        {
            return _nancyHeaders.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _nancyHeaders.GetEnumerator();
        }
    }
}