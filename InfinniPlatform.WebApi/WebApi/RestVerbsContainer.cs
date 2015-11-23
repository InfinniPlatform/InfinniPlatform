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
        public RestVerbsContainer(string version, string metadataConfigurationId, string metadata, Func<IContainerResolver> containerResolverFactory)
        {
            _version = version;
            _metadataConfigurationId = metadataConfigurationId;
            _metadata = metadata;
            _containerResolverFactory = containerResolverFactory;
        }

        private readonly Func<IContainerResolver> _containerResolverFactory;
        private readonly List<MethodInvokationInfo> _invokationInfoList = new List<MethodInvokationInfo>();
        private readonly string _metadata;
        private readonly string _metadataConfigurationId;
        private readonly string _version;

        public string ControllerName
        {
            get { return FormatTemplateName(_version, _metadataConfigurationId, _metadata); }
        }

        public TargetDelegate FindVerbGet(string serviceName, IDictionary<string, object> verbArguments)
        {
            return FindVerb(serviceName, VerbType.Get, verbArguments);
        }

        public TargetDelegate FindUploadVerb(string serviceName, dynamic linkedData, Stream uploadStream)
        {
            return FindVerb(serviceName, VerbType.Upload, new Dictionary<string, object>
                                                          {
                                                              { "linkedData", linkedData },
                                                              { "uploadStream", uploadStream }
                                                          });
        }

        public TargetDelegate FindVerbUrlEncodedData(string serviceName, dynamic argument)
        {
            return FindVerb(serviceName, VerbType.UrlEncodedData, new Dictionary<string, object>
                                                                  {
                                                                      { "parameters", argument }
                                                                  });
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
                var methodInfo = queryHandler.QueryHandlerType.GetMethod(actionHandler.ActionName);
                if (methodInfo != null)
                {
                    _invokationInfoList.Add(new MethodInvokationInfo(methodInfo, queryHandler, verbType,
                        actionHandler.GetInstanceNames()));
                }
                else
                {
                    throw new ArgumentException(string.Format("Method not found: {0}", actionHandler.ActionName));
                }
            }

            return this;
        }

        private string FormatTemplateName(string version, string metadataConfigurationId, string metadataName)
        {
            return string.Format("{0}_{1}_{2}", version, metadataConfigurationId, metadataName).ToLowerInvariant();
        }

        private string FormatBaseTemplateName(string version, string metadataConfigurationId)
        {
            return string.Format("{0}_{1}", version, metadataConfigurationId).ToLowerInvariant();
        }

        internal bool HasRoute(string version, string metadataConfigurationId, string metadataName)
        {
            //если при регистрации сервиса указана версия конфигурации, то определяем роутинг, принимая во внимание номер версии
            //в противном случае, не учитываем номер версии
            return string.IsNullOrEmpty(_version)
                ? FormatTemplateName(null, metadataConfigurationId, metadataName) == ControllerName
                : FormatTemplateName(version, metadataConfigurationId, metadataName) == ControllerName;
        }

        internal bool HasRoute(string version, string metadataConfigurationId)
        {
            return string.IsNullOrEmpty(_version)
                ? ControllerName.StartsWith(FormatBaseTemplateName(null, metadataConfigurationId))
                : ControllerName.StartsWith(FormatBaseTemplateName(version, metadataConfigurationId));
        }

        private TargetDelegate FindVerb(string serviceName, VerbType verbType, IDictionary<string, object> verbArguments)
        {
            verbArguments = verbArguments ?? new Dictionary<string, object>();
            var invokationInfo = _invokationInfoList.Where(i => i.CanVerb(serviceName, verbType)).ToList();
            if (invokationInfo.Count == 0)
            {
                throw new ArgumentException(Resources.ServiceNotFoundError);
            }

            var instance = _containerResolverFactory.Invoke().Resolve(invokationInfo.First().TargetType.QueryHandlerType);
            var verb = invokationInfo.First().ConstructDelegate(verbArguments, instance, invokationInfo.First().TargetType.HttpResultHandlerType);
            return verb;
        }
    }
}