using System;
using System.Collections.Generic;
using System.IO;

using InfinniPlatform.Sdk.ApiContracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Properties;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Sdk.Api
{
    /// <summary>
    /// API для работы с документами
    /// </summary>
    public class InfinniDocumentApi : BaseApi, IDocumentApi
    {
        public InfinniDocumentApi(string server, int port) : base(server, port)
        {
        }

        /// <summary>
        /// Создать клиентскую сессию
        /// </summary>
        /// <returns>Клиентская сессия</returns>
        public dynamic CreateSession()
        {
            var response = RequestExecutor.QueryPut(RouteBuilder.BuildRestRoutingUrlDefaultSession());

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToCreateNewSession, response));
        }

        /// <summary>
        /// Присоединить документ к указанной сессии
        /// </summary>
        /// <param name="session">Идентификатор сессии</param>
        /// <param name="application">Идентификатор приложения</param>
        /// <param name="documentType">Идентификатор документа</param>
        /// <param name="instanceId">Идентификатор экземпляра документа</param>
        /// <param name="document">Экземпляр документа</param>
        public dynamic Attach(string session, string application, string documentType, string instanceId, dynamic document)
        {
            dynamic changesObject = JObject.FromObject(new
            {
                Application = application,
                DocumentType = documentType,
                Document = document
            });

            changesObject.Document.Id = instanceId;

            var response = RequestExecutor.QueryPut(RouteBuilder.BuildRestRoutingUrlDefaultSessionById(session), changesObject);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToAttachDocumentToSession, response));
        }

        /// <summary>
        /// Присоединить к сессии файл, возвращая идентификатор ссылки
        /// </summary>
        /// <param name="session">Идентификатор сессии</param>
        /// <param name="instanceId">Идентификатор документа</param>
        /// <param name="fieldName">Наименование поля для присоединения</param>
        /// <param name="fileName">Наименование файла</param>
        /// <param name="fileStream">Файловый поток</param>
        public void AttachFile(string session, string application, string documentType, string instanceId, string fieldName, string fileName, Stream fileStream)
        {
            var response = RequestExecutor.QueryPostFile(RouteBuilder.BuildRestRoutingUrlDefaultSession(), application, documentType, instanceId, fieldName, fileName, fileStream, session);

            ProcessAsObjectResult(response, string.Format(Resources.UnableToAttachFileToSession, response));
        }

        /// <summary>
        /// Отсоединить от сессии указанный файл
        /// </summary>
        /// <param name="session">Идентификатор сессии</param>
        /// <param name="instanceId">Идентификатор документа</param>
        /// <param name="fieldName">Наименование поля для присоединения</param>
        public void DetachFile(string session, string instanceId, string fieldName)
        {
            dynamic body = new
            {
                InstanceId = instanceId,
                FieldName = fieldName,
                SessionId = session
            };

            var response = RequestExecutor.QueryDelete(RouteBuilder.BuildRestRoutingUrlDefaultSession(), body);

            ProcessAsObjectResult(response, string.Format(Resources.UnableToDetachFileFromSession, response));
        }

        /// <summary>
        /// Отсоединить документ от указанной сессии
        /// </summary>
        /// <param name="session">Идентификатор сессии</param>
        /// <param name="instanceId">Идентификатор отсоединяемого документа</param>
        public dynamic Detach(string session, string instanceId)
        {
            if (string.IsNullOrEmpty(instanceId))
            {
                throw new ArgumentException(Resources.DocumentToDetachShouldntBeEmpty);
            }

            var response = RequestExecutor.QueryDelete(RouteBuilder.BuildRestRoutingUrlDetachDocument(session, instanceId));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToDetachDocument, response));
        }

        /// <summary>
        /// Удалить клиентскую сессию
        /// </summary>
        /// <param name="sessionId">Идентификатор сессии</param>
        /// <returns>Результат удаления сессии</returns>
        public dynamic RemoveSession(string sessionId)
        {
            var response = RequestExecutor.QueryDelete(RouteBuilder.BuildRestRoutingUrlDefaultSessionById(sessionId));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToRemoveSession, response));
        }

        /// <summary>
        /// Получить список документов сессии
        /// </summary>
        /// <param name="sessionId">Идентификатор клиентской сессии</param>
        /// <returns>Объект сессии</returns>
        public dynamic GetSession(string sessionId)
        {
            var response = RequestExecutor.QueryGetById(RouteBuilder.BuildRestRoutingUrlDefaultSessionById(sessionId));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetSession, response));
        }

        /// <summary>
        /// Выполнить фиксацию клиентской сессии
        /// </summary>
        /// <param name="sessionId">Идентификатор клиентской сессии</param>
        /// <returns>Список результатов фиксации клиентской сессии</returns>
        public dynamic SaveSession(string sessionId)
        {
            var response = RequestExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlDefaultSessionById(sessionId));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToCommitException, response));
        }

        /// <summary>
        /// Получить документ по указанному идентификатору
        /// </summary>
        /// <param name="applicationId">Идентификатор приложения</param>
        /// <param name="documentType">Тип документа</param>
        /// <param name="instanceId">Идентификатор документа</param>
        /// <returns>Документ с указанным идентификатором</returns>
        public dynamic GetDocumentById(string applicationId, string documentType, string instanceId)
        {
            var response = RequestExecutor.QueryGetById(RouteBuilder.BuildRestRoutingUrlDefaultById(applicationId, documentType, instanceId));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetDocument, response));
        }

        /// <summary>
        /// Получить документы по указанным фильтрам
        /// </summary>
        /// <param name="applicationId">Идентификатор приложения</param>
        /// <param name="documentType">Тип документа</param>
        /// <param name="filter">Выражение для фильтрации документов</param>
        /// <param name="pageNumber">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <param name="sorting">Выражение для сортировки документов</param>
        /// <returns>Список документов, удовлетворяющих указанному фильтру</returns>
        public IEnumerable<dynamic> GetDocument(string applicationId, string documentType, Action<FilterBuilder> filter, int pageNumber, int pageSize, Action<SortingBuilder> sorting = null)
        {
            var routeBuilder = new RouteBuilder(Server, Port, Route);

            var filterBuilder = new FilterBuilder();
            filter?.Invoke(filterBuilder);

            var sortingBuilder = new SortingBuilder();
            sorting?.Invoke(sortingBuilder);

            var response = RequestExecutor.QueryGet(routeBuilder.BuildRestRoutingUrlDefault(applicationId, documentType),
                                                    RequestExecutorExtensions.CreateQueryString(filterBuilder.GetFilter(), pageNumber, pageSize, sortingBuilder.GetSorting()));

            return ProcessAsObjectResult(response,
                                         string.Format(Resources.UnableToGetDocument, response));
        }

        public long GetNumberOfDocuments(string applicationId, string documentType, Action<FilterBuilder> filter)
        {
            var routeBuilder = new RouteBuilder(Server, Port, Route);

            var filterBuilder = new FilterBuilder();
            filter?.Invoke(filterBuilder);

            var response = RequestExecutor.QueryGet(routeBuilder.BuildRestRoutingUrlDefaultCount(applicationId, documentType),
                                                    RequestExecutorExtensions.CreateQueryStringCount(filterBuilder.GetFilter()));

            return ProcessAsObjectResult(response,
                                         string.Format(Resources.UnableToGetDocument, response));
        }

        /// <summary>
        /// Вставить или полностью заменить существующий документ
        /// </summary>
        /// <param name="applicationId">Идентификатор приложения</param>
        /// <param name="documentType">Тип документа</param>
        /// <param name="document">Экземпляр сохраняемого документа</param>
        /// <returns>Идентификатор сохраненного документа</returns>
        public dynamic SetDocument(string applicationId, string documentType, object document)
        {
            var routeBuilder = new RouteBuilder(Server, Port, Route);

            document = document.ToDynamic();

            var documentId = PrepareDocumentIdentifier(document);

            var response = RequestExecutor.QueryPut(routeBuilder.BuildRestRoutingUrlDefaultById(applicationId, documentType, documentId), document);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToSetDocument, response));
        }

        /// <summary>
        /// Вставить или полностью заменить документы в переданном списке
        /// </summary>
        /// <param name="applicationId">Идентификатор приложения</param>
        /// <param name="documentType">Тип документа</param>
        /// <param name="documents">Список сохраняемых документов</param>
        /// <returns>Идентификатор сохраненного документа</returns>
        public dynamic SetDocuments(string applicationId, string documentType, IEnumerable<object> documents)
        {
            var routeBuilder = new RouteBuilder(Server, Port, Route);

            var response = RequestExecutor.QueryPut(routeBuilder.BuildRestRoutingUrlDefault(applicationId, documentType), new { Documents = documents });

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToSetDocument, response));
        }

        /// <summary>
        /// Внести частичные изменения в документ
        /// </summary>
        /// <param name="applicationId">Идентификатор приложения</param>
        /// <param name="documentType">Тип документа</param>
        /// <param name="instanceId">Идентификатор изменяемого документа</param>
        /// <param name="changesObject">Объект, содержащий изменения</param>
        public void UpdateDocument(string applicationId, string documentType, string instanceId, object changesObject)
        {
            var routeBuilder = new RouteBuilder(Server, Port, Route);

            var parameters = new
            {
                Id = instanceId,
                ChangesObject = changesObject
            };

            var response = RequestExecutor.QueryPost(
                                                     routeBuilder.BuildRestRoutingUrlDefaultById(applicationId, documentType, instanceId),
                                                     parameters);

            ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdateDocument, response));
        }

        /// <summary>
        /// Удалить документ
        /// </summary>
        /// <param name="applicationId">Идентификатор приложения</param>
        /// <param name="documentType">Идентификатор типа документа</param>
        /// <param name="instanceId">Идентификатор экземпляра документа</param>
        /// <returns>Результат удаления документа</returns>
        public dynamic DeleteDocument(string applicationId, string documentType, string instanceId)
        {
            var routeBuilder = new RouteBuilder(Server, Port, Route);

            var response = RequestExecutor.QueryDelete(
                                                       routeBuilder.BuildRestRoutingUrlDefaultById(applicationId, documentType, instanceId));

            return ProcessAsObjectResult(response,
                                         string.Format(Resources.UnableToDeleteDocument, response));
        }

        private static string PrepareDocumentIdentifier(dynamic document)
        {
            object instanceId = document.Id;

            if (instanceId == null)
            {
                instanceId = Guid.NewGuid();

                document.Id = instanceId;
            }

            return instanceId.ToString();
        }
    }
}