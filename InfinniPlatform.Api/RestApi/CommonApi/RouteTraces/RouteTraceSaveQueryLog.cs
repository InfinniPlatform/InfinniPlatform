using System.Collections.Generic;
using Newtonsoft.Json;

namespace InfinniPlatform.Api.RestApi.CommonApi.RouteTraces
{
    /// <summary>
    ///     Трассировщик для создания лога запросов, анализируемого конструктором документации
    /// </summary>
    public class RouteTraceSaveQueryLog : IRouteTrace
    {
        private readonly List<CatchedQueryInfo> _queries = new List<CatchedQueryInfo>();

        /// <summary>
        ///     Трассировать запрос
        /// </summary>
        /// <param name="configuration">Конфигурация</param>
        /// <param name="metadata">Метаданные</param>
        /// <param name="action">Действие</param>
        /// <param name="url">Url трассируемого запроса</param>
        /// <param name="body">Тело трассируемого запроса</param>
        /// <param name="verbType">Тип запроса (POST/GET)</param>
        /// <param name="content">Результат запроса</param>
        public void Trace(
            string configuration,
            string metadata,
            string action,
            string url,
            object body,
            string verbType,
            string content)
        {
            var stringBody = JsonConvert.SerializeObject(body, Formatting.Indented);
            _queries.Add(new CatchedQueryInfo(configuration, metadata, action, verbType, url, stringBody, content));
        }

        public void ClearQueries()
        {
            _queries.Clear();
        }

        public IEnumerable<CatchedQueryInfo> GetCatchedData()
        {
            return _queries.ToArray();
        }
    }

    /// <summary>
    ///     Инкапсулирует информацию об одном REST запросе
    /// </summary>
    public sealed class CatchedQueryInfo
    {
        public CatchedQueryInfo(
            string configuration,
            string metadata,
            string action,
            string queryType,
            string url,
            string body,
            string content)
        {
            Metadata = metadata;
            Configuration = configuration;
            Action = action;
            QueryType = queryType;
            Url = url;
            Body = body;
            ResponseContent = content;
        }

        public string Metadata { get; private set; }
        public string Configuration { get; private set; }
        public string Action { get; private set; }
        public string QueryType { get; private set; }
        public string Url { get; private set; }
        public string Body { get; private set; }
        public string ResponseContent { get; private set; }
    }
}