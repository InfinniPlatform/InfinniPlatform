﻿using InfinniPlatform.ContextComponents;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Index.ElasticSearch.IoC
{
    internal sealed class ElasticSearchContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<ElasticFactory>()
                   .As<IIndexFactory>()
                   .SingleInstance();

            builder.RegisterType<IndexComponent>()
                   .As<IIndexComponent>()
                   .SingleInstance();
        }
    }
}