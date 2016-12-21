using System;
using System.Collections.Generic;
using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace InfinniPlatform.Sdk.Serialization
{
    /// <summary>
    /// Настройки сериализации <see cref="JsonObjectSerializer"/> по умолчанию.
    /// </summary>
    internal class JsonDefaultContractResolver : DefaultContractResolver
    {
        /// <summary>
        /// Возвращает список членов типа, которые будут обрабатываться сериализатором.
        /// </summary>
        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            const BindingFlags membersSearchFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            var serializableMembers = base.GetSerializableMembers(objectType);

            var fields = objectType.GetTypeInfo().GetFields(membersSearchFlags);

            if (fields.Length > 0)
            {
                foreach (var field in fields)
                {
                    if (!field.IsInitOnly
                        && IsSerializerVisible(field)
                        && !serializableMembers.Contains(field))
                    {
                        serializableMembers.Add(field);
                    }
                }
            }

            var properties = objectType.GetTypeInfo().GetProperties(membersSearchFlags);

            if (properties.Length > 0)
            {
                foreach (var property in properties)
                {
                    if (property.CanRead
                        && property.CanWrite
                        && IsSerializerVisible(property)
                        && !serializableMembers.Contains(property))
                    {
                        serializableMembers.Add(property);
                    }
                }
            }

            return serializableMembers;
        }

        /// <summary>
        /// Создает <see cref="JsonProperty"/> соответствующее указанному члену типа <see cref="MemberInfo"/>.
        /// </summary>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            // Если тип свойства не определен
            if (property.PropertyType == typeof(object))
            {
                // Используется дополнительная логика при десериализации значения свойства
                property.MemberConverter = new JsonObjectMemberConverter();
            }

            if ((!property.Readable || !property.Writable) && IsSerializerVisible(member))
            {
                property.Readable = true;
                property.Writable = true;
            }

            return property;
        }

        /// <summary>
        /// Проверяет, что член типа помечен атрибутом <see cref="SerializerVisibleAttribute"/>.
        /// </summary>
        private static bool IsSerializerVisible(MemberInfo member)
        {
            return member.IsDefined(typeof(SerializerVisibleAttribute), false);
        }
    }
}