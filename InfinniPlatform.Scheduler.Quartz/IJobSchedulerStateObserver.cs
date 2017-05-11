using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinniPlatform.Scheduler
{
    /// <summary>
    /// Предоставляет механизм для получения уведомлений об изменении состояния планировщика <see cref="IJobScheduler"/>.
    /// </summary>
    public interface IJobSchedulerStateObserver
    {
        /// <summary>
        /// Вызывается при добавлении или обновлении задания.
        /// </summary>
        /// <param name="jobInfo">Информация о задании.</param>
        Task OnAddOrUpdateJob(JobInfo jobInfo);

        /// <summary>
        /// Вызывается при добавлении или обновлении списка заданий.
        /// </summary>
        /// <param name="jobInfos">Список с информацией о заданиях.</param>
        Task OnAddOrUpdateJobs(IEnumerable<JobInfo> jobInfos);


        /// <summary>
        /// Вызывается при удалении указанного задания.
        /// </summary>
        /// <param name="jobId">Уникальный идентификатор задания.</param>
        Task OnDeleteJob(string jobId);

        /// <summary>
        /// Вызывается при удалении указанных заданий.
        /// </summary>
        /// <param name="jobIds">Список с уникальными идентификаторами заданий.</param>
        Task OnDeleteJobs(IEnumerable<string> jobIds);

        /// <summary>
        /// Вызывается при удалении всех заданий.
        /// </summary>
        Task OnDeleteAllJobs();


        /// <summary>
        /// Вызывается при приостановке планирования указанного задания.
        /// </summary>
        /// <param name="jobId">Уникальный идентификатор задания.</param>
        Task OnPauseJob(string jobId);

        /// <summary>
        /// Вызывается при приостановке планирования указанных заданий.
        /// </summary>
        /// <param name="jobIds">Список с уникальными идентификаторами заданий.</param>
        Task OnPauseJobs(IEnumerable<string> jobIds);

        /// <summary>
        /// Вызывается при приостановке планирования всех заданий.
        /// </summary>
        Task OnPauseAllJobs();


        /// <summary>
        /// Вызывается при возобновлении планирования указанного задания.
        /// </summary>
        /// <param name="jobId">Уникальный идентификатор задания.</param>
        Task OnResumeJob(string jobId);

        /// <summary>
        /// Вызывается при возобновлении планирования указанных заданий.
        /// </summary>
        /// <param name="jobIds">Список с уникальными идентификаторами заданий.</param>
        Task OnResumeJobs(IEnumerable<string> jobIds);

        /// <summary>
        /// Вызывается при возобновлении планирования всех заданий.
        /// </summary>
        Task OnResumeAllJobs();


        /// <summary>
        /// Вызывается при выполнении задания.
        /// </summary>
        /// <param name="jobInfo">Информация о задании.</param>
        /// <param name="jobHandlerContext">Контекст обработки задания.</param>
        Task OnExecuteJob(JobInfo jobInfo, JobHandlerContext jobHandlerContext);
    }
}