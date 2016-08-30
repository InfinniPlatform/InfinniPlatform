using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// Источник заданий.
    /// </summary>
    public interface IJobInfoSource
    {
        /// <summary>
        /// Возвращает список заданий.
        /// </summary>
        /// <param name="factory">Фабрика для создания информации о задании <see cref="IJobInfo"/>.</param>
        Task<IEnumerable<IJobInfo>> GetJobs(IJobInfoFactory factory);
    }
}