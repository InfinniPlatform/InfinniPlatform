using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace InfinniPlatform.Sdk.Serialization
{
    internal class KnownTypesJsonConverterInitializer : IJsonPropertyInitializer
    {
        public KnownTypesJsonConverterInitializer(KnownTypesContainer knownTypes)
        {
            _knownTypes = knownTypes;
            _knownTypesConverter = new KnownTypesJsonConverter(knownTypes);
        }


        private readonly KnownTypesContainer _knownTypes;
        private readonly KnownTypesJsonConverter _knownTypesConverter;


        public void InitializeProperty(JsonProperty property, MemberInfo member, MemberSerialization memberSerialization)
        {
            var propertyType = property.PropertyType;

            // For properties with an uncommon reference type
            if (IsUncommonReferenceType(propertyType))
            {
                Type elementType = null;

                if (propertyType.IsArray)
                {
                    // Retrieves an element type of the array
                    elementType = propertyType.GetElementType();
                }
                else
                {
                    var interfaces = new[] { propertyType }.Concat(propertyType.GetInterfaces());

                    foreach (var i in interfaces)
                    {
                        if (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                        {
                            // Retrieves an element type of the collection
                            elementType = i.GetGenericArguments()[0];
                            break;
                        }
                    }
                }

                // If the property is a collection
                if (elementType != null)
                {
                    // If the element type is known
                    if (IsUncommonReferenceType(elementType) && _knownTypes.IsKnownType(elementType))
                    {
                        property.ItemConverter = _knownTypesConverter;

                        return;
                    }
                }

                // If the property type is known
                if (_knownTypes.IsKnownType(propertyType))
                {
                    property.Converter = _knownTypesConverter;
                    property.MemberConverter = _knownTypesConverter;
                }
            }
        }


        private static bool IsUncommonReferenceType(Type type)
        {
            return !type.IsValueType
                   && type != typeof(object)
                   && type != typeof(string)
                   && type != typeof(IEnumerable);
        }
    }
}