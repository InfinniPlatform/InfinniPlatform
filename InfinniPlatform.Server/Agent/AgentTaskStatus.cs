namespace InfinniPlatform.Server.Agent
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
        /// Форматированный лог выполнения задачи.
        /// </summary>
        public object FormattedOutput { get; set; }
    }
}