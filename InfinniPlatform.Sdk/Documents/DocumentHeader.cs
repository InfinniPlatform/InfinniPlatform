using System;

namespace InfinniPlatform.Sdk.Documents
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
        public string _tenant { get; internal set; }


        /// <summary>
        /// Дата создания документа.
        /// </summary>
        public DateTime? _created { get; internal set; }

        /// <summary>
        /// Имя пользователя, который создал документ.
        /// </summary>
        public string _createUser { get; internal set; }

        /// <summary>
        /// Идентификатор пользователя, который создал документ.
        /// </summary>
        public string _createUserId { get; internal set; }


        /// <summary>
        /// Дата последнего изменения документа.
        /// </summary>
        public DateTime? _updated { get; internal set; }

        /// <summary>
        /// Имя пользователя, который последний изменил документ.
        /// </summary>
        public string _updateUser { get; internal set; }

        /// <summary>
        /// Идентификатор пользователя, который последний изменил документ.
        /// </summary>
        public string _updateUserId { get; internal set; }


        /// <summary>
        /// Дата удаления документа.
        /// </summary>
        public DateTime? _deleted { get; internal set; }

        /// <summary>
        /// Имя пользователя, который удалил документ.
        /// </summary>
        public string _deleteUser { get; internal set; }

        /// <summary>
        /// Идентификатор пользователя, который удалил документ.
        /// </summary>
        public string _deleteUserId { get; internal set; }


        // ReSharper restore InconsistentNaming
    }
}