﻿namespace InfinniPlatform.Sdk.Http.Services
{
    /// <summary>
    /// Модуль регистрации обработчиков запросов сервиса.
    /// </summary>
    public interface IHttpService
    {
        /// <summary>
        /// Загружает модуль.
        /// </summary>
        /// <param name="builder">Регистратор обработчиков запросов.</param>
        void Load(IHttpServiceBuilder builder);
    }
}