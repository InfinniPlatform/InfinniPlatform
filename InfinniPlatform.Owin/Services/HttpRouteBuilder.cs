using System;
using System.Security.Principal;

using InfinniPlatform.Owin.Properties;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Services;

using Nancy;
using Nancy.Responses;

namespace InfinniPlatform.Owin.Services
{
    /// <summary>
    /// Регистратор правил маршрутизации запросов.
    /// </summary>
    internal sealed class HttpRouteBuilder : IHttpRouteBuilder
    {
        public HttpRouteBuilder(NancyModule.RouteBuilder nancyRouteBuilder,
                                Func<NancyContext> nancyContext,
                                Func<IIdentity> userIdentityProvider,
                                Func<Action<IHttpRequest>> onBefore,
                                Func<Action<IHttpRequest, IHttpResponse, Exception>> onAfter)
        {
            _nancyRouteBuilder = nancyRouteBuilder;
            _nancyContext = nancyContext;
            _userIdentityProvider = userIdentityProvider;
            _onBefore = onBefore;
            _onAfter = onAfter;
        }


        private readonly NancyModule.RouteBuilder _nancyRouteBuilder;
        private readonly Func<NancyContext> _nancyContext;
        private readonly Func<IIdentity> _userIdentityProvider;
        private readonly Func<Action<IHttpRequest>> _onBefore;
        private readonly Func<Action<IHttpRequest, IHttpResponse, Exception>> _onAfter;


        public Func<IHttpRequest, IHttpResponse> this[string path]
        {
            set
            {
                _nancyRouteBuilder[path] = form => ProcessRequest(value);
            }
        }


        private Response ProcessRequest(Func<IHttpRequest, IHttpResponse> handler)
        {
            var context = _nancyContext();
            var request = new HttpRequest(context, _userIdentityProvider);

            Exception exception = null;
            IHttpResponse response = null;

            // Предобработка запроса

            var onBefore = _onBefore?.Invoke();

            if (onBefore != null)
            {
                try
                {
                    onBefore.Invoke(request);
                }
                catch (Exception e)
                {
                    var onBeforeException = new InvalidOperationException(Resources.RequestPreprocessingCompletedWithUnexpectedException, e);
                    exception = onBeforeException;
                }
            }

            // Обработка запроса, если предобработка прошла успешно
            if (exception == null)
            {
                try
                {
                    response = handler(request);
                }
                catch (Exception e)
                {
                    var handlerException = new InvalidOperationException(Resources.RequestProcessingCompletedWithUnexpectedException, e);
                    exception = handlerException;
                }
            }

            // Постобработка запроса делается в любом случае

            var onAfter = _onAfter?.Invoke();

            if (onAfter != null)
            {
                try
                {
                    onAfter.Invoke(request, response, exception);
                }
                catch (Exception e)
                {
                    var onAfterException = new InvalidOperationException(Resources.RequestPostprocessingWithUnexpectedException, e);
                    exception = onAfterException;
                }
            }

            return (exception == null) ? BuildResultResponse(response) : BuildFailResponse(exception);
        }

        private static Response BuildFailResponse(Exception exception)
        {
            return new TextResponse(exception.GetMessage())
            {
                StatusCode = HttpStatusCode.InternalServerError
            };
        }

        private static Response BuildResultResponse(IHttpResponse response)
        {
            if (response != null)
            {
                var nancyResponse = new Response
                {
                    StatusCode = (HttpStatusCode)response.StatusCode,
                    ReasonPhrase = response.ReasonPhrase
                };

                nancyResponse.Headers = response.Headers ?? nancyResponse.Headers;
                nancyResponse.ContentType = response.ContentType ?? nancyResponse.ContentType;
                nancyResponse.Contents = response.Content ?? nancyResponse.Contents;

                return nancyResponse;
            }

            return null;
        }
    }
}