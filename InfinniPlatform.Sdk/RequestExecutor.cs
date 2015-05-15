using System.IO;
using System.Net;
using Newtonsoft.Json;
using RestSharp;

namespace InfinniPlatform.Sdk
{
    public sealed class RequestExecutor 
    {
        private readonly CookieContainer _cookieContainer;

        public RequestExecutor(CookieContainer cookieContainer = null)
        {
            _cookieContainer = cookieContainer;
        }


        public RestQueryResponse QueryGet(string url, string arguments)
        {
            var restClient = new RestClient(url);

            restClient.CookieContainer = _cookieContainer;

            restClient.Timeout = 1000 * 60 * 200;
            
            IRestResponse restResponse = restClient.Get(new RestRequest("?{argument}") { RequestFormat = DataFormat.Json }
                                                            .AddUrlSegment("argument", arguments));

            return restResponse.ToQueryResponse();
        }

        public RestQueryResponse QueryPost(string url, object body)
        {
            var restClient = new RestClient(url);


            restClient.CookieContainer = _cookieContainer;

            // Изменение времени ожидания ответа сделано для того, чтобы можно было загрузить большие объемы данных
            restClient.Timeout = 1000 * 60 * 300;

            IRestResponse restResponse = restClient.Post(
                new RestRequest
                {
                    RequestFormat = DataFormat.Json
                }.AddBody(body));

            return restResponse.ToQueryResponse();
        }

        /// <summary>
        ///   Формирование запроса на вставку нового документа
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="body">Тело запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public RestQueryResponse QueryPut(string url, object body)
        {
            var restClient = new RestClient(url);


            restClient.CookieContainer = _cookieContainer;
            restClient.Timeout = 1000 * 60 * 300;

            IRestResponse restResponse = restClient.Put(
                new RestRequest
                {
                    RequestFormat = DataFormat.Json
                }.AddBody(body));

            return restResponse.ToQueryResponse();
        }

        public RestQueryResponse QueryPostFile(string url, object linkedData, string filePath)
        {
            var restClient = new RestClient(url);

            restClient.CookieContainer = _cookieContainer;
            restClient.Timeout = 1000 * 60 * 200;

            IRestResponse restResponse = restClient.Post(new RestRequest("?linkedData={argument}") { RequestFormat = DataFormat.Json }
                                                             .AddUrlSegment("argument", JsonConvert.SerializeObject(linkedData))
                                                             .AddFile(Path.GetFileName(filePath), File.ReadAllBytes(filePath), Path.GetFileName(filePath), "multipart/form-data"));
            return restResponse.ToQueryResponse();
        }

        public RestQueryResponse QueryPostUrlEncodedData(string url, object formData)
        {
            var restClient = new RestClient(url);

            restClient.CookieContainer = _cookieContainer;
            restClient.Timeout = 1000 * 60 * 200;

            var request = new RestRequest("", Method.POST);
            request.AddParameter("Form", JsonConvert.SerializeObject(formData));
            var response = restClient.Execute(request);
            return response.ToQueryResponse();

        }


        public RestQueryResponse QueryGetUrlEncodedData(string url, object formData)
        {
            var restClient = new RestClient(url);

            restClient.CookieContainer = _cookieContainer;
            restClient.Timeout = 1000 * 60 * 200;

            var argumentsString = JsonConvert.SerializeObject(formData);
            IRestResponse restResponse = restClient.Get(new RestRequest("?Form={formData}") { RequestFormat = DataFormat.Json }
                                                            .AddUrlSegment("formData", argumentsString));
            return restResponse.ToQueryResponse();
        }

    }
}
