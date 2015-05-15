using System.Collections.Generic;
using System.Net;
using InfinniPlatform.Sdk.Properties;
using RestSharp;

namespace InfinniPlatform.Sdk
{
    /// <summary>
    ///   Ответ сервера на REST-запрос
    /// </summary>
    public class RestQueryResponse
    {
        public string Content { get; set; }

        public int ContentLength
        {
            get { return string.IsNullOrEmpty(Content) ? 0 : Content.Length; }
        }

        public HttpStatusCode HttpStatusCode { get; set; }

        /// <summary>
        ///   Внутренняя ошибка сервера
        /// </summary>
        public bool IsServerError
        {
            get { return HttpStatusCode == HttpStatusCode.InternalServerError || Content.ToLowerInvariant().Contains("internalservererror"); }
        }

        public bool IsServiceNotRegistered
        {
            get {  return Content.Contains(Resources.ServiceNotRegistered); }
        }

        /// <summary>
        ///   Сервер по указанному адресу не найден
        /// </summary>
        public bool IsRemoteServerNotFound
        {
            get { return HttpStatusCode == 0; }
        }

        /// <summary>
        ///  Ошибка в бизнес-логике сервера
        /// </summary>
        public bool IsBusinessLogicError
        {
            get { return HttpStatusCode == HttpStatusCode.BadRequest; }
        }

        public bool IsAllOk
        {
            get
            {
                return (HttpStatusCode == HttpStatusCode.OK || HttpStatusCode == HttpStatusCode.BadRequest) &&
                       !IsRemoteServerNotFound && !IsServiceNotRegistered && !IsServerError;
            }
        }

    }

    public static class RestQueryExtensions
    {
        public static RestQueryResponse ToQueryResponse(this IRestResponse restResponse)
        {
            return new RestQueryResponse()
            {
                Content = restResponse.Content,
                HttpStatusCode = restResponse.StatusCode,
            };
        }

    }
}
