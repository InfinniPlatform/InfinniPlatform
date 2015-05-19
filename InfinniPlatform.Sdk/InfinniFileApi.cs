using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Sdk.Properties;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Sdk
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
        /// <param name="applicationId">Приложение</param>
        /// <param name="documentId">Идентификатор документа</param>
        /// <param name="instanceId">Экземпляр документа</param>
        /// <param name="fieldName">Наименование поля в документе, хранящее ссылку на файл</param>
        /// <param name="fileName">Наименование файла</param>
        /// <param name="fileStream">Файловый поток</param>
        /// <returns>Результат загрузки файла на сервер</returns>
        public dynamic UploadFile(string applicationId, string documentId, string instanceId, string fieldName, string fileName,
            Stream fileStream)
        {
            var restQueryExecutor = new RequestExecutor(_cookieContainer);

            var linkedData = new
            {
                Configuration = applicationId,
                Metadata = documentId,
                DocumentId = instanceId,
                FieldName = fieldName               
            };

            var response = restQueryExecutor.QueryPostFile(_routeBuilder.BuildRestRoutingUploadFile(_version,applicationId), linkedData, fileName, fileStream);

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
        /// <param name="applicationId">Идентификатор приложения</param>
        /// <param name="documentId">Идентификатор документа</param>
        /// <param name="instanceId">Идентификатор экземпляра документа</param>
        /// <param name="fieldName">Наименование поля ссылки</param>
        /// <returns>Выгруженный контент</returns>
        public dynamic DownloadFile(string applicationId, string documentId, string instanceId, string fieldName)
        {
            var restQueryExecutor = new RequestExecutor(_cookieContainer);

            var linkedData = new
            {
                Configuration = applicationId,
                Metadata = documentId,
                DocumentId = instanceId,
                FieldName = fieldName
            };

            var response = restQueryExecutor.QueryGetUrlEncodedData(
                    _routeBuilder.BuildRestRoutingDownloadFile(_version, applicationId), linkedData);

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
