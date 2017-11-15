using System.Linq;
using System.Threading.Tasks;

using InfinniPlatform.Diagnostics;
using InfinniPlatform.Dynamic;
using InfinniPlatform.Http;
using Microsoft.AspNetCore.Http;

namespace InfinniPlatform.Scheduler.Diagnostics
{
    /// <summary>
    /// Provides the scheduler status.
    /// </summary>
    internal class SchedulerStatusProvider : ISubsystemStatusProvider
    {
        public SchedulerStatusProvider(IJobScheduler jobScheduler)
        {
            _jobScheduler = jobScheduler;
        }


        private readonly IJobScheduler _jobScheduler;


        public string Name => "scheduler";


        /// <summary>
        /// Gets the scheduler status.
        /// </summary>
        public async Task<object> GetStatus(HttpRequest request)
        {
            var isStarted = await _jobScheduler.IsStarted();
            var totalCount = await _jobScheduler.GetStatus(i => i.Count());
            var plannedCount = await _jobScheduler.GetStatus(i => i.Count(j => j.State == JobState.Planned));
            var pausedCount = await _jobScheduler.GetStatus(i => i.Count(j => j.State == JobState.Paused));

            var status = new DynamicDocument
                         {
                             {
                                 "isStarted", isStarted
                             },
                             {
                                 "all", new DynamicDocument
                                        {
                                            { "count", totalCount },
                                            { "ref", $"{request.PathBase}/{Name}/jobs?skip=0&take=10" }
                                        }
                             },
                             {
                                 "planned", new DynamicDocument
                                            {
                                                { "count", plannedCount },
                                                { "ref", $"{request.PathBase}/{Name}/jobs?state=planned&skip=0&take=10" }
                                            }
                             },
                             {
                                 "paused", new DynamicDocument
                                           {
                                               { "count", pausedCount },
                                               { "ref", $"{request.PathBase}/{Name}/jobs?state=paused&skip=0&take=10" }
                                           }
                             }
                         };

            return new ServiceResult<DynamicDocument> { Success = true, Result = status };
        }
    }
}