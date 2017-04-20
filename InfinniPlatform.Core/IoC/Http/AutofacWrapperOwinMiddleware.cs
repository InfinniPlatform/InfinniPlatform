using System.Threading.Tasks;

using Autofac;

using InfinniPlatform.Http.Middlewares;

using Microsoft.AspNetCore.Http;

namespace InfinniPlatform.IoC.Http
{
    /// <summary>
    /// Представляет фиктивный класс-обертку над реальным слоем OWIN для возможности его создания через контейнер зависимостей.
    /// </summary>
    /// <typeparam name="T">Тип реального слоя OWIN.</typeparam>
    /// <seealso cref="AutofacRequestLifetimeScopeOwinMiddleware"/>
    internal sealed class AutofacWrapperOwinMiddleware<T> : OwinMiddleware where T : class//OwinMiddleware
    {
        public AutofacWrapperOwinMiddleware(RequestDelegate next) : base(next)
        {
        }


        public override Task Invoke(HttpContext context)
        {
            // Получение контейнера зависимостей запроса из окружения OWIN
            //TODO Вероятно данный этап уже не нужен. См. http://docs.autofac.org/en/latest/integration/aspnetcore.html?highlight=core#differences-from-asp-net-classic
            //var requestContainer = context.Get<ILifetimeScope>(AutofacHttpConstants.LifetimeScopeKey);
            var requestContainer = context.Items[AutofacHttpConstants.LifetimeScopeKey] as ILifetimeScope;

            // Попытка получения OWIN слоя через контейнер зависимостей запроса

            RequestDelegate realMiddleware = null;

            if (requestContainer != null)
            {
                //realMiddleware = requestContainer.ResolveOptional<T>(TypedParameter.From(Next) as Parameter);
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