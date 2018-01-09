using System;

namespace InfinniPlatform.Serialization
{
    /// <summary>
    /// Ignores all serialization errors.
    /// </summary>
    public class IgnoreSerializerErrorHandler : ISerializerErrorHandler
    {
        /// <inheritdoc />
        public bool Handle(object target, object member, Exception error)
        {
            return true;
        }
    }
}