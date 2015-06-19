using InfinniPlatform.Api.Context;
using InfinniPlatform.Sdk.Application.Contracts;

namespace InfinniPlatform.Api.Metadata
{
    /// <summary>
    ///     Базовый интерфейс для всех правил проверки конфигураций.
    /// </summary>
    public interface IConfigurationVerification
    {
        /// <summary>
        ///     Текстовое описание правила проверки
        /// </summary>
        string Description { get; }

        /// <summary>
        ///     Идентификатор конфигурации, к которой применима проверка.
        ///     В том случае, если идентификатор не указан (null or empty string),
        ///     проверка применима ко всем конфигурациям
        /// </summary>
        string ConfigurationId { get; }

        /// <summary>
        ///     Версия конфигурации, к которой применимо правило проверки.
        ///     В том случае, если версия не указана (null or empty string),
        ///     правило применимо к любой версии конфигурации
        /// </summary>
        string ConfigVersion { get; }

        /// <summary>
        ///     Выполнить проверку
        /// </summary>
        /// <param name="message">Информативное сообщение с результатом выполнения действия</param>
        /// <returns>Результат выполнения проверки</returns>
        bool Check(out string message);

        /// <summary>
        ///     Устанавливает активную конфигурацию для правила проверки
        /// </summary>
        void AssignActiveConfiguration(string version, string configurationId, IGlobalContext context);
    }
}