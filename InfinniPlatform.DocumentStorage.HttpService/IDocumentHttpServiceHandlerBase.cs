using System;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Document HTTP service handler.Обработчик для сервиса по работе с документами.
    /// </summary>
    public interface IDocumentHttpServiceHandlerBase
    {
        /// <summary>
        /// Document type name.
        /// </summary>
        string DocumentType { get; }


        /// <summary>
        /// Flag indicating if service can work with documents in "system" tenant.
        /// </summary>
        bool AsSystem { get; }


        /// <summary>
        /// Flag indicating if GET method is allowed.
        /// </summary>
        bool CanGet { get; }

        /// <summary>
        /// Flag indicating if POST method is allowed.
        /// </summary>
        bool CanPost { get; }

        /// <summary>
        /// Flag indicating if DELETE method is allowed.
        /// </summary>
        bool CanDelete { get; }


        /// <summary>
        /// Action on exception raise.
        /// </summary>
        /// <param name="exception">Exception.</param>
        /// <returns>Erro message.</returns>
        string OnError(Exception exception);
    }
}