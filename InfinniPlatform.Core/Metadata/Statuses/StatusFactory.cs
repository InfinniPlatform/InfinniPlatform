using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using InfinniPlatform.Api.DocumentStatus;

namespace InfinniPlatform.Metadata.Statuses
{
    /// <summary>
    /// Фабрика для создания экземпляров статусов
    /// </summary>
    public class StatusFactory : IStatusFactory
    {
        public const string DefaultStatusField = "Status";

        /// <summary>
        /// Создать статус с единственным полем в ключе. Имя поля задается константой <see cref="DefaultStatusField"/>.
        /// </summary>
        public IStatus Get(object value, DateTime? validFrom, DateTime? validTo)
        {
            return new Status(new List<StatusVersion> { new StatusVersion(DefaultStatusField, value, validFrom, validTo) });
        }

        /// <summary>
        /// Создать статус с единственным полем в ключе.
        /// </summary>
        public IStatus Get(string field, object value, DateTime? validFrom, DateTime? validTo)
        {
            return new Status(new List<StatusVersion> { new StatusVersion(field, value, validFrom, validTo) });
        }

        public IStatus Get(StatusVersion status)
        {
            return new Status(new List<StatusVersion> { status });
        }

        public IStatus Get(IEnumerable<StatusVersion> statuses)
        {
            return new Status(statuses);
        }
    }
}
