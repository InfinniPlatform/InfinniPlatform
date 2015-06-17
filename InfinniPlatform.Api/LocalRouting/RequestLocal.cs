using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.RestQuery;

namespace InfinniPlatform.Api.LocalRouting
{
    public static class RequestLocal
    {
        private static IRequestLocal _requestInstance;
	    private static readonly Type RequestType;


	    static RequestLocal()
        {
            var pathToRequestAssembly = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InfinniPlatform.WebApi.dll");
            if (!File.Exists(pathToRequestAssembly))
            {
                throw new ArgumentException("Can't execute request local. Executor assembly not found.");
            }

            var assembly = Assembly.Load(File.ReadAllBytes(pathToRequestAssembly));
            RequestType = assembly.GetTypes().FirstOrDefault(t => typeof (IRequestLocal).IsAssignableFrom(t));
            if (RequestType == null)
            {
                throw new ArgumentException("Can't find local request executor type.");
            }     
       		
        }

	    public static IApiControllerFactory ApiControllerFactory { get; set; }

	    public static IRequestLocal Instance
		{
		    get
		    {
			    if (_requestInstance != null)
			    {
				    return _requestInstance;
			    }

			    if (ApiControllerFactory == null)
			    {
				    throw new ArgumentException(Resources.FailCreateRequestLocal);
			    }

			    _requestInstance = (IRequestLocal) Activator.CreateInstance(RequestType, ApiControllerFactory);

				return _requestInstance;
		    }
		}

        public static string InvokeRestOperationPost(string version, string configuration, string metadata, string action, IDictionary<string, object> requestBody, string userName)
        {
			return Instance.InvokeRestOperationPost(version, configuration, metadata, action, requestBody, userName);

        }

        public static string InvokeRestOperationGet(string version, string configuration, string metadata, string action,
                                          IDictionary<string, object> requestBody, string userName)
        {
			return Instance.InvokeRestOperationGet(version, configuration, metadata, action, requestBody, userName);

        }

        public static string InvokeRestOperationUpload(string version, string configuration, string metadata, string action, object requestBody, string filePath, string userName)
        {
			return Instance.InvokeRestOperationUpload(version, configuration, metadata, action, requestBody, filePath, userName);
        }

        public static string InvokeRestOperationUpload(string version, string configuration, string metadata, string action, object requestBody, Stream file, string userName)
        {
            return Instance.InvokeRestOperationUpload(version, configuration, metadata, action, requestBody, file, userName);
        }

        public static string InvokeRestOperationDownload(string version, string configuration, string metadata, string action,
            object requestBody, string userName)
        {
            return Instance.InvokeRestOperationDownload(version, configuration, metadata, action, requestBody, userName);
        }
    }
}
