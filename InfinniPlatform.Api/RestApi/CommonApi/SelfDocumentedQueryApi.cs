using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Events;
using InfinniPlatform.Api.RestApi.CommonApi.RouteTraces;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Api.RestQuery.EventObjects;
using InfinniPlatform.Api.RestQuery.EventObjects.EventSerializers;
using InfinniPlatform.Api.SelfDocumentation;
using Newtonsoft.Json;

namespace InfinniPlatform.Api.RestApi.CommonApi
{
    public static class SelfDocumentedQueryApi
    {
        private static readonly List<CatchedQueryInfo> Queries = new List<CatchedQueryInfo>();

        public static void ClearQueries()
        {
            Queries.Clear();
        }

        public static CatchedQueryInfo[] GetCatchedData()
        {
            return Queries.ToArray();
        }

        public static RestQueryResponse QueryPostRaw(
            string configuration,
            string metadata,
            string action, string id,
            object body, bool replace = false)
        {
            IEnumerable<EventDefinition> events = new List<EventDefinition>();

            if (body != null)
            {
                var customSerializer = body as IObjectToEventSerializer;
                events = customSerializer != null
                    ? customSerializer.GetEvents()
                    : new ObjectToEventSerializerStandard(body).GetEvents();
            }

            var eventsBody = new Dictionary<string, object>
            {
                {"id", id},
                {"events", events},
                {"replace", replace}
            };

            var url = ControllerRoutingFactory.Instance.BuildRestRoutingUrlStandardApi(configuration, metadata, action);

            var restResponse = new RestQueryExecutor().QueryPost(
                url,
                eventsBody);

            Queries.Add(new CatchedQueryInfo(
                "POST",
                url,
                JsonConvert.SerializeObject(eventsBody, Formatting.Indented)
                ));

            return restResponse;
        }

        public static RestQueryResponse QueryPostJsonRaw(
            string configuration,
            string metadata,
            string action, string id,
            object body, bool replace = false)
        {
            var url = ControllerRoutingFactory.Instance.BuildRestRoutingUrlStandardApi(configuration, metadata, action);



            var restResponse = new RestQueryBuilder(configuration,metadata,action, TODO).QueryPostJson(
                id,
                body,replace);


            Queries.Add(new CatchedQueryInfo(
                "POST",
                url,
                JsonConvert.SerializeObject(body.ToJObject(),Formatting.Indented)));

            return restResponse;
        }

        public static RestQueryResponse QueryGetRaw(string configuration, string metadata, string action,
            IEnumerable<object> filter, int pageNumber, int pageSize)
        {
            var searchBody = new Dictionary<string, object>
            {
                {"FilterObject", filter != null ? filter.Select(f => f.ToJObject()).ToList() : null},
                {"PageNumber", pageNumber},
                {"PageSize", pageSize},
                {"SearchType", 1},
                {"Version", null}
            };

            var url = ControllerRoutingFactory.Instance.BuildRestRoutingUrlStandardApi(configuration, metadata, action);

            var restResponse = new RestQueryExecutor().QueryGet(url, searchBody);

            Queries.Add(new CatchedQueryInfo(
                "GET",
                string.Format("{0}?query={1}", url, JsonConvert.SerializeObject(searchBody,Formatting.Indented)),
                ""));

            if (!restResponse.IsAllOk)
            {
                throw new ArgumentException(restResponse.Content);
            }
            return restResponse;
        }


    }
}
