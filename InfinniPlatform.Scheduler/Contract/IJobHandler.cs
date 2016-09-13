using System.Threading.Tasks;

namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// Обработчик заданий.
    /// </summary>
    public interface IJobHandler
    {
        /// <summary>
        /// Обрабатывает задание.
        /// </summary>
        /// <param name="jobInfo">Информация о задании.</param>
        /// <param name="context">Контекст обработки задания.</param>
        Task Handle(IJobInfo jobInfo, IJobHandlerContext context);
    }
}