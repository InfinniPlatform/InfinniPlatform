﻿using InfinniPlatform.MessageQueue.IoC;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.Extensions
{
    public static class AspNetExtensions
    {
        public static IServiceCollection AddMessageQueue(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(provider => new MessageQueueContainerModule());
            return serviceCollection;
        }
    }
}