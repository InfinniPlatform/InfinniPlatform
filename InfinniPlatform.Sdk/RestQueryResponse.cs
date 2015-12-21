using System.Net;
using System.Text;

using InfinniPlatform.Sdk.Properties;

namespace InfinniPlatform.Sdk
{
    /// <summary>
    /// Ответ сервера на REST-запрос
    /// </summary>
    public class RestQueryResponse
    {
        public string Content { get; set; }

        public int ContentLength
        {
            get
            {
                return string.IsNullOrEmpty(Content) ? 0 : Content.Length;
            }
        }

        public HttpStatusCode HttpStatusCode { get; set; }

        /// <summary>
        /// Внутренняя ошибка сервера
        /// </summary>
        public bool IsServerError
        {
            get { return HttpStatusCode == HttpStatusCode.InternalServerError || Content.ToLowerInvariant().Contains("internalservererror"); }
        }

        public bool IsServiceNotRegistered
        {
            get { return Content.Contains(Resources.ServiceNotRegistered); }
        }

        /// <summary>
        /// Сервер по указанному адресу не найден
        /// </summary>
        public bool IsRemoteServerNotFound
        {
            get { return HttpStatusCode == 0; }
        }

        /// <summary>
        /// Ошибка в бизнес-логике сервера
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
                       !IsBusinessLogicError && !IsRemoteServerNotFound && !IsServiceNotRegistered && !IsServerError;
            }
        }

        public string GetErrorContent()
        {
            var builder = new StringBuilder();

            if (IsBusinessLogicError)
            {
                builder.Append(Resources.BusinessLogicError);
            }
            else if (IsRemoteServerNotFound)
            {
                builder.Append(Resources.RemoteServerNotFound);
            }
            else if (IsServerError)
            {
                builder.Append(Resources.InternalServerError);
            }
            else if (IsServiceNotRegistered)
            {
                builder.Append(Resources.ServiceNotRegisteredError);
            }

            if (!string.IsNullOrEmpty(Content))
            {
                builder.AppendFormat("Additional info: {0}", Content);
            }

            return builder.ToString();
        }

        public override string ToString()
        {
            return GetErrorContent();
        }
    }
}