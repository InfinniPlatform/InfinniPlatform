using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinniPlatform.Scheduler
{
    /// <summary>
    /// Предоставляет механизм для хранения состояния планировщика <see cref="IJobScheduler"/>.
    /// </summary>
    public interface IJobSchedulerRepository
    {
        /// <summary>
        /// Возвращает список с информацией об актуальных заданиях.
        /// </summary>
        Task<IEnumerable<JobInfo>> GetActualJobInfos();


        /// <summary>
        /// Добавляет или обновляет задание.
        /// </summary>
        /// <param name="jobInfo">Информация о задании.</param>
        Task AddOrUpdateJob(JobInfo jobInfo);

        /// <summary>
        /// Добавляет или обновляет список заданий.
        /// </summary>
        /// <param name="jobInfos">Список с информацией о заданиях.</param>
        Task AddOrUpdateJobs(IEnumerable<JobInfo> jobInfos);


        /// <summary>
        /// Удаляет указанное задание.
        /// </summary>
        /// <param name="jobId">Уникальный идентификатор задания.</param>
        Task DeleteJob(string jobId);

        /// <summary>
        /// Удаляет указанные задания.
        /// </summary>
        /// <param name="jobIds">Список с уникальными идентификаторами заданий.</param>
        Task DeleteJobs(IEnumerable<string> jobIds);

        /// <summary>
        /// Удаляет все задания.
        /// </summary>
        Task DeleteAllJobs();


        /// <summary>
        /// Приостанавливает планирование указанного задания.
        /// </summary>
        /// <param name="jobId">Уникальный идентификатор задания.</param>
        Task PauseJob(string jobId);

        /// <summary>
        /// Приостанавливает планирование указанных заданий.
        /// </summary>
        /// <param name="jobIds">Список с уникальными идентификаторами заданий.</param>
        Task PauseJobs(IEnumerable<string> jobIds);

        /// <summary>
        /// Приостанавливает планирование всех заданий.
        /// </summary>
        Task PauseAllJobs();


        /// <summary>
        /// Возобновляет планирование указанного задания.
        /// </summary>
        /// <param name="jobId">Уникальный идентификатор задания.</param>
        Task ResumeJob(string jobId);

        /// <summary>
        /// Возобновляет планирование указанных заданий.
        /// </summary>
        /// <param name="jobIds">Список с уникальными идентификаторами заданий.</param>
        Task ResumeJobs(IEnumerable<string> jobIds);

        /// <summary>
        /// Возобновляет планирование всех заданий.
        /// </summary>
        Task ResumeAllJobs();


        /// <summary>
        /// Проверяет, был ли обработан экземпляр указанного задания.
        /// </summary>
        /// <param name="jobInstance">Уникальный идентификатор экземпляра задания.</param>
        /// <remarks>
        /// Поскольку каждый узел кластера осуществляет планирование заданий, то вполне вероятно, что
        /// событие о необходимости выполнения очередного задания на каждом узле возникнет примерно
        /// в одно и то же время. Данный интерфейс предоставляет метод, который позволяет проверить,
        /// что обработка указанного экземпляра задания осуществляется в первый раз.
        /// </remarks>
        Task<bool> IsHandledJob(string jobInstance);
    }
}