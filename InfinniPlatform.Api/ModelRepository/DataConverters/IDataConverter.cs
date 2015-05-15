using InfinniPlatform.Api.ModelRepository.MetadataObjectModel;

using Newtonsoft.Json.Linq;

using System.Collections.Generic;
using System.IO;

namespace InfinniPlatform.Api.ModelRepository.DataConverters
{
    /// <summary>
    /// Сервис для преобразования произвольного структурированного документа
    /// в JSON документ определенной структуры, соответствующей схеме документа 
    /// </summary>
    public interface IDataConverter
    {
        /// <summary>
        /// Преобразует блок данных в JSON документ определенной структуры 
        /// </summary>
        /// <param name="sourceData">Исходные данные для конвертации</param>
        /// <param name="documentModel">Структура получаемого документа</param>
        /// <param name="inlineDocuments">Набор схем встроенных документов</param>
        /// <param name="errorMessage">Ошибки, возникшие в ходе преобразования</param>
        /// <param name="extractedData">Преобразованный документ</param>
        /// <returns>Результат конвертации, если true - конвертация
        /// завершилась успешно, false - нет (ошибки содержатся в поле errorMessage)</returns>
        bool ConvertData(
            Stream sourceData,
            DataSchema documentModel,
            IDictionary<string, DataSchema> inlineDocuments,
            out string errorMessage,
            out JObject extractedData);
    }
}
