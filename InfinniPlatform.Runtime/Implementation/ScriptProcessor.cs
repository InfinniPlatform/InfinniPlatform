using System;
using System.Collections.Generic;

using InfinniPlatform.Core.Runtime;
using InfinniPlatform.Runtime.Properties;
using InfinniPlatform.Sdk.Logging;

namespace InfinniPlatform.Runtime.Implementation
{
    internal sealed class ScriptProcessor : IScriptProcessor
    {
        private const string PerformanceLogComponent = "ScriptProcessor";
        private const string PerformanceLogMethod = "Invoke";


        public ScriptProcessor(IScriptMetadataProvider scriptMetadataProvider, ActionUnitFactory actionUnitFactory, IPerformanceLog performanceLog, ILog log)
        {
            _scriptMetadataProvider = scriptMetadataProvider;
            _actionUnitFactory = actionUnitFactory;
            _performanceLog = performanceLog;
            _log = log;
        }


        private readonly IScriptMetadataProvider _scriptMetadataProvider;
        private readonly ActionUnitFactory _actionUnitFactory;
        private readonly IPerformanceLog _performanceLog;
        private readonly ILog _log;


        public void InvokeScript(string actionUnitId, object actionUnitContext)
        {
            var start = DateTime.Now;

            string actionUnitType = null;

            try
            {
                // TODO: Уже нет смысла хранить скрипты, как сущность конфигурации

                var scriptMetadata = _scriptMetadataProvider.GetScriptMetadata(actionUnitId);

                if (scriptMetadata == null)
                {
                    throw new ArgumentException(string.Format(Resources.ScriptMetadataIsNotRegistered, actionUnitId));
                }

                actionUnitType = scriptMetadata.Type;

                var actionUnit = _actionUnitFactory.CreateActionUnit(actionUnitType);

                actionUnit(actionUnitContext);

                LogSuccessComplete(actionUnitId, actionUnitType, start);
            }
            catch (Exception e)
            {
                LogErrorComplete(actionUnitId, actionUnitType, start, Resources.ScriptCompletedWithError, e);

                throw;
            }
        }


        private void LogSuccessComplete(string actionUnitId, string actionUnitType, DateTime start)
        {
            _performanceLog.Log(PerformanceLogComponent, actionUnitType ?? actionUnitId ?? PerformanceLogMethod, start, null);
        }

        private void LogErrorComplete(string actionUnitId, string actionUnitType, DateTime start, string message, Exception error)
        {
            var errorContext = new Dictionary<string, object>
                                   {
                                       { "actionUnitId", actionUnitId },
                                       { "actionUnitType", actionUnitType }
                                   };

            _log.Error(message, errorContext, error);

            _performanceLog.Log(PerformanceLogComponent, PerformanceLogMethod, start, message);
        }
    }
}