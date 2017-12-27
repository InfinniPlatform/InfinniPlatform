using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace InfinniPlatform.Serialization
{
    /// <summary>
    /// Interface for <see cref="JsonProperty" /> set up.
    /// </summary>
    public interface IJsonPropertyInitializer
    {
        /// <summary>
        /// Setting up a <see cref="JsonProperty" /> for corresponding <see cref="MemberInfo" />.
        /// </summary>
        void InitializeProperty(JsonProperty property, MemberInfo member, MemberSerialization memberSerialization);
    }
}