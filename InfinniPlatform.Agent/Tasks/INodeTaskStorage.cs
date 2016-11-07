namespace InfinniPlatform.Agent.Tasks
{
    /// <summary>
    /// Обеспечивает доступ к хранилищу статуса задач агента.
    /// </summary>
    public interface INodeTaskStorage
    {
        /// <summary>
        /// Создает новую задачу.
        /// </summary>
        /// <returns>Идентификатор задачи.</returns>
        string AddNewTask();

        /// <summary>
        /// Удаляет задачу.
        /// </summary>
        /// <param name="taskId">Идентификатор задачи.</param>
        void RemoveTask(string taskId);

        /// <summary>
        /// Возвращает статус выполнения задачи.
        /// </summary>
        /// <param name="taskId">Идентификатор задачи.</param>
        TaskStatus GetTaskStatus(string taskId);

        /// <summary>
        /// Добавляет сообщение в лог выполнения задачи.
        /// </summary>
        /// <param name="taskId">Идентификатор задачи.</param>
        /// <param name="output">Сообщение.</param>
        void AddOutput(string taskId, string output);

        /// <summary>
        /// Помечает задачу как выполненную.
        /// </summary>
        /// <param name="taskId">Идентификатор задачи.</param>
        void SetCompleted(string taskId);
    }
}