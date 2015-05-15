using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Sdk
{
    /// <summary>
    ///   Документ, присоединяемый к клиентской сессии
    /// </summary>
    public sealed class AttachedInstance
    {
        /// <summary>
        ///   Идентификатор приложения
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        ///   Идентификатор типа документа
        /// </summary>
        public string DocumentType { get; set; }

        /// <summary>
        ///  Присоединяемая сущность
        /// </summary>
        public dynamic Instance { get; set; }
    }
}
