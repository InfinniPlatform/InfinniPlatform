using System.IO;

namespace InfinniPlatform.Sdk.Services
{
    /// <summary>
    /// Абстрактный класс обработчика запросов с простой логикой формирования ответа.
    /// </summary>
    /// <remarks>
    /// Наследники данного класса должны реализовать метод <see cref="ActionResult"/>, который возвращает результат обработки
    /// запроса в свободной форме. Если тип возвращаемого значения реализует интерфейс <see cref="IHttpResponse"/>, то ответ
    /// отдается клиенту без изменений. Для остальных случаев результат интерпретируется исходя из следующих правил:
    /// <list type="bullet">
    /// <item><term><see cref="int"/></term><description> - возвращается как <see cref="HttpResponse.StatusCode"/></description></item>
    /// <item><term><see cref="string"/></term><description> - возвращается как текст в формате <see cref="TextHttpResponse"/></description></item>
    /// <item><term><see cref="T:byte[]"/></term><description> - возвращается как поток в формате <see cref="StreamHttpResponse"/></description></item>
    /// <item><term><see cref="Stream"/></term><description> - возвращается как поток в формате <see cref="StreamHttpResponse"/></description></item>
    /// <item><term>иные типы</term><description> - возвращается как объект в формате <see cref="JsonHttpResponse"/></description></item>
    /// </list>
    /// </remarks>
    public abstract class SimpleHttpRequestHandler : IHttpRequestHandler
    {
        public IHttpResponse Action(IHttpRequest request)
        {
            var result = ActionResult(request);

            if (result != null)
            {
                if (result is IHttpResponse)
                {
                    return (IHttpResponse)result;
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
                    return new StreamHttpResponse((Stream)result);
                }

                return new JsonHttpResponse(result);
            }

            return null;
        }

        /// <summary>
        /// Возвращает результат обработки запроса в свободной форме.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <returns>Ответ.</returns>
        protected abstract object ActionResult(IHttpRequest request);
    }
}