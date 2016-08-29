using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// Источник заданий.
    /// </summary>
    public interface IJobSource
    {
        /// <summary>
        /// Возвращает список заданий.
        /// </summary>
        Task<IEnumerable<JobInfo>> GetJobs();
    }
}