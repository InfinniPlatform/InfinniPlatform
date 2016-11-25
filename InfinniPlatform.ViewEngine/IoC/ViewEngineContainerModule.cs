﻿using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Settings;
using InfinniPlatform.Sdk.ViewEngine;
using InfinniPlatform.ViewEngine.Nancy;
using InfinniPlatform.ViewEngine.Settings;

namespace InfinniPlatform.ViewEngine.IoC
{
    /// <summary>
    /// Регистрация компонентов в IoC-контейнере.
    /// </summary>
    public sealed class WatcherContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            //Hosting 

            builder.RegisterFactory(r => r.Resolve<IAppConfiguration>()
                                          .GetSection<ViewEngineSettings>(ViewEngineSettings.SectionName))
                   .As<ViewEngineSettings>()
                   .SingleInstance();

            builder.RegisterType<ViewEngineBootstrapperExtension>()
                   .As<IViewEngineBootstrapperExtension>()
                   .SingleInstance();

            builder.RegisterHttpServices(typeof(WatcherContainerModule).Assembly);
        }
    }
}