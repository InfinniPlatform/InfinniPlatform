using InfinniPlatform.PrintView;
using InfinniPlatform.PrintView.IoC;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    /// <summary>
    /// Extension methods for print view dependencies registration.
    /// </summary>
    public static class PrintViewExtensions
    {
        /// <summary>
        /// Register print view dependencies.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <returns>Service collection for further services registration.</returns>
        public static IServiceCollection AddPrintView(this IServiceCollection services)
        {
            var options = PrintViewOptions.Default;

            return AddPrintView(services, options);
        }

        /// <summary>
        /// Register print view dependencies.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <param name="configuration">Configuration properties set.</param>
        /// <returns>Service collection for further services registration.</returns>
        public static IServiceCollection AddPrintView(this IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration.GetSection(PrintViewOptions.SectionName).Get<PrintViewOptions>();

            return AddPrintView(services, options);
        }

        /// <summary>
        /// Register print view dependencies.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <param name="options">Print view options.</param>
        /// <returns>Service collection for further services registration.</returns>
        public static IServiceCollection AddPrintView(this IServiceCollection services, PrintViewOptions options)
        {
            return services.AddSingleton(provider => new PrintViewContainerModule(options ?? PrintViewOptions.Default));
        }
    }
}