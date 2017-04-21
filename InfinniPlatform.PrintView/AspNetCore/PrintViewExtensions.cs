using InfinniPlatform.PrintView;
using InfinniPlatform.PrintView.IoC;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    public static class PrintViewExtensions
    {
        public static IServiceCollection AddPrintView(this IServiceCollection services)
        {
            var options = PrintViewOptions.Default;

            return AddPrintView(services, options);
        }

        public static IServiceCollection AddPrintView(this IServiceCollection services, IConfigurationRoot configuration)
        {
            var options = configuration.GetSection(PrintViewOptions.SectionName).Get<PrintViewOptions>();

            return AddPrintView(services, options);
        }

        public static IServiceCollection AddPrintView(this IServiceCollection services, PrintViewOptions options)
        {
            return services.AddSingleton(provider => new PrintViewContainerModule(options ?? PrintViewOptions.Default));
        }
    }
}