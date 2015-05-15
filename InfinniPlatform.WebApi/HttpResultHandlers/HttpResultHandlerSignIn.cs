using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Owin.Formatting;

namespace InfinniPlatform.WebApi.HttpResultHandlers
{
    /// <summary>
    ///  Обработчик запроса аутентификации пользователей
    /// </summary>
    public sealed class HttpResultHandlerSignIn : IHttpResultHandler
    {
        public HttpResponseMessage WrapResult(object result)
        {
            dynamic response = result;

            var validProperty = result.GetProperty("IsValid");
            var isInternalServerError = result.GetProperty("IsInternalServerError");

            if (validProperty == null || Convert.ToBoolean(validProperty))
            {
                var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);

                var cookieHeaderValues = new List<CookieHeaderValue>();

                IEnumerable<CookieHeaderValue> cookieValues = CookieExtensions.ToCookieList(DynamicWrapperExtensions.ToEnumerable(response.ResponseCookies)); 

                cookieHeaderValues.AddRange(cookieValues);
                responseMessage.Headers.AddCookies(cookieHeaderValues);

                responseMessage.Content = new ObjectContent(typeof (object), response.UserInfo, new JsonMediaTypeFormatter());

                return responseMessage;

            }

            if (isInternalServerError != null && Convert.ToBoolean(isInternalServerError))
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent(typeof(object), result, new JsonMediaTypeFormatter())
                };
            }

            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new ObjectContent(typeof(object), result, new JsonMediaTypeFormatter())
            };
        }
    }

    public static class CookieExtensions
    {
        public static IEnumerable<CookieHeaderValue> ToCookieList(this IEnumerable<dynamic> cookieHeaders)
        {
            var result = new List<CookieHeaderValue>();

            foreach (var cookieHeader in cookieHeaders)
            {
                var cookieHeaderValue = new CookieHeaderValue(cookieHeader.Name,cookieHeader.Value);
                cookieHeaderValue.Path = cookieHeader.Path;

                result.Add(cookieHeaderValue);
            }

            return result;
        }
    }

}
