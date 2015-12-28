using System;
using System.Threading.Tasks;

using Autofac;
using Autofac.Core.Lifetime;

using Microsoft.Owin;

namespace InfinniPlatform.IoC.Owin.Middleware
{
    /// <summary>
    /// Слой OWIN для регистрации контейнера зависимостей запроса.
    /// </summary>
    /// <remarks>
    /// Данный слой необходим для работоспособности стратегии InstancePerRequest.
    /// </remarks>
    /// <seealso cref="AutofacWrapperOwinMiddleware{T}"/>
    internal sealed class AutofacRequestLifetimeScopeOwinMiddleware : OwinMiddleware
    {
        public AutofacRequestLifetimeScopeOwinMiddleware(OwinMiddleware next, ILifetimeScope rootContainer) : base(next)
        {
            _rootContainer = rootContainer;
        }


        private readonly ILifetimeScope _rootContainer;


        public override Task Invoke(IOwinContext context)
        {
            // Создание контейнера зависимостей запроса на время его обработки
            using (var requestContainer = _rootContainer.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag, b => b.RegisterInstance(context).As<IOwinContext>()))
            {
                // Регистрация контейнера зависимостей запроса в окружении OWIN для создания через него последующих OWIN-слоев
                context.Set(AutofacOwinConstants.LifetimeScopeKey, requestContainer);

                // Регистрация контейнера зависимостей запроса в статическом контексте для возможности доступа к нему извне
                SetRequestContainer(requestContainer);

                try
                {
                    // Обработка запроса в рамках контейнера
                    return Next.Invoke(context);
                }
                finally
                {
                    SetRequestContainer(null);
                }
            }
        }


        [ThreadStatic]
        private static WeakReference<ILifetimeScope> _requestContainerReference;

        /// <summary>
        /// Возвращает слабую ссылку на контейнер зависимостей запроса.
        /// </summary>
        /// <remarks>
        /// Во-первых, хранится только слабая ссылка на контейнер зависимостей запроса, поскольку он создается только
        /// на время обработки запроса и уничтожается автоматически (using). В противном случае будет нарушен жизненный
        /// цикл контейнеров. Во-вторых, ссылка хранится, как ThreadStatic, так как запросы обрабатываются параллельно,
        /// но при этом есть необходимость в доступе к контейнеру запроса, например, чтобы была возможность сделать
        /// абстракцию единого контейнера так и не задумываться о ILifetimeScope в прикладном коде. В-третьих, ссылка
        /// устанавливается перед началом обработки запроса и сбрасывается (null) в конце, так как поток обработки
        /// запроса берется из пула и может быть использован повторно.
        /// </remarks>
        private static WeakReference<ILifetimeScope> RequestContainerReference
        {
            get
            {
                if (_requestContainerReference == null)
                {
                    _requestContainerReference = new WeakReference<ILifetimeScope>(null);
                }

                return _requestContainerReference;
            }
        }

        /// <summary>
        /// Устанавливает контейнер зависимостей запроса.
        /// </summary>
        private static void SetRequestContainer(ILifetimeScope requestContainer)
        {
            RequestContainerReference.SetTarget(requestContainer);
        }

        /// <summary>
        /// Возвращает контейнер зависимостей запроса, если он доступен.
        /// </summary>
        public static ILifetimeScope TryGetRequestContainer()
        {
            ILifetimeScope requestContainer;

            return RequestContainerReference.TryGetTarget(out requestContainer) ? requestContainer : null;
        }
    }
}