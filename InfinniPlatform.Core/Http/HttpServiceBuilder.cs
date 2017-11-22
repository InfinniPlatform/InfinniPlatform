using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace InfinniPlatform.Http
{
    internal class HttpServiceBuilder : IHttpServiceBuilder
    {
        public HttpServiceBuilder()
        {
            
        }


        public Func<IHttpRequest, Task<object>> OnBefore { get; set; }

        public Func<IHttpRequest, object, Task<object>> OnAfter { get; set; }

        public Func<IHttpRequest, Exception, Task<object>> OnError { get; set; }


        public Func<object, IActionResult> ResultConverter { get; set; }
    }
}