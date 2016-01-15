using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Index;
using InfinniPlatform.Core.Metadata;

namespace InfinniPlatform.SystemConfig.StartupInitializers
{
    /// <summary>
    /// Загружает метаданные конфигурации в кэш.
    /// </summary>
    public sealed class PackageJsonConfigurationInstaller
    {
        public PackageJsonConfigurationInstaller(
            IEnumerable<dynamic> menu,
            IEnumerable<dynamic> registers,
            IEnumerable<dynamic> documents,
            IEnumerable<dynamic> scenario,
            IEnumerable<dynamic> views,
            IEnumerable<dynamic> printViews)
        {
            _documents = documents;
            _menu = menu;
            _scenario = scenario;
            _views = views;
            _printViews = printViews;
            _registers = registers;
        }

        private readonly IEnumerable<dynamic> _documents;
        private readonly IEnumerable<dynamic> _menu;
        private readonly IEnumerable<dynamic> _printViews;
        private readonly IEnumerable<dynamic> _registers;
        private readonly IEnumerable<dynamic> _scenario;
        private readonly IEnumerable<dynamic> _views;

        public void InstallConfiguration(IMetadataConfiguration metadataCacheManager)
        {
            foreach (var document in _documents)
            {
                string documentId = document.Name;

                SetCommonOptions(metadataCacheManager, document);
                RegisterSchema(metadataCacheManager, document);

                RegisterDocumentMetadataItems(metadataCacheManager, documentId, _scenario, RegisterScenario);
                RegisterDocumentMetadataItems(metadataCacheManager, documentId, _views, (c, d, i) => c.RegisterView(d, i));
                RegisterDocumentMetadataItems(metadataCacheManager, documentId, _printViews, (c, d, i) => c.RegisterPrintView(d, i));
            }

            foreach (var register in _registers)
            {
                metadataCacheManager.RegisterRegister(register);
            }

            metadataCacheManager.RegisterMenu(_menu);
        }

        private static void SetCommonOptions(IMetadataConfiguration metadataCacheManager, dynamic document)
        {
            string documentId = document.Name;

            if (document.MetadataIndex != null)
            {
                metadataCacheManager.SetMetadataIndexType(documentId, document.MetadataIndex);
            }

            if (document.SearchAbility == null)
            {
                metadataCacheManager.SetSearchAbilityType(documentId, SearchAbilityType.KeywordBasedSearch);
            }
            else
            {
                metadataCacheManager.SetSearchAbilityType(documentId, (SearchAbilityType)document.SearchAbility);
            }
        }

        private static void RegisterSchema(IMetadataConfiguration metadataCacheManager, dynamic document)
        {
            metadataCacheManager.SetSchemaVersion(document.Name, document.Schema);
        }

        private static void RegisterDocumentMetadataItems(IMetadataConfiguration metadataCacheManager, string documentId, IEnumerable<dynamic> metadataItems, Action<IMetadataConfiguration, string, object> registerItemFunc)
        {
            metadataItems = metadataItems.Where(i => i.DocumentId == documentId).ToArray();

            foreach (var metadataItem in metadataItems)
            {
                registerItemFunc(metadataCacheManager, documentId, metadataItem);
            }
        }

        private static void RegisterScenario(IMetadataConfiguration metadataCacheManager, string documentId, dynamic scenario)
        {
            metadataCacheManager.RegisterScenario(documentId, scenario);

            metadataCacheManager.ScriptConfiguration.RegisterAction(scenario.Id, scenario.ScenarioId);
        }
    }
}