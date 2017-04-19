namespace InfinniPlatform.Cache
{
    /// <summary>
    /// Двухуровневый кэш.
    /// </summary>
    /// <remarks>
    /// Использует два уровня кэширования <see cref="IInMemoryCache"/> и <see cref="ISharedCache"/> для увеличения производительности доступа к данным.
    /// </remarks>
    public interface ITwoLayerCache
    {
        /// <summary>
        /// Проверяет наличие ключа в кэше.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <returns>Возвращает true, если кэш содержит ключ, иначе - false.</returns>
        bool Contains(string key);

        /// <summary>
        /// Возвращает значение ключа из кэша.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <returns>Значение.</returns>
        string Get(string key);

        /// <summary>
        /// Возвращает значение ключа из кэша.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <param name="value">Значение.</param>
        /// <returns>Возвращает true, если кэш содержит ключ, иначе - false.</returns>
        bool TryGet(string key, out string value);

        /// <summary>
        /// Устанавливает значение ключа в кэше.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <param name="value">Значение.</param>
        void Set(string key, string value);

        /// <summary>
        /// Удаляет ключ из кэша.
        /// </summary>
        /// <param name="key">Ключ.</param>
        bool Remove(string key);
    }
}