using System;
using System.IO;
using System.Net;
using InfinniPlatform.Sdk.Properties;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Sdk.Api
{
    /// <summary>
    ///   API для работы с файлами
    /// </summary>
    public sealed class InfinniFileApi
    {
        private readonly string _server;
        private readonly string _port;
        private readonly string _version;
        private CookieContainer _cookieContainer;
        private readonly RouteBuilder _routeBuilder;

        public InfinniFileApi(string server, string port, string version)
        {
            _server = server;
            _port = port;
            _version = version;
            _routeBuilder = new RouteBuilder(_server, _port);
        }

        /// <summary>
        ///   Загрузить файл на сервер
        /// </summary>
        /// <param name="application">Приложение</param>
        /// <param name="documentType">Идентификатор документа</param>
        /// <param name="instanceId">Экземпляр документа</param>
        /// <param name="fieldName">Наименование поля в документе, хранящее ссылку на файл</param>
        /// <param name="fileName">Наименование файла</param>
        /// <param name="fileStream">Файловый поток</param>
        /// <returns>Результат загрузки файла на сервер</returns>
        public dynamic UploadFile(string application, string documentType, string instanceId, string fieldName, string fileName,
            Stream fileStream)
        {
            var restQueryExecutor = new RequestExecutor(_cookieContainer);


            var response = restQueryExecutor.QueryPostFile(_routeBuilder.BuildRestRoutingUploadFile(_version,application), instanceId, fieldName, fileName, fileStream);

            if (response.IsAllOk)
            {
                try
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        //гребаный JsonObjectSerializer вставляет служебный символ в начало строки
                        return JObject.Parse(response.Content.Remove(0, 1));
                    }
                }
                catch (Exception)
                {
                    throw new ArgumentException(Resources.ResultIsNotOfObjectType);
                }
            }
            throw new ArgumentException(string.Format(Resources.FailToUploadFileOnServer, response.GetErrorContent()));
        }

        /// <summary>
        ///   Загрузить файл с сервера
        /// </summary>
        /// <param name="application">Идентификатор приложения</param>
        /// <param name="documentType">Идентификатор документа</param>
        /// <param name="instanceId">Идентификатор экземпляра документа</param>
        /// <param name="fieldName">Наименование поля ссылки</param>
        /// <returns>Выгруженный контент</returns>
        public dynamic DownloadFile(string application, string documentType, string instanceId, string fieldName)
        {
            var restQueryExecutor = new RequestExecutor(_cookieContainer);

            var linkedData = new
            {
                Application = application,
                DocumentType = documentType,
                InstanceId = instanceId,
                FieldName = fieldName
            };

            var response = restQueryExecutor.QueryGetUrlEncodedData(
                    _routeBuilder.BuildRestRoutingDownloadFile(_version, application), linkedData);

            if (response.IsAllOk)
            {
                try
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        //гребаный JsonObjectSerializer вставляет служебный символ в начало строки
                        return JObject.Parse(response.Content.Remove(0, 1));
                    }
                }
                catch (Exception)
                {
                    throw new ArgumentException(Resources.ResultIsNotOfObjectType);
                }
            }
            throw new ArgumentException(string.Format(Resources.FailToDownloadFileFromServer, response.GetErrorContent()));
        }
    }
}
