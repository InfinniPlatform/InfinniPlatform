using System;

using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Core.IoC
{
    /// <summary>
    /// Информация о нахождении модуля регистрации зависимостей <see cref="IContainerModule"/>.
    /// </summary>
    [Serializable]
    internal struct ContainerModuleLocation
    {
        public ContainerModuleLocation(string assemblyPath, string typeFullName)
        {
            AssemblyPath = assemblyPath;
            TypeFullName = typeFullName;
        }

        /// <summary>
        /// Путь к файлу сборки.
        /// </summary>
        public readonly string AssemblyPath;

        /// <summary>
        /// Полное имя типа.
        /// </summary>
        public readonly string TypeFullName;
    }
}