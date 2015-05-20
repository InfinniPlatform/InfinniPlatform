using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Events;
using InfinniPlatform.Api.LocalRouting;
using InfinniPlatform.Api.Profiling;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.CommonApi.RouteTraces;
using InfinniPlatform.Api.RestQuery.EventObjects;
using InfinniPlatform.Api.RestQuery.EventObjects.EventSerializers;
using InfinniPlatform.Api.RestQuery.RestQueryExecutors;
using InfinniPlatform.Api.SearchOptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Api.RestQuery.RestQueryBuilders
{
    public sealed class LocalQueryBuilder : IRestQueryBuilder
    {

	    private readonly string _configuration;
	    private readonly string _metadata;
	    private readonly string _action;
	    private readonly string _userName;
	    private readonly Func<string, string, string, dynamic, IOperationProfiler> _operationProfiler;

	    private void ExecuteProfiledOperation(Action operation, dynamic body)
	    {
	        dynamic bodyObject = null;
	        if (body != null)
	        {
	            bodyObject = JObject.FromObject(body).ToString();
	        }
	        if (_operationProfiler != null)
			{
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

        public LocalQueryBuilder(string configuration, string metadata, string action,string userName, 
			Func<string, string, string, dynamic, IOperationProfiler> operationProfiler)
		{
		    _configuration = configuration;
		    _metadata = metadata;
		    _action = action;
	        _userName = userName;
	        _operationProfiler = operationProfiler;
		}

        public RestQueryResponse QueryPost(string id, dynamic changesObject, bool replaceObject, CookieContainer cookieContainer)
        {
            IEnumerable<EventDefinition> events = new List<EventDefinition>();


            if (changesObject != null)
            {
                var customSerializer = changesObject as IObjectToEventSerializer;
                events = customSerializer != null ? customSerializer.GetEvents() : new ObjectToEventSerializerStandard(changesObject).GetEvents();
            }

            var body = new Dictionary<string, object>
				           {
					           {"id", id},					           
					           {"events", events},
                               {"replace",replaceObject}
				           };


	        string result = null;
			ExecuteProfiledOperation(() =>
				                         {
											 result = RequestLocal.InvokeRestOperationPost(_configuration, _metadata, _action,  body, _userName);					                         
				                         },body);

            return new RestQueryResponse()
                {
                    Content = result,
                    HttpStatusCode = HttpStatusCode.OK
                };
        }

        public RestQueryResponse QueryGet(IEnumerable<object> filterObject, int pageNumber, int pageSize, int searchType = 1, string version = null, CookieContainer cookieContainer = null)
        {
            var searchBody = new Dictionary<string, object>
				           {
					           {"FilterObject", filterObject != null ? filterObject.Select(f => f.ToDynamic()).ToList() : null},							   
					           {"PageNumber", pageNumber},
							   {"PageSize",pageSize},
			           		   {"SearchType",searchType},
							   {"Version",version}
				           };


	        string result = null;
			ExecuteProfiledOperation(() =>
				                         {
											 result = RequestLocal.InvokeRestOperationGet(_configuration, _metadata, _action, searchBody, _userName);
				                         }, searchBody);


            return new RestQueryResponse()
                {
                    Content = result,
                    HttpStatusCode = HttpStatusCode.OK
                };
        }

        public RestQueryResponse QueryPostFile(object linkedData, string filePath, CookieContainer cookieContainer = null)
        {

	        string result = null;
			ExecuteProfiledOperation(() =>
				                         {
											 result = RequestLocal.InvokeRestOperationUpload(_configuration, _metadata, _action, linkedData, filePath, _userName);
				                         },null);

            return new RestQueryResponse()
                {
                    Content = result,
                    HttpStatusCode = HttpStatusCode.OK
                };
        }

        public RestQueryResponse QueryPostFile(object linkedData, Stream file, CookieContainer cookieContainer = null)
        {
            string result = null;
            ExecuteProfiledOperation(() =>
            {
                result = RequestLocal.InvokeRestOperationUpload(_configuration, _metadata, _action, linkedData, file, _userName);
            }, null);

            return new RestQueryResponse()
            {
                Content = result,
                HttpStatusCode = HttpStatusCode.OK
            };
        }


        public RestQueryResponse QueryAggregation(string aggregationConfiguration, string aggregationMetadata, IEnumerable<object> filterObject, IEnumerable<object> dimensions, IEnumerable<AggregationType> aggregationTypes, IEnumerable<string> aggregationFields, int pageNumber, int pageSize, CookieContainer cookieContainer = null)
        {
            var searchBody = new Dictionary<string, object>
				           {
                               {"AggregationConfiguration", aggregationConfiguration},
                               {"AggregationMetadata", aggregationMetadata},
					           {"FilterObject", filterObject},							   
					           {"Dimensions", dimensions},
					           {"AggregationTypes", aggregationTypes},
                               {"AggregationFields", aggregationFields},
                               {"PageNumber",pageNumber},
							   {"PageSize",pageSize},
				           };

	        string result = null;
			ExecuteProfiledOperation(() =>
				                         {
											 result = RequestLocal.InvokeRestOperationGet(_configuration, _metadata, _action, searchBody, _userName);
				                         },searchBody);

            return new RestQueryResponse()
                {
                    Content = result,
                    HttpStatusCode = HttpStatusCode.OK
                };
        }

        public RestQueryResponse QueryNotify(string metadataConfigurationId)
        {
            var body = new Dictionary<string, object>()
            {
                {"metadataConfigurationId", metadataConfigurationId}
            };

			string result = null;
	        ExecuteProfiledOperation(() =>
		                                 {
											 result = RequestLocal.InvokeRestOperationPost(_configuration, _metadata, _action, body, _userName);
		                                 },body);


            return new RestQueryResponse()
                {
                    Content = result,
                    HttpStatusCode = HttpStatusCode.OK
                };
        }

        public RestQueryResponse QueryPostJson(string id, object jsonObject, bool replaceObject = false, CookieContainer cookieContainer = null)
        {
            var body = new Dictionary<string, object>()
	            {
	                {"id", id},
	                {"changesObject", jsonObject},
	                {"replace", replaceObject}
	            };

	        string result = null;
			ExecuteProfiledOperation(() =>
				                         {
											 result = RequestLocal.InvokeRestOperationPost(_configuration, _metadata, _action, body,_userName);                    
				                         },body);
            return new RestQueryResponse()
                {
                    Content = result,
                    HttpStatusCode = HttpStatusCode.OK
                };
        }

	    public RestQueryResponse QueryPostUrlEncodedData(object linkedData)
	    {
		    throw new NotImplementedException("Can't make multipart data operations on server side");
	    }

	    public RestQueryResponse QueryGetUrlEncodedData(dynamic linkedData)
	    {
            string result = null;

            Dictionary<string,object> body = new Dictionary<string, object>
				           {
                               {"InstanceId",linkedData.InstanceId },
                               {"FieldName",linkedData.FieldName}
				           };

           
            ExecuteProfiledOperation(() =>
            {
                result = RequestLocal.InvokeRestOperationDownload(_configuration, _metadata, _action, body, _userName);
            }, body);


            return new RestQueryResponse()
            {
                Content = result,
                HttpStatusCode = HttpStatusCode.OK
            };
	    }
    }
}
