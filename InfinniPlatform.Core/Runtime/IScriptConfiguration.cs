using System;

namespace InfinniPlatform.Core.Runtime
{
    /// <summary>
    /// Конфигурация метаданных прикладных скриптов.
    /// </summary>
    public interface IScriptConfiguration
    {
        void RegisterAction(string actionId, string actionType);

        Action<dynamic> GetAction(string actionId);
    }
}