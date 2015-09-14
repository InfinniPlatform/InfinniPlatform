using System.Threading.Tasks;
using Microsoft.Owin;

namespace InfinniPlatform.Owin.Middleware
{
    /// <summary>
    ///     Результат успешной обработки HTTP-запроса в пустого значения.
    /// </summary>
    public sealed class EmptyRequestHandlerResult : IRequestHandlerResult
    {
        private static readonly Task EmptyTask = Task.FromResult<object>(null);

        public Task GetResult(IOwinContext context)
        {
            return EmptyTask;
        }
    }
}