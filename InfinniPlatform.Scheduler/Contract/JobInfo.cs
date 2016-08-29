using System;

using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// Информация о задании.
    /// </summary>
    [DocumentType("Scheduler.JobInfo")]
    public class JobInfo : Document
    {
        /// <summary>
        /// Имя группы по умолчанию.
        /// </summary>
        public const string DefaultGroup = "Default";


        /// <summary>
        /// Конструктор.
        /// </summary>
        public JobInfo()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="name">Имя задания.</param>
        /// <param name="group">Группа задания.</param>
        /// <param name="handlerType">Тип обработчика задания.</param>
        public JobInfo(string name, string group, string handlerType)
        {
            Key = new JobKey(name, group);
            HandlerType = handlerType;
        }


        /// <summary>
        /// Уникальный идентификатор задания.
        /// </summary>
        public JobKey Key { get; set; }

        /// <summary>
        /// Состояние выполнения задания.
        /// </summary>
        public JobState State { get; set; }

        /// <summary>
        /// Описание назначения задания.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Тип обработчика задания.
        /// </summary>
        public string HandlerType { get; set; }

        /// <summary>
        /// CRON-выражение планирования задания.
        /// </summary>
        public string CronExpression { get; set; }

        /// <summary>
        /// Время начала планирования задания.
        /// </summary>
        public DateTimeOffset? StartTimeUtc { get; set; }

        /// <summary>
        /// Время окончания планирования задания.
        /// </summary>
        public DateTimeOffset? EndTimeUtc { get; set; }

        /// <summary>
        /// Политика обработки пропущенных срабатываний задания.
        /// </summary>
        public JobMisfirePolicy? MisfirePolicy { get; set; }

        /// <summary>
        /// Данные для выполнения задания.
        /// </summary>
        public object Data { get; set; }
    }
}