using System.Collections.Generic;
using System.IO;

using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.Api.RestQuery
{
    public delegate IRestQueryBuilder RestQueryBuilderFactory(string configuration, string documentType, string action);

    /// <summary>
    /// Конструктор запросов к REST сервисам платформы.
    /// </summary>
    public interface IRestQueryBuilder
    {
        /// <summary>
        /// Сформировать и выполнить запрос на применение изменений.
        /// </summary>
        /// <param name="id">Идентификатор объекта, который необходимо изменить.</param>
        /// <param name="changesObject">Объект, из которого будет сформирован список изменений.</param>
        /// <returns>Ответ на вызов сервиса.</returns>
        RestQueryResponse QueryPost(string id, object changesObject);

        /// <summary>
        /// Сформировать и выполнить запрос на поиск данных.
        /// </summary>
        /// <param name="filterObject">Фильтр по данным.</param>
        /// <param name="pageNumber">Номер страницы данных.</param>
        /// <param name="pageSize">Размер страницы.</param>
        /// <param name="searchType">Тип поиска записей.</param>
        /// <returns>Ответ на вызов сервиса.</returns>
        RestQueryResponse QueryGet(IEnumerable<object> filterObject, int pageNumber, int pageSize, int searchType = 1);

        /// <summary>
        /// Выгрузить файл на сервер.
        /// </summary>
        /// <param name="linkedData">Связанный объект.</param>
        /// <param name="filePath">Путь к файлу.</param>
        /// <returns>Ответ на вызов сервиса.</returns>
        RestQueryResponse QueryPostFile(object linkedData, string filePath);

        /// <summary>
        /// Выгрузить файл из указанного потока на сервер.
        /// </summary>
        /// <param name="linkedData">Связанный информационный объект.</param>
        /// <param name="file">Файловый поток.</param>
        /// <returns>Ответ на вызов сервиса.</returns>
        RestQueryResponse QueryPostFile(object linkedData, Stream file);

        /// <summary>
        /// Сформировать и выполнить запрос на агрегацию данных.
        /// </summary>
        /// <param name="aggregationConfiguration">Конфигурация для выполнения агрегации.</param>
        /// <param name="aggregationMetadata">Метаданные для выполнения агрегации.</param>
        /// <param name="filterObject">Фильтр записей.</param>
        /// <param name="dimensions">Описание срезов куба.</param>
        /// <param name="aggregationTypes">Тип агрегации.</param>
        /// <param name="aggregationFields">Имя поля, по которому необходимо рассчитать значение агрегации.</param>
        /// <param name="pageNumber">Номер страницы данных.</param>
        /// <param name="pageSize">Размер страницы.</param>
        /// <returns>Ответ на вызов сервиса.</returns>
        RestQueryResponse QueryAggregation(string aggregationConfiguration, string aggregationMetadata,
                                IEnumerable<object> filterObject, IEnumerable<object> dimensions,
                                IEnumerable<AggregationType> aggregationTypes, IEnumerable<string> aggregationFields, int pageNumber,
                                int pageSize);

        /// <summary>
        /// Сформировать и выполнить запрос на системную нотификацию.
        /// </summary>
        /// <returns>Ответ на вызов сервиса.</returns>
        RestQueryResponse QueryNotify(string metadataConfigurationId);

        /// <summary>
        /// Сформировать и выполнить запрос на применение изменений, представленных в виде JSON-объекта.
        /// </summary>
        /// <param name="id">Идентификатор объекта, к которому будут применены изменения.</param>
        /// <param name="jsonObject">Объект, который будет представлен в виде событий.</param>
        /// <returns>Ответ сервиса.</returns>
        RestQueryResponse QueryPostJson(string id, object jsonObject);

        /// <summary>
        /// Выполнить POST-запрос на сервер для UrlEncodedData.
        /// </summary>
        /// <param name="linkedData">Связанный объект.</param>
        /// <returns>Ответ на вызов сервиса.</returns>
        RestQueryResponse QueryPostUrlEncodedData(object linkedData);

        /// <summary>
        /// Выполнить GET запрос на сервер для UrlEncodedData.
        /// </summary>
        /// <param name="linkedData">Связанный объект.</param>
        /// <returns>Ответ на вызов сервиса.</returns>
        RestQueryResponse QueryGetUrlEncodedData(object linkedData);
    }
}