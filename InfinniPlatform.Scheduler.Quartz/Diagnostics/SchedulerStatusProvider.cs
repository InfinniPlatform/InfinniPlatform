using System;
using System.Linq;
using System.Threading.Tasks;

using InfinniPlatform.Core.Diagnostics;
using InfinniPlatform.Core.Dynamic;
using InfinniPlatform.Core.Http;
using InfinniPlatform.Core.Logging;

namespace InfinniPlatform.Scheduler.Diagnostics
{
    /// <summary>
    /// Provides the scheduler status.
    /// </summary>
    internal class SchedulerStatusProvider : ISubsystemStatusProvider
    {
        public SchedulerStatusProvider(IJobScheduler jobScheduler,
                                       IHostAddressParser hostAddressParser,
                                       ILog log)
        {
            _jobScheduler = jobScheduler;
            _hostAddressParser = hostAddressParser;
            _log = log;
        }


        private readonly IJobScheduler _jobScheduler;
        private readonly IHostAddressParser _hostAddressParser;
        private readonly ILog _log;


        public string Name => "scheduler";


        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = Name;

            builder.OnBefore = async r =>
                               {
                                   // Запрос статуса разрешен только с локального узла
                                   if (!await _hostAddressParser.IsLocalAddress(r.UserHostAddress))
                                   {
                                       return HttpResponse.Forbidden;
                                   }

                                   return null;
                               };

            // Состояние планировщика заданий
            builder.Get["/"] = r => HandleRequest(r, GetStatus);
            builder.Post["/"] = r => HandleRequest(r, GetStatus);
        }


        /// <summary>
        /// Gets the scheduler status.
        /// </summary>
        public async Task<object> GetStatus(IHttpRequest request)
        {
            var isStarted = await _jobScheduler.IsStarted();
            var totalCount = await _jobScheduler.GetStatus(i => i.Count());
            var plannedCount = await _jobScheduler.GetStatus(i => i.Count(j => j.State == JobState.Planned));
            var pausedCount = await _jobScheduler.GetStatus(i => i.Count(j => j.State == JobState.Paused));

            var status = new DynamicWrapper
                         {
                             {
                                 "isStarted", isStarted
                             },
                             {
                                 "all", new DynamicWrapper
                                        {
                                            { "count", totalCount },
                                            { "ref", $"{request.BasePath}/{Name}/jobs?skip=0&take=10" }
                                        }
                             },
                             {
                                 "planned", new DynamicWrapper
                                            {
                                                { "count", plannedCount },
                                                { "ref", $"{request.BasePath}/{Name}/jobs?state=planned&skip=0&take=10" }
                                            }
                             },
                             {
                                 "paused", new DynamicWrapper
                                           {
                                               { "count", pausedCount },
                                               { "ref", $"{request.BasePath}/{Name}/jobs?state=paused&skip=0&take=10" }
                                           }
                             }
                         };

            return new ServiceResult<DynamicWrapper> { Success = true, Result = status };
        }


        private async Task<object> HandleRequest(IHttpRequest request, Func<IHttpRequest, Task<object>> requestHandler)
        {
            try
            {
                return await requestHandler(request);
            }
            catch (Exception exception)
            {
                _log.Error(exception);

                return new ServiceResult<object>
                {
                    Success = false,
                    Error = exception.GetFullMessage()
                };
            }
        }
    }
}