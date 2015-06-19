using System.IO;
using System.Net;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Extensions;

namespace InfinniPlatform.Api.RestQuery.RestQueryExecutors
{
    public sealed class RestQueryExecutor : IRestQueryExecutor
    {
        private readonly CookieContainer _cookieContainer;

        public RestQueryExecutor(CookieContainer cookieContainer = null)
        {
            _cookieContainer = cookieContainer;
        }

        public RestQueryResponse QueryGet(string url, object queryObject)
        {
            var restClient = new RestClient(url);

            restClient.CookieContainer = _cookieContainer;

            restClient.Timeout = 1000*60*200;
            var argumentsString = JsonConvert.SerializeObject(queryObject);
            var restResponse = restClient.Get(new RestRequest("?query={argument}") {RequestFormat = DataFormat.Json}
                .AddUrlSegment("argument", argumentsString));

            return restResponse.ToQueryResponse();
        }

        public RestQueryResponse QueryPost(string url, object body)
        {
            var restClient = new RestClient(url);


            restClient.CookieContainer = _cookieContainer;


            // TODO: HOT FIX
            // Изменение времени ожидания ответа сделано для того, чтобы можно было загрузить
            // объемные справочники из реестра Росминздрава. Дело в том, что в конфигурации
            // ExternalClassifiersLoader может быть запущен импорт очень больших справочников,
            // например, справочник лекарственных средств 1.2.643.5.1.13.2.1.1.587 имеет 22 тысячи записей,
            // которые загружаются страницами по 500 через медленный SOAP. Загрузка справочника 
            // занимает порядка 20 минут.
            // 
            // Необходимо переделать реализацию и использовать шину сообщений,
            // пока сделано простое увеличение таймаута до 200 минут 
            restClient.Timeout = 1000*60*300;

            var restResponse = restClient.Post(
                new RestRequest
                {
                    RequestFormat = DataFormat.Json
                }
                    .AddBody(body));

            return restResponse.ToQueryResponse();
        }

        public RestQueryResponse QueryPostFile(string url, object linkedData, string filePath)
        {
            var restClient = new RestClient(url);

            restClient.CookieContainer = _cookieContainer;
            restClient.Timeout = 1000*60*200;

            var restResponse =
                restClient.Post(new RestRequest("?linkedData={argument}") {RequestFormat = DataFormat.Json}
                    .AddUrlSegment("argument", JsonConvert.SerializeObject(linkedData))
                    .AddFile(Path.GetFileName(filePath), File.ReadAllBytes(filePath), Path.GetFileName(filePath),
                        "multipart/form-data"));
            return restResponse.ToQueryResponse();
        }

        public RestQueryResponse QueryPostUrlEncodedData(string url, object formData)
        {
            var restClient = new RestClient(url);

            restClient.CookieContainer = _cookieContainer;
            restClient.Timeout = 1000*60*200;

            var request = new RestRequest("", Method.POST);
            request.AddParameter("Form", JsonConvert.SerializeObject(formData));
            var response = restClient.Execute(request);
            return response.ToQueryResponse();
        }

        public RestQueryResponse QueryGetUrlEncodedData(string url, object formData)
        {
            var restClient = new RestClient(url);

            restClient.CookieContainer = _cookieContainer;
            restClient.Timeout = 1000*60*200;

            var argumentsString = JsonConvert.SerializeObject(formData);
            var restResponse = restClient.Get(new RestRequest("?Form={formData}") {RequestFormat = DataFormat.Json}
                .AddUrlSegment("formData", argumentsString)
                );
            return restResponse.ToQueryResponse();
        }

        public RestQueryResponse QueryPostFile(string url, object linkedData, Stream fileStream)
        {
            var restClient = new RestClient(url);

            restClient.CookieContainer = _cookieContainer;
            restClient.Timeout = 1000*60*200;

            string fileName = ((dynamic) linkedData).FileName.ToString();

            var restResponse =
                restClient.Post(new RestRequest("?linkedData={argument}") {RequestFormat = DataFormat.Json}
                    .AddUrlSegment("argument", JsonConvert.SerializeObject(linkedData))
                    .AddFile(fileName, fileStream.ReadAsBytes(), fileName, "multipart/form-data"));
            return restResponse.ToQueryResponse();
        }
    }
}