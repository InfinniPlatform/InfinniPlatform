using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// Планировщик заданий.
    /// </summary>
    public interface IJobScheduler
    {
        /// <summary>
        /// Добавляет задание.
        /// </summary>
        /// <param name="jobInfo">Информация о задании.</param>
        Task AddJob(JobInfo jobInfo);

        /// <summary>
        /// Добавляет список заданий.
        /// </summary>
        /// <param name="jobInfos">Список с информацией о заданиях.</param>
        Task AddJobs(IEnumerable<JobInfo> jobInfos);


        /// <summary>
        /// Удаляет указанное задание.
        /// </summary>
        /// <param name="jobKey">Уникальный идентификатор задания.</param>
        Task DeleteJob(JobKey jobKey);

        /// <summary>
        /// Удаляет указанные задания.
        /// </summary>
        /// <param name="jobKeys">Список с уникальными идентификаторами заданий.</param>
        Task DeleteJobs(IEnumerable<JobKey> jobKeys);

        /// <summary>
        /// Удаляет все задания.
        /// </summary>
        Task DeleteAllJobs();


        /// <summary>
        /// Возвращает список уникальных идентификаторов заданий.
        /// </summary>
        /// <param name="group">Регулярное выражение для поиска заданий по группе.</param>
        Task<IEnumerable<JobKey>> GetJobKeys(Regex group);

        /// <summary>
        /// Возвращает информацию о задании.
        /// </summary>
        /// <param name="jobKey">Уникальный идентификатор задания.</param>
        Task<JobInfo> GetJobInfo(JobKey jobKey);


        /// <summary>
        /// Приостанавливает планирование указанного задания.
        /// </summary>
        /// <param name="jobKey">Уникальный идентификатор задания.</param>
        Task PauseJob(JobKey jobKey);

        /// <summary>
        /// Приостанавливает планирование указанных заданий.
        /// </summary>
        /// <param name="jobKeys">Список с уникальными идентификаторами заданий.</param>
        Task PauseJobs(IEnumerable<JobKey> jobKeys);

        /// <summary>
        /// Приостанавливает планирование всех заданий.
        /// </summary>
        Task PauseAllJobs();


        /// <summary>
        /// Возобновляет планирование указанного задания.
        /// </summary>
        /// <param name="jobKey">Уникальный идентификатор задания.</param>
        Task ResumeJob(JobKey jobKey);

        /// <summary>
        /// Возобновляет планирование указанных заданий.
        /// </summary>
        /// <param name="jobKeys">Список с уникальными идентификаторами заданий.</param>
        Task ResumeJobs(IEnumerable<JobKey> jobKeys);

        /// <summary>
        /// Возобновляет планирование всех заданий.
        /// </summary>
        Task ResumeAllJobs();


        /// <summary>
        /// Вызывает досрочное выполнение указанного задания.
        /// </summary>
        /// <param name="jobKey">Уникальный идентификатор задания.</param>
        /// <param name="data">Данные для выполнения задания.</param>
        Task TriggerJob(JobKey jobKey, object data = null);

        /// <summary>
        /// Вызывает досрочное выполнение указанных заданий.
        /// </summary>
        /// <param name="jobKeys">Список с уникальными идентификаторами заданий.</param>
        /// <param name="data">Данные для выполнения заданий.</param>
        Task TriggerJobs(IEnumerable<JobKey> jobKeys, object data = null);

        /// <summary>
        /// Вызывает досрочное выполнение всех заданий.
        /// </summary>
        /// <param name="data">Данные для выполнения заданий.</param>
        Task TriggerAllJob(object data = null);
    }
}