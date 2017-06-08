using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

using InfinniPlatform.Logging;
using InfinniPlatform.Security;
using InfinniPlatform.Serialization;

using Nancy;
using Nancy.Helpers;

namespace InfinniPlatform.Http
{
    /// <summary>
    /// Инициализатор модулей Nancy для <see cref="IHttpService"/>.
    /// </summary>
    /// <remarks>
    /// Модуль Nancy создается один раз при старте системы и каждый раз перед началом обработки запроса, за который он отвечает.
    /// Регистрация правил маршрутизации модуля происходит в его конструкторе. По этой причине конструктор модуля Nancy должен
    /// быть как можно более простым и быстрым. Данный класс осуществляет предварительную подготовку правил маршрутизации,
    /// конвертируя правила маршрутизации системы в правила маршрутизации Nancy таким образом, чтобы этап инициализации
    /// модулей Nancy был максимально простым и быстрым.
    /// </remarks>
    [LoggerName(nameof(IHttpService))]
    internal class HttpServiceNancyModuleInitializer
    {
        public HttpServiceNancyModuleInitializer(IMimeTypeResolver mimeTypeResolver,
                                                 IUserIdentityProvider userIdentityProvider,
                                                 IJsonObjectSerializer jsonObjectSerializer,
                                                 IHttpServiceContextProvider httpServiceContextProvider,
                                                 HttpRequestExcutorFactory httpRequestExcutorFactory,
                                                 IEnumerable<IHttpGlobalHandler> httpGlobalHandlers,
                                                 IEnumerable<IHttpServiceSource> httpServiceSources,
                                                 IPerformanceLogger<HttpServiceNancyModuleInitializer> perfLogger)
        {
            _mimeTypeResolver = mimeTypeResolver;
            _userIdentityProvider = userIdentityProvider;
            _jsonObjectSerializer = jsonObjectSerializer;
            _httpServiceContextProvider = httpServiceContextProvider;
            _httpRequestExcutorFactory = httpRequestExcutorFactory;
            _httpGlobalHandlers = httpGlobalHandlers;
            _httpServices = new Lazy<IEnumerable<IHttpService>>(() => httpServiceSources.SelectMany(i => i.GetServices()).ToArray());
            _perfLogger = perfLogger;

            _nancyHttpServices = new Lazy<Dictionary<Type, NancyHttpService>>(CreateNancyHttpServices);
        }


        private readonly IMimeTypeResolver _mimeTypeResolver;
        private readonly IUserIdentityProvider _userIdentityProvider;
        private readonly IJsonObjectSerializer _jsonObjectSerializer;
        private readonly IHttpServiceContextProvider _httpServiceContextProvider;
        private readonly HttpRequestExcutorFactory _httpRequestExcutorFactory;
        private readonly IEnumerable<IHttpGlobalHandler> _httpGlobalHandlers;
        private readonly Lazy<IEnumerable<IHttpService>> _httpServices;
        private readonly IPerformanceLogger _perfLogger;

        private readonly Lazy<Dictionary<Type, NancyHttpService>> _nancyHttpServices;


        /// <summary>
        /// Возвращает список типов зарегистрированных модулей Nancy.
        /// </summary>
        public IEnumerable<Type> GetModuleTypes()
        {
            return _httpServices.Value.Select(s => typeof(HttpServiceNancyModule<>).MakeGenericType(s.GetType()));
        }

        /// <summary>
        /// Возвращает базовый путь модуля Nancy для указанного типа сервиса.
        /// </summary>
        public string GetModulePath<TService>()
        {
            NancyHttpService nancyHttpService;

            if (_nancyHttpServices.Value.TryGetValue(typeof(TService), out nancyHttpService))
            {
                return nancyHttpService.ServicePath;
            }

            return null;
        }

        /// <summary>
        /// Инициализирует модуль Nancy правилами маршрутизации указанного типа сервиса.
        /// </summary>
        public void InitializeModuleRoutes<TService>(NancyModule nancyModule) where TService : IHttpService
        {
            NancyHttpService nancyHttpService;

            if (_nancyHttpServices.Value.TryGetValue(typeof(TService), out nancyHttpService))
            {
                foreach (var route in nancyHttpService.Get)
                {
                    nancyModule.Get(route.Path, (p, t) => route.Action(nancyModule));
                }

                foreach (var route in nancyHttpService.Post)
                {
                    nancyModule.Post(route.Path, (p, t) => route.Action(nancyModule));
                }

                foreach (var route in nancyHttpService.Put)
                {
                    nancyModule.Put(route.Path, (p, t) => route.Action(nancyModule));
                }

                foreach (var route in nancyHttpService.Patch)
                {
                    nancyModule.Patch(route.Path, (p, t) => route.Action(nancyModule));
                }

                foreach (var route in nancyHttpService.Delete)
                {
                    nancyModule.Delete(route.Path, (p, t) => route.Action(nancyModule));
                }
            }
        }


        private Dictionary<Type, NancyHttpService> CreateNancyHttpServices()
        {
            var nancyHttpServices = new Dictionary<Type, NancyHttpService>();

            if (_httpServices != null)
            {
                var httpGlobalHandler = CreateNancyHttpGlobalHandler();

                foreach (var httpService in _httpServices.Value)
                {
                    var serviceBuilder = new HttpServiceBuilder();

                    httpService.Load(serviceBuilder);

                    var nancyHttpService = new NancyHttpService
                                           {
                                               ServicePath = serviceBuilder.ServicePath,
                                               Get = CreateNancyHttpServiceRoutes(httpGlobalHandler, serviceBuilder, s => s.Get),
                                               Post = CreateNancyHttpServiceRoutes(httpGlobalHandler, serviceBuilder, s => s.Post),
                                               Put = CreateNancyHttpServiceRoutes(httpGlobalHandler, serviceBuilder, s => s.Put),
                                               Patch = CreateNancyHttpServiceRoutes(httpGlobalHandler, serviceBuilder, s => s.Patch),
                                               Delete = CreateNancyHttpServiceRoutes(httpGlobalHandler, serviceBuilder, s => s.Delete)
                                           };

                    nancyHttpServices[httpService.GetType()] = nancyHttpService;
                }
            }

            return nancyHttpServices;
        }

        private NancyHttpGlobalHandler CreateNancyHttpGlobalHandler()
        {
            Func<IHttpRequest, Task<object>> onBeforeGlobal = null;
            Func<IHttpRequest, object, Task<object>> onAfterGlobal = null;
            Func<IHttpRequest, Exception, Task<object>> onErrorGlobal = null;
            Func<object, IHttpResponse> resultConverterGlobal = null;

            if (_httpGlobalHandlers != null)
            {
                foreach (var globalHandler in _httpGlobalHandlers)
                {
                    onBeforeGlobal += globalHandler.OnBefore;
                    onAfterGlobal += globalHandler.OnAfter;
                    onErrorGlobal += globalHandler.OnError;
                    resultConverterGlobal += globalHandler.ResultConverter;
                }
            }

            if (resultConverterGlobal == null)
            {
                resultConverterGlobal = DefaultHttpResultConverter.Instance.Convert;
            }

            return new NancyHttpGlobalHandler
                   {
                       OnBefore = onBeforeGlobal,
                       OnAfter = onAfterGlobal,
                       OnError = onErrorGlobal,
                       ResultConverter = resultConverterGlobal
                   };
        }

        private IEnumerable<NancyHttpServiceRoute> CreateNancyHttpServiceRoutes(NancyHttpGlobalHandler httpGlobalHandler, IHttpServiceBuilder httpService, Func<IHttpServiceBuilder, IHttpServiceRouteBuilder> httpServiceRoutesSelector)
        {
            var onBefore = httpService.OnBefore;
            var onAfter = httpService.OnAfter;
            var onError = httpService.OnError;
            var resultConverter = httpService.ResultConverter;
            var httpServiceRoutes = httpServiceRoutesSelector(httpService);

            Func<IIdentity> userIdentityProvider = _userIdentityProvider.GetUserIdentity;

            foreach (var route in httpServiceRoutes.Routes)
            {
                // Функция обработки запросов метода сервиса
                var onHandle = _httpRequestExcutorFactory.CreateExecutor(onBefore, route.Action, onAfter, onError, resultConverter);

                // Глобальная функция обработки запросов, вызывающая метод сервиса
                var onHandleGlobal = _httpRequestExcutorFactory.CreateExecutor(httpGlobalHandler.OnBefore, onHandle, httpGlobalHandler.OnAfter, httpGlobalHandler.OnError, httpGlobalHandler.ResultConverter);

                // Функция обработки метода сервиса в контексте выполнения Nancy
                Func<NancyModule, Task<object>> nancyAction = async nancyModule =>
                                                              {
                                                                  var nancyContext = nancyModule.Context;

                                                                  var start = DateTime.Now;

                                                                  var method = $"{nancyContext.Request.Method}::{nancyContext.Request.Path}";

                                                                  Exception error = null;

                                                                  try
                                                                  {
                                                                      var httpRequest = new NancyHttpRequest(nancyContext, userIdentityProvider, _jsonObjectSerializer);

                                                                      // Локализация ответа в зависимости от региональных параметров запроса
                                                                      CultureInfo.CurrentCulture = httpRequest.Culture;
                                                                      CultureInfo.CurrentUICulture = httpRequest.Culture;

                                                                      // Инициализация контекста выполнения запроса
                                                                      var httpServiceContext = (HttpServiceContext)_httpServiceContextProvider.GetContext();
                                                                      httpServiceContext.Request = httpRequest;

                                                                      // Обработка запроса
                                                                      var result = await onHandleGlobal(httpRequest);

                                                                      // Формирование ответа
                                                                      var nancyHttpResponse = CreateNancyHttpResponse(nancyModule, result);

                                                                      return nancyHttpResponse;
                                                                  }
                                                                  catch (Exception exception)
                                                                  {
                                                                      error = exception;

                                                                      // Формирование ответа с ошибкой
                                                                      var errorResult = DefaultHttpResultConverter.Instance.Convert(exception);
                                                                      var nancyErrorHttpResponse = CreateNancyHttpResponse(nancyModule, errorResult);

                                                                      return nancyErrorHttpResponse;
                                                                  }
                                                                  finally
                                                                  {
                                                                      _perfLogger.Log(method, start, error);
                                                                  }
                                                              };

                var nancyRoute = new NancyHttpServiceRoute
                                 {
                                     Path = route.Path,
                                     Action = nancyAction
                                 };

                yield return nancyRoute;
            }
        }

        private object CreateNancyHttpResponse(NancyModule nancyModule, object result)
        {
            var httpResponse = result as IHttpResponse;

            if (httpResponse != null)
            {
                var viewHttpRespose = result as ViewHttpResponce;

                if (viewHttpRespose != null)
                {
                    return nancyModule.View[viewHttpRespose.ViewName, viewHttpRespose.Model];
                }

                var nancyResponse = new Response
                                    {
                                        StatusCode = (HttpStatusCode)httpResponse.StatusCode,
                                        ReasonPhrase = httpResponse.ReasonPhrase
                                    };

                // Установка заголовков
                if (httpResponse.Headers != null)
                {
                    nancyResponse.Headers = httpResponse.Headers;
                }

                // Установка содержимого
                if (httpResponse.Content != null)
                {
                    nancyResponse.Contents = httpResponse.Content;
                }

                // Установка типа содержимого
                nancyResponse.ContentType = httpResponse.ContentType;

                // Установка сериализатора объектов (если применимо)

                var jsonHttpResponse = result as JsonHttpResponse;

                if (jsonHttpResponse != null)
                {
                    if (jsonHttpResponse.Serializer == null)
                    {
                        jsonHttpResponse.Serializer = _jsonObjectSerializer;
                    }

                    nancyResponse.ContentType += "; charset=" + (jsonHttpResponse.Serializer.Encoding?.WebName ?? "utf-8");
                }

                // Установка заголовка для файлов (если применимо)

                var streamHttpResponse = result as StreamHttpResponse;

                if (streamHttpResponse != null)
                {
                    SetNancyStreamHttpResponse(nancyModule.Context, nancyResponse, streamHttpResponse);
                }

                return nancyResponse;
            }

            return result;
        }

        private void SetNancyStreamHttpResponse(NancyContext nancyContext, Response nancyResponse, StreamHttpResponse streamHttpResponse)
        {
            var lastWriteTimeUtc = streamHttpResponse.LastWriteTimeUtc;

            if (lastWriteTimeUtc != null)
            {
                var eTag = "\"" + lastWriteTimeUtc.Value.Ticks.ToString("x") + "\"";

                // Если файл не изменился, возвращается статус NotModified (304)
                if (CacheHelpers.ReturnNotModified(eTag, streamHttpResponse.LastWriteTimeUtc, nancyContext))
                {
                    nancyResponse.StatusCode = HttpStatusCode.NotModified;
                    nancyResponse.Contents = Response.NoBody;
                    nancyResponse.ContentType = null;
                    return;
                }

                nancyResponse.Headers["ETag"] = eTag;
                nancyResponse.Headers["Last-Modified"] = lastWriteTimeUtc.Value.ToString("R");
            }

            var fileName = streamHttpResponse.FileName;

            // Если тип содержимого файла не задан, он определяется автоматически
            if (!string.IsNullOrEmpty(fileName)
                && (string.IsNullOrEmpty(streamHttpResponse.ContentType)
                    || string.Equals(streamHttpResponse.ContentType, HttpConstants.StreamContentType, StringComparison.OrdinalIgnoreCase)))
            {
                nancyResponse.ContentType = _mimeTypeResolver.GetMimeType(fileName);
            }

            var contentLength = streamHttpResponse.ContentLength;

            // Установка информация о размере файла
            if (contentLength != null)
            {
                nancyResponse.Headers["Content-Length"] = contentLength.ToString();
            }
        }


        private sealed class NancyHttpGlobalHandler
        {
            public Func<IHttpRequest, Task<object>> OnBefore;
            public Func<IHttpRequest, object, Task<object>> OnAfter;
            public Func<IHttpRequest, Exception, Task<object>> OnError;
            public Func<object, IHttpResponse> ResultConverter;
        }


        private sealed class NancyHttpService
        {
            public string ServicePath;
            public IEnumerable<NancyHttpServiceRoute> Get;
            public IEnumerable<NancyHttpServiceRoute> Post;
            public IEnumerable<NancyHttpServiceRoute> Put;
            public IEnumerable<NancyHttpServiceRoute> Patch;
            public IEnumerable<NancyHttpServiceRoute> Delete;
        }


        private sealed class NancyHttpServiceRoute
        {
            public string Path;
            public Func<NancyModule, Task<object>> Action;
        }
    }
}