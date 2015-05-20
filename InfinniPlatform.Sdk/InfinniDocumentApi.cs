using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using InfinniPlatform.Sdk.Properties;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Sdk
{
    /// <summary>
    ///   API для работы с документами
    /// </summary>
    public class InfinniDocumentApi
    {
        private readonly string _server;
        private readonly string _port;
        private readonly string _version;
        private CookieContainer _cookieContainer;
        private readonly RouteBuilder _routeBuilder;

        public InfinniDocumentApi(string server, string port, string version)
        {
            _server = server;
            _port = port;
            _version = version;
            _routeBuilder = new RouteBuilder(_server, _port);
        }

        /// <summary>
        ///   Создать клиентскую сессию
        /// </summary>
        /// <returns>Клиентская сессия</returns>
        public string CreateSession()
        {
            var restQueryExecutor = new RequestExecutor(_cookieContainer);

            var response = restQueryExecutor.QueryPut(_routeBuilder.BuildRestRoutingUrlDefaultSession(_version));

            string sessionId = null;

            if (response.IsAllOk)
            {
                try
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        //гребаный JsonObjectSerializer вставляет служебный символ в начало строки
                        dynamic result = JObject.Parse(response.Content.Remove(0, 1));
                        if (result.SessionId != null && result.IsValid == true)
                        {
                            sessionId = result.SessionId;
                        }
                    }
                }
                catch (Exception)
                {
                    throw new ArgumentException(Resources.ResultIsNotOfObjectType);
                }
            }

            if (string.IsNullOrEmpty(sessionId))
            {
                throw new ArgumentException(Resources.FailToCreateNewSession);
            }

            return sessionId;
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
            var restQueryExecutor = new RequestExecutor(_cookieContainer);
           
            dynamic changesObject = JObject.FromObject(new
            {
                Application = application,
                DocumentType = documentType,
                Document = document
            });

            changesObject.Document.Id = instanceId;

            var response = restQueryExecutor.QueryPut(_routeBuilder.BuildRestRoutingUrlDefaultSessionById(session, _version), changesObject.ToString());

            if (response.IsAllOk)
            {
                try
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        //гребаный JsonObjectSerializer вставляет служебный символ в начало строки
                        dynamic responseObject = JObject.Parse(response.Content.Remove(0, 1));
                        if (responseObject.IsValid == true && responseObject.Id != null)
                        {
                            return responseObject.Id.ToString();
                        }
                        throw new ArgumentException(string.Format(Resources.FailToAttachDocumentToSession, response.GetErrorContent()));
                    }
                }
                catch (Exception)
                {
                    throw new ArgumentException(Resources.ResultIsNotOfObjectType);
                }
            }

            throw new ArgumentException(string.Format(Resources.FailToAttachDocumentToSession, response.GetErrorContent()));
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
            var restQueryExecutor = new RequestExecutor(_cookieContainer);

            var response = restQueryExecutor.QueryPostFile(_routeBuilder.BuildRestRoutingUrlDefaultSession(_version), instanceId, fieldName, fileName, fileStream, session);

            if (response.IsAllOk)
            {
                try
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        //гребаный JsonObjectSerializer вставляет служебный символ в начало строки
                        dynamic responseObject = JObject.Parse(response.Content.Remove(0, 1));
                        if (responseObject.IsValid != true)
                        {
                            throw new ArgumentException(string.Format(Resources.FailToAttachFileToSession, response.GetErrorContent()));
                        }
                        return;
                    }
                }
                catch (Exception)
                {
                    throw new ArgumentException(Resources.ResultIsNotOfObjectType);
                }
            }

            throw new ArgumentException(string.Format(Resources.FailToAttachFileToSession, response.GetErrorContent()));
            
        }

        /// <summary>
        ///   Отсоединить документ от указанной сессии
        /// </summary>
        /// <param name="session">Идентификатор сессии</param>
        /// <param name="instanceId">Идентификатор отсоединяемого документа</param>
        public dynamic Detach(string session, string instanceId)
        {
            var restQueryExecutor = new RequestExecutor(_cookieContainer);

            if (string.IsNullOrEmpty(instanceId))
            {
                throw new ArgumentException(Resources.DocumentToDetachShouldntBeEmpty);
            }

            var response = restQueryExecutor.QueryDelete(_routeBuilder.BuildRestRoutingUrlDetachDocument(session, _version, instanceId));

            if (response.IsAllOk)
            {
                try
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        //гребаный JsonObjectSerializer вставляет служебный символ в начало строки
                        return JObject.Parse(response.Content.Remove(0, 1));
                    }
                }
                catch (Exception)
                {
                    throw new ArgumentException(Resources.ResultIsNotOfObjectType);
                }
            }
            throw new ArgumentException(string.Format(Resources.FailToDetachDocument, response.GetErrorContent()));
        }

        /// <summary>
        ///   Удалить клиентскую сессию
        /// </summary>
        /// <param name="sessionId">Идентификатор сессии</param>
        /// <returns>Результат удаления сессии</returns>
        public dynamic RemoveSession(string sessionId)
        {
            var restQueryExecutor = new RequestExecutor(_cookieContainer);

            var response = restQueryExecutor.QueryDelete(_routeBuilder.BuildRestRoutingUrlDefaultSessionById(sessionId, _version));

            if (response.IsAllOk)
            {
                try
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        //гребаный JsonObjectSerializer вставляет служебный символ в начало строки
                        return JObject.Parse(response.Content.Remove(0, 1));
                    }
                }
                catch (Exception)
                {
                    throw new ArgumentException(Resources.ResultIsNotOfObjectType);
                }
            }
            throw new ArgumentException(string.Format(Resources.FailToDetachDocument, response.GetErrorContent()));
        }

        /// <summary>
        ///   Получить список документов сессии
        /// </summary>
        /// <param name="sessionId">Идентификатор клиентской сессии</param>
        /// <returns>Объект сессии</returns>
        public dynamic GetSession(string sessionId)
        {
            var restQueryExecutor = new RequestExecutor(_cookieContainer);

            var response = restQueryExecutor.QueryGetById(_routeBuilder.BuildRestRoutingUrlDefaultSessionById(sessionId, _version));

            if (response.IsAllOk)
            {
                try
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        //гребаный JsonObjectSerializer вставляет служебный символ в начало строки
                        return JObject.Parse(response.Content.Remove(0, 1));
                    }
                }
                catch (Exception)
                {
                    throw new ArgumentException(Resources.ResultIsNotOfObjectType);
                }
            }
            throw new ArgumentException(string.Format(Resources.FailToGetSession, response.GetErrorContent()));
        }

        /// <summary>
        ///   Выполнить фиксацию клиентской сессии
        /// </summary>
        /// <param name="sessionId">Идентификатор клиентской сессии</param>
        /// <returns>Список результатов фиксации клиентской сессии</returns>
        public dynamic SaveSession(string sessionId)
        {
            var restQueryExecutor = new RequestExecutor(_cookieContainer);

            var response = restQueryExecutor.QueryPost(_routeBuilder.BuildRestRoutingUrlDefaultSessionById(sessionId, _version));

            if (response.IsAllOk)
            {
                try
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        //гребаный JsonObjectSerializer вставляет служебный символ в начало строки
                        return JObject.Parse(response.Content.Remove(0, 1));
                    }
                }
                catch (Exception)
                {
                    throw new ArgumentException(Resources.ResultIsNotOfObjectType);
                }
            }
            throw new ArgumentException(string.Format(Resources.FailToCommitException, response.GetErrorContent()));
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
            var restQueryExecutor = new RequestExecutor(_cookieContainer);

            var response = restQueryExecutor.QueryGetById(_routeBuilder.BuildRestRoutingUrlDefaultById(_version, applicationId, documentType, instanceId));

            if (response.IsAllOk)
            {
                try
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        //гребаный JsonObjectSerializer вставляет служебный символ в начало строки
                        return JObject.Parse(response.Content.Remove(0, 1));
                    }
                    return null;
                }
                catch (Exception)
                {
                    throw new ArgumentException(Resources.ResultIsNotOfArrayType);
                }
            }
            throw new ArgumentException(string.Format(Resources.FailGetDocument, response.GetErrorContent()));
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
        public IEnumerable<dynamic> GetDocument(
            string applicationId,
            string documentType,
            Action<FilterBuilder> filter,
            int pageNumber,
            int pageSize,
            Action<SortingBuilder> sorting = null)
        {

            var restQueryExecutor = new RequestExecutor(_cookieContainer);

            var routeBuilder = new RouteBuilder(_server, _port);

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

            var response = restQueryExecutor.QueryGet(routeBuilder.BuildRestRoutingUrlDefault(_version, applicationId, documentType),
                RequestExecutorExtensions.CreateQueryString(filterBuilder.GetFilter(), pageNumber, pageSize, sortingBuilder.GetSorting()));

            if (response.IsAllOk)
            {
                try
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        //гребаный JsonObjectSerializer вставляет служебный символ в начало строки
                        return JArray.Parse(response.Content.Remove(0, 1));
                    }
                    return null;
                }
                catch (Exception)
                {
                    throw new ArgumentException(Resources.ResultIsNotOfArrayType);
                }
            }
            throw new ArgumentException(string.Format(Resources.FailGetDocument, response.GetErrorContent()));
        }

        /// <summary>
        ///   Вставить или полностью заменить существующий документ
        /// </summary>
        /// <param name="applicationId">Идентификатор приложения</param>
        /// <param name="documentType">Тип документа</param>
        /// <param name="documentId">Идентификатор документа</param>
        /// <param name="document">Экземпляр сохраняемого документа</param>
        /// <returns>Идентификатор сохраненного документа</returns>
        public string SetDocument(string applicationId, string documentType, string documentId, object document)
        {
            var restQueryExecutor = new RequestExecutor(_cookieContainer);

            var routeBuilder = new RouteBuilder(_server, _port);

            var response = restQueryExecutor.QueryPut(
                routeBuilder.BuildRestRoutingUrlDefaultById(_version, applicationId, documentType, documentId),
                JObject.FromObject(document).ToString());

            if (response.IsAllOk)
            {
                if (!string.IsNullOrEmpty(response.Content))
                {
                    //гребаный JsonObjectSerializer вставляет служебный символ в начало строки
                    dynamic result = null;
                    try
                    {
                        result = JObject.Parse(response.Content.Remove(0, 1));
                    }
                    catch (Exception)
                    {
                        throw new ArgumentException(Resources.ResultIsNotOfObjectType);
                    }

                    if (result.InstanceId != null && result.IsValid == true)
                    {
                        return result.InstanceId;
                    }
                }
                return null;

            }
            throw new ArgumentException(string.Format(Resources.FailToSetDocument, response.GetErrorContent()));
        }

        /// <summary>
        ///   Вставить или полностью заменить документы в переданном списке
        /// </summary>
        /// <param name="applicationId">Идентификатор приложения</param>
        /// <param name="documentType">Тип документа</param>
        /// <param name="documents">Список сохраняемых документов</param>
        /// <returns>Идентификатор сохраненного документа</returns>
        public string SetDocuments(string applicationId, string documentType, IEnumerable<dynamic> documents)
        {
            var restQueryExecutor = new RequestExecutor(_cookieContainer);

            var routeBuilder = new RouteBuilder(_server, _port);

            var response = restQueryExecutor.QueryPut(
                routeBuilder.BuildRestRoutingUrlDefault(_version, applicationId, documentType),
                JArray.FromObject(documents).ToString());

            if (response.IsAllOk)
            {
                try
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        //гребаный JsonObjectSerializer вставляет служебный символ в начало строки
                        dynamic result = JObject.Parse(response.Content.Remove(0, 1));
                        if (result.InstanceId != null && result.IsValid == true)
                        {
                            return result.InstanceId;
                        }
                        return null;
                    }
                    return null;
                }
                catch (Exception)
                {
                    throw new ArgumentException(Resources.ResultIsNotOfObjectType);
                }
            }
            throw new ArgumentException(string.Format(Resources.FailToSetDocument, response.GetErrorContent()));
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
            var restQueryExecutor = new RequestExecutor(_cookieContainer);

            var routeBuilder = new RouteBuilder(_server, _port);

            var parameters = new
            {
                Id = instanceId,
                ChangesObject = changesObject
            };

            var response = restQueryExecutor.QueryPost(
                routeBuilder.BuildRestRoutingUrlDefaultById(_version, applicationId, documentType, instanceId),
                JObject.FromObject(parameters).ToString());

            if (!response.IsAllOk)
            {
                throw new ArgumentException(string.Format(Resources.FailToUpdateDocument, response.GetErrorContent()));
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
            var restQueryExecutor = new RequestExecutor(_cookieContainer);

            var routeBuilder = new RouteBuilder(_server, _port);

            var response = restQueryExecutor.QueryDelete(
                routeBuilder.BuildRestRoutingUrlDefaultById(_version, applicationId, documentType, instanceId));

            if (response.IsAllOk)
            {
                try
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        //гребаный JsonObjectSerializer вставляет служебный символ в начало строки
                        return JObject.Parse(response.Content.Remove(0, 1));
                    }
                    return null;
                }
                catch (Exception)
                {
                    throw new ArgumentException(Resources.ResultIsNotOfObjectType);
                }
            }
            throw new ArgumentException(string.Format(Resources.FailToDeleteDocument, response.GetErrorContent()));

        }
    }
}
