using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestApi.CommonApi;

namespace InfinniPlatform.Api.RestApi.DataApi
{
    /// <summary>
    ///   API для работы с клиентскими сессиями
    /// </summary>
    public sealed class SessionApi
    {
        /// <summary>
        ///  Создать клиентскую сессию на сервере
        /// </summary>
        /// <returns>Идентификатор созданной сессии</returns>
        public dynamic CreateSession()
        {
            return RestQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", "createsession", null,null).ToDynamic();
        }

        /// <summary>
        ///   Сохранить клиентскую сессию
        /// </summary>
        /// <returns>Результат сохранения сессии</returns>
        public dynamic SaveSession(string sessionId)
        {
            return RestQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", "savesession", null, new
                {
                    SessionId = sessionId,
                }).ToDynamic();
        }

        /// <summary>
        ///   Удалить клиентскую сессию
        /// </summary>
        /// <returns>Результат удаления сессии</returns>
        public dynamic RemoveSession(string sessionId)
        {
            return RestQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", "removesession", null, new
            {
                SessionId = sessionId,
            }).ToDynamic();
        }

        /// <summary>
        ///   Присоединить экземпляр документа к клиентской сессии
        /// </summary>
        /// <param name="version">Версия конфигурации</param>
        /// <param name="sessionId">Идентификатор клиентской сессии</param>
        /// <param name="attachedDocument">Присоединяемый документ</param>
        /// <returns>Результат присоединения</returns>
        public dynamic Attach(string version, string sessionId, dynamic attachedDocument)
        {
            
            return RestQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", "attachdocumentsession", null,
               new
               {
                   Version = version,
                   SessionId = sessionId,
                   AttachedInfo = attachedDocument
               } ).ToDynamic();
        }

        /// <summary>
        ///   Присоединяемый файл
        /// </summary>
        /// <param name="version">Версия конфигурации</param>
        /// <param name="sessionId">Идентификатор клиентской сессии</param>
        /// <param name="linkedData">Связанные с указанным файлом данные</param>
        /// <param name="file">Присоединяемый файл</param>
        /// <returns>Результат присоединения</returns>
        public dynamic AttachFile(string version, string sessionId, dynamic linkedData, Stream file)
        {
            return RestQueryApi.QueryPostFile("RestfulApi", "configuration", "attachfile",linkedData, file).ToDynamic();
        }

        /// <summary>
        ///   Отсоединить экземпляр документа от клиентской сессии
        /// </summary>
        /// <param name="version">Версия конфигурации</param>
        /// <param name="sessionId">Идентификатор клиентской сессии</param>
        /// <param name="attachmentId">Отсоединяемый документ</param>
        /// <returns>Результат отсоединения</returns>
        public dynamic Detach(string version, string sessionId, string attachmentId)
        {
            return RestQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", "detachdocumentsession", null,
                new
                {
                    Version = version,
                    SessionId = sessionId,
                    AttachmentId = attachmentId
                }).ToDynamic();
        }

        /// <summary>
        ///   Получить данные из клиентской сессии с указанным идентификатором
        /// </summary>
        /// <returns>Данные клиентской сессии</returns>
        public dynamic GetSession(string version, string sessionId)
        {
            return RestQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", "getsession", null, new
            {
                Version = version,
                SessionId = sessionId
            });
        } 

    }
}
