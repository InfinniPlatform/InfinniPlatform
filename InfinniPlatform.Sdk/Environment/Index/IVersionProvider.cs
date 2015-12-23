using System.Collections.Generic;

namespace InfinniPlatform.Sdk.Environment.Index
{
    /// <summary>
    ///     Стратегия хранения, получения и обновления истории объектов индекса
    /// </summary>
    public interface IVersionProvider
    {
        /// <summary>
        ///     Получить актуальные версии объектов, отсортированные по дате вставки в индекс по убыванию
        /// </summary>
        /// <param name="filterObject">Фильтр объектов</param>
        /// <param name="pageNumber">Номер страницы данных</param>
        /// <param name="pageSize">Размер страницы данных</param>
        /// <param name="sortingDescription">Описание правил сортировки</param>
        /// <param name="skipSize"></param>
        /// <returns>Список актуальных версий</returns>
        dynamic GetDocument(IEnumerable<object> filterObject, int pageNumber, int pageSize, IEnumerable<object> sortingDescription = null, int skipSize = 0);

        /// <summary>
        ///     Получить общее количество объектов по заданному фильтру
        /// </summary>
        /// <param name="filterObject">Фильтр объектов</param>
        /// <returns>Количество объектов</returns>
        int GetNumberOfDocuments(IEnumerable<object> filterObject);

        /// <summary>
        /// Получить версию по уникальному идентификатору
        /// </summary>
        /// <param name="id">Уникальный идентификатор версии</param>
        /// <returns>Версия объекта</returns>
        dynamic GetDocument(string id);

        /// <summary>
        /// Получить список версий по уникальному идентификатору
        /// </summary>
        /// <param name="ids">Список идентификаторов версий</param>
        /// <returns>Список версий</returns>
        IEnumerable<object> GetDocuments(IEnumerable<string> ids);

        /// <summary>
        /// Удалить документ
        /// </summary>
        /// <param name="id">Идентификатор версии</param>
        void DeleteDocument(string id);

        /// <summary>
        ///     Удалить документы с идентификаторами из списка
        /// </summary>
        /// <param name="ids">Список идентификаторов</param>
        void DeleteDocuments(IEnumerable<string> ids);

        /// <summary>
        /// Записать версию объекта в индекс
        /// </summary>
        /// <param name="version">Обновляемая версия объекта</param>
        void SetDocument(dynamic version);

        /// <summary>
        /// Вставить список версий в индекс
        /// </summary>
        /// <param name="versions">Список версий</param>
        void SetDocuments(IEnumerable<object> versions);
    }
}