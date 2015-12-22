using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using InfinniPlatform.Owin.Formatting;
using InfinniPlatform.Owin.Properties;
using Microsoft.Owin;

namespace InfinniPlatform.Owin.Middleware
{
    /// <summary>
    ///     Обработчик HTTP-запросов на базе OWIN.
    /// </summary>
    public class RoutingOwinMiddleware : OwinMiddleware
    {
        private readonly List<IHandlerRegistration> _handlers = new List<IHandlerRegistration>();

        public RoutingOwinMiddleware(OwinMiddleware next)
            : base(next)
        {
        }

        public void RegisterHandler(IHandlerRegistration handlerRegistration)
        {
            _handlers.Add(handlerRegistration);
        }

        /// <summary>
        ///     Обрабатывает HTTP-запрос.
        /// </summary>
        public override Task Invoke(IOwinContext context)
        {
            Task task;

            var request = context.Request;
            var requestPath = NormalizePath(request.Path);

            IHandlerRegistration handlerInfo;

            //var handlersRegistered = _handlers.Select(h => new KeyValuePair<PathStringProvider, HandlerRouting>(
            //    h.ContextRouting.Invoke(context), h)).Where(h => NormalizePath(h.Key.PathString).ToLowerInvariant() == requestPath.ToLowerInvariant()).ToList();

            var handlersRegistered = _handlers.Where(h => h.CanProcessRequest(context, requestPath)).ToList();


            // Если найден обработчик входящего запроса
            if (handlersRegistered.Any())
            {
                IRequestHandlerResult result;

                handlerInfo =
                    handlersRegistered.OrderByDescending(h => h.Priority)
                        .FirstOrDefault(h => string.Equals(request.Method, h.Method, StringComparison.OrdinalIgnoreCase));

                if (handlersRegistered.Any() && handlerInfo == null)
                {
                    result =
                        new ErrorRequestHandlerResult(string.Format(Resources.MethodIsNotSupported, request.Method,
                            request.Path));
                }

                // Если метод входящего запроса совпадает с ожидаемым
                else
                {
                    try
                    {
                        result = handlerInfo.Execute(context);
                    }
                    catch (Exception error)
                    {
                        if (error is TargetInvocationException)
                        {
                            result = new ErrorRequestHandlerResult(BuildErrorMessage(error.InnerException));
                        }
                        else
                        {
                            result = new ErrorRequestHandlerResult(BuildErrorMessage(error));
                        }
                    }
                }

                task = result.GetResult(context);
            }
            else
            {
                task = Next.Invoke(context);
            }

            return task;
        }

        private static string NormalizePath(PathString path)
        {
            if (path.HasValue)
            {
                return
                    path.Value.Split(new[] {'?'}, StringSplitOptions.RemoveEmptyEntries).First().TrimEnd('/').ToLower();
            }
            return string.Empty;
        }

        private static object BuildErrorMessage(Exception error)
        {
            var aggregateException = error as AggregateException;

            return (aggregateException != null && aggregateException.InnerExceptions != null &&
                    aggregateException.InnerExceptions.Count > 0)
                ? string.Join(Environment.NewLine, aggregateException.InnerExceptions.Select(i => i.Message))
                : error.Message;
        }

        public static object ReadRequestBody(IOwinContext context)
        {
            object result;

            var request = context.Request;
            var requestContentType = request.ContentType ?? JsonBodyFormatter.Instance.ContentType;

            if (requestContentType.StartsWith(JsonBodyFormatter.Instance.ContentType, StringComparison.OrdinalIgnoreCase))
            {
                result = JsonBodyFormatter.Instance.ReadBody(request);
            }
            else if (requestContentType.StartsWith(FormBodyFormatter.Instance.ContentType,
                StringComparison.OrdinalIgnoreCase))
            {
                result = FormBodyFormatter.Instance.ReadBody(request);
            }
            else
            {
                throw new ArgumentException(string.Format(Resources.RequestHasUnsupportedContentType, requestContentType));
            }

            if (result == null)
            {
                throw new ArgumentException(Resources.RequestBodyCannotBeNull);
            }

            return result;
        }
    }
}