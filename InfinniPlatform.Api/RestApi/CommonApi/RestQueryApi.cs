using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

using InfinniPlatform.Api.Profiling;
using InfinniPlatform.Api.Profiling.Implementation;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Api.RestQuery.RestQueryBuilders;
using InfinniPlatform.Api.SearchOptions;

namespace InfinniPlatform.Api.RestApi.CommonApi
{
	public enum RoutingType
	{
		Local,
		Remote
	}

	public static class RestQueryApi
	{
		private static RoutingType _routingType;
		private static Func<string, string, string, string, IRestQueryBuilder> _queryBuilder;
		private static Func<string, string, string, dynamic, IOperationProfiler> _operationProfiler;

		static RestQueryApi()
		{
			SetRoutingType(RoutingType.Remote);
			SetProfilingType(ProfilingType.None);
		}

		public static RoutingType RoutingType
		{
			get { return _routingType; }
		}

		public static void SetRoutingType(RoutingType routingType)
		{
			if (routingType == RoutingType.Local)
			{
				_queryBuilder = (version, configuration, metadata, action) =>
								new LocalQueryBuilder(configuration, metadata, action, !string.IsNullOrEmpty(Thread.CurrentPrincipal.Identity.Name) ? Thread.CurrentPrincipal.Identity.Name : AuthorizationStorageExtensions.UnknownUser, version, _operationProfiler);
			}
			else
			{
                _queryBuilder = (version, configuration, metadata, action) =>
								new RestQueryBuilder(version,configuration, metadata, action, _operationProfiler);
			}

			_routingType = routingType;
		}

		public static ILog Log { get; set; }

		public static void SetProfilingType(ProfilingType profilingType)
		{
			if (profilingType == ProfilingType.ProfileWatch)
			{
				_operationProfiler =
					(configuration, metadata, action, body) => new RestQueryProfiler(Log, configuration, metadata, action, body);
			}
			else
			{
				_operationProfiler = (configuration, metadata, action, body) => new NoQueryProfiler();
			}
		}

		public static RestQueryResponse QueryPostFile(string configuration, string metadata, string action, object linkedData, string filePath, string version = null)
		{
			var builder = _queryBuilder(version, configuration, metadata, action);

			var profiler = _operationProfiler(configuration, metadata, action, null);

			profiler.Reset();
			var response = builder.QueryPostFile(linkedData, filePath, SignInApi.CookieContainer);
			profiler.TakeSnapshot();

			CheckResponse(response);

			return response;
		}

        public static RestQueryResponse QueryPostFile(string configuration, string metadata, string action, object linkedData, Stream fileStream, string version = null)
        {
            var builder = _queryBuilder(version, configuration, metadata, action);

            var profiler = _operationProfiler(configuration, metadata, action, null);

            profiler.Reset();
            var response = builder.QueryPostFile(linkedData, fileStream, SignInApi.CookieContainer);
            profiler.TakeSnapshot();

            CheckResponse(response);

            return response;
        }

		public static RestQueryResponse QueryPostUrlEncodedData(string configuration, string metadata, string action, object linkedData, string version = null)
		{
			var builder = _queryBuilder(version, configuration, metadata, action);

			var profiler = _operationProfiler(configuration, metadata, action, null);

			profiler.Reset();
			var response = builder.QueryPostUrlEncodedData(linkedData);
			profiler.TakeSnapshot();

			CheckResponse(response);

			return response;
		}

		public static RestQueryResponse QueryGetUrlEncodedData(string configuration, string metadata, string action, object linkedData, string version = null)
		{
			var builder = _queryBuilder(version, configuration, metadata, action);

			var profiler = _operationProfiler(configuration, metadata, action, null);

			profiler.Reset();
			var response = builder.QueryGetUrlEncodedData(linkedData);
			profiler.TakeSnapshot();

			CheckResponse(response);

			return response;
		}


		public static RestQueryResponse QueryPostRaw(string configuration, string metadata, string action, string id, object body, string version = null, bool replace = false)
		{
			var builder = _queryBuilder(version, configuration, metadata, action);

			var profiler = _operationProfiler(configuration, metadata, action, body);

			profiler.Reset();
			var response = builder.QueryPost(id, body, replace, SignInApi.CookieContainer);
			profiler.TakeSnapshot();

			CheckResponse(response);

			return response;
		}

		public static RestQueryResponse QueryPostJsonRaw(string configuration, string metadata, string action, string id, object body, string version = null, bool replace = false)
		{
			var profiler = _operationProfiler(configuration, metadata, action, body);

			profiler.Reset();
            var builder = _queryBuilder(version, configuration, metadata, action);
			profiler.TakeSnapshot();

			var response = builder.QueryPostJson(id, body, replace, SignInApi.CookieContainer);

			CheckResponse(response);

			return response;
		}

		public static RestQueryResponse QueryGetRaw(string configuration, string metadata, string action, IEnumerable<object> filter, int pageNumber, int pageSize, string version = null)
		{
			var profiler = _operationProfiler(configuration, metadata, action, null);

			profiler.Reset();
			var builder = _queryBuilder(version, configuration, metadata, action);
			profiler.TakeSnapshot();

            var response = builder.QueryGet(filter, pageNumber, pageSize, 1, SignInApi.CookieContainer);

			CheckResponse(response);

			return response;
		}

		public static RestQueryResponse QueryAggregationRaw(string configuration, string metadata, string action, string aggregationConfiguration, string aggregationMetadata, IEnumerable<object> filterObject, IEnumerable<dynamic> dimensions, IEnumerable<AggregationType> aggregationTypes, IEnumerable<string> aggregationFields, int pageNumber, int pageSize, IRouteTrace routeTrace = null, string version = null)
		{
			var builder = _queryBuilder(null, configuration, metadata, action);

			var response = builder.QueryAggregation(
				aggregationConfiguration,
				aggregationMetadata,
				filterObject,
				dimensions,
				aggregationTypes,
				aggregationFields,
				pageNumber,
				pageSize, SignInApi.CookieContainer);

			CheckResponse(response);

			return response;
		}

		public static RestQueryResponse QueryPostNotify(string version, string configurationId)
		{
			var builder = _queryBuilder(version,"Update", "Package", "Notify");

			var profiler = _operationProfiler(configurationId, string.Empty, "Notify", null);

			profiler.Reset();
			var response = builder.QueryNotify(configurationId);
			profiler.TakeSnapshot();

			CheckResponse(response);

			return response;
		}

		private static void CheckResponse(RestQueryResponse response)
		{
			if (!response.IsAllOk)
			{
				var errorMessage = response.Content;

				if (string.IsNullOrEmpty(errorMessage))
				{
					if (response.IsRemoteServerNotFound)
					{
						errorMessage = Resources.RemoteServerNotFound;
					}
					else if (response.IsServiceNotRegistered)
					{
						errorMessage = Resources.ServiceNotRegistered;
					}
					else if (response.IsBusinessLogicError)
					{
						errorMessage = Resources.BusinessLogicError;
					}
					else if (response.IsServerError)
					{
						errorMessage = Resources.InternalServerError;
					}
				}

				throw new ArgumentException(errorMessage);
			}
		}
	}
}