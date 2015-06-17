using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Api.Metadata
{
    /// <summary>
    ///   Менеджер для работы с идентификаторами метаданных
    /// </summary>
    public interface IManagerIdentifiers
    {
        /// <summary>
        ///   Получить идентификатор элемента конфигурации
        /// </summary>
        /// <param name="version"></param>
        /// <param name="name">Наименование элемента</param>
        /// <returns>Идентификатор элемента</returns>
        string GetConfigurationUid(string version, string name);

        string GetDocumentUid(string version, string configurationId, string documentId);
    }
}
