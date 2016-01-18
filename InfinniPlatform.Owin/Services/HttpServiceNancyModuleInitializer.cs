using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

using InfinniPlatform.Sdk.Security;
using InfinniPlatform.Sdk.Services;

using Nancy;

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
        public HttpServiceNancyModuleInitializer(IUserIdentityProvider userIdentityProvider,
                                                 HttpRequestExcutorFactory httpRequestExcutorFactory,
                                                 IEnumerable<IHttpGlobalHandler> httpGlobalHandlers,
                                                 IEnumerable<IHttpService> httpServices)
        {
            _userIdentityProvider = userIdentityProvider;
            _httpRequestExcutorFactory = httpRequestExcutorFactory;
            _httpGlobalHandlers = httpGlobalHandlers;
            _httpServices = httpServices;

            _nancyHttpServices = new Lazy<Dictionary<Type, NancyHttpService>>(CreateNancyHttpServices);
        }


        private readonly IUserIdentityProvider _userIdentityProvider;
        private readonly HttpRequestExcutorFactory _httpRequestExcutorFactory;
        private readonly IEnumerable<IHttpGlobalHandler> _httpGlobalHandlers;
        private readonly IEnumerable<IHttpService> _httpServices;

        private readonly Lazy<Dictionary<Type, NancyHttpService>> _nancyHttpServices;


        /// <summary>
        /// Возвращает список типов зарегистрированных модулей Nancy.
        /// </summary>
        public IEnumerable<Type> GetModuleTypes()
        {
            return _httpServices?.Select(s => typeof(HttpServiceNancyModule<>).MakeGenericType(s.GetType())) ?? Type.EmptyTypes;

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
            Func<IHttpRequest, object> onBeforeGlobal = null;
            Func<IHttpRequest, object, object> onAfterGlobal = null;
            Func<IHttpRequest, Exception, object> onErrorGlobal = null;
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

            Func<IIdentity> userIdentityProvider = _userIdentityProvider.GetCurrentUserIdentity;

            foreach (var route in httpServiceRoutes.Routes)
            {
                // Функция обработки запросов метода сервиса
                var onHandle = _httpRequestExcutorFactory.CreateExecutor(onBefore, route.Action, onAfter, onError, resultConverter);

                // Глобальная функция обработки запросов, вызывающая метод сервиса
                var onHandleGlobal = _httpRequestExcutorFactory.CreateExecutor(httpGlobalHandler.OnBefore, onHandle, httpGlobalHandler.OnAfter, httpGlobalHandler.OnError, httpGlobalHandler.ResultConverter);

                // Функция обработки метода сервиса в контексте выполнения Nancy
                Func<NancyContext, object> nancyAction = nancyContext =>
                                                         {
                                                             var httpRequest = new NancyHttpRequest(nancyContext, userIdentityProvider);
                                                             var result = onHandleGlobal(httpRequest);
                                                             return CreateNancyHttpResponse(result);
                                                         };

                var nancyRoute = new NancyHttpServiceRoute
                {
                    Path = route.Path,
                    Action = nancyAction
                };

                yield return nancyRoute;
            }
        }

        private static object CreateNancyHttpResponse(object result)
        {
            var httpResponse = result as IHttpResponse;

            if (httpResponse != null)
            {
                var nancyResponse = new Response
                {
                    StatusCode = (HttpStatusCode)httpResponse.StatusCode,
                    ReasonPhrase = httpResponse.ReasonPhrase,
                    ContentType = httpResponse.ContentType,
                    Contents = httpResponse.Content
                };

                if (httpResponse.Headers != null)
                {
                    nancyResponse.Headers = httpResponse.Headers;
                }

                return nancyResponse;
            }

            return result;
        }


        private static void ApplyNancyHttpServiceRoutes(Func<NancyContext> nancyContext, NancyModule.RouteBuilder nancyRouteBuilder, IEnumerable<NancyHttpServiceRoute> nancyHttpServiceRoutes)
        {
            foreach (var route in nancyHttpServiceRoutes)
            {
                nancyRouteBuilder[route.Path] = p => route.Action(nancyContext());
            }
        }


        private sealed class NancyHttpGlobalHandler
        {
            public Func<IHttpRequest, object> OnBefore;
            public Func<IHttpRequest, object, object> OnAfter;
            public Func<IHttpRequest, Exception, object> OnError;
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
            public Func<NancyContext, object> Action;
            public string Path;
        }
    }
}