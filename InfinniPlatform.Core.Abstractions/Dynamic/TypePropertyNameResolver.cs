using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

using InfinniPlatform.Serialization;

namespace InfinniPlatform.Dynamic
{
    internal class TypePropertyNameResolver
    {
        private readonly ConcurrentDictionary<Type, TypePropertyMap> _typeProperties
            = new ConcurrentDictionary<Type, TypePropertyMap>();


        public string TryGetPropertyName(Type type, string propertyAlias)
        {
            if (!_typeProperties.TryGetValue(type, out TypePropertyMap typePropertyMap))
            {
                typePropertyMap = _typeProperties.GetOrAdd(type, new TypePropertyMap(type));
            }

            return typePropertyMap.TryGetPropertyName(propertyAlias);
        }


        private class TypePropertyMap
        {
            public TypePropertyMap(Type type)
            {
                Dictionary<string, string> GetPropertyAliases()
                {
                    var result = new Dictionary<string, string>();

                    var properties = type.GetTypeInfo().GetProperties();

                    foreach (var property in properties)
                    {
                        var propertyNameAttribute = property.GetCustomAttribute<SerializerPropertyNameAttribute>(false);

                        if (!string.IsNullOrEmpty(propertyNameAttribute?.Name))
                        {
                            result[propertyNameAttribute.Name] = property.Name;
                        }
                    }

                    return result;
                }

                _propertyAliases = new Lazy<Dictionary<string, string>>(GetPropertyAliases);
            }


            private readonly Lazy<Dictionary<string, string>> _propertyAliases;


            public string TryGetPropertyName(string propertyAlias)
            {
                return _propertyAliases.Value.TryGetValue(propertyAlias, out string propertyName) ? propertyName : null;
            }
        }
    }
}