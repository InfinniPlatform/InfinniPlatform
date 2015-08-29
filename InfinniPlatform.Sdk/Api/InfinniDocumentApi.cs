using System;
using System.Collections.Generic;
using System.IO;
using InfinniPlatform.Sdk.ApiContracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Sdk.Api
{
    /// <summary>
    ///   API для работы с документами
    /// </summary>
    public class InfinniDocumentApi : BaseApi, IDocumentApi
    {
        public InfinniDocumentApi(string server, string port, string route)
            : base(server, port, route)
        {
        }

        /// <summary>
        ///   Создать клиентскую сессию
        /// </summary>
        /// <returns>Клиентская сессия</returns>
        public dynamic CreateSession()
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPut(RouteBuilder.BuildRestRoutingUrlDefaultSession());

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToCreateNewSession, response.GetErrorContent())); 
        }

        /// <summary>
        ///   Присоединить документ к указанной сессии
        /// </summary>
        /// <param name="session">Идентификатор сессии</param>
        /// <param name="application">Идентификатор приложения</param>
        /// <param name="documentType">Идентификатор документа</param>
        /// <param name="instanceId">Идентификатор элкземпляра документа</param>
        /// <param name="document">Экземпляр документа</param>
        public dynamic Attach(string session, string application, string documentType, string instanceId, dynamic document)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);
           
            dynamic changesObject = JObject.FromObject(new
            {
                Application = application,
                DocumentType = documentType,
                Document = document
            });

            changesObject.Document.Id = instanceId;

            var response = restQueryExecutor.QueryPut(RouteBuilder.BuildRestRoutingUrlDefaultSessionById(session), changesObject);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToAttachDocumentToSession, response.GetErrorContent())); 
        }

        /// <summary>
        ///   Присоединить к сессии файл, возвращая идентификатор ссылки
        /// </summary>
        /// <param name="session">Идентификатор сессии</param>
        /// <param name="instanceId">Идентификатор документа</param>
        /// <param name="fieldName">Наименование поля для присоединения</param>
        /// <param name="fileName">Наименование файла</param>
        /// <param name="fileStream">Файловый поток</param>
        public void AttachFile(string session, string instanceId, string fieldName, string fileName, Stream fileStream)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPostFile(RouteBuilder.BuildRestRoutingUrlDefaultSession(), instanceId, fieldName, fileName, fileStream, session);

            ProcessAsObjectResult(response, string.Format(Resources.UnableToAttachFileToSession, response.GetErrorContent()));            
        }

        /// <summary>
        ///   Отсоединить от сессии указанный файл
        /// </summary>
        /// <param name="session">Идентификатор сессии</param>
        /// <param name="instanceId">Идентификатор документа</param>
        /// <param name="fieldName">Наименование поля для присоединения</param>
        public void DetachFile(string session, string instanceId, string fieldName)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            dynamic body = new
            {
                InstanceId = instanceId,
                FieldName = fieldName,
                SessionId = session
            };

            var response = restQueryExecutor.QueryDelete(RouteBuilder.BuildRestRoutingUrlDefaultSession(), body);

            ProcessAsObjectResult(response, string.Format(Resources.UnableToDetachFileFromSession, response.GetErrorContent())); 
        }

        /// <summary>
        ///   Отсоединить документ от указанной сессии
        /// </summary>
        /// <param name="session">Идентификатор сессии</param>
        /// <param name="instanceId">Идентификатор отсоединяемого документа</param>
        public dynamic Detach(string session, string instanceId)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            if (string.IsNullOrEmpty(instanceId))
            {
                throw new ArgumentException(Resources.DocumentToDetachShouldntBeEmpty);
            }

            var response = restQueryExecutor.QueryDelete(RouteBuilder.BuildRestRoutingUrlDetachDocument(session, instanceId));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToDetachDocument, response.GetErrorContent())); 
        }

        /// <summary>
        ///   Удалить клиентскую сессию
        /// </summary>
        /// <param name="sessionId">Идентификатор сессии</param>
        /// <returns>Результат удаления сессии</returns>
        public dynamic RemoveSession(string sessionId)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryDelete(RouteBuilder.BuildRestRoutingUrlDefaultSessionById(sessionId));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToRemoveSession, response.GetErrorContent())); 
        }

        /// <summary>
        ///   Получить список документов сессии
        /// </summary>
        /// <param name="sessionId">Идентификатор клиентской сессии</param>
        /// <returns>Объект сессии</returns>
        public dynamic GetSession(string sessionId)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryGetById(RouteBuilder.BuildRestRoutingUrlDefaultSessionById(sessionId));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetSession, response.GetErrorContent()));
        }

        /// <summary>
        ///   Выполнить фиксацию клиентской сессии
        /// </summary>
        /// <param name="sessionId">Идентификатор клиентской сессии</param>
        /// <returns>Список результатов фиксации клиентской сессии</returns>
        public dynamic SaveSession(string sessionId)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlDefaultSessionById(sessionId));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToCommitException, response.GetErrorContent()));
        }


        /// <summary>
        ///   Получить документ по указанному идентификатору
        /// </summary>
        /// <param name="applicationId">Идентификатор приложения</param>
        /// <param name="documentType">Тип документа</param>
        /// <param name="instanceId">Идентификатор документа</param>
        /// <returns>Документ с указанным идентификатором</returns>
        public dynamic GetDocumentById(string applicationId, string documentType, string instanceId)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryGetById(RouteBuilder.BuildRestRoutingUrlDefaultById(applicationId, documentType, instanceId));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetDocument, response.GetErrorContent()));
        }

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
        public IEnumerable<dynamic> GetDocument(string applicationId, string documentType, Action<FilterBuilder> filter, int pageNumber, int pageSize, Action<SortingBuilder> sorting = null)
        {

            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var routeBuilder = new RouteBuilder(Server, Port, Route);

            var filterBuilder = new FilterBuilder();
            if (filter != null)
            {
                filter.Invoke(filterBuilder);
            }

            var sortingBuilder = new SortingBuilder();
            if (sorting != null)
            {
                sorting.Invoke(sortingBuilder);
            }

            var response = restQueryExecutor.QueryGet(routeBuilder.BuildRestRoutingUrlDefault(applicationId, documentType),
                RequestExecutorExtensions.CreateQueryString(filterBuilder.GetFilter(), pageNumber, pageSize, sortingBuilder.GetSorting()));

            return ProcessAsArrayResult(response,
                string.Format(Resources.UnableToGetDocument, response.GetErrorContent()));
        }

        /// <summary>
        ///   Вставить или полностью заменить существующий документ
        /// </summary>
        /// <param name="applicationId">Идентификатор приложения</param>
        /// <param name="documentType">Тип документа</param>
        /// <param name="documentId">Идентификатор документа</param>
        /// <param name="document">Экземпляр сохраняемого документа</param>
        /// <returns>Идентификатор сохраненного документа</returns>
        public dynamic SetDocument(string applicationId, string documentType, string documentId, object document)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var routeBuilder = new RouteBuilder(Server, Port, Route);

            document = document.ToDynamic();

            documentId = PrepareDocumentIdentifier(documentId,document);
           
            var response = restQueryExecutor.QueryPut(
                routeBuilder.BuildRestRoutingUrlDefaultById(applicationId, documentType, documentId), document);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToSetDocument, response.GetErrorContent()));
        }

        private static string PrepareDocumentIdentifier(string documentId, dynamic document)
        {
            if (documentId == null)
            {
                var id = ObjectHelper.GetProperty(document, "Id");
                if (id == null)
                {
                    id = Guid.NewGuid().ToString();
                }
                documentId = id.ToString();
                ObjectHelper.SetProperty(document, "Id", documentId);
            }
            else
            {
                ObjectHelper.SetProperty(document, "Id", documentId);
            }
            return documentId;
        }

        /// <summary>
        ///   Вставить или полностью заменить документы в переданном списке
        /// </summary>
        /// <param name="applicationId">Идентификатор приложения</param>
        /// <param name="documentType">Тип документа</param>
        /// <param name="documents">Список сохраняемых документов</param>
        /// <returns>Идентификатор сохраненного документа</returns>
        public dynamic SetDocuments(string applicationId, string documentType, IEnumerable<dynamic> documents)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var routeBuilder = new RouteBuilder(Server, Port, Route);

            var response = restQueryExecutor.QueryPut(
                routeBuilder.BuildRestRoutingUrlDefault(applicationId, documentType),
                documents);

            return ProcessAsObjectResult(response,
                string.Format(Resources.UnableToSetDocument, response.GetErrorContent()));

        }


        /// <summary>
        ///   Внести частичные изменения в документ
        /// </summary>
        /// <param name="applicationId">Идентификатор приложения</param>
        /// <param name="documentType">Тип документа</param>
        /// <param name="instanceId">Идентификатор изменяемого документа</param>
        /// <param name="changesObject">Объект, содержащий изменения</param>
        public void UpdateDocument(string applicationId, string documentType, string instanceId, object changesObject)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var routeBuilder = new RouteBuilder(Server, Port, Route);

            var parameters = new
            {
                Id = instanceId,
                ChangesObject = changesObject
            };

            var response = restQueryExecutor.QueryPost(
                routeBuilder.BuildRestRoutingUrlDefaultById(applicationId, documentType, instanceId),
                parameters);

            if (!response.IsAllOk)
            {
                throw new ArgumentException(string.Format(Resources.UnableToUpdateDocument, response.GetErrorContent()));
            }
        }

        /// <summary>
        ///  Удалить документ
        /// </summary>
        /// <param name="applicationId">Идентификатор приложения</param>
        /// <param name="documentType">Идентификатор типа документа</param>
        /// <param name="instanceId">Идентификатор экземпляра документа</param>
        /// <returns>Результат удаления документа</returns>
        public dynamic DeleteDocument(string applicationId, string documentType, string instanceId)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var routeBuilder = new RouteBuilder(Server, Port, Route);

            var response = restQueryExecutor.QueryDelete(
                routeBuilder.BuildRestRoutingUrlDefaultById(applicationId, documentType, instanceId));

            return ProcessAsObjectResult(response,
                            string.Format(Resources.UnableToDeleteDocument, response.GetErrorContent()));

        }


    }
}
