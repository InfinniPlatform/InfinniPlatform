using System;
using System.Collections.Generic;
using System.Reflection;

using InfinniPlatform.Sdk.Properties;

namespace InfinniPlatform.Sdk.Serialization
{
    /// <summary>
    /// Контейнер известных типов для сериализации.
    /// </summary>
    public sealed class KnownTypesContainer
    {
        private readonly Dictionary<string, Type> _names
            = new Dictionary<string, Type>();

        private readonly Dictionary<Type, string> _types
            = new Dictionary<Type, string>();

        /// <summary>
        /// Содержит указанный тип.
        /// </summary>
        /// <param name="type">Тип.</param>
        public bool HasType(Type type)
        {
            return _types.ContainsKey(type);
        }

        /// <summary>
        /// Содержит тип с указанным именем.
        /// </summary>
        /// <param name="name">Имя типа.</param>
        public bool HasName(string name)
        {
            return _names.ContainsKey(name);
        }

        /// <summary>
        /// Получить тип по имени типа.
        /// </summary>
        /// <param name="name">Имя типа.</param>
        /// <returns>Тип.</returns>
        public Type GetType(string name)
        {
            Type result;

            _names.TryGetValue(name, out result);

            return result;
        }

        /// <summary>
        /// Получить имя типа по типу.
        /// </summary>
        /// <param name="type">Тип.</param>
        /// <returns>Имя типа.</returns>
        public string GetName(Type type)
        {
            string result;

            _types.TryGetValue(type, out result);

            return result;
        }

        /// <summary>
        /// Добавить тип.
        /// </summary>
        /// <param name="type">Тип.</param>
        /// <param name="name">Имя типа.</param>
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

        /// <summary>
        /// Добавить тип.
        /// </summary>
        /// <typeparam name="T">Тип.</typeparam>
        /// <param name="name">Имя типа.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public KnownTypesContainer Add<T>(string name)
        {
            return Add(typeof(T), name);
        }
    }
}