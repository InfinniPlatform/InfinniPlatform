using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Routing;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.LocalRouting;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Hosting;
using InfinniPlatform.WebApi.ConfigRequestProviders;
using InfinniPlatform.WebApi.Factories;
using InfinniPlatform.WebApi.WebApi;
using Newtonsoft.Json;

namespace InfinniPlatform.WebApi.ProvidersLocal
{
    public sealed class ApiControllerLocal : IRequestLocal
    {
        private readonly IApiControllerFactory _apiControllerFactory;

        private readonly IHttpResultHandlerFactory _httpResultHandlerFactory;


        public ApiControllerLocal(IApiControllerFactory apiControllerFactory)
        {
            _apiControllerFactory = apiControllerFactory;

            _httpResultHandlerFactory = new HttpResultHandlerFactory();
        }


        private IRestVerbsContainer GetMetadata(string configuration, string metadata)
        {
            var serviceMetadata = _apiControllerFactory.GetTemplate(configuration, metadata);

            if (serviceMetadata == null)
            {
                throw new ArgumentException(string.Format("Не найдены метаданные для {0} ({1})", configuration, metadata));
            }
            return serviceMetadata;
        }

        public string InvokeRestOperationPost(string configuration, string metadata, string action, IDictionary<string, object> requestBody, string userName)
        {

            TargetDelegate verbProcessor = GetMetadata(configuration, metadata).FindVerbPost(action, requestBody);

            var httpResultHandler = _httpResultHandlerFactory.GetResultHandler(verbProcessor.HttpResultHandler);

            return httpResultHandler.WrapResult(InvokeRestVerb(verbProcessor, configuration, metadata, action, userName)).Content.ReadAsStringAsync().Result;

        }

        public string InvokeRestOperationGet(string configuration, string metadata, string action, IDictionary<string, object> requestBody, string userName)
        {

            TargetDelegate verbProcessor = GetMetadata(configuration, metadata).FindVerbGet(action, requestBody);

            var httpResultHandler = _httpResultHandlerFactory.GetResultHandler(verbProcessor.HttpResultHandler);

            return httpResultHandler.WrapResult(InvokeRestVerb(verbProcessor, configuration, metadata, action, userName)).Content.ReadAsStringAsync().Result;

        }

        public string InvokeRestOperationUpload(string configuration, string metadata, string action, object requestBody, string filePath, string userName)
        {
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                TargetDelegate verbProcessor = GetMetadata(configuration, metadata)
                    .FindUploadVerb(action, requestBody, stream);

                var httpResultHandler = _httpResultHandlerFactory.GetResultHandler(verbProcessor.HttpResultHandler);

                return httpResultHandler.WrapResult(InvokeRestVerb(verbProcessor, configuration, metadata, action, userName)).Content.ReadAsStringAsync().Result;

            }
        }

        public string InvokeRestOperationUpload(string configuration, string metadata, string action, object requestBody, string fileName, Stream file, string userName)
        {
            TargetDelegate verbProcessor = GetMetadata(configuration, metadata)
                .FindUploadVerb(action, requestBody, file);

            var httpResultHandler = _httpResultHandlerFactory.GetResultHandler(verbProcessor.HttpResultHandler);

            return httpResultHandler.WrapResult(InvokeRestVerb(verbProcessor, configuration, metadata, action, userName)).Content.ReadAsStringAsync().Result;
        }

        public string InvokeRestOperationDownload(string configuration, string metadata, string action, object requestBody, string userName)
        {
            TargetDelegate verbProcessor = GetMetadata(configuration, metadata)
                .FindVerbUrlEncodedData(action, requestBody);

            var httpResultHandler = _httpResultHandlerFactory.GetResultHandler(verbProcessor.HttpResultHandler);
            
            return httpResultHandler.WrapResult(InvokeRestVerb(verbProcessor, configuration, metadata, action, userName)).Content.ReadAsStringAsync().Result;
        }

        private object InvokeRestVerb(TargetDelegate verbProcessor, string configuration, string metadata, string action, string userName)
        {
            if (verbProcessor != null)
            {
                var prop = verbProcessor.Target.GetType().GetProperties().FirstOrDefault(p => p.PropertyType.IsAssignableFrom(typeof(IConfigRequestProvider)));
                if (prop != null)
                {
                    prop.SetValue(verbProcessor.Target, new LocalDataProvider(configuration, metadata, action, userName));
                }
                return verbProcessor.Invoke();
            }
            return null;
        }


    }
}
