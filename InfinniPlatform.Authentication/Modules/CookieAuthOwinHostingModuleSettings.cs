﻿namespace InfinniPlatform.Authentication.Modules
{
    /// <summary>
    /// Настройки модуля <see cref="CookieAuthOwinHostingMiddleware"/>.
    /// </summary>
    internal sealed class CookieAuthOwinHostingModuleSettings
    {
        public const string SectionName = "authCookie";


        /// <summary>
        /// Домен для создания Cookie.
        /// </summary>
        public string CookieDomain { get; set; }
    }
}