namespace InfinniPlatform.Auth.Vk.Middlewares
{
    /// <summary>
    /// Настройки для <see cref="AuthVkHttpMiddleware"/>.
    /// </summary>
    public class AuthVkHttpMiddlewareSettings
    {
        public const string SectionName = "authVk";


        /// <summary>
        /// Разрешает аутентификацию через ВКонтакте.
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// ClientId приложения в ВКонтакте.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// ClientSecret приложения в ВКонтакте.
        /// </summary>
        public string ClientSecret { get; set; }
    }
}