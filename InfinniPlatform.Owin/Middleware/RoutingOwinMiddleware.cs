using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using InfinniPlatform.Owin.Formatting;
using InfinniPlatform.Owin.Properties;
using InfinniPlatform.Sdk.Logging;

using Microsoft.Owin;

namespace InfinniPlatform.Owin.Middleware
{
    /// <summary>
    /// Обработчик HTTP-запросов на базе OWIN.
    /// </summary>
    public class RoutingOwinMiddleware : OwinMiddleware
    {
        public RoutingOwinMiddleware(OwinMiddleware next) : base(next)
        {
        }


        private readonly List<IHandlerRegistration> _handlers = new List<IHandlerRegistration>();


        public void RegisterHandler(IHandlerRegistration handlerRegistration)
        {
            _handlers.Add(handlerRegistration);
        }


        /// <summary>
        /// Обрабатывает HTTP-запрос.
        /// </summary>
        public override Task Invoke(IOwinContext context)
        {
            Task task;

            var request = context.Request;
            var requestPath = NormalizePath(request.Path);

            var handlersRegistered = _handlers.Where(h => h.CanProcessRequest(context, requestPath)).ToList();

            // Если найден обработчик входящего запроса
            if (handlersRegistered.Any())
            {
                IRequestHandlerResult result;

                var handlerInfo = handlersRegistered
                    .OrderByDescending(h => h.Priority)
                    .FirstOrDefault(h => string.Equals(request.Method, h.Method, StringComparison.OrdinalIgnoreCase));

                if (handlerInfo != null)
                {
                    try
                    {
                        result = handlerInfo.Execute(context);
                        result = OnRequestExecuted(result);
                    }
                    catch (TargetInvocationException error)
                    {
                        result = new ErrorRequestHandlerResult(error.InnerException.GetMessage());
                        result = OnRequestExecuted(result);
                    }
                    catch (Exception error)
                    {
                        result = new ErrorRequestHandlerResult(error.GetMessage());
                        result = OnRequestExecuted(result);
                    }
                }
                else
                {
                    result = new ErrorRequestHandlerResult(string.Format(Resources.MethodIsNotSupported, request.Method, request.Path));
                }

                task = result.GetResult(context);
            }
            else
            {
                task = Next.Invoke(context);
            }

            return task;
        }

        protected virtual IRequestHandlerResult OnRequestExecuted(IRequestHandlerResult result)
        {
            return result;
        }

        private static string NormalizePath(PathString path)
        {
            if (path.HasValue)
            {
                return path.Value.Split(new[] { '?' }, StringSplitOptions.RemoveEmptyEntries).First().TrimEnd('/').ToLower();
            }

            return string.Empty;
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
            else if (requestContentType.StartsWith(FormBodyFormatter.Instance.ContentType, StringComparison.OrdinalIgnoreCase))
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