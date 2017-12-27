using System;
using System.Threading.Tasks;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Обработчик для сервиса по работе с документами.
    /// </summary>
    public class DocumentHttpServiceHandler : IDocumentHttpServiceHandler
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DocumentHttpServiceHandler" />.
        /// </summary>
        /// <param name="documentType">Document type name.</param>
        public DocumentHttpServiceHandler(string documentType)
        {
            if (string.IsNullOrEmpty(documentType))
            {
                throw new ArgumentNullException(nameof(documentType));
            }

            DocumentType = documentType;
        }


        /// <inheritdoc />
        public string DocumentType { get; }


        /// <inheritdoc />
        public bool AsSystem { get; set; }


        /// <inheritdoc />
        public virtual bool CanGet { get; set; } = true;

        /// <inheritdoc />
        public virtual Task<DocumentGetQueryResult> OnBeforeGet(DocumentGetQuery query)
        {
            return Task.FromResult<DocumentGetQueryResult>(null);
        }

        /// <inheritdoc />
        public virtual Task OnAfterGet(DocumentGetQuery query, DocumentGetQueryResult result, Exception exception)
        {
            return Task.FromResult<object>(null);
        }


        /// <inheritdoc />
        public virtual bool CanPost { get; set; } = true;

        /// <inheritdoc />
        public virtual Task<DocumentPostQueryResult> OnBeforePost(DocumentPostQuery query)
        {
            return Task.FromResult<DocumentPostQueryResult>(null);
        }

        /// <inheritdoc />
        public virtual Task OnAfterPost(DocumentPostQuery query, DocumentPostQueryResult result, Exception exception)
        {
            return Task.FromResult<object>(null);
        }


        /// <inheritdoc />
        public virtual bool CanDelete { get; set; } = true;

        /// <inheritdoc />
        public virtual Task<DocumentDeleteQueryResult> OnBeforeDelete(DocumentDeleteQuery query)
        {
            return Task.FromResult<DocumentDeleteQueryResult>(null);
        }

        /// <inheritdoc />
        public virtual Task OnAfterDelete(DocumentDeleteQuery query, DocumentDeleteQueryResult result, Exception exception)
        {
            return Task.FromResult<object>(null);
        }


        /// <inheritdoc />
        public string OnError(Exception exception)
        {
            return null;
        }
    }


    /// <summary>
    /// Обработчик по умолчанию для сервиса по работе с документами.
    /// </summary>
    /// <typeparam name="TDocument">Тип документа.</typeparam>
    public class DocumentHttpServiceHandler<TDocument> : IDocumentHttpServiceHandler<TDocument> where TDocument : Document
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DocumentHttpServiceHandler{TDocument}" />.
        /// </summary>
        /// <param name="documentType"></param>
        public DocumentHttpServiceHandler(string documentType = null)
        {
            DocumentType = documentType ?? DocumentStorageExtensions.GetDefaultDocumentTypeName<TDocument>();
        }


        /// <inheritdoc />
        public string DocumentType { get; }


        /// <inheritdoc />
        public bool AsSystem { get; set; }


        /// <inheritdoc />
        public virtual bool CanGet { get; set; } = true;

        /// <inheritdoc />
        public virtual Task<DocumentGetQueryResult> OnBeforeGet(DocumentGetQuery<TDocument> query)
        {
            return Task.FromResult<DocumentGetQueryResult>(null);
        }

        /// <inheritdoc />
        public virtual Task OnAfterGet(DocumentGetQuery<TDocument> query, DocumentGetQueryResult result, Exception exception)
        {
            return Task.FromResult<object>(null);
        }


        /// <inheritdoc />
        public virtual bool CanPost { get; set; } = true;

        /// <inheritdoc />
        public virtual Task<DocumentPostQueryResult> OnBeforePost(DocumentPostQuery<TDocument> query)
        {
            return Task.FromResult<DocumentPostQueryResult>(null);
        }

        /// <inheritdoc />
        public virtual Task OnAfterPost(DocumentPostQuery<TDocument> query, DocumentPostQueryResult result, Exception exception)
        {
            return Task.FromResult<object>(null);
        }


        /// <inheritdoc />
        public virtual bool CanDelete { get; set; } = true;

        /// <inheritdoc />
        public virtual Task<DocumentDeleteQueryResult> OnBeforeDelete(DocumentDeleteQuery<TDocument> query)
        {
            return Task.FromResult<DocumentDeleteQueryResult>(null);
        }

        /// <inheritdoc />
        public virtual Task OnAfterDelete(DocumentDeleteQuery<TDocument> query, DocumentDeleteQueryResult result, Exception exception)
        {
            return Task.FromResult<object>(null);
        }


        /// <inheritdoc />
        public string OnError(Exception exception)
        {
            return null;
        }
    }
}