using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Sdk.RestApi
{
    /// <summary>
    /// Реализует REST-клиент для SessionApi.
    /// </summary>
    public sealed class SessionApiClient : BaseRestClient
    {
        public SessionApiClient(string server, int port) : base(server, port)
        {
        }

        public dynamic GetSessionData(string claimType)
        {
            var requestUri = BuildRequestUri("/RestfulApi/StandardApi/authorization/GetSessionData");

            var requestData = new DynamicWrapper
                              {
                                  ["changesObject"] = new DynamicWrapper
                                                      {
                                                          ["ClaimType"] = claimType
                                                      }
                              };

            return RequestExecutor.PostObject(requestUri, requestData);
        }

        public void SetSessionData(string claimType, string claimValue)
        {
            var requestUri = BuildRequestUri("/RestfulApi/StandardApi/authorization/SetSessionData");

            var requestData = new DynamicWrapper
                              {
                                  ["changesObject"] = new DynamicWrapper
                                                      {
                                                          ["ClaimType"] = claimType,
                                                          ["ClaimValue"] = claimValue
                                                      }
                              };

            RequestExecutor.PostObject(requestUri, requestData);
        }

        public void RemoveSessionData(string claimType)
        {
            var requestUri = BuildRequestUri("/RestfulApi/StandardApi/authorization/RemoveSessionData");

            var requestData = new DynamicWrapper
                              {
                                  ["changesObject"] = new DynamicWrapper
                                                      {
                                                          ["ClaimType"] = claimType
                                                      }
                              };

            RequestExecutor.PostObject(requestUri, requestData);
        }
    }
}