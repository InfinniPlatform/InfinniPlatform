﻿namespace InfinniPlatform.Cache
{
    /// <summary>
    /// Распределленый кэш.
    /// </summary>
    /// <remarks>
    /// Хранит данные в постоянной памяти, позволяя разделять данные между несколькими экземплярами приложения.
    /// </remarks>
    public interface ISharedCache
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