using System.IO;

using InfinniPlatform.Api.RestApi.CommonApi;

namespace InfinniPlatform.Api.RestApi.DataApi
{
    /// <summary>
    /// API для работы с клиентскими сессиями
    /// </summary>
    public sealed class SessionApi
    {
        public SessionApi(RestQueryApi restQueryApi)
        {
            _restQueryApi = restQueryApi;
        }

        private readonly RestQueryApi _restQueryApi;

        /// <summary>
        /// Создать клиентскую сессию на сервере
        /// </summary>
        /// <returns>Идентификатор созданной сессии</returns>
        public dynamic CreateSession()
        {
            return
                _restQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", "createsession", null, null)
                             .ToDynamic();
        }

        /// <summary>
        /// Сохранить клиентскую сессию
        /// </summary>
        /// <returns>Результат сохранения сессии</returns>
        public dynamic SaveSession(string sessionId)
        {
            return _restQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", "savesession", null, new
                                                                                                      {
                                                                                                          SessionId = sessionId
                                                                                                      }).ToDynamic();
        }

        /// <summary>
        /// Удалить клиентскую сессию
        /// </summary>
        /// <returns>Результат удаления сессии</returns>
        public dynamic RemoveSession(string sessionId)
        {
            return _restQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", "removesession", null, new
                                                                                                        {
                                                                                                            SessionId = sessionId
                                                                                                        }).ToDynamic();
        }

        /// <summary>
        /// Присоединить экземпляр документа к клиентской сессии
        /// </summary>
        /// <param name="sessionId">Идентификатор клиентской сессии</param>
        /// <param name="attachedDocument">Присоединяемый документ</param>
        /// <returns>Результат присоединения</returns>
        public dynamic Attach(string sessionId, dynamic attachedDocument)
        {
            return _restQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", "attachdocumentsession", null,
                new
                {
                    SessionId = sessionId,
                    AttachedInfo = attachedDocument
                }).ToDynamic();
        }

        /// <summary>
        /// Присоединяемый файл
        /// </summary>
        /// <param name="linkedData">Связанные с указанным файлом данные</param>
        /// <param name="file">Присоединяемый файл</param>
        /// <returns>Результат присоединения</returns>
        public dynamic AttachFile(dynamic linkedData, Stream file)
        {
            return
                _restQueryApi.QueryPostFile("RestfulApi", "configuration", "attachfile", linkedData, file)
                             .ToDynamic();
        }

        /// <summary>
        /// Отсоединяемый файл
        /// </summary>
        /// <param name="linkedData">Связанные с отсоединяемым файлом данные</param>
        /// <returns></returns>
        public dynamic DetachFile(dynamic linkedData)
        {
            return
                _restQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", "detachfile", null, linkedData)
                             .ToDynamic();
        }

        /// <summary>
        /// Отсоединить экземпляр документа от клиентской сессии
        /// </summary>
        /// <param name="sessionId">Идентификатор клиентской сессии</param>
        /// <param name="attachmentId">Отсоединяемый документ</param>
        /// <returns>Результат отсоединения</returns>
        public dynamic Detach(string sessionId, string attachmentId)
        {
            return _restQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", "detachdocumentsession", null,
                new
                {
                    SessionId = sessionId,
                    AttachmentId = attachmentId
                }).ToDynamic();
        }

        /// <summary>
        /// Получить данные из клиентской сессии с указанным идентификатором
        /// </summary>
        /// <returns>Данные клиентской сессии</returns>
        public dynamic GetSession(string sessionId)
        {
            return _restQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", "getsession", null, new
                                                                                                     {
                                                                                                         SessionId = sessionId
                                                                                                     }).ToDynamic();
        }
    }
}