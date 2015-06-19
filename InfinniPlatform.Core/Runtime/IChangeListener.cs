using System;

namespace InfinniPlatform.Runtime
{
    /// <summary>
    ///     Слушатель изменений загруженных модулей
    /// </summary>
    public interface IChangeListener
    {
        /// <summary>
        ///     Зарегистрировать обработчик события изменения конфигурации
        /// </summary>
        /// <param name="registrator">Зарегистрированный слушатель изменений</param>
        /// <param name="action">Действие</param>
        void RegisterOnChange(string registrator, Action<string, string> action);

        /// <summary>
        ///     Выполнить список действий, зарегистрированных для события изменения модуля
        /// </summary>
        /// <param name="version">Версия модуля</param>
        /// <param name="changedModule">Изменившийся модуль</param>
        void Invoke(string version, string changedModule);
    }
}