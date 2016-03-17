using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Sdk.Documents.Services
{
    /// <summary>
    /// Обработчик для сервиса по работе с документами.
    /// </summary>
    public interface IDocumentHttpServiceHandlerBase
    {
        /// <summary>
        /// Имя типа документа.
        /// </summary>
        string DocumentType { get; }


        /// <summary>
        /// Разрешено ли получение документов.
        /// </summary>
        bool CanGet { get; }

        /// <summary>
        /// Разрешено ли сохранение документов.
        /// </summary>
        bool CanPost { get; }

        /// <summary>
        /// Разрешено ли удаление документов.
        /// </summary>
        bool CanDelete { get; }


        /// <summary>
        /// Загружает модуль.
        /// </summary>
        /// <param name="builder">Регистратор обработчиков запросов.</param>
        void Load(IHttpServiceBuilder builder);
    }
}