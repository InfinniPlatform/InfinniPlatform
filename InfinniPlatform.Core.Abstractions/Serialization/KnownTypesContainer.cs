using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using InfinniPlatform.Properties;

namespace InfinniPlatform.Serialization
{
    /// <summary>
    /// Container of known types for serialization.
    /// </summary>
    public class KnownTypesContainer : IEnumerable<KeyValuePair<Type, string>>
    {
        private readonly Dictionary<string, Type> _names
            = new Dictionary<string, Type>();

        private readonly Dictionary<Type, string> _types
            = new Dictionary<Type, string>();


        /// <summary>
        /// Flag indicating if type is known.
        /// </summary>
        /// <param name="type">Type.</param>
        public bool IsKnownType(Type type)
        {
            return _types.Keys.Any(type.GetTypeInfo().IsAssignableFrom);
        }


        /// <summary>
        /// Flag indicating if container has specified type.
        /// </summary>
        /// <param name="type">Type.</param>
        public bool HasType(Type type)
        {
            return _types.ContainsKey(type);
        }

        /// <summary>
        /// Returns type by name.
        /// </summary>
        /// <param name="name">Type name.</param>
        /// <returns>Type.</returns>
        public Type GetType(string name)
        {
            _names.TryGetValue(name, out var result);

            return result;
        }


        /// <summary>
        /// Flag indicating if container has type with specified name.
        /// </summary>
        /// <param name="name">Type name.</param>
        public bool HasName(string name)
        {
            return _names.ContainsKey(name);
        }

        /// <summary>
        /// Returns type name by type.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <returns>Type name.</returns>
        public string GetName(Type type)
        {
            _types.TryGetValue(type, out var result);

            return result;
        }


        /// <summary>
        /// Adds type to container.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="name">Type name.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public KnownTypesContainer Add<T>(string name)
        {
            return Add(typeof(T), name);
        }

        /// <summary>
        /// Adds type to container.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="name">Type name.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public KnownTypesContainer Add(Type type, string name)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (_types.ContainsKey(type))
            {
                throw new ArgumentException(string.Format(Resources.TypeIsAlreadyAdded, type), nameof(type));
            }

            if (_names.ContainsKey(name))
            {
                throw new ArgumentException(string.Format(Resources.NameIsAlreadyAdded, name), nameof(name));
            }

            if (type.GetTypeInfo().IsInterface || type.GetTypeInfo().IsAbstract)
            {
                throw new ArgumentException(Resources.TypeShouldNotBeAbstract, nameof(type));
            }

            if (type.GetTypeInfo().GetConstructor(Type.EmptyTypes) == null)
            {
                throw new ArgumentException(Resources.TypeShouldHaveDefaultConstructor, nameof(type));
            }

            _types.Add(type, name);
            _names.Add(name, type);

            return this;
        }


        /// <inheritdoc />
        public IEnumerator<KeyValuePair<Type, string>> GetEnumerator()
        {
            return _types.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}