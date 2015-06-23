using System;
using System.Collections.Generic;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.ContextComponents
{
    /// <summary>
    ///     Компонент работы с регистрами для глобального контекста
    /// </summary>
    public sealed class RegistryComponent : IRegistryComponent
    {
        private readonly string _version;

        public RegistryComponent(string version)
        {
            _version = version;
        }

        /// <summary>
        ///     Создаёт и инициализирует новую запись регистра накоплений
        /// </summary>
        /// <param name="configId">Идентификатор конфигурации</param>
        /// <param name="registerId">Идентификатор регистра</param>
        /// <param name="documentId">Идентификатор метаданных объекта</param>
        /// <param name="sourceDocument">Содержимое проводящегося документа</param>
        /// <param name="documentDate">Дата документа</param>
        /// <returns>Заготовка записи в регистр, которая может быть дополнена при необходимости</returns>
        public dynamic CreateAccumulationRegisterEntry(string configId, string registerId, string documentId,
            dynamic sourceDocument, DateTime? documentDate = null)
        {
            dynamic body = new
            {
                Configuration = configId,
                RegisterId = registerId,
                DocumentId = documentId,
                SourceDocument = sourceDocument,
                IsInfoRegister = false
            }.ToDynamic();

            if (documentDate != null)
            {
                body.DocumentDate = documentDate.Value;
            }

            var resp = RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "createregisterentry", null, body,
                _version);

            return resp.IsAllOk ? resp.ToDynamic() : null;
        }

        /// <summary>
        ///     Создаёт и инициализирует новую запись регистра сведений
        /// </summary>
        /// <param name="configId">Идентификатор конфигурации</param>
        /// <param name="registerId">Идентификатор регистра</param>
        /// <param name="documentId">Идентификатор метаданных объекта</param>
        /// <param name="sourceDocument">Содержимое проводящегося документа</param>
        /// <param name="documentDate">Дата документа</param>
        /// <returns>Заготовка записи в регистр, которая может быть дополнена при необходимости</returns>
        public dynamic CreateInfoRegisterEntry(string configId, string registerId, string documentId,
            dynamic sourceDocument, DateTime? documentDate = null)
        {
            dynamic body = new
            {
                Configuration = configId,
                RegisterId = registerId,
                DocumentId = documentId,
                SourceDocument = sourceDocument,
                IsInfoRegister = true
            }.ToDynamic();

            if (documentDate != null)
            {
                body.DocumentDate = documentDate.Value;
            }

            var resp = RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "createregisterentry", null, body,
                _version);

            return resp.IsAllOk ? resp.ToDynamic() : null;
        }

        /// <summary>
        ///     Выполняет проводку в регистр
        /// </summary>
        /// <param name="configId">Идентификатор конфигурации</param>
        /// <param name="registerId">Идентификатор регистра</param>
        /// <param name="registerEntries">Записи регистра</param>
        public void PostRegisterEntries(string configId, string registerId, IEnumerable<object> registerEntries)
        {
            var body = new
            {
                Configuration = configId,
                Register = registerId,
                RegisterEntries = registerEntries
            };

            RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "postregisterentries", null, body, _version);
        }

        /// <summary>
        ///     Удаляет проводку из регистра
        /// </summary>
        /// <param name="configId">Идентификатор конфигурации</param>
        /// <param name="registerId">Идентификатор регистра</param>
        /// <param name="registar">Идентификатор документа-регистратора</param>
        public void DeleteRegisterEntry(string configId, string registerId, string registar)
        {
            var body = new
            {
                Configuration = configId,
                Register = registerId,
                Registar = registar
            };

            RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "deleteregisterentry", null, body, _version);
        }
    }
}