using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Hosting
{

		public sealed class TargetDelegate
		{
			private readonly MethodInfo _methodInfo;
			private readonly object _target;
			private readonly object[] _arguments;
			private readonly HttpResultHandlerType _httpResultHandler;

			public TargetDelegate(MethodInfo methodInfo, object target, object[] arguments, HttpResultHandlerType httpResultHandler)
			{
				_methodInfo = methodInfo;
				_target = target;
				_arguments = arguments;
				_httpResultHandler = httpResultHandler;
			}

			public object Target
			{
				get { return _target; }
			}

			public object[] Arguments
			{
				get { return _arguments; }
			}

			public MethodInfo MethodInfo
			{
				get { return _methodInfo; }
			}

			public HttpResultHandlerType HttpResultHandler
			{
				get { return _httpResultHandler; }
			}

			public object Invoke()
			{
				return MethodInfo.Invoke(Target, Arguments);
			}
		}
	
}
