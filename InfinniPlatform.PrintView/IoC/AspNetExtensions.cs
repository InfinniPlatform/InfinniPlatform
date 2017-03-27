using InfinniPlatform.PrintView.IoC;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.Extensions
{
    public static class AspNetExtensions
    {
        /// <summary>
        /// Регистрирует сервисы печатных представлений.
        /// </summary>
        /// <param name="serviceCollection">Коллекция зарегистрированных сервисов.</param>
        public static IServiceCollection AddPrintView(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(provider => new PrintViewContainerModule());
            return serviceCollection;
        }
    }
}