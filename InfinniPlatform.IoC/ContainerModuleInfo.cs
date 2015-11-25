using System;

using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.IoC
{
    /// <summary>
    /// Информация о модуле регистрации зависимостей <see cref="IContainerModule"/>.
    /// </summary>
    internal struct ContainerModuleInfo
    {
        public ContainerModuleInfo(ContainerModuleLocation location, Lazy<Type> type)
        {
            Location = location;
            Type = type;
        }

        /// <summary>
        /// Информация о нахождении модуля регистрации зависимостей.
        /// </summary>
        public readonly ContainerModuleLocation Location;

        /// <summary>
        /// Тип модуля регистрации зависимостей.
        /// </summary>
        public readonly Lazy<Type> Type;
    }
}