using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace InfinniPlatform.Core.Serialization
{
    /// <summary>
    /// Интерфейс настройки <see cref="JsonProperty" />.
    /// </summary>
    internal interface IJsonPropertyInitializer
    {
        /// <summary>
        /// Настраивает <see cref="JsonProperty" /> для соответствующего <see cref="MemberInfo" />.
        /// </summary>
        void InitializeProperty(JsonProperty property, MemberInfo member, MemberSerialization memberSerialization);
    }
}