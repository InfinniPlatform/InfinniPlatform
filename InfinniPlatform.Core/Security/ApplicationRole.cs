namespace InfinniPlatform.Core.Security
{
    /// <summary>
    /// Сведения о роли системы.
    /// </summary>
    public class ApplicationRole
    {
        /// <summary>
        /// Уникальный идентификатор роли.
        /// </summary>
        /// <remarks>
        /// Уникальный идентификатор или уникальное имя роли.
        /// </remarks>
        /// <example>
        /// EF112677-863F-4F63-AA06-87115D18BBDA
        /// </example>
        /// <example>
        /// ProjectManager
        /// </example>
        public string Id { get; set; }

        /// <summary>
        /// Наименование роли.
        /// </summary>
        /// <remarks>
        /// Как правило, человекочитаемое уникальное имя роли.
        /// </remarks>
        /// <example>
        /// ProjectManager
        /// </example>
        public string Name { get; set; }

        /// <summary>
        /// Заголовок роли.
        /// </summary>
        /// <remarks>
        /// Отображаемое имя роли.
        /// </remarks>
        /// <example>
        /// Менеджер проекта
        /// </example>
        public string Caption { get; set; }

        /// <summary>
        /// Описание роли.
        /// </summary>
        /// <remarks>
        /// Описание назначения роли.
        /// </remarks>
        /// <example>
        /// Доступны все операции по управлению проектом
        /// </example>
        public string Description { get; set; }
    }
}