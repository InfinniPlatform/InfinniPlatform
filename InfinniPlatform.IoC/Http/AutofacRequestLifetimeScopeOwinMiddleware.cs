﻿using System;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;

using Autofac;
using Autofac.Core.Lifetime;
using Autofac.Util;

using Microsoft.Owin;

namespace InfinniPlatform.IoC.Http
{
    /// <summary>
    /// Слой OWIN для регистрации контейнера зависимостей запроса.
    /// </summary>
    /// <remarks>
    /// Данный слой необходим для работоспособности стратегии InstancePerRequest.
    /// </remarks>
    /// <seealso cref="AutofacWrapperOwinMiddleware{T}" />
    internal sealed class AutofacRequestLifetimeScopeOwinMiddleware : OwinMiddleware
    {
        private const string RequestContainerKey = "RequestContainer";


        public AutofacRequestLifetimeScopeOwinMiddleware(OwinMiddleware next, ILifetimeScope rootContainer) : base(next)
        {
            _rootContainer = rootContainer;
        }


        private readonly ILifetimeScope _rootContainer;


        public override async Task Invoke(IOwinContext context)
        {
            // Создание контейнера зависимостей запроса на время его обработки
            using (var requestContainer = _rootContainer.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag, b => b.RegisterInstance(context).As<IOwinContext>()))
            {
                // Регистрация контейнера зависимостей запроса в окружении OWIN для создания через него последующих OWIN-слоев
                context.Set(AutofacHttpConstants.LifetimeScopeKey, requestContainer);

                // Регистрация контейнера зависимостей запроса в статическом контексте для возможности доступа к нему извне
                SetRequestContainer(requestContainer);

                // Обработка запроса в рамках контейнера
                await Next.Invoke(context);
            }
        }


        /// <summary>
        /// Устанавливает контейнер зависимостей запроса.
        /// </summary>
        /// <remarks>
        /// Во-первых, хранится только слабая ссылка на контейнер зависимостей запроса, поскольку он создается только
        /// на время обработки запроса и уничтожается автоматически (using). В противном случае будет нарушен жизненный
        /// цикл контейнера. Во-вторых, ссылка хранится в CallContext, чтобы она была доступна в любом месте обработки
        /// запроса, включая места распараллеливания через async/await. В-третьих, ссылка устанавливается перед началом
        /// обработки запроса и сбрасывается (null) в конце, так как поток обработки запроса берется из пула и может быть
        /// использован повторно.
        /// </remarks>
        private static void SetRequestContainer(ILifetimeScope requestContainer)
        {
            var requestContainerReference = new WeakReference<ILifetimeScope>(requestContainer);
            var noSerializeAppDomain = new NoSerializeAppDomain { LifetimeScope = requestContainerReference };
            requestContainer.Disposer.AddInstanceForDisposal(noSerializeAppDomain);
            CallContext.LogicalSetData(RequestContainerKey, noSerializeAppDomain);
        }

        /// <summary>
        /// Возвращает контейнер зависимостей запроса, если он доступен.
        /// </summary>
        public static ILifetimeScope TryGetRequestContainer()
        {
            ILifetimeScope requestContainer;
            var noSerializeAppDomain = CallContext.LogicalGetData(RequestContainerKey) as NoSerializeAppDomain;
            var requestContainerReference = noSerializeAppDomain?.LifetimeScope;
            return (requestContainerReference != null && requestContainerReference.TryGetTarget(out requestContainer)) ? requestContainer : null;
        }


        /// <summary>
        /// Обертка, позволяющая передавать <see cref="ILifetimeScope" /> между потоками, но не между <see cref="AppDomain" />.
        /// </summary>
        /// <remarks>
        /// Необходимость в этом классе появилась ввиду следующих причин:
        /// <list type="bullet">
        /// <item>механизм на базе <see cref="CallContext" /> передает данные не только между потоками, но и между <see cref="AppDomain" />;</item>
        /// <item>данные, передаваемые между <see cref="AppDomain" />, должны быть помечены атрибутом <see cref="SerializableAttribute"/>;</item>
        /// <item>экземпляр <see cref="ILifetimeScope" /> не может быть сериализован по целому ряду причин.</item>
        /// </list>
        /// Дополнительные материалы:
        /// <list type="bullet">
        /// <item>http://www.wintellect.com/devcenter/jeffreyr/logical-call-context-flowing-data-across-threads-appdomains-and-processes</item>
        /// <item>https://github.com/autofac/Autofac/issues/456</item>
        /// </list>
        /// </remarks>
        [Serializable]
        private class NoSerializeAppDomain : IDisposable
        {
            [NonSerialized]
            private WeakReference<ILifetimeScope> _lifetimeScope;

            public WeakReference<ILifetimeScope> LifetimeScope
            {
                get { return _lifetimeScope; }
                set { _lifetimeScope = value; }
            }

            public void Dispose()
            {
                _lifetimeScope = null;
            }
        }
    }
}