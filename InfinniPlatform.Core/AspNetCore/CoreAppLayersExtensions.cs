using System.Linq;

using InfinniPlatform.Hosting;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.IoC;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace InfinniPlatform.AspNetCore
{
    public static class CoreAppLayersExtensions
    {
        /// <summary>
        /// Register application layers defined by user in IoC.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="resolver">IoC container resolver.</param>
        public static void UseRegisteredAppLayers(this IApplicationBuilder app, IContainerResolver resolver)
        {
            app.RegisterAppLayers<IGlobalHandlingAppLayer>(resolver);
            app.RegisterAppLayers<IErrorHandlingAppLayer>(resolver);
            app.RegisterAppLayers<IBeforeAuthenticationAppLayer>(resolver);
            app.RegisterAppLayers<IAuthenticationBarrierAppLayer>(resolver);
            app.RegisterAppLayers<IExternalAuthenticationAppLayer>(resolver);
            app.RegisterAppLayers<IInternalAuthenticationAppLayer>(resolver);
            app.RegisterAppLayers<IAfterAuthenticationAppLayer>(resolver);
            app.RegisterAppLayers<IBusinessAppLayer>(resolver);
        }

        /// <summary>
        /// Register default application layers.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="resolver">IoC container resolver.</param>
        public static void UseDefaultAppLayers(this IApplicationBuilder app, IContainerResolver resolver)
        {
            var layers = resolver.Resolve<IDefaultAppLayer[]>();

            foreach (var layer in layers.OfType<IGlobalHandlingAppLayer>())
            {
                layer.Configure(app);
            }

            foreach (var layer in layers.OfType<IErrorHandlingAppLayer>())
            {
                layer.Configure(app);
            }

            foreach (var layer in layers.OfType<IBeforeAuthenticationAppLayer>())
            {
                layer.Configure(app);
            }

            foreach (var layer in layers.OfType<IAuthenticationBarrierAppLayer>())
            {
                layer.Configure(app);
            }

            foreach (var layer in layers.OfType<IExternalAuthenticationAppLayer>())
            {
                layer.Configure(app);
            }

            foreach (var layer in layers.OfType<IInternalAuthenticationAppLayer>())
            {
                layer.Configure(app);
            }

            foreach (var layer in layers.OfType<IAfterAuthenticationAppLayer>())
            {
                layer.Configure(app);
            }

            foreach (var layer in layers.OfType<IBusinessAppLayer>())
            {
                layer.Configure(app);
            }
        }

        /// <summary>
        /// Register application lifetime handlers.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="resolver">IoC container resolver.</param>
        public static void RegisterAppLifetimeHandlers(this IApplicationBuilder app, IContainerResolver resolver)
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

        /// <summary>
        /// Register application layers using <see cref="AppLayersMap"/>.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="appLayersMap">Application layers map.</param>
        public static void UseAppLayersMap(this IApplicationBuilder app, AppLayersMap appLayersMap)
        {
            var dictionary = appLayersMap.GetMap();
            foreach (var map in dictionary)
            {
                foreach (var appLayer in map.Value)
                {
                    appLayer.Configure(app);
                }
            }
        }

        private static void RegisterAppLayers<T>(this IApplicationBuilder app, IContainerResolver resolver) where T : IAppLayer
        {
            var appLayers = resolver.Resolve<T[]>();

            foreach (var layer in appLayers)
            {
                layer.Configure(app);
            }
        }
    }
}