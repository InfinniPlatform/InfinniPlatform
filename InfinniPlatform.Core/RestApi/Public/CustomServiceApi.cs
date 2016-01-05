using InfinniPlatform.Core.RestApi.CommonApi;
using InfinniPlatform.Sdk.ApiContracts;

namespace InfinniPlatform.Core.RestApi.Public
{
    public class CustomServiceApi : ICustomServiceApi
    {
        public CustomServiceApi(RestQueryApi restQueryApi)
        {
            _restQueryApi = restQueryApi;
        }

        private readonly RestQueryApi _restQueryApi;

        public dynamic ExecuteAction(string application, string documentType, string service, dynamic body)
        {
            return _restQueryApi.QueryPostJsonRaw(application, documentType, service, null, body).ToDynamic();
        }
    }
}