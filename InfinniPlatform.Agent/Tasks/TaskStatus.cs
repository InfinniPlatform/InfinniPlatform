namespace InfinniPlatform.Agent.Tasks
{
    /// <summary>
    /// Информация о выполнении задачи.
    /// </summary>
    public class TaskStatus
    {
        /// <summary>
        /// Завершено ли выполнение задачи.
        /// </summary>
        public bool? Completed { get; set; }

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
    }
}