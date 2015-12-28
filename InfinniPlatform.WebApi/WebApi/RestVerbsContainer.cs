using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.RestApi;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Hosting;
using InfinniPlatform.Sdk.Environment.Hosting;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.WebApi.WebApi
{
    internal class RestVerbsContainer : IRestVerbsContainer, IRestVerbsRegistrator
    {
        public RestVerbsContainer(string configId, string documentType, IContainerResolver containerResolver)
        {
            _configId = configId;
            _documentType = documentType;
            _containerResolver = containerResolver;
            _invokationInfoList = new List<MethodInvokationInfo>();
        }


        private readonly string _configId;
        private readonly string _documentType;
        private readonly IContainerResolver _containerResolver;
        private readonly List<MethodInvokationInfo> _invokationInfoList;


        public TargetDelegate FindVerbGet(string serviceName, IDictionary<string, object> verbArguments)
        {
            return FindVerb(serviceName, VerbType.Get, verbArguments);
        }

        public TargetDelegate FindUploadVerb(string serviceName, dynamic linkedData, Stream uploadStream)
        {
            return FindVerb(serviceName, VerbType.Upload, new Dictionary<string, object> { { "linkedData", linkedData }, { "uploadStream", uploadStream } });
        }

        public TargetDelegate FindVerbUrlEncodedData(string serviceName, dynamic argument)
        {
            return FindVerb(serviceName, VerbType.UrlEncodedData, new Dictionary<string, object> { { "parameters", argument } });
        }

        public TargetDelegate FindVerbPost(string serviceName, IDictionary<string, object> verbArguments)
        {
            return FindVerb(serviceName, VerbType.Post, verbArguments);
        }

        public TargetDelegate FindVerbPut(string serviceName, IDictionary<string, object> verbArguments)
        {
            return FindVerb(serviceName, VerbType.Put, verbArguments);
        }

        public IRestVerbsContainer AddVerb(IQueryHandler queryHandler)
        {
            foreach (var actionHandler in queryHandler.ActionHandlers)
            {
                var verbType = actionHandler.VerbType;

                var serviceMethodInfo = queryHandler.QueryHandlerType.GetMethod(actionHandler.ActionName);

                if (serviceMethodInfo != null)
                {
                    var serviceNames = actionHandler.GetInstanceNames();

                    _invokationInfoList.Add(new MethodInvokationInfo(serviceMethodInfo, queryHandler, verbType, serviceNames));
                }
                else
                {
                    throw new ArgumentException($"Method not found: {actionHandler.ActionName}");
                }
            }

            return this;
        }


        public bool HasRoute(string configId, string documentType)
        {
            return string.Equals(configId, _configId, StringComparison.OrdinalIgnoreCase)
                   && string.Equals(documentType, _documentType, StringComparison.OrdinalIgnoreCase);
        }

        public bool HasRoute(string configId)
        {
            return string.Equals(configId, _configId, StringComparison.OrdinalIgnoreCase);
        }


        private TargetDelegate FindVerb(string serviceName, VerbType verbType, IDictionary<string, object> verbArguments)
        {
            verbArguments = verbArguments ?? new Dictionary<string, object>();

            var serviceMethodInfo = _invokationInfoList.FirstOrDefault(i => i.CanVerb(serviceName, verbType));

            if (serviceMethodInfo == null)
            {
                throw new ArgumentException(Resources.ServiceNotFoundError);
            }

            var serviceHandlerType = serviceMethodInfo.TargetType.QueryHandlerType;
            var serviceHandlerInstance = _containerResolver.Resolve(serviceHandlerType);
            var serviceResultHandlerType = serviceMethodInfo.TargetType.HttpResultHandlerType;
            var serviceHandlerDelegate = serviceMethodInfo.ConstructDelegate(verbArguments, serviceHandlerInstance, serviceResultHandlerType);

            return serviceHandlerDelegate;
        }
    }
}