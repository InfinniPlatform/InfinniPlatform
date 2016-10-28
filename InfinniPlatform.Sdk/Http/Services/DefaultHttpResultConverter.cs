using System;
using System.IO;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Logging;

namespace InfinniPlatform.Sdk.Http.Services
{
    /// <summary>
    /// Конвертер результата выполнения запроса по умолчанию.
    /// </summary>
    /// <remarks>
    /// Метод <see cref="Convert"/> принимает результат обработки запроса в свободной форме формирует на основе него
    /// ответ в формате <see cref="IHttpResponse"/>. Если тип результата реализует интерфейс <see cref="IHttpResponse"/>,
    /// то он возвращается без изменений. Для остальных случаев результат интерпретируется исходя из следующих правил:
    /// <list type="bullet">
    /// <item><term><c>null</c></term><description> - возвращается как <see cref="HttpResponse.Ok"/></description></item>
    /// <item><term><see cref="int"/></term><description> - возвращается как <see cref="HttpResponse.StatusCode"/></description></item>
    /// <item><term><see cref="string"/></term><description> - возвращается как текст в формате <see cref="TextHttpResponse"/></description></item>
    /// <item><term><see cref="T:byte[]"/>, <see cref="Stream"/>, <see cref="T:Func&lt;Stream&gt;"/></term><description> - возвращается как поток в формате <see cref="StreamHttpResponse"/></description></item>
    /// <item><term><see cref="Exception"/></term><description> - возвращается как <c>InternalServerError</c> с текстом исключения в формате <see cref="TextHttpResponse"/></description></item>
    /// <item><term>иные типы</term><description> - возвращается как объект в формате <see cref="JsonHttpResponse"/></description></item>
    /// </list>
    /// </remarks>
    public sealed class DefaultHttpResultConverter
    {
        /// <summary>
        /// Экземпляр класса по умолчанию.
        /// </summary>
        public static readonly DefaultHttpResultConverter Instance = new DefaultHttpResultConverter();


        /// <summary>
        /// Конвертирует результат выполнения запроса в <see cref="IHttpResponse"/>.
        /// </summary>
        public IHttpResponse Convert(object result)
        {
            if (result != null)
            {
                if (result is IHttpResponse)
                {
                    return (IHttpResponse)result;
                }

                if (result is Exception)
                {
                    return CreateErrorHttpResponse((Exception)result);
                }

                if (result is int)
                {
                    return new HttpResponse { StatusCode = (int)result };
                }

                if (result is string)
                {
                    return new TextHttpResponse((string)result);
                }

                if (result is byte[])
                {
                    return new StreamHttpResponse((byte[])result);
                }

                if (result is Stream)
                {
                    return new StreamHttpResponse(() => (Stream)result);
                }

                if (result is Func<Stream>)
                {
                    return new StreamHttpResponse((Func<Stream>)result);
                }

                return new JsonHttpResponse(result);
            }

            return HttpResponse.Ok;
        }


        private static IHttpResponse CreateErrorHttpResponse(Exception exception)
        {
            var error = new ServiceResult<DynamicWrapper>
            {
                Success = false,
                Error = exception.GetFullMessage()
            };

            return new JsonHttpResponse(error) { StatusCode = 500 };
        }
    }
}