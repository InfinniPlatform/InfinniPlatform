using System;
using System.Linq.Expressions;

using InfinniPlatform.Core.Abstractions.Dynamic;

namespace InfinniPlatform.DocumentStorage.Abstractions.Interceptors
{
    /// <summary>
    /// Команда замены одного документа в хранилище.
    /// </summary>
    public sealed class DocumentReplaceOneCommand : IDocumentWriteCommand
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="replacement">Документ замены.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <param name="insertIfNotExists">Следует ли создать документ, если ничего не найдено.</param>
        public DocumentReplaceOneCommand(DynamicWrapper replacement, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false)
        {
            Replacement = replacement;
            Filter = filter;
            InsertIfNotExists = insertIfNotExists;
        }

        /// <summary>
        /// Документ замены.
        /// </summary>
        public DynamicWrapper Replacement { get; set; }

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
    /// Команда замены одного документа в хранилище.
    /// </summary>
    public sealed class DocumentReplaceOneCommand<TDocument> : IDocumentWriteCommand<TDocument>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="replacement">Документ замены.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <param name="insertIfNotExists">Следует ли создать документ, если ничего не найдено.</param>
        public DocumentReplaceOneCommand(TDocument replacement, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false)
        {
            Replacement = replacement;
            Filter = filter;
            InsertIfNotExists = insertIfNotExists;
        }

        /// <summary>
        /// Документ замены.
        /// </summary>
        public TDocument Replacement { get; set; }

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