using System.Collections.Generic;

namespace InfinniPlatform.Sdk.Http.Services
{
    /// <summary>
    /// Источник модулей регистрации обработчиков запросов сервиса.
    /// </summary>
    public interface IHttpServiceSource
    {
        /// <summary>
        /// Возвращает список модулей регистрации обработчиков запросов сервиса.
        /// </summary>
        IEnumerable<IHttpService> GetServices();
    }
}