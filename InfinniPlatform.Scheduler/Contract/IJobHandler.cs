using System.Threading.Tasks;

namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// Обработчик задания.
    /// </summary>
    public interface IJobHandler
    {
        /// <summary>
        /// Обрабатывает задание.
        /// </summary>
        /// <param name="context">Контекст обработки задания.</param>
        Task Handle(IJobHandlerContext context);
    }
}