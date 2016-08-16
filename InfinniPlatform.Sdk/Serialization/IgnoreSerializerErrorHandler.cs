using System;

namespace InfinniPlatform.Sdk.Serialization
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