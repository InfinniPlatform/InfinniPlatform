using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Hosting;
using InfinniPlatform.Sdk.Environment.Hosting;
using InfinniPlatform.WebApi.Properties;

namespace InfinniPlatform.WebApi.WebApi
{
    internal class RestVerbsContainer : IRestVerbsContainer, IRestVerbsRegistrator
    {
        private readonly string _version;
        private readonly string _metadataConfigurationId;
        private readonly string _metadata;

        private readonly Func<IContainer> _container;

	    private readonly List<MethodInvokationInfo> _invokationInfoList = new List<MethodInvokationInfo>(); 

        public RestVerbsContainer(string version, string metadataConfigurationId, string metadata, Func<IContainer> container)
        {
            _version = version;
            _metadataConfigurationId = metadataConfigurationId;
            _metadata = metadata;
            _container = container;
        }

        public string ControllerName {
            get { return FormatTemplateName(_version, _metadataConfigurationId, _metadata); }
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
            return string.IsNullOrEmpty(_version) ? 
                FormatTemplateName(null, metadataConfigurationId, metadataName) == ControllerName :
                FormatTemplateName(version, metadataConfigurationId, metadataName) == ControllerName;
        }

        internal bool HasRoute(string version, string metadataConfigurationId)
        {
            return string.IsNullOrEmpty(_version) ?
                ControllerName.StartsWith(FormatBaseTemplateName(null, metadataConfigurationId)) :
                ControllerName.StartsWith(FormatBaseTemplateName(version, metadataConfigurationId));
        }

	    public IRestVerbsContainer AddVerb(IQueryHandler queryHandler)
	    {
		    foreach (var actionHandler in queryHandler.ActionHandlers)
		    {
			    var verbType = actionHandler.VerbType;
			    MethodInfo methodInfo = queryHandler.QueryHandlerType.GetMethod(actionHandler.ActionName);
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

        public TargetDelegate FindVerbGet(string serviceName, IDictionary<string, object> verbArguments)
        {
            return FindVerb(serviceName, VerbType.Get, verbArguments);
        }

        public TargetDelegate FindUploadVerb(string serviceName, dynamic linkedData, Stream uploadStream)
        {
            return FindVerb(serviceName, VerbType.Upload, new Dictionary<string, object>()
                {
                    {"linkedData",linkedData},
                    {"uploadStream",uploadStream}
                });
        }

	    public TargetDelegate FindVerbUrlEncodedData(string serviceName, dynamic argument)
	    {
		    return FindVerb(serviceName, VerbType.UrlEncodedData, new Dictionary<string, object>()
			                                                          {
				                                                          {"parameters",argument}
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

	    private TargetDelegate FindVerb(string serviceName, VerbType verbType, IDictionary<string, object> verbArguments)
	    {
		    verbArguments = verbArguments ?? new Dictionary<string, object>();
            var invokationInfo = _invokationInfoList.Where(i => i.CanVerb(serviceName, verbType)).ToList();
            if (invokationInfo.Count == 0)
            {
                throw new ArgumentException(Api.Properties.Resources.ServiceNotFoundError);
            }
            if (invokationInfo.Count > 1)
            {
                throw new ArgumentException(Api.Properties.Resources.AmbiguousServiceDefinitionError);
            }

	        var instance = _container.Invoke().Resolve(invokationInfo.First().TargetType.QueryHandlerType);
            TargetDelegate verb = invokationInfo.First().ConstructDelegate(verbArguments, instance, invokationInfo.First().TargetType.HttpResultHandlerType);
            return verb;
        }


    }
}