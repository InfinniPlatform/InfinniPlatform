using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using InfinniPlatform.Core.Hosting;
using InfinniPlatform.Core.LocalRouting;
using InfinniPlatform.Core.RestQuery;
using InfinniPlatform.WebApi.Factories;

namespace InfinniPlatform.WebApi.ProvidersLocal
{
    public sealed class ApiControllerLocal : IRequestLocal
    {
        public ApiControllerLocal(IApiControllerFactory apiControllerFactory)
        {
            _apiControllerFactory = apiControllerFactory;

            _httpResultHandlerFactory = new HttpResultHandlerFactory();
        }

        private readonly IApiControllerFactory _apiControllerFactory;
        private readonly IHttpResultHandlerFactory _httpResultHandlerFactory;

        public string InvokeRestOperationPost(string configuration, string metadata, string action, IDictionary<string, object> requestBody, string userName)
        {
            var verbProcessor = GetMetadata(configuration, metadata).FindVerbPost(action, requestBody);

            var httpResultHandler = _httpResultHandlerFactory.GetResultHandler(verbProcessor.HttpResultHandler);

            return httpResultHandler.WrapResult(InvokeRestVerb(verbProcessor, configuration, metadata, action, userName)).Content.ReadAsStringAsync().Result;
        }

        public string InvokeRestOperationGet(string configuration, string metadata, string action, IDictionary<string, object> requestBody, string userName)
        {
            var verbProcessor = GetMetadata(configuration, metadata).FindVerbGet(action, requestBody);

            var httpResultHandler = _httpResultHandlerFactory.GetResultHandler(verbProcessor.HttpResultHandler);

            return httpResultHandler.WrapResult(InvokeRestVerb(verbProcessor, configuration, metadata, action, userName)).Content.ReadAsStringAsync().Result;
        }

        public string InvokeRestOperationUpload(string configuration, string metadata, string action, object requestBody, string filePath, string userName)
        {
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                var verbProcessor = GetMetadata(configuration, metadata)
                    .FindUploadVerb(action, requestBody, stream);

                var httpResultHandler = _httpResultHandlerFactory.GetResultHandler(verbProcessor.HttpResultHandler);

                return httpResultHandler.WrapResult(InvokeRestVerb(verbProcessor, configuration, metadata, action, userName)).Content.ReadAsStringAsync().Result;
            }
        }

        public string InvokeRestOperationUpload(string configuration, string metadata, string action, object requestBody, Stream file, string userName)
        {
            var verbProcessor = GetMetadata(configuration, metadata)
                .FindUploadVerb(action, requestBody, file);

            var httpResultHandler = _httpResultHandlerFactory.GetResultHandler(verbProcessor.HttpResultHandler);

            return httpResultHandler.WrapResult(InvokeRestVerb(verbProcessor, configuration, metadata, action, userName)).Content.ReadAsStringAsync().Result;
        }

        public string InvokeRestOperationDownload(string configuration, string metadata, string action, object requestBody, string userName)
        {
            var verbProcessor = GetMetadata(configuration, metadata)
                .FindVerbUrlEncodedData(action, requestBody);

            var httpResultHandler = _httpResultHandlerFactory.GetResultHandler(verbProcessor.HttpResultHandler);

            return httpResultHandler.WrapResult(InvokeRestVerb(verbProcessor, configuration, metadata, action, userName)).Content.ReadAsStringAsync().Result;
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

        private object InvokeRestVerb(TargetDelegate verbProcessor, string configuration, string metadata, string action, string userName)
        {
            if (verbProcessor != null)
            {
                var prop = verbProcessor.Target.GetType().GetProperties().FirstOrDefault(p => p.PropertyType.IsAssignableFrom(typeof(IConfigRequestProvider)));
                if (prop != null)
                {
                    prop.SetValue(verbProcessor.Target, new LocalDataProvider(configuration, metadata, action));
                }
                return verbProcessor.Invoke();
            }
            return null;
        }
    }
}