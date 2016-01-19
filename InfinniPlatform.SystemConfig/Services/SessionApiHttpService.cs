using System;

using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Services;
using InfinniPlatform.Sdk.Session;

namespace InfinniPlatform.SystemConfig.Services
{
    /// <summary>
    /// Реализует REST-сервис для SessionApi.
    /// </summary>
    internal sealed class SessionApiHttpService : IHttpService
    {
        public SessionApiHttpService(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        private readonly ISessionManager _sessionManager;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "/RestfulApi/StandardApi/authorization";

            builder.Post["/GetSessionData"] = CreateHandler(GetSessionData);
            builder.Post["/SetSessionData"] = CreateHandler(SetSessionData);
            builder.Post["/RemoveSessionData"] = CreateHandler(RemoveSessionData);
        }

        private static Func<IHttpRequest, object> CreateHandler(Action<IActionContext> action)
        {
            return new ChangeHttpRequestHandler(action).Action;
        }

        private void GetSessionData(IActionContext target)
        {
            var tenantId = _sessionManager.GetSessionData(target.Item.ClaimType);

            target.Result = new DynamicWrapper();
            target.Result.ClaimValue = tenantId;
        }

        private void SetSessionData(IActionContext target)
        {
            _sessionManager.SetSessionData(target.Item.ClaimType, target.Item.ClaimValue);

            target.Result = new DynamicWrapper();
            target.Result.IsValid = true;
        }

        private void RemoveSessionData(IActionContext target)
        {
            _sessionManager.SetSessionData(target.Item.ClaimType, null);

            target.Result = new DynamicWrapper();
            target.Result.IsValid = true;
        }
    }
}