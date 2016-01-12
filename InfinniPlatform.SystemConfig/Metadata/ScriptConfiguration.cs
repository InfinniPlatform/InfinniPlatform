using System;
using System.Collections.Generic;

using InfinniPlatform.Core.Runtime;

namespace InfinniPlatform.SystemConfig.Metadata
{
    /// <summary>
    /// Настройки метаданных конфигурации скриптов
    /// </summary>
    internal sealed class ScriptConfiguration : IScriptConfiguration
    {
        public ScriptConfiguration(IScriptMetadataProvider scriptMetadataProvider, IScriptProcessor scriptProcessor)
        {
            _actionUnits = new Dictionary<string, Action<dynamic>>(StringComparer.OrdinalIgnoreCase);
            _scriptMetadataProvider = scriptMetadataProvider;
            _scriptProcessor = scriptProcessor;
        }

        private readonly Dictionary<string, Action<dynamic>> _actionUnits;
        private readonly IScriptMetadataProvider _scriptMetadataProvider;
        private readonly IScriptProcessor _scriptProcessor;

        public void RegisterAction(string actionId, string actionType)
        {
            _scriptMetadataProvider.SetScriptMetadata(new ScriptMetadata { Id = actionId, Type = actionType });

            Action<dynamic> actionUnit = actionContext => _scriptProcessor.InvokeScript(actionId, actionContext);

            _actionUnits[actionId] = actionUnit;
        }

        public Action<dynamic> GetAction(string unitIdentifier)
        {
            Action<dynamic> actionUnit;

            _actionUnits.TryGetValue(unitIdentifier, out actionUnit);

            return actionUnit;
        }
    }
}