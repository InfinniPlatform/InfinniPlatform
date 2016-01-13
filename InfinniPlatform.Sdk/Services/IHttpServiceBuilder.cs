using System;

namespace InfinniPlatform.Sdk.Services
{
    /// <summary>
    /// Регистратор обработчиков запросов сервиса.
    /// </summary>
    public interface IHttpServiceBuilder
    {
        /// <summary>
        /// Правила обработки GET-запросов.
        /// </summary>
        IHttpRouteBuilder Get { get; }

        /// <summary>
        /// Правила обработки POST-запросов.
        /// </summary>
        IHttpRouteBuilder Post { get; }

        /// <summary>
        /// Правила обработки PUT-запросов.
        /// </summary>
        IHttpRouteBuilder Put { get; }

        /// <summary>
        /// Правила обработки PATCH-запросов.
        /// </summary>
        IHttpRouteBuilder Patch { get; }

        /// <summary>
        /// Правила обработки DELETE-запросов.
        /// </summary>
        IHttpRouteBuilder Delete { get; }

        /// <summary>
        /// Предобработчик запросов.
        /// </summary>
        Action<IHttpRequest> OnBefore { get; set; }

        /// <summary>
        /// Постобработчик запросов.
        /// </summary>
        Action<IHttpRequest, IHttpResponse> OnAfter { get; set; }
    }
}