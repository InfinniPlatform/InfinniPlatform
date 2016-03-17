﻿using System;
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
    /// Реализация <see cref="IHttpRequest"/> на базе Nancy.
    /// </summary>
    internal sealed class NancyHttpRequest : IHttpRequest
    {
        public NancyHttpRequest(NancyContext nancyContext, Func<IIdentity> userIdentityProvider, IJsonObjectSerializer jsonObjectSerializer)
        {
            _nancyRequest = nancyContext.Request;
            _userIdentityProvider = userIdentityProvider;
            _jsonObjectSerializer = jsonObjectSerializer;

            Parameters = nancyContext.Parameters;

            _form = new Lazy<dynamic>(() => ParseRequestForm());
            _headers = new Lazy<IHttpRequestHeaders>(() => new NancyHttpRequestHeaders(_nancyRequest.Headers));
            _files = new Lazy<IEnumerable<IHttpRequestFile>>(() => _nancyRequest.Files.Select(i => new NancyHttpRequestFile(i)));
        }


        private readonly Request _nancyRequest;
        private readonly Func<IIdentity> _userIdentityProvider;
        private readonly IJsonObjectSerializer _jsonObjectSerializer;

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
                    return _jsonObjectSerializer.Deserialize(_nancyRequest.Body, typeof(DynamicWrapper));
                }
            }

            return _nancyRequest.Form;
        }
    }
}