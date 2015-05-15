using System;
using System.Reflection;
using InfinniPlatform.Api.Factories;

namespace InfinniPlatform.Runtime.Implementation.ScriptProcessors
{
	public sealed class ScriptProcessor : IScriptProcessor
	{
		private MethodInvokationCacheList _methodInvokationCacheList;
		private readonly IScriptMetadataProvider _scriptMetadataProvider;

		public ScriptProcessor(MethodInvokationCacheList methodInvokationCacheList, IScriptMetadataProvider scriptMetadataProvider)
		{
			_methodInvokationCacheList = methodInvokationCacheList;
			_scriptMetadataProvider = scriptMetadataProvider;
		}


		public object InvokeScript(string scriptIdentifier, dynamic scriptContext)
		{
			var scriptMetadata = _scriptMetadataProvider.GetScriptMetadata(scriptIdentifier);
			if (scriptMetadata == null)
			{
				throw new ArgumentException(string.Format("Script metadata for {0} not registered", scriptIdentifier));
			}

		    MethodInfo methodInfo = null;
		    if (scriptContext.Version != null)
		    {
		        var cache = _methodInvokationCacheList.GetCache(scriptContext.Version,true);
		        if (cache == null)
		        {
		            throw new ArgumentException(string.Format("script configuration version \"{0}\" not found!",
		                                                      scriptContext.Version));
		        }
                methodInfo = cache.FindMethodInfo(scriptMetadata.Type, scriptMetadata.Method);
		    }
		    else
		    {
                //если версия не указана, то не знаем, в каком именно кэше искать необходимый метод
                //поэтому, перебираем все, упорядоченные по дате кэши методов
		        foreach (var cache in _methodInvokationCacheList.CacheList)
		        {
                    methodInfo = cache.FindMethodInfo(scriptMetadata.Type, scriptMetadata.Method);
                    if (methodInfo != null)
                    {
                        break;
                    }
		        }
		    }
		    
			if (methodInfo != null)
			{
				return methodInfo.Invoke(Activator.CreateInstance(methodInfo.ReflectedType), new object[] {scriptContext});
			}

		    throw new ArgumentException(string.Format("instance type \"{0}\", action \"{1}\", not found!", scriptMetadata.Type, scriptMetadata.Method));
		}

	    public void UpdateCache(MethodInvokationCacheList methodInvokationCacheList)
		{
			_methodInvokationCacheList = methodInvokationCacheList;
		}
	}
}
