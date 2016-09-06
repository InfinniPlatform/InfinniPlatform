using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InfinniPlatform.Scheduler.Common;
using InfinniPlatform.Scheduler.Contract;
using InfinniPlatform.Sdk.Diagnostics;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Scheduler.Diagnostics
{
    /// <summary>
    /// Предоставляет информацию о состоянии планировщика заданий.
    /// </summary>
    internal class SchedulerStatusProvider : ISubsystemStatusProvider, IHttpService
    {
        public SchedulerStatusProvider(IJobScheduler jobScheduler,
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

            builder.OnBefore = r =>
                               {
                                   // Запрос статуса разрешен только с локального узла
                                   if (!_hostAddressParser.IsLocalAddress(r.UserHostAddress))
                                   {
                                       return Task.FromResult<object>(HttpResponse.Forbidden);
                                   }

                                   return Task.FromResult<object>(null);
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
        /// Возвращает состояние планировщика заданий.
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


        /// <summary>
        /// Возвращает список заданий планировщика.
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
        /// Возвращает информацию о задании планировщика.
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
        /// Добавляет или обновляет задание планировщика.
        /// </summary>
        private async Task<object> PostJob(IHttpRequest request)
        {
            var response = new ServiceResult<bool>();

            string jobId = TryGetValue(request.Parameters.id, string.Empty);

            if (!string.IsNullOrEmpty(jobId))
            {
                var jobInfo = _jsonObjectSerializer.ConvertFromDynamic<JobInfo>((object)request.Form);

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
        /// Удаляет задание планировщика.
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
        /// Приостанавливает планирование указанных заданий планировщика.
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
        /// Возобновляет планирование заданий указанных планировщика.
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
        /// Вызывает досрочное выполнение указанных заданий планировщика.
        /// </summary>
        private async Task<object> TriggerJobs(IHttpRequest request)
        {
            var response = new ServiceResult<bool>();

            // Список уникальных идентификаторов заданий
            List<string> jobIds = TryGetValues(request.Query.ids);

            // Данные для выполнения задания
            var jobData = _jsonObjectSerializer.ConvertFromDynamic<DynamicWrapper>((object)request.Form);

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
    }
}