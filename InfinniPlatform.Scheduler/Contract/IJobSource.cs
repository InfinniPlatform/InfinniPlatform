using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// �������� �������.
    /// </summary>
    public interface IJobSource
    {
        /// <summary>
        /// ���������� ������ �������.
        /// </summary>
        Task<IEnumerable<JobInfo>> GetJobs();
    }
}