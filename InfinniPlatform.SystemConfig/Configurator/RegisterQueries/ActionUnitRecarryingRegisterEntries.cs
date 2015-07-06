using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Registers;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.SystemConfig.Configurator.RegisterQueries
{
    /// <summary>
    ///     Точка расширения для выполнения перепроведения документов до указанной даты
    /// </summary>
    public sealed class ActionUnitRecarryingRegisterEntries
    {
        // TODO: Механизм перепроведения нуждается в переработке!

        public void Action(IApplyContext target)
        {
            var endDate = target.Item.EndDate;
            string configurationId = target.Item.Configuration.ToString();
            string registerId = target.Item.Register.ToString();

            bool deteleExistingRegisterEntries = true;
            if (target.Item.DeteleExistingRegisterEntries != null &&
                target.Item.DeteleExistingRegisterEntries == false)
            {
                deteleExistingRegisterEntries = false;
            }

            // Один документ может создать две записи в одном регистре в ходе проведения, 
            // однако перепровести документ нужно только один раз. Для этого будем хранить
            // список идентификаторов уже перепроведенных документов
            var recarriedDocuments = new List<string>();

            int pageNumber = 0;

            while (true)
            {
                // Получаем записи из регистра постранично
                var registerEntries = target.Context.GetComponent<DocumentApi>().GetDocument(
                    configurationId,
                    RegisterConstants.RegisterNamePrefix + registerId,
                    f =>
                    f.AddCriteria(
                        c => c.Property(RegisterConstants.DocumentDateProperty).IsLessThanOrEquals(endDate)),
                    pageNumber++, 1000).ToArray();

                if (registerEntries.Length == 0)
                {
                    break;
                }

                // Перепроводить документы нужно все сразу после удаления соответствующих
                // записей из регистра. Поэтому сначала сохраняем пары
                // <Тип документа - содержимое документа> чтобы далее выполнить SetDocument для каждого элемента
                var documentsToRecarry = new List<Tuple<string, dynamic>>();

                foreach (var registerEntry in registerEntries)
                {
                    // Получаем документ-регистратор
                    string registrarId = registerEntry.Registrar;
                    string registrarType = registerEntry.RegistrarType;
                    var documentRegistrar =
                        target.Context.GetComponent<DocumentApi>()
                              .GetDocument(configurationId, registrarType,
                                           f => f.AddCriteria(c => c.Property("Id").IsEquals(registrarId)), 0, 1)
                              .FirstOrDefault();

                    if (deteleExistingRegisterEntries)
                    {
                        // Удаляем запись из регистра
                        new DocumentApi().DeleteDocument(configurationId,
                                                                       RegisterConstants.RegisterNamePrefix + registerId,
                                                                       registerEntry.Id);
                    }

                    if (documentRegistrar != null && !recarriedDocuments.Contains(registrarId))
                    {
                        documentsToRecarry.Add(new Tuple<string, dynamic>(registrarType, documentRegistrar));
                        recarriedDocuments.Add(registrarId);
                    }
                }

                foreach (var document in documentsToRecarry)
                {
                    // Перепроводка документа
                    target.Context.GetComponent<DocumentApi>()
                          .SetDocument(configurationId, document.Item1, document.Item2);
                }
            }

            // Удаляем значения из таблицы итогов
            var registerTotalEntries = target.Context.GetComponent<DocumentApi>().GetDocument(
                configurationId,
                RegisterConstants.RegisterTotalNamePrefix + registerId,
                f =>
                f.AddCriteria(c => c.Property(RegisterConstants.DocumentDateProperty).IsLessThanOrEquals(endDate)),
                0, 10000);

            foreach (var registerEntry in registerTotalEntries)
            {
                new DocumentApi().DeleteDocument(configurationId,
                                                               RegisterConstants.RegisterTotalNamePrefix + registerId,
                                                               registerEntry.Id);
            }
        }
    }
}