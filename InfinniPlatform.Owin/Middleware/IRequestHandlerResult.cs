using System.Threading.Tasks;
using Microsoft.Owin;

namespace InfinniPlatform.Owin.Middleware
{
    /// <summary>
    ///     Результат обработки HTTP-запроса.
    /// </summary>
    public interface IRequestHandlerResult
    {
        /// <summary>
        ///     Получить результат.
        /// </summary>
        Task GetResult(IOwinContext context);
    }
}