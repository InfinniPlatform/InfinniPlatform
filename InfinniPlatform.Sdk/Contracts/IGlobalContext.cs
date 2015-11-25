using System;

namespace InfinniPlatform.Sdk.Contracts
{
    /// <summary>
    ///     Глобальный контекст выполнения скрипта
    /// </summary>
    [Obsolete("Use IoC")]
    public interface IGlobalContext
    {
        /// <summary>
        ///     Получить компонент, реализующий тип Т из глоабльного контекста
        /// </summary>
        /// <typeparam name="T">Тип ожидаемого контракта</typeparam>
        /// <returns>Экземпляр контракта</returns>
        [Obsolete("Use IoC")]
        T GetComponent<T>() where T : class;
    }
}