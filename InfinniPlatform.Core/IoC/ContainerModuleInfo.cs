using System;
using System.Diagnostics;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Core.IoC
{
    /// <summary>
    /// Информация о модуле регистрации зависимостей <see cref="IContainerModule"/>.
    /// </summary>
    [DebuggerDisplay("{Location.TypeFullName}")]
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