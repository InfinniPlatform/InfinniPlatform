using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Hosting;
using InfinniPlatform.WebApi.Properties;

namespace InfinniPlatform.WebApi.WebApi
{
    internal class RestVerbsContainer : IRestVerbsContainer, IRestVerbsRegistrator
    {
        private readonly string _controllerName;
	    private readonly Func<IContainer> _container;

	    private readonly List<MethodInvokationInfo> _invokationInfoList = new List<MethodInvokationInfo>(); 

        public RestVerbsContainer(string controllerName, Func<IContainer> container)
        {
            _controllerName = controllerName;
	        _container = container;
        }

        public string ControllerName {
            get { return _controllerName; }
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