using System;
using System.Linq;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;

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

                var version = target.Events.Where(e => e.Property == "Version").Select(e => e.Value).FirstOrDefault() as string;

                var managerIdentifiers =
                    target.Context.GetComponent<ISystemComponent>().ManagerIdentifiers;

                if (target.Metadata != null && target.Metadata.ToLowerInvariant() == "solutionmetadata")
                {
                    target.Id = managerIdentifiers.GetSolutionUid(version, eventNameMetadata);
                }
                else
                {
                    target.Id = managerIdentifiers.GetConfigurationUid(version, eventNameMetadata);
                }
            }

            dynamic document;

            if (!string.IsNullOrEmpty(target.Id))
            {
                var documentApi = target.Context.GetComponent<DocumentApi>();

                document = documentApi.GetDocument(target.Id);
                if (document != null)
                {
                    target.Item = document;
                }
            }
        }
    }
}