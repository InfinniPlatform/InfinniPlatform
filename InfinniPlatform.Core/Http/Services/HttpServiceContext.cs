﻿using InfinniPlatform.Core.Abstractions.Http;

namespace InfinniPlatform.Core.Http.Services
{
    internal class HttpServiceContext : IHttpServiceContext
    {
        public IHttpRequest Request { get; set; }
    }
}