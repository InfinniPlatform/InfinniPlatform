using System.Threading.Tasks;

using Microsoft.Owin;

namespace InfinniPlatform.Owin.Middleware
{
    /// <summary>
    /// Результат успешной обработки HTTP-запроса в пустого значения.
    /// </summary>
    public sealed class EmptyRequestHandlerResult : IRequestHandlerResult
    {
        public static readonly EmptyRequestHandlerResult Instance = new EmptyRequestHandlerResult();

        private static readonly Task EmptyTask = Task.FromResult<object>(null);

        public bool IsSuccess => true;

        public Task GetResult(IOwinContext context)
        {
            return EmptyTask;
        }
    }
}