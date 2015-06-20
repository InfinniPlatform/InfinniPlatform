using System;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.Application.Contracts;

namespace InfinniPlatform.SystemConfig.Configurator
{
    /// <summary>
    ///     Поиск объекта метаданных по наименованию, если оно предоставлено в качестве идентификатора
    ///     Возвращается первый найденный элемент
    /// </summary>
    public sealed class ActionUnitFilterMetadata
    {
        public void Action(IProcessEventContext target)
        {
            if (target.Id == null)
            {
                return;
            }

            Guid guid;

            if (!Guid.TryParse(target.Id, out guid))
            {
                var eventNameMetadata = target.Id;

                var managerIdentifiers =
                    target.Context.GetComponent<ISystemComponent>(target.Version).ManagerIdentifiers;

                target.Id = managerIdentifiers.GetConfigurationUid(target.Version, eventNameMetadata);
            }

            dynamic document;

            if (!string.IsNullOrEmpty(target.Id))
            {
                var documentApi = target.Context.GetComponent<DocumentApi>(target.Version);

                document = documentApi.GetDocument(target.Id);
                if (document != null)
                {
                    target.Item = document;
                }
            }
        }
    }
}