using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// <param name="attachedInfo">Присоединяемый документ</param>
        /// <returns>Результат присоединения</returns>
        public dynamic Attach(string version, string sessionId, dynamic attachedInfo)
        {
            
            return RestQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", "attachdocumentsession", null,
               new
               {
                   Version = version,
                   SessionId = sessionId,
                   AttachedInfo = attachedInfo
               } );
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
                });
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
