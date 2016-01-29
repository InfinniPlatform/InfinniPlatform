using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Security;
using InfinniPlatform.Sdk.Services;

using Nancy;
using Nancy.Helpers;

namespace InfinniPlatform.Owin.Services
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
    internal sealed class HttpServiceNancyModuleInitializer
    {
        public HttpServiceNancyModuleInitializer(IMimeTypeResolver mimeTypeResolver,
                                                 IUserIdentityProvider userIdentityProvider,
                                                 HttpRequestExcutorFactory httpRequestExcutorFactory,
                                                 IEnumerable<IHttpGlobalHandler> httpGlobalHandlers,
                                                 IEnumerable<IHttpService> httpServices,
                                                 IPerformanceLog performanceLog)
        {
            _mimeTypeResolver = mimeTypeResolver;
            _userIdentityProvider = userIdentityProvider;
            _httpRequestExcutorFactory = httpRequestExcutorFactory;
            _httpGlobalHandlers = httpGlobalHandlers;
            _httpServices = httpServices;
            _performanceLog = performanceLog;

            _nancyHttpServices = new Lazy<Dictionary<Type, NancyHttpService>>(CreateNancyHttpServices);
        }


        private readonly IMimeTypeResolver _mimeTypeResolver;
        private readonly IUserIdentityProvider _userIdentityProvider;
        private readonly HttpRequestExcutorFactory _httpRequestExcutorFactory;
        private readonly IEnumerable<IHttpGlobalHandler> _httpGlobalHandlers;
        private readonly IEnumerable<IHttpService> _httpServices;
        private readonly IPerformanceLog _performanceLog;

        private readonly Lazy<Dictionary<Type, NancyHttpService>> _nancyHttpServices;


        /// <summary>
        /// Возвращает список типов зарегистрированных модулей Nancy.
        /// </summary>
        public IEnumerable<Type> GetModuleTypes()
        {
            return (_httpServices != null)
                ? _httpServices.Select(s => typeof(HttpServiceNancyModule<>).MakeGenericType(s.GetType()))
                : Type.EmptyTypes;
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
                ApplyNancyHttpServiceRoutes(() => nancyModule.Context, nancyModule.Get, nancyHttpService.Get);
                ApplyNancyHttpServiceRoutes(() => nancyModule.Context, nancyModule.Post, nancyHttpService.Post);
                ApplyNancyHttpServiceRoutes(() => nancyModule.Context, nancyModule.Put, nancyHttpService.Put);
                ApplyNancyHttpServiceRoutes(() => nancyModule.Context, nancyModule.Patch, nancyHttpService.Patch);
                ApplyNancyHttpServiceRoutes(() => nancyModule.Context, nancyModule.Delete, nancyHttpService.Delete);
            }
        }


        private Dictionary<Type, NancyHttpService> CreateNancyHttpServices()
        {
            var nancyHttpServices = new Dictionary<Type, NancyHttpService>();

            if (_httpServices != null)
            {
                var httpGlobalHandler = CreateNancyHttpGlobalHandler();

                foreach (var httpService in _httpServices)
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
                Func<NancyContext, Task<object>> nancyAction = async nancyContext =>
                                                                     {
                                                                         var start = DateTime.Now;
                                                                         var httpRequest = new NancyHttpRequest(nancyContext, userIdentityProvider);
                                                                         var result = await onHandleGlobal(httpRequest);
                                                                         var nancyHttpResponse = CreateNancyHttpResponse(nancyContext, result);
                                                                         _performanceLog.Log("HttpServiceNancyModuleInitializer", "CreateNancyHttpServiceRoutes", start);
                                                                         return nancyHttpResponse;
                                                                     };

                var nancyRoute = new NancyHttpServiceRoute
                {
                    Path = route.Path,
                    Action = nancyAction
                };

                yield return nancyRoute;
            }
        }

        private object CreateNancyHttpResponse(NancyContext nancyContext, object result)
        {
            var httpResponse = result as IHttpResponse;

            if (httpResponse != null)
            {
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

                // Установка заголовка для файлов (если применимо)

                var streamHttpResponse = result as StreamHttpResponse;

                if (streamHttpResponse != null)
                {
                    SetNancyStreamHttpResponse(nancyContext, nancyResponse, streamHttpResponse);
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


        private static void ApplyNancyHttpServiceRoutes(Func<NancyContext> nancyContext, NancyModule.RouteBuilder nancyRouteBuilder, IEnumerable<NancyHttpServiceRoute> nancyHttpServiceRoutes)
        {
            foreach (var route in nancyHttpServiceRoutes)
            {
                nancyRouteBuilder[route.Path, true] = (p, t) => route.Action(nancyContext());
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
            public Func<NancyContext, Task<object>> Action;
        }
    }
}