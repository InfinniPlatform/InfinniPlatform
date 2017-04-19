using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InfinniPlatform.Core.Dynamic;
using InfinniPlatform.Core.Http;
using InfinniPlatform.Core.Logging;
using InfinniPlatform.Core.Serialization;

namespace InfinniPlatform.Scheduler
{
    /// <summary>
    /// Provides HTTP API to the shceduler.
    /// </summary>
    public class SchedulerHttpService : IHttpService
    {
        public SchedulerHttpService(IJobScheduler jobScheduler,
                                    IHostAddressParser hostAddressParser,
                                    IJsonObjectSerializer jsonObjectSerializer,
                                    ILog log)
        {
            _jobScheduler = jobScheduler;
            _hostAddressParser = hostAddressParser;
            _jsonObjectSerializer = jsonObjectSerializer;
            _log = log;
        }


        private readonly IJobScheduler _jobScheduler;
        private readonly IHostAddressParser _hostAddressParser;
        private readonly IJsonObjectSerializer _jsonObjectSerializer;
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

            // Список заданий планировщика
            builder.Get["/jobs"] = r => HandleRequest(r, GetJobs);
            builder.Post["/jobs"] = r => HandleRequest(r, GetJobs);

            // Управление заданием планировщика
            builder.Get["/jobs/{id}"] = r => HandleRequest(r, GetJob);
            builder.Post["/jobs/{id}"] = r => HandleRequest(r, PostJob);
            builder.Delete["/jobs/{id}"] = r => HandleRequest(r, DeleteJob);

            // Управление планировщиком заданий
            builder.Post["/pause"] = r => HandleRequest(r, PauseJobs);
            builder.Post["/resume"] = r => HandleRequest(r, ResumeJobs);
            builder.Post["/trigger"] = r => HandleRequest(r, TriggerJobs);
        }


        /// <summary>
        /// Gets the scheduler status.
        /// </summary>
        private async Task<object> GetStatus(IHttpRequest request)
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


        /// <summary>
        /// Gets the scheduler jobs.
        /// </summary>
        private async Task<object> GetJobs(IHttpRequest request)
        {
            const int skipMin = 0;
            const int skipDefault = 0;

            const int takeMin = 1;
            const int takeMax = 100;
            const int takeDefault = 10;

            var state = (string)TryGetValue(request.Query.state, string.Empty);
            var skip = Math.Max((int)TryGetValue(request.Query.skip, skipDefault), skipMin);
            var take = Math.Max(Math.Min((int)TryGetValue(request.Query.take, takeDefault), takeMax), takeMin);

            var jobs = await _jobScheduler.GetStatus(i =>
                                                     {
                                                         if (string.Equals(state, "planned", StringComparison.OrdinalIgnoreCase))
                                                         {
                                                             i = i.Where(j => j.State == JobState.Planned);
                                                         }
                                                         else if (string.Equals(state, "paused", StringComparison.OrdinalIgnoreCase))
                                                         {
                                                             i = i.Where(j => j.State == JobState.Paused);
                                                         }

                                                         i = i.Skip(skip).Take(take);

                                                         return i.Select(j => new DynamicWrapper
                                                                              {
                                                                                  { "id", j.Info.Id },
                                                                                  { "ref", $"{request.BasePath}/{Name}/jobs/{j.Info.Id}" }
                                                                              })
                                                                 .ToList();
                                                     });

            return new ServiceResult<List<DynamicWrapper>> { Success = true, Result = jobs };
        }

        /// <summary>
        /// Gets the scheduler job info.
        /// </summary>
        private async Task<object> GetJob(IHttpRequest request)
        {
            var response = new ServiceResult<IJobInfo>();

            string jobId = TryGetValue(request.Parameters.id, string.Empty);

            if (!string.IsNullOrEmpty(jobId))
            {
                var jobInfo = await _jobScheduler.GetStatus(i => i.Select(j => j.Info).FirstOrDefault(j => j.Id == jobId));

                if (jobInfo != null)
                {
                    response.Success = true;
                    response.Result = jobInfo;
                }
            }

            return response;
        }

        /// <summary>
        /// Adds or updates the scheduler job.
        /// </summary>
        private async Task<object> PostJob(IHttpRequest request)
        {
            var response = new ServiceResult<bool>();

            string jobId = TryGetValue(request.Parameters.id, string.Empty);

            if (!string.IsNullOrEmpty(jobId))
            {
                var jobInfo = _jsonObjectSerializer.Deserialize<JobInfo>(request.Content);

                if (jobInfo != null)
                {
                    jobInfo.Id = jobId;

                    await _jobScheduler.AddOrUpdateJob(jobInfo);

                    response.Success = true;
                    response.Result = true;
                }
            }

            return response;
        }

        /// <summary>
        /// Deletes the scheduler job.
        /// </summary>
        private async Task<object> DeleteJob(IHttpRequest request)
        {
            var response = new ServiceResult<bool>();

            string jobId = TryGetValue(request.Parameters.id, string.Empty);

            if (!string.IsNullOrEmpty(jobId))
            {
                await _jobScheduler.DeleteJob(jobId);

                response.Success = true;
                response.Result = true;
            }

            return response;
        }


        /// <summary>
        /// Pauses the scheduler jobs.
        /// </summary>
        private async Task<object> PauseJobs(IHttpRequest request)
        {
            var response = new ServiceResult<bool>();

            // Список уникальных идентификаторов заданий
            List<string> jobIds = TryGetValues(request.Query.ids);

            // Если список пустой, приостанавливаются все задания
            if (jobIds == null || jobIds.Count <= 0)
            {
                await _jobScheduler.PauseAllJobs();
            }
            else
            {
                await _jobScheduler.PauseJobs(jobIds);
            }

            response.Success = true;
            response.Result = true;

            return response;
        }

        /// <summary>
        /// Resumes the scheduler jobs.
        /// </summary>
        private async Task<object> ResumeJobs(IHttpRequest request)
        {
            var response = new ServiceResult<bool>();

            // Список уникальных идентификаторов заданий
            List<string> jobIds = TryGetValues(request.Query.ids);

            // Если список пустой, возобновляются все задания
            if (jobIds == null || jobIds.Count <= 0)
            {
                await _jobScheduler.ResumeAllJobs();
            }
            else
            {
                await _jobScheduler.ResumeJobs(jobIds);
            }

            response.Success = true;
            response.Result = true;

            return response;
        }

        /// <summary>
        /// Triggers the scheduler jobs.
        /// </summary>
        private async Task<object> TriggerJobs(IHttpRequest request)
        {
            var response = new ServiceResult<bool>();

            // Список уникальных идентификаторов заданий
            List<string> jobIds = TryGetValues(request.Query.ids);

            // Данные для выполнения задания
            var jobData = _jsonObjectSerializer.Deserialize<DynamicWrapper>(request.Content);

            // Если список пустой, досрочно вызываются все задания
            if (jobIds == null || jobIds.Count <= 0)
            {
                await _jobScheduler.TriggerAllJob(jobData);
            }
            else
            {
                await _jobScheduler.TriggerJobs(jobIds, jobData);
            }

            response.Success = true;
            response.Result = true;

            return response;
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


        private static TResult TryGetValue<TResult>(dynamic value, TResult defaultValue = default(TResult))
        {
            var result = defaultValue;

            if (value != null)
            {
                try
                {
                    result = (TResult)value;
                }
                catch
                {
                }
            }

            return result;
        }

        private static List<string> TryGetValues(dynamic value)
        {
            string result = null;

            if (value != null)
            {
                try
                {
                    result = (string)value;
                }
                catch
                {
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                return null;
            }

            return result.Split(',', ';')
                         .Select(i => i.Trim())
                         .Where(i => i.Length > 0)
                         .ToList();
        }


        // ReSharper disable UnusedAutoPropertyAccessor.Local

        private class JobInfo : IJobInfo
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public string Group { get; set; }

            public JobState State { get; set; }

            public string Description { get; set; }

            public string HandlerType { get; set; }

            public string CronExpression { get; set; }

            public DateTimeOffset? StartTimeUtc { get; set; }

            public DateTimeOffset? EndTimeUtc { get; set; }

            public JobMisfirePolicy MisfirePolicy { get; set; }

            public DynamicWrapper Data { get; set; }
        }

        // ReSharper restore UnusedAutoPropertyAccessor.Local
    }
}