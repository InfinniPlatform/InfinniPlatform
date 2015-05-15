using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;

namespace InfinniPlatform.Hosting.WebApi.Implementation
{
    internal class RestVerbsContainer : IRestVerbsContainer
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


	    public IRestVerbsContainer AddVerb(QueryHandler queryHandler)
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
            var invokationInfo = _invokationInfoList.Where(i => i.CanVerb(serviceName, verbType, verbArguments.Select(v => v.GetType()).ToList())).ToList();
            if (invokationInfo.Count == 0)
            {
                throw new ArgumentException("service not found!");
            }
            if (invokationInfo.Count > 1)
            {
                throw new ArgumentException("ambiguous method definitions!");
            }

            var instance = _container.Invoke().Resolve(invokationInfo.First().TargetType.QueryHandlerType);
            TargetDelegate verb = invokationInfo.First().ConstructDelegate(verbArguments, instance, invokationInfo.First().TargetType.HttpResultHandler);
            return verb;
        }
    }
}