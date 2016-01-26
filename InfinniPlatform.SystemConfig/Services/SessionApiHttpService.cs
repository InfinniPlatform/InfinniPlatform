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

            builder.Post["/GetSessionData"] = GetSessionData;
            builder.Post["/SetSessionData"] = SetSessionData;
            builder.Post["/RemoveSessionData"] = RemoveSessionData;
        }

        private object GetSessionData(IHttpRequest request)
        {
            dynamic requestForm = request.Form.changesObject;
            string sessionKey = requestForm.ClaimType;

            var sessionData = _sessionManager.GetSessionData(sessionKey);

            var result = new DynamicWrapper { ["ClaimValue"] = sessionData };

            return result;
        }

        private object SetSessionData(IHttpRequest request)
        {
            dynamic requestForm = request.Form.changesObject;
            string sessionKey = requestForm.ClaimType;
            string sessionData = requestForm.ClaimValue;

            _sessionManager.SetSessionData(sessionKey, sessionData);

            var result = new DynamicWrapper { ["IsValid"] = true };

            return result;
        }

        private object RemoveSessionData(IHttpRequest request)
        {
            dynamic requestForm = request.Form.changesObject;
            string sessionKey = requestForm.ClaimType;

            _sessionManager.SetSessionData(sessionKey, null);

            var result = new DynamicWrapper { ["IsValid"] = true };

            return result;
        }
    }
}