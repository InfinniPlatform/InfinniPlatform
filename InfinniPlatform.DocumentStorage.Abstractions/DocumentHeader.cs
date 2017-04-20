using System;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Заголовок документа.
    /// </summary>
    public class DocumentHeader
    {
        // ReSharper disable InconsistentNaming


        /// <summary>
        /// Идентификатор арендатора сервиса.
        /// </summary>
        public string _tenant { get; set; }


        /// <summary>
        /// Дата создания документа.
        /// </summary>
        public DateTime? _created { get; set; }

        /// <summary>
        /// Имя пользователя, который создал документ.
        /// </summary>
        public string _createUser { get; set; }

        /// <summary>
        /// Идентификатор пользователя, который создал документ.
        /// </summary>
        public string _createUserId { get; set; }


        /// <summary>
        /// Дата последнего изменения документа.
        /// </summary>
        public DateTime? _updated { get; set; }

        /// <summary>
        /// Имя пользователя, который последний изменил документ.
        /// </summary>
        public string _updateUser { get; set; }

        /// <summary>
        /// Идентификатор пользователя, который последний изменил документ.
        /// </summary>
        public string _updateUserId { get; set; }


        /// <summary>
        /// Дата удаления документа.
        /// </summary>
        public DateTime? _deleted { get; set; }

        /// <summary>
        /// Имя пользователя, который удалил документ.
        /// </summary>
        public string _deleteUser { get; set; }

        /// <summary>
        /// Идентификатор пользователя, который удалил документ.
        /// </summary>
        public string _deleteUserId { get; set; }


        // ReSharper restore InconsistentNaming
    }
}