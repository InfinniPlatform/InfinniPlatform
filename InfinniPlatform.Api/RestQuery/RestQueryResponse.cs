using System.Collections.Generic;
using System.Net;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.Api.RestQuery
{
    /// <summary>
    ///     Ответ сервера на REST-запрос
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
        ///     Внутренняя ошибка сервера
        /// </summary>
        public bool IsServerError
        {
            get
            {
                return HttpStatusCode == HttpStatusCode.InternalServerError ||
                       Content.ToLowerInvariant().Contains("internalservererror");
            }
        }

        public bool IsServiceNotRegistered
        {
            get { return Content.Contains(Resources.ServiceNotFoundError); }
        }

        /// <summary>
        ///     Сервер по указанному адресу не найден
        /// </summary>
        public bool IsRemoteServerNotFound
        {
            get { return HttpStatusCode == 0; }
        }

        /// <summary>
        ///     Ошибка в бизнес-логике сервера
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

        public dynamic ToDynamic()
        {
            return Content.ToDynamic();
        }

        public IEnumerable<dynamic> ToDynamicList()
        {
            return Content.ToDynamicList();
        }
    }
}