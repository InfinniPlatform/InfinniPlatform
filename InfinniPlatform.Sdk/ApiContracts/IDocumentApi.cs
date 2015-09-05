using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Sdk.ApiContracts
{
    public interface IDocumentApi
    {
        /// <summary>
        ///   Создать клиентскую сессию
        /// </summary>
        /// <returns>Клиентская сессия</returns>
        dynamic CreateSession();

        /// <summary>
        ///   Присоединить документ к указанной сессии
        /// </summary>
        /// <param name="session">Идентификатор сессии</param>
        /// <param name="application">Идентификатор приложения</param>
        /// <param name="documentType">Идентификатор документа</param>
        /// <param name="instanceId">Идентификатор элкземпляра документа</param>
        /// <param name="document">Экземпляр документа</param>
        dynamic Attach(string session, string application, string documentType, string instanceId, dynamic document);

        /// <summary>
        ///   Присоединить к сессии файл, возвращая идентификатор ссылки
        /// </summary>
        /// <param name="session">Идентификатор сессии</param>
        /// <param name="instanceId">Идентификатор документа</param>
        /// <param name="fieldName">Наименование поля для присоединения</param>
        /// <param name="fileName">Наименование файла</param>
        /// <param name="fileStream">Файловый поток</param>
        void AttachFile(string session, string instanceId, string fieldName, string fileName, Stream fileStream);

        /// <summary>
        ///   Отсоединить от сессии указанный файл
        /// </summary>
        /// <param name="session">Идентификатор сессии</param>
        /// <param name="instanceId">Идентификатор документа</param>
        /// <param name="fieldName">Наименование поля для присоединения</param>
        void DetachFile(string session, string instanceId, string fieldName);

        /// <summary>
        ///   Отсоединить документ от указанной сессии
        /// </summary>
        /// <param name="session">Идентификатор сессии</param>
        /// <param name="instanceId">Идентификатор отсоединяемого документа</param>
        dynamic Detach(string session, string instanceId);

        /// <summary>
        ///   Удалить клиентскую сессию
        /// </summary>
        /// <param name="sessionId">Идентификатор сессии</param>
        /// <returns>Результат удаления сессии</returns>
        dynamic RemoveSession(string sessionId);

        /// <summary>
        ///   Получить список документов сессии
        /// </summary>
        /// <param name="sessionId">Идентификатор клиентской сессии</param>
        /// <returns>Объект сессии</returns>
        dynamic GetSession(string sessionId);

        /// <summary>
        ///   Выполнить фиксацию клиентской сессии
        /// </summary>
        /// <param name="sessionId">Идентификатор клиентской сессии</param>
        /// <returns>Список результатов фиксации клиентской сессии</returns>
        dynamic SaveSession(string sessionId);

        /// <summary>
        ///   Получить документ по указанному идентификатору
        /// </summary>
        /// <param name="applicationId">Идентификатор приложения</param>
        /// <param name="documentType">Тип документа</param>
        /// <param name="instanceId">Идентификатор документа</param>
        /// <returns>Документ с указанным идентификатором</returns>
        dynamic GetDocumentById(string applicationId, string documentType, string instanceId);

        /// <summary>
        ///   Получить документы по указанным фильтрам
        /// </summary>
        /// <param name="applicationId">Идентификатор приложения</param>
        /// <param name="documentType">Тип документа</param>
        /// <param name="filter">Выражение для фильтрации документов</param>
        /// <param name="pageNumber">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <param name="sorting">Выражение для сортировки документов</param>
        /// <returns>Список документов, удовлетворяющих указанному фильтру</returns>
        IEnumerable<dynamic> GetDocument(string applicationId, string documentType, Action<FilterBuilder> filter, int pageNumber, int pageSize, Action<SortingBuilder> sorting = null);

        /// <summary>
        ///   Вставить или полностью заменить существующий документ
        /// </summary>
        /// <param name="applicationId">Идентификатор приложения</param>
        /// <param name="documentType">Тип документа</param>
        /// <param name="document">Экземпляр сохраняемого документа</param>
        /// <returns>Идентификатор сохраненного документа</returns>
        dynamic SetDocument(string applicationId, string documentType, object document);

        /// <summary>
        ///   Вставить или полностью заменить документы в переданном списке
        /// </summary>
        /// <param name="applicationId">Идентификатор приложения</param>
        /// <param name="documentType">Тип документа</param>
        /// <param name="documents">Список сохраняемых документов</param>
        /// <returns>Идентификатор сохраненного документа</returns>
        dynamic SetDocuments(string applicationId, string documentType, IEnumerable<object> documents);

        /// <summary>
        ///   Внести частичные изменения в документ
        /// </summary>
        /// <param name="applicationId">Идентификатор приложения</param>
        /// <param name="documentType">Тип документа</param>
        /// <param name="instanceId">Идентификатор изменяемого документа</param>
        /// <param name="changesObject">Объект, содержащий изменения</param>
        void UpdateDocument(string applicationId, string documentType, string instanceId, object changesObject);

        /// <summary>
        ///  Удалить документ
        /// </summary>
        /// <param name="applicationId">Идентификатор приложения</param>
        /// <param name="documentType">Идентификатор типа документа</param>
        /// <param name="instanceId">Идентификатор экземпляра документа</param>
        /// <returns>Результат удаления документа</returns>
        dynamic DeleteDocument(string applicationId, string documentType, string instanceId);

        /// <summary>
        ///  Получить количество документов по указанному фильтру
        /// </summary>
        /// <param name="applicationId">Идентификатор приложения</param>
        /// <param name="documentType">Идентификатор типа документа</param>
        /// <param name="filter">Фильтр документов</param>
        /// <returns>Количество документов</returns>
        long GetNumberOfDocuments(string applicationId, string documentType, Action<FilterBuilder> filter);
    }
}
