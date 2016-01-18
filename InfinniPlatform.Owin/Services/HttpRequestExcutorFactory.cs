﻿using System;

using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Owin.Services
{
    /// <summary>
    /// Фабрика создания функции обработки запросов.
    /// </summary>
    internal sealed class HttpRequestExcutorFactory
    {
        /// <summary>
        /// Создает функцию обработки запросов.
        /// </summary>
        /// <param name="onBefore">Предобработчик запросов.</param>
        /// <param name="onHandle">Основной обработчик запросов.</param>
        /// <param name="onAfter">Постобработчик запросов.</param>
        /// <param name="onError">Обработчик исключений.</param>
        /// <param name="resultConverter">Конвертер результата.</param>
        /// <returns>Функция обработки запросов.</returns>
        /// <remarks>
        /// Функция обработки запросов выполнена по принципу Chain of Responsibility.
        /// На каждом шаге обработки запроса принимается решение, какой шаг будет вызван далее.
        /// </remarks>
        public Func<IHttpRequest, object> CreateExecutor(Func<IHttpRequest, object> onBefore,
                                                         Func<IHttpRequest, object> onHandle,
                                                         Func<IHttpRequest, object, object> onAfter,
                                                         Func<IHttpRequest, Exception, object> onError,
                                                         Func<object, IHttpResponse> resultConverter)
        {
            // Создание цепочки обработки запроса

            var beforeExcutor = new BeforeHttpRequestExcutor(onBefore);
            var handlerExcutor = new HandlerHttpRequestExcutor(onHandle);
            var afterExcutor = new AfterHttpRequestExcutor(onAfter);
            var errorExcutor = new ErrorHttpRequestExcutor(onError);

            beforeExcutor.After = beforeExcutor;
            beforeExcutor.Handler = handlerExcutor;
            beforeExcutor.Error = errorExcutor;

            handlerExcutor.After = afterExcutor;
            handlerExcutor.Error = errorExcutor;

            afterExcutor.Error = errorExcutor;

            // Оптимизация длины цепочки обработки запроса

            IHttpRequestExcutor firstExcutor = null;

            if (onBefore != null)
            {
                firstExcutor = beforeExcutor;
            }
            else if (onHandle != null)
            {
                firstExcutor = handlerExcutor;
            }
            else if (onAfter != null)
            {
                firstExcutor = afterExcutor;
            }

            // Формирование итогового обработчика запроса

            if (firstExcutor != null)
            {
                if (resultConverter != null)
                {
                    return request =>
                           {
                               var context = new HttpRequestExcutorContext(request);
                               firstExcutor.Execute(context);
                               return resultConverter(context.Result);
                           };
                }

                return request =>
                       {
                           var context = new HttpRequestExcutorContext(request);
                           firstExcutor.Execute(context);
                           return context.Result;
                       };
            }

            return null;
        }


        private interface IHttpRequestExcutor
        {
            void Execute(HttpRequestExcutorContext context);
        }


        private class HttpRequestExcutorContext
        {
            public HttpRequestExcutorContext(IHttpRequest request)
            {
                Request = request;
            }

            public readonly IHttpRequest Request;
            public Exception Error;
            public object Result;
        }


        private sealed class BeforeHttpRequestExcutor : IHttpRequestExcutor
        {
            public BeforeHttpRequestExcutor(Func<IHttpRequest, object> onBefore)
            {
                _onBefore = onBefore;
            }

            private readonly Func<IHttpRequest, object> _onBefore;

            public IHttpRequestExcutor After;
            public IHttpRequestExcutor Error;
            public IHttpRequestExcutor Handler;

            public void Execute(HttpRequestExcutorContext context)
            {
                if (_onBefore != null)
                {
                    try
                    {
                        context.Result = _onBefore(context.Request);
                    }
                    catch (Exception error)
                    {
                        context.Error = error;

                        Error.Execute(context);

                        return;
                    }
                }

                if (context.Result == null)
                {
                    Handler.Execute(context);
                }
                else
                {
                    After.Execute(context);
                }
            }
        }


        private sealed class HandlerHttpRequestExcutor : IHttpRequestExcutor
        {
            public HandlerHttpRequestExcutor(Func<IHttpRequest, object> onHandle)
            {
                _onHandle = onHandle;
            }

            private readonly Func<IHttpRequest, object> _onHandle;

            public IHttpRequestExcutor After;
            public IHttpRequestExcutor Error;

            public void Execute(HttpRequestExcutorContext context)
            {
                if (_onHandle != null)
                {
                    try
                    {
                        context.Result = _onHandle(context.Request);
                    }
                    catch (Exception error)
                    {
                        context.Error = error;

                        Error.Execute(context);

                        return;
                    }
                }

                After.Execute(context);
            }
        }


        private sealed class AfterHttpRequestExcutor : IHttpRequestExcutor
        {
            public AfterHttpRequestExcutor(Func<IHttpRequest, object, object> onAfter)
            {
                _onAfter = onAfter;
            }

            private readonly Func<IHttpRequest, object, object> _onAfter;

            public IHttpRequestExcutor Error;

            public void Execute(HttpRequestExcutorContext context)
            {
                if (_onAfter != null)
                {
                    try
                    {
                        context.Result = _onAfter(context.Request, context.Result);
                    }
                    catch (Exception error)
                    {
                        context.Error = error;

                        Error.Execute(context);
                    }
                }
            }
        }


        private sealed class ErrorHttpRequestExcutor : IHttpRequestExcutor
        {
            public ErrorHttpRequestExcutor(Func<IHttpRequest, Exception, object> onError)
            {
                _onError = onError;
            }

            private readonly Func<IHttpRequest, Exception, object> _onError;

            public void Execute(HttpRequestExcutorContext context)
            {
                if (_onError != null)
                {
                    context.Result = _onError(context.Request, context.Error);
                }
                else
                {
                    throw context.Error;
                }
            }
        }
    }
}