using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InfinniPlatform.Dynamic;
using InfinniPlatform.Http;
using InfinniPlatform.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace InfinniPlatform.Scheduler
{
    /// <summary>
    /// Provides HTTP API to the scheduler.
    /// </summary>
    [Route(Name)]
    public class SchedulerController : Controller
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SchedulerController" />.
        /// </summary>
        /// <param name="jobScheduler">Job scheduler.</param>
        /// <param name="jsonObjectSerializer">JSON object serializer.</param>
        /// <param name="logger">Logger.</param>
        public SchedulerController(IJobScheduler jobScheduler,
                                   IJsonObjectSerializer jsonObjectSerializer,
                                   ILogger<SchedulerController> logger)
        {
            _jobScheduler = jobScheduler;
            _jsonObjectSerializer = jsonObjectSerializer;
            _logger = logger;
        }


        private readonly IJobScheduler _jobScheduler;
        private readonly IJsonObjectSerializer _jsonObjectSerializer;
        private readonly ILogger _logger;
        private const string Name = "scheduler";

        /// <summary>
        /// Returns scheduler status.
        /// </summary>
        [HttpGet("")]
        [HttpPost("")]
        public async Task<object> ProcessGetStatus()
        {
            return await HandleRequest(GetStatus);
        }

        /// <summary>
        /// Returns jobs list.
        /// </summary>
        [HttpGet("jobs")]
        [HttpPost("jobs")]
        public async Task<object> ProcessGetJobs()
        {
            return await HandleRequest(GetJobs);
        }

        /// <summary>
        /// Returns job by id.
        /// </summary>
        [HttpGet("jobs/{id}")]
        public async Task<object> ProcessGetJob()
        {
            return await HandleRequest(GetJob);
        }

        /// <summary>
        /// Saves job by id.
        /// </summary>
        [HttpPost("jobs/{id}")]
        public async Task<object> ProcessPostJob(string id)
        {
            return await HandleRequest(PostJob);
        }

        /// <summary>
        /// Deletes job by id.
        /// </summary>
        [HttpDelete("jobs/{id}")]
        public async Task<object> ProcessDeleteJob(string id)
        {
            return await HandleRequest(DeleteJob);
        }

        /// <summary>
        /// Pause jobs.
        /// </summary>
        [HttpPost("pause")]
        public async Task<object> ProcessPauseJobs()
        {
            return await HandleRequest(PauseJobs);
        }

        /// <summary>
        /// Resume jobs.
        /// </summary>
        [HttpPost("resume")]
        public async Task<object> ProcessResumeJobs()
        {
            return await HandleRequest(ResumeJobs);
        }

        /// <summary>
        /// Triggers jobs.
        /// </summary>
        [HttpPost("trigger")]
        public async Task<object> ProcessTriggerJobs()
        {
            return await HandleRequest(TriggerJobs);
        }


        /// <summary>
        /// Gets the scheduler status.
        /// </summary>
        private async Task<object> GetStatus()
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
                                            { "ref", $"{Request.Scheme}://{Request.Host}/{Name}/jobs?skip=0&take=10" }
                                        }
                             },
                             {
                                 "planned", new DynamicDocument
                                            {
                                                { "count", plannedCount },
                                                { "ref", $"{Request.Scheme}://{Request.Host}/{Name}/jobs?state=planned&skip=0&take=10" }
                                            }
                             },
                             {
                                 "paused", new DynamicDocument
                                           {
                                               { "count", pausedCount },
                                               { "ref", $"{Request.Scheme}://{Request.Host}/{Name}/jobs?state=paused&skip=0&take=10" }
                                           }
                             }
                         };

            return new ServiceResult<DynamicDocument> { Success = true, Result = status };
        }


        /// <summary>
        /// Gets the scheduler jobs.
        /// </summary>
        private async Task<object> GetJobs()
        {
            const int skipMin = 0;

            const int takeMin = 1;
            const int takeMax = 100;
            const int takeDefault = 10;
            
            var state = TryGetStringValue(Request.Query["state"]);
            var skip = Math.Max(TryGetIntValue(Request.Query["skip"]), skipMin);
            var take = Math.Max(Math.Min(TryGetIntValue(Request.Query["take"], takeDefault), takeMax), takeMin);

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

                                                         return i.Select(j => new DynamicDocument
                                                                              {
                                                                                  { "id", j.Info.Id },
                                                                                  { "ref", $"{Request.Scheme}://{Request.Host}/{Name}/jobs/{j.Info.Id}" }
                                                                              })
                                                                 .ToList();
                                                     });

            return new ServiceResult<List<DynamicDocument>> { Success = true, Result = jobs };
        }

        /// <summary>
        /// Gets the scheduler job info.
        /// </summary>
        private async Task<object> GetJob()
        {
            var response = new ServiceResult<IJobInfo>();

            string jobId = RouteData.Values["id"] as string ?? string.Empty;

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
        private async Task<object> PostJob()
        {
            var response = new ServiceResult<bool>();

            string jobId = RouteData.Values["id"] as string ?? string.Empty;

            if (!string.IsNullOrEmpty(jobId))
            {
                var jobInfo = _jsonObjectSerializer.Deserialize<JobInfo>(Request.Body);

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
        private async Task<object> DeleteJob()
        {
            var response = new ServiceResult<bool>();

            string jobId = RouteData.Values["id"] as string ?? string.Empty;

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
        private async Task<object> PauseJobs()
        {
            var response = new ServiceResult<bool>();

            // Список уникальных идентификаторов заданий
            var jobIds = TryGetValues(RouteData.Values["ids"]);

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
        private async Task<object> ResumeJobs()
        {
            var response = new ServiceResult<bool>();

            // Список уникальных идентификаторов заданий
            List<string> jobIds = TryGetValues(RouteData.Values["ids"]);

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
        private async Task<object> TriggerJobs()
        {
            var response = new ServiceResult<bool>();

            // Список уникальных идентификаторов заданий
            List<string> jobIds = TryGetValues(RouteData.Values["ids"]);

            // Данные для выполнения задания
            var jobData = _jsonObjectSerializer.Deserialize<DynamicDocument>(Request.Body);

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


        private async Task<object> HandleRequest(Func<Task<object>> requestHandler)
        {
            // TODO On before
            // Запрос статуса разрешен только с локального узла
            if (!HttpContext.IsLocal())
            {
                return Forbid();
            }

            try
            {
                return Json(await requestHandler());
            }
            catch (Exception exception)
            {
                _logger.LogError(exception);

                return new ServiceResult<object>
                {
                    Success = false,
                    Error = exception.GetFullMessage()
                };
            }
        }

        private static string TryGetStringValue(StringValues stringValues)
        {
            return stringValues.ToString();
        }

        private static int TryGetIntValue(StringValues stringValues, int defaultValue = default(int))
        {
            if (int.TryParse(stringValues.ToString(), out var value))
            {
                return value;
            }

            return defaultValue;
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
                    // ignored
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

            public DynamicDocument Data { get; set; }
        }

        // ReSharper restore UnusedAutoPropertyAccessor.Local
    }
}