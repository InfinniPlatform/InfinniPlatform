using System.Threading.Tasks;

using Autofac;
using Autofac.Core;
using InfinniPlatform.Http.Middlewares;
using Microsoft.AspNetCore.Http;

namespace InfinniPlatform.Core.IoC.Http
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


        public override Task Invoke(HttpContext context)
        {
            // Получение контейнера зависимостей запроса из окружения OWIN
            //TODO Check if this approach is correct.
            //var requestContainer = context.Get<ILifetimeScope>(AutofacHttpConstants.LifetimeScopeKey);
            var requestContainer = context.Items[AutofacHttpConstants.LifetimeScopeKey] as ILifetimeScope;

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