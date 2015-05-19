using System;
using System.Collections.Generic;
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

        public static string InvokeRestOperationPost(string configuration, string metadata, string action,
                                          IDictionary<string, object> requestBody, string userName)
        {
			return Instance.InvokeRestOperationPost(configuration, metadata, action, requestBody, userName);

        }

        public static string InvokeRestOperationGet(string configuration, string metadata, string action,
                                          IDictionary<string, object> requestBody, string userName)
        {
			return Instance.InvokeRestOperationGet(configuration, metadata, action, requestBody, userName);

        }

        public static string InvokeRestOperationUpload(string configuration, string metadata, string action, object requestBody, string filePath, string userName)
        {
			return Instance.InvokeRestOperationUpload(configuration, metadata, action, requestBody, filePath, userName);
        }

        public static string InvokeRestOperationUpload(string configuration, string metadata, string action, object requestBody, string fileName, Stream file, string userName)
        {
            return Instance.InvokeRestOperationUpload(configuration, metadata, action, requestBody, fileName, file, userName);
        }

        public static string InvokeRestOperationDownload(string configuration, string metadata, string action,
            object requestBody, string userName)
        {
            return Instance.InvokeRestOperationDownload(configuration, metadata, action, requestBody, userName);
        }
    }
}
