﻿using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.Environment.Settings;

using Microsoft.Owin.Security.Google;

using Owin;

namespace InfinniPlatform.Authentication.Modules
{
    /// <summary>
    /// Модуль хостинга обработчика запросов к подсистеме внешней аутентификации через Google.
    /// </summary>
    /// <remarks>
    /// Информация о реализации Google OAuth: "https://developers.google.com/+/api/oauth"
    /// Информация о создании проекта Google:"https://developers.google.com/console/help/#creatingdeletingprojects"
    /// После создания проекта нужно не забыть включить "Google+ API".
    /// </remarks>
    internal sealed class ExternalAuthGoogleOwinHostingModule : IOwinHostingModule
    {
        public ExternalAuthGoogleOwinHostingModule(IAppConfiguration appConfiguration)
        {
            _settings = appConfiguration.GetSection<ExternalAuthGoogleOwinHostingModuleSettings>(ExternalAuthGoogleOwinHostingModuleSettings.SectionName);
        }


        private readonly ExternalAuthGoogleOwinHostingModuleSettings _settings;


        public OwinHostingModuleType ModuleType => OwinHostingModuleType.ExternalAuth;

        public void Configure(IAppBuilder builder, IOwinHostingContext context)
        {
            if (_settings.Enable)
            {
                builder.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions
                {
                    ClientId = _settings.ClientId,
                    ClientSecret = _settings.ClientSecret
                });
            }
        }
    }
}