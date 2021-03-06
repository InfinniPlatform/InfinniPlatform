﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.Dynamic;

namespace InfinniPlatform.Scheduler
{
    /// <summary>
    /// Диспетчер планировщика заданий <see cref="IJobScheduler"/>.
    /// </summary>
    internal interface IJobSchedulerDispatcher
    {
        /// <summary>
        /// Определяет, запущено ли планирование заданий.
        /// </summary>
        Task<bool> IsStarted();

        /// <summary>
        /// Запускает планирование заданий.
        /// </summary>
        Task Start();

        /// <summary>
        /// Останавливает планирование заданий.
        /// </summary>
        Task Stop();


        /// <summary>
        /// Позволяет сделать выборку для определения статуса заданий.
        /// </summary>
        /// <typeparam name="TResult">Тип результата.</typeparam>
        /// <param name="selector">Функция для выборки результата.</param>
        Task<TResult> GetStatus<TResult>(Func<IEnumerable<IJobStatus>, TResult> selector);


        /// <summary>
        /// Добавляет или обновляет задание.
        /// </summary>
        /// <param name="jobInfo">Информация о задании.</param>
        Task AddOrUpdateJob(IJobInfo jobInfo);

        /// <summary>
        /// Добавляет или обновляет список заданий.
        /// </summary>
        /// <param name="jobInfos">Список с информацией о заданиях.</param>
        Task AddOrUpdateJobs(IEnumerable<IJobInfo> jobInfos);


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
        /// Вызывает досрочное выполнение указанного задания.
        /// </summary>
        /// <param name="jobId">Уникальный идентификатор задания.</param>
        /// <param name="data">Данные для выполнения задания.</param>
        Task TriggerJob(string jobId, DynamicDocument data = null);

        /// <summary>
        /// Вызывает досрочное выполнение указанных заданий.
        /// </summary>
        /// <param name="jobIds">Список с уникальными идентификаторами заданий.</param>
        /// <param name="data">Данные для выполнения заданий.</param>
        Task TriggerJobs(IEnumerable<string> jobIds, DynamicDocument data = null);

        /// <summary>
        /// Вызывает досрочное выполнение всех заданий.
        /// </summary>
        /// <param name="data">Данные для выполнения заданий.</param>
        Task TriggerAllJob(DynamicDocument data = null);
    }
}