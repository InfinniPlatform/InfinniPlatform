using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using InfinniPlatform.Api.DocumentStatus;
using InfinniPlatform.Metadata.Statuses.StatusAppliers;

namespace InfinniPlatform.Metadata.Statuses
{
    /// <summary>
    /// Представляет статус документа с readonly ключом (<see cref="System.Collections.ObjectModel.ReadOnlyDictionary"/>)
    /// <para>Статус представляет из себя набор значений определенных полей (составной ключ), и период, для которого этот статус актуален.</para>
    /// <para>При реализации поиска по статусу предполагается включение границ временного диапазона в критерии фильтрации</para>
    /// </summary>
    public sealed class Status : IStatus
    {
        public Status(IEnumerable<StatusVersion> versions)
        {
            var versionList = versions as List<StatusVersion> ?? versions.ToList();

            Versions = versionList.AsReadOnly();
        }

        public ReadOnlyCollection<StatusVersion> Versions { get; private set; }

        private static readonly IStatusApplier[] Appliers = new IStatusApplier[]
                                                                {
                                                                    new ExpandoStatusApplier(), 
                                                                    new FieldStatusApplier(), 
                                                                    new PropertyStatusApplier(), 
                                                                };

        public void ApplyTo(object target, DateTime? onDate = null)
        {
            var validStatus = Actual(onDate);

            if (!Appliers.Any(x => x.TryApply(validStatus, target)))
                throw new Exception(string.Format("Не удалось применить статус. Возможно, объект не содержит необходимого поля, либо поле не доступно для записи: {0}", validStatus.Field));
        }

        public bool CheckDocument(object target, DateTime? docDate = null)
        {
            var validStatus = Actual(docDate);

            return Appliers.Any(x => x.CheckValue(validStatus, target));
        }

        public StatusVersion Actual(DateTime? date = null)
        {
            var validDate = date ?? DateTime.Now;
            var validStatus = Versions.FirstOrDefault(x => (x.ValidFrom == null || x.ValidFrom <= validDate) &&
                                                           (x.ValidTo == null || x.ValidTo >= validDate));
            if (validStatus == null)
                throw new Exception(string.Format("Версия статуса, актуальная на {0}, не обнаружена", validDate));

            return validStatus;
        }
    }
}
