﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Owin;

using InfinniPlatform.Owin.Formatting;
using InfinniPlatform.Owin.Properties;

namespace InfinniPlatform.Owin.Middleware
{
	/// <summary>
	/// Обработчик HTTP-запросов на базе OWIN.
	/// </summary>
	public class RoutingOwinMiddleware : OwinMiddleware
	{
		public RoutingOwinMiddleware(OwinMiddleware next)
			: base(next)
		{
		}


        private readonly List<HandlerRouting> _handlers = new List<HandlerRouting>(); 


		/// <summary>
		/// Регистрирует обработчик GET-запросов.
		/// </summary>
		public void RegisterGetRequestHandler(PathString path, Func<IOwinContext, IRequestHandlerResult> handler)
		{
			RegisterRequestHandler("GET", (context) => path, handler);
		}

		/// <summary>
		/// Регистрирует обработчик POST-запросов.
		/// </summary>
		public void RegisterPostRequestHandler(PathString path, Func<IOwinContext, IRequestHandlerResult> handler)
		{
			RegisterRequestHandler("POST", context => path, handler);
		}

		/// <summary>
		///   Регистрирует обработчик DELETE-запросов
		/// </summary>
		public void RegisterDeleteRequestHandler(PathString path, Func<IOwinContext, IRequestHandlerResult> handler)
		{
			RegisterRequestHandler("DELETE", context => path, handler);
		}

		/// <summary>
		///   Регистрирует обработчик PUT-запросов
		/// </summary>
		public void RegisterPutRequestHandler(PathString path, Func<IOwinContext, IRequestHandlerResult> handler)
		{
			RegisterRequestHandler("PUT", context => path, handler);
		}

		/// <summary>
		/// Регистрирует обработчик GET-запросов.
		/// </summary>
		public void RegisterGetRequestHandler(Func<IOwinContext, PathString> path, Func<IOwinContext, IRequestHandlerResult> handler)
		{
			RegisterRequestHandler("GET", path, handler);
		}

		/// <summary>
		/// Регистрирует обработчик POST-запросов.
		/// </summary>
		public void RegisterPostRequestHandler(Func<IOwinContext, PathString> path, Func<IOwinContext, IRequestHandlerResult> handler)
		{
			RegisterRequestHandler("POST", path, handler);
		}

		/// <summary>
		///   Регистрирует обработчик DELETE-запросов
		/// </summary>
		public void RegisterDeleteRequestHandler(Func<IOwinContext, PathString> path, Func<IOwinContext, IRequestHandlerResult> handler)
		{
			RegisterRequestHandler("DELETE", path, handler);
		}

		/// <summary>
		///   Регистрирует обработчик PUT-запросов
		/// </summary>
		public void RegisterPutRequestHandler(Func<IOwinContext, PathString> path, Func<IOwinContext, IRequestHandlerResult> handler)
		{
			RegisterRequestHandler("PUT", path, handler);
		}

		/// <summary>
		/// Регистрирует обработчик HTTP-запросов.
		/// </summary>
		public void RegisterRequestHandler(string method, Func<IOwinContext, PathString> path, Func<IOwinContext, IRequestHandlerResult> handler)
		{
            _handlers.Add(new HandlerRouting()
            {
                ContextRouting = path,
                Handler = handler,
                Method = method
            });			
		}


		/// <summary>
		/// Обрабатывает HTTP-запрос.
		/// </summary>
		public override Task Invoke(IOwinContext context)
		{
			Task task;

			var request = context.Request;
			var requestPath = NormalizePath(request.Path);

			HandlerRouting handlerInfo;

			var handlersRegistered = _handlers.Select(h => new KeyValuePair<string, HandlerRouting>(NormalizePath(h.ContextRouting.Invoke(context)),h)).Where(h => h.Key == requestPath).ToList();

			// Если найден обработчик входящего запроса
			if (handlersRegistered.Any())
			{
				IRequestHandlerResult result;

				handlerInfo = handlersRegistered.Where(h => string.Equals(request.Method, h.Value.Method, StringComparison.OrdinalIgnoreCase)).Select(h => h.Value).FirstOrDefault();

			    if (handlersRegistered.Any() && handlerInfo == null)
			    {
                    result = new ErrorRequestHandlerResult(string.Format(Resources.MethodIsNotSupported, request.Method, request.Path, handlerInfo.Method));
			    }

				// Если метод входящего запроса совпадает с ожидаемым
				else 
				{
					try
					{
						result = handlerInfo.Handler(context);
					}
					catch (Exception error)
					{
						result = new ErrorRequestHandlerResult(BuildErrorMessage(error));
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
			return path.HasValue ? path.Value.TrimEnd('/').ToLower() : string.Empty;
		}

		private static object BuildErrorMessage(Exception error)
		{
			var aggregateException = error as AggregateException;

			return (aggregateException != null && aggregateException.InnerExceptions != null && aggregateException.InnerExceptions.Count > 0)
				? string.Join(Environment.NewLine, aggregateException.InnerExceptions.Select(i => i.Message))
				: error.Message;
		}

		protected static object ReadRequestBody(IOwinContext context)
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