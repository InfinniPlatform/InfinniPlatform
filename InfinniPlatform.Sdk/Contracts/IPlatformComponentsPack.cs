using System;

namespace InfinniPlatform.Sdk.Contracts
{
    /// <summary>
    /// Контракт для разрешния зависимости пакета стандартных компонентов платформы
    /// </summary>
    [Obsolete("Use IoC")]
    public interface IPlatformComponentsPack
    {
        /// <summary>
        /// Получить компонент платформы, реализующий указанный контракт
        /// </summary>
        /// <typeparam name="T">Тип контракта</typeparam>
        /// <returns>Экземпляр компонента</returns>
        [Obsolete("Use IoC")]
        T GetComponent<T>() where T : class;
    }
}