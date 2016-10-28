using System.Threading.Tasks;

using Autofac;
using Autofac.Core;

using Microsoft.Owin;

namespace InfinniPlatform.IoC.Http
{
    /// <summary>
    /// Представляет фиктивный класс-обертку над реальным слоем OWIN для возможности его создания через контейнер зависимостей.
    /// </summary>
    /// <typeparam name="T">Тип реального слоя OWIN.</typeparam>
    /// <seealso cref="AutofacRequestLifetimeScopeOwinMiddleware"/>
    internal sealed class AutofacWrapperOwinMiddleware<T> : OwinMiddleware where T : OwinMiddleware
    {
        public AutofacWrapperOwinMiddleware(OwinMiddleware next) : base(next)
        {
        }


        public override Task Invoke(IOwinContext context)
        {
            // Получение контейнера зависимостей запроса из окружения OWIN
            var requestContainer = context.Get<ILifetimeScope>(AutofacHttpConstants.LifetimeScopeKey);

            // Попытка получения OWIN слоя через контейнер зависимостей запроса

            OwinMiddleware realMiddleware = null;

            if (requestContainer != null)
            {
                realMiddleware = requestContainer.ResolveOptional<T>(TypedParameter.From(Next) as Parameter);
            }

            if (realMiddleware == null)
            {
                realMiddleware = Next;
            }

            // Направление запроса реальному слою OWIN
            return realMiddleware.Invoke(context);
        }
    }
}