using System;
using System.Collections.Generic;
using System.Reflection;

using InfinniPlatform.Sdk.Dynamic;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace InfinniPlatform.Sdk.Serialization
{
    /// <summary>
    /// Настройки сериализации <see cref="JsonObjectSerializer"/> по умолчанию.
    /// </summary>
    internal class JsonDefaultContractResolver : DefaultContractResolver
    {
        public JsonDefaultContractResolver(IEnumerable<IJsonPropertyInitializer> propertyInitializers)
        {
            _propertyInitializers = propertyInitializers;
        }


        private readonly IEnumerable<IJsonPropertyInitializer> _propertyInitializers;


        /// <summary>
        /// Возвращает список членов типа, которые будут обрабатываться сериализатором.
        /// </summary>
        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            const BindingFlags membersSearchFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            var serializableMembers = base.GetSerializableMembers(objectType);

            var fields = objectType.GetFields(membersSearchFlags);

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

            var properties = objectType.GetProperties(membersSearchFlags);

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
                property.MemberConverter = new ObjectMemberJsonConverter();
            }

            if ((!property.Readable || !property.Writable) && IsSerializerVisible(member))
            {
                property.Readable = true;
                property.Writable = true;
            }

            property.PropertyName = GetSerializerPropertyName(member, property.PropertyName);

            InitializeProperty(property, member, memberSerialization);

            return property;
        }


        /// <summary>
        /// Проверяет, что член типа помечен атрибутом <see cref="SerializerVisibleAttribute"/>.
        /// </summary>
        private static bool IsSerializerVisible(MemberInfo member)
        {
            return member.IsDefined(typeof(SerializerVisibleAttribute), false);
        }

        /// <summary>
        /// Возвращает имя свойства с учетом настроек атрибута <see cref="SerializerPropertyNameAttribute"/>.
        /// </summary>
        private static string GetSerializerPropertyName(MemberInfo member, string name)
        {
            return member.GetAttributeValue<SerializerPropertyNameAttribute, string>(i => i.Name, name);
        }


        /// <summary>
        /// Настраивает <see cref="JsonProperty" /> соответствующее указанному члену типа <see cref="MemberInfo" />.
        /// </summary>
        private void InitializeProperty(JsonProperty property, MemberInfo member, MemberSerialization memberSerialization)
        {
            if (_propertyInitializers != null)
            {
                foreach (var propertyInitializer in _propertyInitializers)
                {
                    propertyInitializer.InitializeProperty(property, member, memberSerialization);
                }
            }
        }
    }
}