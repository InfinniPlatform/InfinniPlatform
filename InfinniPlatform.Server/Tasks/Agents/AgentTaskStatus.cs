using System;

namespace InfinniPlatform.Server.Tasks.Agents
{
    /// <summary>
    /// Информация о выполнении задачи.
    /// </summary>
    public class AgentTaskStatus
    {
        /// <summary>
        /// Завершено ли выполнение задачи.
        /// </summary>
        public bool Completed { get; set; }

        /// <summary>
        /// Идентификатор задачи.
        /// </summary>
        public string TaskId { get; set; }

        /// <summary>
        /// Лог выполнения задачи.
        /// </summary>
        public string Output { get; set; }

        /// <summary>
        /// Лог выполнения задачи.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Лог выполнения задачи.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Время начала выполнения задачи.
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Время окончания выполнения задачи.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Форматированный лог выполнения задачи.
        /// </summary>
        public object FormattedOutput { get; set; }
    }
}