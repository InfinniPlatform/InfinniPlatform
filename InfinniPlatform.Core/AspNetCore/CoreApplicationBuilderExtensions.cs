using InfinniPlatform.Hosting;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

// ReSharper disable once CheckNamespace
namespace InfinniPlatform.AspNetCore
{
    /// <summary>
    /// Extension methods for <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class CoreApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds MVC with internal controllers to the request execution pipeline.
        /// </summary>
        /// <param name="app">Application builder.</param>
        public static IApplicationBuilder UseMvcWithInternalServices(this IApplicationBuilder app)
        {
            return app.UseMvc();
        }

        /// <summary>
        /// Adds error handling middleware to the request execution pipeline.
        /// </summary>
        /// <param name="app">Application builder instance.</param>
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ErrorHandlingMiddleware>();
        }

        /// <summary>
        /// Add error handling middleware to the request execution pipeline.
        /// </summary>
        /// <param name="app">Application builder.</param>
        public static IApplicationBuilder UsePerformanceLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<PerformanceLoggingMiddleware>();
        }

        /// <summary>
        /// Registers <see cref="IAppStartedHandler" /> and <see cref="IAppStoppedHandler" /> implementations.
        /// </summary>
        /// <param name="resolver">Application container resolver.</param>
        public static void RegisterAppLifetimeHandlers(IContainerResolver resolver)
        {
            var appStartedHandlers = resolver.Resolve<IAppStartedHandler[]>();
            var appStoppedHandlers = resolver.Resolve<IAppStoppedHandler[]>();
            var lifetime = resolver.Resolve<IApplicationLifetime>();

            foreach (var handler in appStartedHandlers)
            {
                lifetime.ApplicationStarted.Register(handler.Handle);
            }

            foreach (var handler in appStoppedHandlers)
            {
                lifetime.ApplicationStopped.Register(handler.Handle);
            }
        }
    }
}