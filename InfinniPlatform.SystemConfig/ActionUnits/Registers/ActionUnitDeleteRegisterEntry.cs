using System;

using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Environment.Register;

namespace InfinniPlatform.SystemConfig.ActionUnits.Registers
{
    /// <summary>
    ///     Точка расширения для удаления записи регистра по идентификатору документа-регистратора
    /// </summary>
    public sealed class ActionUnitDeleteRegisterEntry
    {
        public void Action(IApplyContext target)
        {
            string configuration = target.Item.Configuration;
            string registerId = target.Item.Register;
            string registar = target.Item.Registar;

            if (string.IsNullOrEmpty(configuration))
            {
                throw new ArgumentException("ConfigurationId name should be specified via 'configuration' property");
            }

            if (string.IsNullOrEmpty(registerId))
            {
                throw new ArgumentException("registerId should be specified via 'RegisterId' property");
            }

            if (string.IsNullOrEmpty(registar))
            {
                throw new ArgumentException("registar should be specified via 'Registar' property");
            }

            var api = target.Context.GetComponent<DocumentApi>();

            // Находим все записи в регистре, соответствующие регистратору
            var registerEntries = target.Context.GetComponent<RegisterApi>().GetRegisterEntries(
                configuration,
                registerId,
                f => f.AddCriteria(
                    c => c.Property(RegisterConstants.RegistrarProperty).IsEquals(registar)), 0, 10000);

            DateTime earliestDocumentDate = DateTime.MaxValue;

            foreach (var registerEntry in registerEntries)
            {
                api.DeleteDocument(configuration, RegisterConstants.RegisterNamePrefix + registerId, registerEntry.Id);

                var documentDate = registerEntry[RegisterConstants.DocumentDateProperty];
                if (documentDate < earliestDocumentDate)
                {
                    earliestDocumentDate = documentDate;
                }
            }

            // Необходимо удалить все записи регистра после earliestDocumentDate
            var notActualRegisterEntries = target.Context.GetComponent<RegisterApi>().GetRegisterEntries(
                configuration,
                registerId,
                f => f.AddCriteria(
                    c => c.Property(RegisterConstants.DocumentDateProperty).IsMoreThanOrEquals(earliestDocumentDate)), 0,
                10000);

            foreach (var registerEntry in notActualRegisterEntries)
            {
                api.DeleteDocument(configuration, RegisterConstants.RegisterNamePrefix + registerId, registerEntry.Id);
            }
        }
    }
}