using System;
using System.Linq.Expressions;

namespace InfinniPlatform.DocumentStorage.Interceptors
{
    /// <summary>
    /// Команда обновления одного документа в хранилище.
    /// </summary>
    public sealed class DocumentUpdateOneCommand : IDocumentWriteCommand
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="update">Оператор обновления документов.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <param name="insertIfNotExists">Следует ли создать документ, если ничего не найдено.</param>
        public DocumentUpdateOneCommand(Action<IDocumentUpdateBuilder> update, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false)
        {
            Update = update;
            Filter = filter;
            InsertIfNotExists = insertIfNotExists;
        }

        /// <summary>
        /// Оператор обновления документов.
        /// </summary>
        public Action<IDocumentUpdateBuilder> Update { get; set; }

        /// <summary>
        /// Фильтр для поиска документов.
        /// </summary>
        public Func<IDocumentFilterBuilder, object> Filter { get; set; }

        /// <summary>
        /// Следует ли создать документ, если ничего не найдено.
        /// </summary>
        public bool InsertIfNotExists { get; set; }
    }


    /// <summary>
    /// Команда обновления одного документа в хранилище.
    /// </summary>
    public sealed class DocumentUpdateOneCommand<TDocument> : IDocumentWriteCommand<TDocument>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="update">Оператор обновления документов.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <param name="insertIfNotExists">Следует ли создать документ, если ничего не найдено.</param>
        public DocumentUpdateOneCommand(Action<IDocumentUpdateBuilder<TDocument>> update, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false)
        {
            Update = update;
            Filter = filter;
            InsertIfNotExists = insertIfNotExists;
        }

        /// <summary>
        /// Оператор обновления документов.
        /// </summary>
        public Action<IDocumentUpdateBuilder<TDocument>> Update { get; set; }

        /// <summary>
        /// Фильтр для поиска документов.
        /// </summary>
        public Expression<Func<TDocument, bool>> Filter { get; set; }

        /// <summary>
        /// Следует ли создать документ, если ничего не найдено.
        /// </summary>
        public bool InsertIfNotExists { get; set; }
    }
}