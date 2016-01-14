using System;

using InfinniPlatform.Sdk.Services;

using Nancy;

namespace InfinniPlatform.Owin.Services
{
    /// <summary>
    /// Регистратор правил маршрутизации запросов.
    /// </summary>
    internal sealed class HttpRouteBuilder : IHttpRouteBuilder
    {
        public HttpRouteBuilder(NancyModule.RouteBuilder nancyRouteBuilder,
                                Func<NancyContext> nancyContext,
                                Func<Action<IHttpRequest>> onBefore,
                                Func<Action<IHttpRequest, IHttpResponse>> onAfter)
        {
            _nancyRouteBuilder = nancyRouteBuilder;
            _nancyContext = nancyContext;
            _onBefore = onBefore;
            _onAfter = onAfter;
        }


        private readonly NancyModule.RouteBuilder _nancyRouteBuilder;
        private readonly Func<NancyContext> _nancyContext;
        private readonly Func<Action<IHttpRequest>> _onBefore;
        private readonly Func<Action<IHttpRequest, IHttpResponse>> _onAfter;


        public Func<IHttpRequest, IHttpResponse> this[string path]
        {
            set
            {
                _nancyRouteBuilder[path]
                    = form =>
                      {
                          var context = _nancyContext();
                          var request = new HttpRequest(context);

                          _onBefore()?.Invoke(request);

                          IHttpResponse response = null;

                          try
                          {
                              response = value(request);
                          }
                          finally
                          {
                              _onAfter()?.Invoke(request, response);
                          }

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
                      };
            }
        }
    }
}