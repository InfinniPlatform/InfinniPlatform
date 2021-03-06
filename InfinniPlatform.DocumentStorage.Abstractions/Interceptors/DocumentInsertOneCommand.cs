﻿using InfinniPlatform.Dynamic;

namespace InfinniPlatform.DocumentStorage.Interceptors
{
    /// <summary>
    /// Команда вставки одного документа в хранилище.
    /// </summary>
    public sealed class DocumentInsertOneCommand : IDocumentWriteCommand
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="document">Документ для вставки.</param>
        public DocumentInsertOneCommand(DynamicDocument document)
        {
            Document = document;
        }

        /// <summary>
        /// Документ для вставки.
        /// </summary>
        public DynamicDocument Document { get; set; }
    }


    /// <summary>
    /// Команда вставки одного документа в хранилище.
    /// </summary>
    public sealed class DocumentInsertOneCommand<TDocument> : IDocumentWriteCommand<TDocument>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="document">Документ для вставки.</param>
        public DocumentInsertOneCommand(TDocument document)
        {
            Document = document;
        }

        /// <summary>
        /// Документ для вставки.
        /// </summary>
        public TDocument Document { get; set; }
    }
}