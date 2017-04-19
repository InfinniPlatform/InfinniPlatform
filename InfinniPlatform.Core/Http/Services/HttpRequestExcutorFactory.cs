using System;
using System.Threading.Tasks;

using InfinniPlatform.Core.Logging;
using InfinniPlatform.Core.Properties;

namespace InfinniPlatform.Core.Http.Services
{
    /// <summary>
    /// Фабрика создания функции обработки запросов.
    /// </summary>
    internal class HttpRequestExcutorFactory
    {
        public HttpRequestExcutorFactory(ILog log)
        {
            _log = log;
        }


        private readonly ILog _log;


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
        public Func<IHttpRequest, Task<object>> CreateExecutor(Func<IHttpRequest, Task<object>> onBefore,
                                                               Func<IHttpRequest, Task<object>> onHandle,
                                                               Func<IHttpRequest, object, Task<object>> onAfter,
                                                               Func<IHttpRequest, Exception, Task<object>> onError,
                                                               Func<object, IHttpResponse> resultConverter)
        {
            // Создание цепочки обработки запроса

            var beforeExcutor = new BeforeHttpRequestExcutor(onBefore);
            var handlerExcutor = new HandlerHttpRequestExcutor(onHandle);
            var afterExcutor = new AfterHttpRequestExcutor(onAfter);
            var errorExcutor = new ErrorHttpRequestExcutor(onError, _log);

            beforeExcutor.Handler = handlerExcutor;
            beforeExcutor.After = afterExcutor;
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
                    return async request =>
                                 {
                                     var context = new HttpRequestExcutorContext(request);
                                     await firstExcutor.Execute(context);
                                     return resultConverter(context.Result);
                                 };
                }

                return async request =>
                             {
                                 var context = new HttpRequestExcutorContext(request);
                                 await firstExcutor.Execute(context);
                                 return context.Result;
                             };
            }

            return null;
        }


        private interface IHttpRequestExcutor
        {
            Task Execute(HttpRequestExcutorContext context);
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
            public BeforeHttpRequestExcutor(Func<IHttpRequest, Task<object>> onBefore)
            {
                _onBefore = onBefore;
            }

            private readonly Func<IHttpRequest, Task<object>> _onBefore;

            public IHttpRequestExcutor After;
            public IHttpRequestExcutor Error;
            public IHttpRequestExcutor Handler;

            public async Task Execute(HttpRequestExcutorContext context)
            {
                if (_onBefore != null)
                {
                    try
                    {
                        context.Result = await _onBefore(context.Request);
                    }
                    catch (Exception error)
                    {
                        context.Error = error;

                        await Error.Execute(context);

                        return;
                    }
                }

                if (context.Result == null)
                {
                    await Handler.Execute(context);
                }
                else
                {
                    await After.Execute(context);
                }
            }
        }


        private sealed class HandlerHttpRequestExcutor : IHttpRequestExcutor
        {
            public HandlerHttpRequestExcutor(Func<IHttpRequest, Task<object>> onHandle)
            {
                _onHandle = onHandle;
            }

            private readonly Func<IHttpRequest, Task<object>> _onHandle;

            public IHttpRequestExcutor After;
            public IHttpRequestExcutor Error;

            public async Task Execute(HttpRequestExcutorContext context)
            {
                if (_onHandle != null)
                {
                    try
                    {
                        context.Result = await _onHandle(context.Request);
                    }
                    catch (Exception error)
                    {
                        context.Error = error;

                        await Error.Execute(context);

                        return;
                    }
                }

                await After.Execute(context);
            }
        }


        private sealed class AfterHttpRequestExcutor : IHttpRequestExcutor
        {
            public AfterHttpRequestExcutor(Func<IHttpRequest, object, Task<object>> onAfter)
            {
                _onAfter = onAfter;
            }

            private readonly Func<IHttpRequest, object, Task<object>> _onAfter;

            public IHttpRequestExcutor Error;

            public async Task Execute(HttpRequestExcutorContext context)
            {
                if (_onAfter != null)
                {
                    try
                    {
                        context.Result = await _onAfter(context.Request, context.Result);
                    }
                    catch (Exception error)
                    {
                        context.Error = error;

                        await Error.Execute(context);
                    }
                }
            }
        }


        private sealed class ErrorHttpRequestExcutor : IHttpRequestExcutor
        {
            public ErrorHttpRequestExcutor(Func<IHttpRequest, Exception, Task<object>> onError, ILog log)
            {
                _onError = onError;
                _log = log;
            }

            private readonly Func<IHttpRequest, Exception, Task<object>> _onError;
            private readonly ILog _log;

            public async Task Execute(HttpRequestExcutorContext context)
            {
                var error = context.Error;

                if (error != null)
                {
                    if (_onError != null)
                    {
                        context.Result = await _onError(context.Request, context.Error);
                    }
                    else
                    {
                        // Вне зависимости от наличия прикладных обработчиков, фиксируем событие ошибки
                        _log.Error(Resources.RequestProcessingCompletedWithUnexpectedException, error);

                        throw new InvalidOperationException(Resources.RequestProcessingCompletedWithUnexpectedException, error);
                    }
                }
            }
        }
    }
}