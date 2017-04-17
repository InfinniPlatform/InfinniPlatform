using System;

namespace InfinniPlatform.Core.Abstractions.Serialization
{
    /// <summary>
    /// Игнорирует все ошибки сериализации и десериализации.
    /// </summary>
    public class IgnoreSerializerErrorHandler : ISerializerErrorHandler
    {
        public bool Handle(object target, object member, Exception error)
        {
            return true;
        }
    }
}