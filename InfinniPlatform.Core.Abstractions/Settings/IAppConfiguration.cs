using System;

namespace InfinniPlatform.Core.Settings
{
    /// <summary>
    /// Конфигурация приложения.
    /// </summary>
    [Obsolete("Use Microsoft.Extensions.Configuration.IConfigurationRoot")]
    public interface IAppConfiguration
    {
        /// <summary>
        /// Возвращает динамический объект с описанием секции конфигурации.
        /// </summary>
        /// <param name="sectionName">Имя секции конфигурации.</param>
        [Obsolete("Use Microsoft.Extensions.Configuration.IConfigurationRoot")]
        dynamic GetSection(string sectionName);

        /// <summary>
        /// Возвращает типизированный объект с описанием секции конфигурации.
        /// </summary>
        /// <typeparam name="TSection">Тип секции конфигурации.</typeparam>
        /// <param name="sectionName">Имя секции конфигурации.</param>
        [Obsolete("Use Microsoft.Extensions.Configuration.IConfigurationRoot")]
        TSection GetSection<TSection>(string sectionName) where TSection : new();
    }
}