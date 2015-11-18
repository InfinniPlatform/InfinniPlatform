using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Sdk.ApiContracts;

namespace InfinniPlatform.Api.RestApi.Public
{
    public class CustomServiceApi : ICustomServiceApi
    {
        public dynamic ExecuteAction(string application, string documentType, string service, dynamic body)
        {
            return RestQueryApi.QueryPostJsonRaw(application, documentType, service, null, body).ToDynamic();
        }
    }
}
