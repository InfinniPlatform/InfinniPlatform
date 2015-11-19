using System;
using System.Collections.Generic;
using System.IO;

using InfinniPlatform.Api.RestQuery.EventObjects;
using InfinniPlatform.Api.RestQuery.EventObjects.EventSerializers;
using InfinniPlatform.Api.RestQuery.RestQueryExecutors;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Profiling;
using InfinniPlatform.Sdk.Events;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Api.RestQuery.RestQueryBuilders
{
    /// <summary>
    /// Данный класс используется только на самом низком уровне абстракции в платформе
    /// Для выполнения всех без исключения запросов следует использовать RestQueryApi
    /// </summary>
    public class RestQueryBuilder : IRestQueryBuilder
    {
        public RestQueryBuilder(string configuration, string metadata, string action,
                                Func<string, string, string, dynamic, IOperationProfiler> operationProfiler)
        {
            _configuration = configuration;
            _metadata = metadata;
            _action = action;
            _operationProfiler = operationProfiler;
            _controllerRoutingFactory = ControllerRoutingFactory.Instance;
        }

        private readonly string _action;
        private readonly string _configuration;
        private readonly ControllerRoutingFactory _controllerRoutingFactory;
        private readonly string _metadata;
        private readonly Func<string, string, string, dynamic, IOperationProfiler> _operationProfiler;

        /// <summary>
        /// Выгрузить файл на сервер
        /// </summary>
        /// <param name="linkedData">Связанный объект</param>
        /// <param name="filePath">Путь к файлу</param>
        /// <returns>Ответ на вызов сервиса</returns>
        public RestQueryResponse QueryPostFile(object linkedData, string filePath)
        {
            var url = _controllerRoutingFactory.BuildRestRoutingUrlUpload(_configuration, _metadata, _action);

            RestQueryResponse response = null;
            ExecuteProfiledOperation(() => { response = new RestQueryExecutor().QueryPostFile(url, linkedData, filePath); }, null);

            return response;
        }

        public RestQueryResponse QueryPostFile(object linkedData, Stream fileStream)
        {
            var url = _controllerRoutingFactory.BuildRestRoutingUrlUpload(_configuration, _metadata, _action);

            RestQueryResponse response = null;

            ExecuteProfiledOperation(() => { response = new RestQueryExecutor().QueryPostFile(url, linkedData, fileStream); }, null);

            return response;
        }

        /// <summary>
        /// Сформировать и выполнить запрос на применение изменений
        /// </summary>
        /// <param name="id">Идентификатор объекта, который необходимо изменить</param>
        /// <param name="changesObject">Объект, из которого будет сформирован список изменений</param>
        /// <returns>Ответ на вызов сервиса</returns>
        public RestQueryResponse QueryPost(string id, object changesObject)
        {
            IEnumerable<EventDefinition> events = new List<EventDefinition>();

            if (changesObject != null)
            {
                var customSerializer = changesObject as IObjectToEventSerializer;
                events = customSerializer != null
                             ? customSerializer.GetEvents()
                             : new ObjectToEventSerializerStandard(changesObject).GetEvents();
            }

            var body = new Dictionary<string, object>
                       {
                           { "id", id },
                           { "events", events }
                       };

            var url = _controllerRoutingFactory.BuildRestRoutingUrlStandardApi(_configuration, _metadata, _action);

            RestQueryResponse response = null;
            ExecuteProfiledOperation(() => { response = new RestQueryExecutor().QueryPost(url, body); },
                                     body);

            return response;
        }

        /// <summary>
        /// Сформировать и выполнить запрос на применение изменений, представленных в виде JSON-объекта
        /// </summary>
        /// <param name="id">Идентификатор объекта, к которому будут применены изменения</param>
        /// <param name="jsonObject">Объект, который будет представлен в виде событий</param>
        /// <returns>Ответ сервиса</returns>
        public RestQueryResponse QueryPostJson(string id, object jsonObject)
        {
            var body = new Dictionary<string, object>
                       {
                           { "id", id },
                           { "changesObject", JsonConvert.SerializeObject(jsonObject, Formatting.Indented) }
                       };

            var url = _controllerRoutingFactory.BuildRestRoutingUrlStandardApi(_configuration, _metadata, _action);

            RestQueryResponse response = null;
            ExecuteProfiledOperation(() => { response = new RestQueryExecutor().QueryPost(url, body); }, body);

            return response;
        }

        /// <summary>
        /// Выполнить POST-запрос на сервер
        /// </summary>
        /// <param name="linkedData">Связанный объект</param>
        /// <returns>Ответ на вызов сервиса</returns>
        public RestQueryResponse QueryPostUrlEncodedData(object linkedData)
        {
            var url = _controllerRoutingFactory.BuildRestRoutingUrlUrlEncodedData(_configuration, _metadata, _action);

            RestQueryResponse response = null;
            ExecuteProfiledOperation(() => { response = new RestQueryExecutor().QueryPostUrlEncodedData(url, linkedData); }, null);

            return response;
        }

        public RestQueryResponse QueryGetUrlEncodedData(object linkedData)
        {
            var url = _controllerRoutingFactory.BuildRestRoutingUrlUrlEncodedData(_configuration, _metadata, _action);

            RestQueryResponse response = null;
            ExecuteProfiledOperation(() => { response = new RestQueryExecutor().QueryGetUrlEncodedData(url, linkedData); }, null);

            return response;
        }

        /// <summary>
        /// Сформировать и выполнить запрос на поиск данных
        /// </summary>
        /// <param name="filterObject">Фильтр по данным</param>
        /// <param name="pageNumber">Номер страницы данных</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <param name="searchType">Тип поиска записей</param>
        /// <returns>Ответ на вызов сервиса</returns>
        public RestQueryResponse QueryGet(IEnumerable<object> filterObject, int pageNumber, int pageSize,
                                          int searchType = 1)
        {
            var url = _controllerRoutingFactory.BuildRestRoutingUrlStandardApi(_configuration, _metadata, _action);

            var searchBody = new Dictionary<string, object>
                             {
                                 { "FilterObject", filterObject },
                                 { "PageNumber", pageNumber },
                                 { "PageSize", pageSize },
                                 { "SearchType", searchType }
                             };

            RestQueryResponse response = null;
            ExecuteProfiledOperation(() => { response = new RestQueryExecutor().QueryGet(url, searchBody); }, searchBody);

            return response;
        }

        /// <summary>
        /// Сформировать и выполнить запрос на агрегацию данных
        /// </summary>
        /// <param name="aggregationConfiguration">Конфигурация для выполнения агрегации</param>
        /// <param name="aggregationMetadata">Метаданные для выполнения агрегации</param>
        /// <param name="filterObject">Фильтр записей</param>
        /// <param name="dimensions">Описание срезов куба</param>
        /// <param name="aggregationTypes">Тип агрегации</param>
        /// <param name="aggregationFields">Имя поля, по которому необходимо рассчитать значение агрегации</param>
        /// <param name="pageNumber">Номер страницы данных</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <returns>Ответ на вызов сервиса</returns>
        public RestQueryResponse QueryAggregation(string aggregationConfiguration, string aggregationMetadata,
                                                  IEnumerable<object> filterObject, IEnumerable<object> dimensions,
                                                  IEnumerable<AggregationType> aggregationTypes, IEnumerable<string> aggregationFields, int pageNumber,
                                                  int pageSize)
        {
            var url = _controllerRoutingFactory.BuildRestRoutingUrlStandardApi(_configuration, _metadata, _action);

            var searchBody = new Dictionary<string, object>
                             {
                                 { "AggregationConfiguration", aggregationConfiguration },
                                 { "AggregationMetadata", aggregationMetadata },
                                 { "FilterObject", filterObject },
                                 { "Dimensions", dimensions },
                                 { "AggregationTypes", aggregationTypes },
                                 { "AggregationFields", aggregationFields },
                                 { "PageNumber", pageNumber },
                                 { "PageSize", pageSize }
                             };

            RestQueryResponse response = null;
            ExecuteProfiledOperation(() => { response = new RestQueryExecutor().QueryGet(url, searchBody); }, searchBody);
            return response;
        }

        /// <summary>
        /// Сформировать и выполнить запрос на системную нотификацию
        /// </summary>
        /// <returns>Ответ на вызов сервиса</returns>
        public RestQueryResponse QueryNotify(string metadataConfigurationId)
        {
            var url = _controllerRoutingFactory.BuildRestRoutingUrlStandardApi(_configuration, _metadata, _action);
            var body = new
                       {
                           version = "", metadataConfigurationId
                       };

            RestQueryResponse response = null;
            ExecuteProfiledOperation(() => { response = new RestQueryExecutor().QueryPost(url, body); }, body);
            return response;
        }

        private void ExecuteProfiledOperation(Action operation, dynamic body)
        {
            if (_operationProfiler != null)
            {
                dynamic bodyObject = null;
                if (body != null)
                {
                    bodyObject = JObject.FromObject(body).ToString();
                }

                var profiler = _operationProfiler(_configuration, _metadata, _action, bodyObject);
                profiler.Reset();
                operation();
                profiler.TakeSnapshot();
            }
            else
            {
                operation();
            }
        }

        /// <summary>
        /// Сформировать и выполнить запрос на системную нотификацию
        /// </summary>
        /// <returns>Ответ на вызов сервиса</returns>
        public RestQueryResponse QueryHelp(string metadataConfigurationId)
        {
            var searchBody = new Dictionary<string, object>
                             {
                                 { "id", metadataConfigurationId }
                             };

            var url = _controllerRoutingFactory.BuildRestRoutingUrlStandardApi(_configuration, _metadata, _action);

            RestQueryResponse response = null;
            ExecuteProfiledOperation(() => { response = new RestQueryExecutor().QueryGet(url, searchBody); }, null);
            return response;
        }
    }
}