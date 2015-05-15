using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Api.ModelRepository
{
    /// <summary>
    /// Сервис для получения моделей документа
    /// </summary>
    public interface IDocumentModelProvider
    {
        /// <summary>
        /// Возвращает модель документа по его идентификатору. 
        /// Ссылки на все архетипы в возвращаемом документе разрешены 
        /// </summary>
        JObject GetDocumentMetadata(string documentId);
    }
}
