namespace InfinniPlatform.Sdk.Session
{
    /// <summary>
    /// Предоставляет методы для работы с данными сессии текущего пользователя.
    /// </summary>
    public interface ISessionManager
    {
        /// <summary>
        /// Установить данные сессии текущего пользователя.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <param name="value">Значение.</param>
        void SetSessionData(string key, string value);

        /// <summary>
        /// Получить данные сессии текущего пользователя.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <returns>Значение.</returns>
        string GetSessionData(string key);
    }
}