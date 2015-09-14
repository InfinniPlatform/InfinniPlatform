﻿using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Metadata;

namespace InfinniPlatform.Modules
{
    /// <summary>
    ///     Модуль установки сервисов конфигурации
    /// </summary>
    public interface IModuleInstaller
    {
        /// <summary>
        ///     Получить наименование модуля приложения
        /// </summary>
        string ModuleName { get; }

        /// <summary>
        ///     Является ли модуль системным (стартует перед остальными, не может ссылаться на другие системные модули)
        /// </summary>
        bool IsSystem { get; }

        /// <summary>
        ///     Установить модуль приложения
        /// </summary>
        IModule InstallModule();
    }
}