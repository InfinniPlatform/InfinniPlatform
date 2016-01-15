using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Services;

using Nancy;

namespace InfinniPlatform.Owin.Services
{
    /// <summary>
    /// Запрос.
    /// </summary>
    internal sealed class HttpRequest : IHttpRequest
    {
        public HttpRequest(NancyContext nancyContext, Func<IIdentity> userIdentityProvider)
        {
            _nancyRequest = nancyContext.Request;
            _userIdentityProvider = userIdentityProvider;

            Parameters = nancyContext.Parameters;

            _form = new Lazy<dynamic>(() => ParseRequestForm());
            _headers = new Lazy<IHttpRequestHeaders>(() => new HttpRequestHeaders(_nancyRequest.Headers));
            _files = new Lazy<IEnumerable<IHttpRequestFile>>(() => _nancyRequest.Files.Select(i => new HttpRequestFile(i)));
        }


        private readonly Request _nancyRequest;
        private readonly Func<IIdentity> _userIdentityProvider;

        private readonly Lazy<object> _form;
        private readonly Lazy<IHttpRequestHeaders> _headers;
        private readonly Lazy<IEnumerable<IHttpRequestFile>> _files;


        public string Method => _nancyRequest.Method;

        public string Path => _nancyRequest.Path;

        public IHttpRequestHeaders Headers => _headers.Value;

        public dynamic Parameters { get; }

        public dynamic Query => _nancyRequest.Query;

        public dynamic Form => _form.Value;

        public IEnumerable<IHttpRequestFile> Files => _files.Value;

        public Stream Content => _nancyRequest.Body;

        public IIdentity User => _userIdentityProvider();


        private object ParseRequestForm()
        {
            var method = _nancyRequest.Method;

            if (string.Equals(method, "POST", StringComparison.OrdinalIgnoreCase)
                || string.Equals(method, "PUT", StringComparison.OrdinalIgnoreCase)
                || string.Equals(method, "PATCH", StringComparison.OrdinalIgnoreCase))
            {
                var contentType = _nancyRequest.Headers.ContentType;

                if (contentType != null && contentType.StartsWith(HttpConstants.JsonContentType, StringComparison.OrdinalIgnoreCase))
                {
                    return JsonObjectSerializer.Default.Deserialize(_nancyRequest.Body, typeof(DynamicWrapper));
                }
            }

            return _nancyRequest.Form;
        }
    }
}