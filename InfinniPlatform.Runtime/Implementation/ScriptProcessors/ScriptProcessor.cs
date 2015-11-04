using System;
using System.Collections.Generic;
using System.Reflection;

using InfinniPlatform.Logging;
using InfinniPlatform.Runtime.Properties;
using InfinniPlatform.Sdk.Environment.Log;
using InfinniPlatform.Sdk.Environment.Scripts;

namespace InfinniPlatform.Runtime.Implementation.ScriptProcessors
{
    public sealed class ScriptProcessor : IScriptProcessor
    {
        public ScriptProcessor(IMethodInvokationCacheList scriptCache, IScriptMetadataProvider scriptMetadataProvider)
        {
            _scriptCache = scriptCache;
            _scriptMetadataProvider = scriptMetadataProvider;
        }

        private readonly IMethodInvokationCacheList _scriptCache;
        private readonly IScriptMetadataProvider _scriptMetadataProvider;

        public object InvokeScript(string scriptIdentifier, object scriptContext)
        {
            var start = DateTime.Now;

            var scriptMetadata = _scriptMetadataProvider.GetScriptMetadata(scriptIdentifier);

            if (scriptMetadata == null)
            {
                throw new ArgumentException(string.Format(Resources.ScriptMetadataIsNotRegistered, scriptIdentifier));
            }

            var scriptType = scriptMetadata.Type;
            var scriptMethod = scriptMetadata.Method;

            MethodInfo scriptMethodInfo = null;

            // Авторский комментарий: "Если версия не указана, то не знаем, в каком именно кэше искать необходимый метод
            // поэтому, перебираем все, упорядоченные по дате кэши методов". Еще раз подтверждает тот факт, что две версии
            // в рамках одного процесса работать не будут. А если будут, то это будет случайностью, так как берется первый
            // попавшийся скрипт из наиболее подходящего кэша. Кстати говоря, такой перебор увеличивает сложность алгоритма
            // с O(n^3) до O(n^4). В переделанной реализации, от которой так же придется избавиться в силу ее неактуальности,
            // сложность этого кода была доведена практически до O(log(n)).

            foreach (var cache in _scriptCache.CacheList)
            {
                scriptMethodInfo = cache.FindMethodInfo(scriptType, scriptMethod);

                if (scriptMethodInfo != null)
                {
                    break;
                }
            }

            var context = new Dictionary<string, object>
                          {
                              { "scriptIdentifier", scriptIdentifier },
                              { "scriptType", scriptType },
                              { "scriptMethod", scriptMethod }
                          };

            const string component = "ScriptProcessor";
            const string method = "Invoke";

            if (scriptMethodInfo != null)
            {
                var handlerType = scriptMethodInfo.ReflectedType;

                if (handlerType != null)
                {
                    var handler = Activator.CreateInstance(handlerType);
                    var parameters = new[] { scriptContext };

                    try
                    {
                        var result = scriptMethodInfo.Invoke(handler, parameters);

                        Logger.PerformanceLog.Log(component, method, start, null);

                        return result;
                    }
                    catch (TargetInvocationException e)
                    {
                        var error = e.InnerException ?? e;

                        Logger.Log.Error(Resources.ScriptCompletedWithError, context, error);
                        Logger.PerformanceLog.Log(component, method, start, Resources.ScriptCompletedWithError);

                        throw error;
                    }
                    catch (Exception e)
                    {
                        Logger.Log.Error(Resources.ScriptCompletedWithError, context, e);
                        Logger.PerformanceLog.Log(component, method, start, Resources.ScriptCompletedWithError);

                        throw;
                    }
                }
            }

            Logger.Log.Warn(Resources.CannotFindScriptImplementation, context);
            Logger.PerformanceLog.Log(component, method, start, Resources.CannotFindScriptImplementation);

            throw new ArgumentException(Resources.CannotFindScriptImplementation).AddContextData(context);
        }
    }
}