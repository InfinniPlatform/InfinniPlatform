using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Registers;
using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.Sdk;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.SystemConfig.ActionUnits.Registers
{
    /// <summary>
    /// Точка расширения для выполнения перепроведения документов до указанной даты
    /// </summary>
    public sealed class ActionUnitRecarryingRegisterEntries
    {
        public ActionUnitRecarryingRegisterEntries(DocumentApi documentApi)
        {
            _documentApi = documentApi;
        }

        private readonly DocumentApi _documentApi;


        // TODO: Механизм перепроведения нуждается в переработке!

        public void Action(IApplyContext target)
        {
            var endDate = target.Item.EndDate;
            string configurationId = target.Item.Configuration.ToString();
            string registerId = target.Item.Register.ToString();

            var deteleExistingRegisterEntries = true;
            if (target.Item.DeteleExistingRegisterEntries != null &&
                target.Item.DeteleExistingRegisterEntries == false)
            {
                deteleExistingRegisterEntries = false;
            }

            // Один документ может создать две записи в одном регистре в ходе проведения, 
            // однако перепровести документ нужно только один раз. Для этого будем хранить
            // список идентификаторов уже перепроведенных документов
            var recarriedDocuments = new List<string>();

            var pageNumber = 0;

            while (true)
            {
                // Получаем записи из регистра постранично
                Action<FilterBuilder> filter = f => f.AddCriteria(c => c.Property(RegisterConstants.DocumentDateProperty)
                                                                        .IsLessThanOrEquals(endDate));

                var registerEntries = _documentApi.GetDocument(
                    configurationId,
                    RegisterConstants.RegisterNamePrefix + registerId,
                    filter,
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
                        _documentApi
                              .GetDocument(configurationId, registrarType,
                                  f => f.AddCriteria(c => c.Property("Id").IsEquals(registrarId)), 0, 1)
                              .FirstOrDefault();

                    if (deteleExistingRegisterEntries)
                    {
                        // Удаляем запись из регистра
                        _documentApi.DeleteDocument(configurationId,
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
                    _documentApi
                          .SetDocument(configurationId, document.Item1, document.Item2);
                }
            }

            // Удаляем значения из таблицы итогов
            Action<FilterBuilder> action = f =>
                            f.AddCriteria(c => c.Property(RegisterConstants.DocumentDateProperty).IsLessThanOrEquals(endDate));
            var registerTotalEntries = _documentApi.GetDocument(
                configurationId,
                RegisterConstants.RegisterTotalNamePrefix + registerId,
                action,
                0, 10000);

            foreach (var registerEntry in registerTotalEntries)
            {
                _documentApi.DeleteDocument(configurationId,
                    RegisterConstants.RegisterTotalNamePrefix + registerId,
                    registerEntry.Id);
            }
        }
    }
}