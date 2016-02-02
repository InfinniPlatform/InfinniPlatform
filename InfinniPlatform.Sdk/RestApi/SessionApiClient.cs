using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.Sdk.RestApi
{
    /// <summary>
    /// Реализует REST-клиент для SessionApi.
    /// </summary>
    public sealed class SessionApiClient : BaseRestClient
    {
        public SessionApiClient(string server, int port, IJsonObjectSerializer serializer = null) : base(server, port, serializer)
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