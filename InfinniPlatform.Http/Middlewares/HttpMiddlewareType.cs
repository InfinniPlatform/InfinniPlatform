﻿using System;
using Microsoft.AspNetCore.Builder;

namespace InfinniPlatform.Http.Middlewares
{
    /// <summary>
    /// Тип промежуточного слоя обработки HTTP запросов приложения <see cref="IHttpMiddleware" />.
    /// </summary>
    public enum HttpMiddlewareType
    {
        /// <summary>
        /// Уровень для глобальной обработки запросов.
        /// </summary>
        /// <remarks>
        /// Позволяет осуществлять глобальный контроль обработки запросов. Например, управлять жизненным циклом зависимостей.
        /// </remarks>
        GlobalHandling = 0,

        /// <summary>
        /// Уровень для обработки ошибок выполнения запросов.
        /// </summary>
        /// <remarks>
        /// Позволяет осуществлять обработку ошибок выполнения запросов. Например, вести журнал со статистикой обработки запросов.
        /// </remarks>
        ErrorHandling = 1,

        /// <summary>
        /// Уровень для обработки запросов до аутентификации пользователя.
        /// </summary>
        /// <remarks>
        /// Позволяет осуществлять любую логику до начала аутентификации пользователя. Например, добавлять заголовки CORS.
        /// </remarks>
        BeforeAuthentication = 2,

        /// <summary>
        /// Уровень для барьерной аутентификации пользователя.
        /// </summary>
        /// <remarks>
        /// Позволяет осуществлять логику барьерной аутентификации пользователя. Например, на основе Cookie или токена безопасности.
        /// </remarks>
        AuthenticationBarrier = 4,

        /// <summary>
        /// Уровень для аутентификации пользователя на основе внешнего провайдера.
        /// </summary>
        /// <remarks>
        /// Позволяет осуществлять аутентификацию пользователя на основе внешнего провайдера. Например, Google, Facebook, Twitter и т.п.
        /// </remarks>
        ExternalAuthentication = 8,

        /// <summary>
        /// Уровень для аутентификации пользователя средствами приложения.
        /// </summary>
        /// <remarks>
        /// Позволяет осщуествлять аутентификацию пользователя на основе логики приложения. Например, на основе базы данных пользователей.
        /// </remarks>
        InternalAuthentication = 16,

        /// <summary>
        /// Уровень для обработки запросов после аутентификации пользователя.
        /// </summary>
        /// <remarks>
        /// Позволяет осуществлять любую логику после окончания аутентификации пользователя. Например, обрабатывать запросы к ASP.NET SignalR.
        /// </remarks>
        AfterAuthentication = 32,

        /// <summary>
        /// Уровень для обработки прикладных запросов.
        /// </summary>
        /// <remarks>
        /// Позволяет осуществлять любую логику после прохождения всех остальных уровней обработки запросов.
        /// </remarks>
        Application = 64
    }
}