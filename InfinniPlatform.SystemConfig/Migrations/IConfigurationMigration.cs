using System.Collections.Generic;

namespace InfinniPlatform.SystemConfig.Migrations
{
    /// <summary>
    /// Базовый интерфейс для всех миграций.
    /// </summary>
    public interface IConfigurationMigration
    {
        /// <summary>
        /// Текстовое описание миграции
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Идентификатор конфигурации, к которой применима миграция.
        /// В том случае, если идентификатор не указан (null or empty string),
        /// миграция применима ко всем конфигурациям
        /// </summary>
        string ConfigurationId { get; }

        /// <summary>
        /// Версия конфигурации, к которой применима миграция.
        /// В том случае, если версия не указана (null or empty string),
        /// миграция применима к любой версии конфигурации
        /// </summary>
        string ConfigVersion { get; }

        /// <summary>
        /// Признак того, что миграцию можно откатить
        /// </summary>
        bool IsUndoable { get; }

        /// <summary>
        /// Возвращает параметры миграции
        /// </summary>
        IEnumerable<MigrationParameter> Parameters { get; }

        /// <summary>
        /// Выполнить миграцию
        /// </summary>
        /// <param name="message">Информативное сообщение с результатом выполнения действия</param>
        /// <param name="parameters">Параметры миграции</param>
        void Up(out string message, object[] parameters);

        /// <summary>
        /// Отменить миграцию
        /// </summary>
        /// <param name="message">Информативное сообщение с результатом выполнения действия</param>
        /// <param name="parameters">Параметры миграции</param>
        void Down(out string message, object[] parameters);

        /// <summary>
        /// Устанавливает активную конфигурацию для миграции
        /// </summary>
        void AssignActiveConfiguration(string configurationId);
    }
}