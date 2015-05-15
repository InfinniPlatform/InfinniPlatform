using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Sdk.Properties;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Sdk
{
    /// <summary>
    ///   API для работы с документами
    /// </summary>
    public static class InfinniDocumentApi
    {
        /// <summary>
        ///   Получить документы по указанным фильтрам
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="documentType"></param>
        /// <param name="filter"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="sorting"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> GetDocument(
            string applicationId,
            string documentType,
            Action<FilterBuilder> filter,
            int pageNumber,
            int pageSize,
            Action<SortingBuilder> sorting = null)
        {

            if (!InfinniSession.Initialized)
            {
                throw new ArgumentException(Resources.SessionNotInitialized);
            }

            var restQueryExecutor = new RequestExecutor(InfinniSession.CookieContainer);

            var routeBuilder = new RouteBuilder(InfinniSession.Server, InfinniSession.Port);

            var filterBuilder = new FilterBuilder();
            if (filter != null)
            {
                filter.Invoke(filterBuilder);
            }

            var sortingBuilder = new SortingBuilder();
            if (sorting != null)
            {
                sorting.Invoke(sortingBuilder);
            }

            var response = restQueryExecutor.QueryGet(routeBuilder.BuildRestRoutingUrlDefault(InfinniSession.Version, applicationId, documentType),
                RequestExecutorExtensions.CreateQueryString(filterBuilder.GetFilter(), pageNumber, pageSize, sortingBuilder.GetSorting()));

            if (response.IsAllOk)
            {
                try
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        //гребаный JsonObjectSerializer вставляет служебный символ в начало строки
                        return JArray.Parse(response.Content.Remove(0,1));
                    }
                    return null;
                }
                catch (Exception)
                {
                    throw new ArgumentException(Resources.ResultIsNotOfArrayType);
                }
            }
            throw new ArgumentException(string.Format(Resources.FailGetDocument, response.Content));
        }


        public static string SetDocument(string applicationId, string documentType, object document)
        {
            if (!InfinniSession.Initialized)
            {
                throw new ArgumentException(Resources.SessionNotInitialized);
            }

            var restQueryExecutor = new RequestExecutor(InfinniSession.CookieContainer);

            var routeBuilder = new RouteBuilder(InfinniSession.Server, InfinniSession.Port);

            var response = restQueryExecutor.QueryPut(
                routeBuilder.BuildRestRoutingUrlDefault(InfinniSession.Version, applicationId, documentType),
                JObject.FromObject(document).ToString());

            if (response.IsAllOk)
            {
                try
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        //гребаный JsonObjectSerializer вставляет служебный символ в начало строки
                        dynamic result = JObject.Parse(response.Content.Remove(0, 1));
                        if (result.InstanceId != null && result.IsValid == true)
                        {
                            return result.InstanceId;
                        }
                        return null;
                    }
                    return null;
                }
                catch (Exception)
                {
                    throw new ArgumentException(Resources.ResultIsNotOfObjectType);
                }
            }
            throw new ArgumentException(string.Format("Fail to set document with exception: {0}", response.Content));
        }
    }
}
