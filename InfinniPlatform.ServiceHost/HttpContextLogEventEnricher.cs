using Microsoft.AspNetCore.Http;

using Serilog.Core;
using Serilog.Events;

namespace InfinniPlatform.ServiceHost
{
    public class HttpContextLogEventEnricher : ILogEventEnricher
    {
        private const string RequestIdProperty = "RequestId";
        private const string UserNameProperty = "UserName";


        public HttpContextLogEventEnricher(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        private readonly IHttpContextAccessor _httpContextAccessor;


        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var context = _httpContextAccessor.HttpContext;

            if (context != null)
            {
                var requestId = context.TraceIdentifier ?? "";
                var requestIdProperty = propertyFactory.CreateProperty(RequestIdProperty, requestId);
                logEvent.AddPropertyIfAbsent(requestIdProperty);

                var userName = context.User?.Identity?.Name ?? "";
                var userNameProperty = propertyFactory.CreateProperty(UserNameProperty, userName);
                logEvent.AddPropertyIfAbsent(userNameProperty);
            }
        }
    }
}