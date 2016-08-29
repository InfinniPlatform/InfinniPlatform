namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// Уникальный идентификатор задания.
    /// </summary>
    public class JobKey
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public JobKey()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="name">Имя задания.</param>
        /// <param name="group">Группа задания.</param>
        public JobKey(string name, string group)
        {
            Name = name;
            Group = group;
        }


        /// <summary>
        /// Имя задания.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Группа задания.
        /// </summary>
        public string Group { get; set; }
    }
}