using System;
using System.Collections.Generic;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.Runtime;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.SystemConfig.Properties;

namespace InfinniPlatform.SystemConfig.Runtime
{
    [LoggerName("ScriptProcessor")]
    internal sealed class ScriptProcessor : IScriptProcessor
    {
        private const string PerformanceLogMethod = "Invoke";


        public ScriptProcessor(IMetadataApi metadataApi, ActionUnitFactory actionUnitFactory, IPerformanceLog performanceLog, ILog log)
        {
            _metadataApi = metadataApi;
            _actionUnitFactory = actionUnitFactory;
            _performanceLog = performanceLog;
            _log = log;
        }


        private readonly IMetadataApi _metadataApi;
        private readonly ActionUnitFactory _actionUnitFactory;
        private readonly IPerformanceLog _performanceLog;
        private readonly ILog _log;


        public void InvokeScript(string actionUnitId, IActionContext actionUnitContext)
        {
            var start = DateTime.Now;

            string actionUnitType = null;

            try
            {
                // TODO: Уже нет смысла хранить скрипты, как сущность конфигурации

                var scriptMetadata = _metadataApi.GetAction(actionUnitContext.Configuration, actionUnitContext.DocumentType, actionUnitId);

                if (scriptMetadata == null)
                {
                    throw new ArgumentException(string.Format(Resources.ActionUnitMetadataIsNotRegistered, actionUnitId));
                }

                actionUnitType = scriptMetadata.Name;

                var actionUnit = _actionUnitFactory.CreateActionUnit(actionUnitType);

                actionUnit(actionUnitContext);

                LogSuccessComplete(actionUnitId, actionUnitType, start);
            }
            catch (Exception e)
            {
                LogErrorComplete(actionUnitId, actionUnitType, start, Resources.ActionUnitCompletedWithError, e);

                throw;
            }
        }

        public void InvokeScriptByType(string actionUnitType, IActionContext actionUnitContext)
        {
            var start = DateTime.Now;

            try
            {
                var actionUnit = _actionUnitFactory.CreateActionUnit(actionUnitType);

                actionUnit(actionUnitContext);

                LogSuccessComplete(actionUnitType, actionUnitType, start);
            }
            catch (Exception e)
            {
                LogErrorComplete(actionUnitType, actionUnitType, start, Resources.ActionUnitCompletedWithError, e);

                throw;
            }
        }


        private void LogSuccessComplete(string actionUnitId, string actionUnitType, DateTime start)
        {
            _performanceLog.Log(actionUnitType ?? actionUnitId ?? PerformanceLogMethod, start);
        }

        private void LogErrorComplete(string actionUnitId, string actionUnitType, DateTime start, string message, Exception error)
        {
            var errorContext = new Dictionary<string, object>
                                   {
                                       { "actionUnitId", actionUnitId },
                                       { "actionUnitType", actionUnitType }
                                   };

            _log.Error(message, errorContext, error);

            _performanceLog.Log(actionUnitType ?? actionUnitId ?? PerformanceLogMethod, start, message);
        }
    }
}