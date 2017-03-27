using InfinniPlatform.PrintView.IoC;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.Extensions
{
    public static class AspNetExtensions
    {
        public static IServiceCollection AddPrintView(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(provider => new PrintViewContainerModule());
            return serviceCollection;
        }
    }
}