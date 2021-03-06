﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace InfinniPlatform.Serialization
{
    /// <summary>
    /// Interface for <see cref="JsonProperty" /> set up.
    /// </summary>
    public class KnownTypesJsonConverterInitializer : IJsonPropertyInitializer
    {
        /// <summary>
        /// Initializes a new instance of <see cref="KnownTypesJsonConverterInitializer" />.
        /// </summary>
        /// <param name="knownTypes"></param>
        public KnownTypesJsonConverterInitializer(KnownTypesContainer knownTypes)
        {
            _knownTypes = knownTypes;
            _knownTypesConverter = new KnownTypesJsonConverter(knownTypes);
        }


        private readonly KnownTypesContainer _knownTypes;
        private readonly KnownTypesJsonConverter _knownTypesConverter;


        /// <inheritdoc />
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
                    var interfaces = new[] { propertyType }.Concat(propertyType.GetTypeInfo().GetInterfaces());

                    foreach (var i in interfaces)
                    {
                        var interfaceType = i.GetTypeInfo();

                        if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                        {
                            // Retrieves an element type of the collection
                            elementType = interfaceType.GetGenericArguments()[0];
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
            return !type.GetTypeInfo().IsValueType
                   && type != typeof(object)
                   && type != typeof(string)
                   && type != typeof(IEnumerable);
        }
    }
}