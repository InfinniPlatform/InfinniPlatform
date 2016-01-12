using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Hosting;
using InfinniPlatform.Core.Index;
using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Hosting.Implementation.ExtensionPointHandling;

namespace InfinniPlatform.SystemConfig.StartupInitializers
{
    /// <summary>
    /// Загружает метаданные конфигурации в кэш.
    /// </summary>
    public sealed class PackageJsonConfigurationInstaller
    {
        public PackageJsonConfigurationInstaller(
            IEnumerable<dynamic> documents,
            IEnumerable<dynamic> menu,
            IEnumerable<dynamic> scenario,
            IEnumerable<dynamic> processes,
            IEnumerable<dynamic> services,
            IEnumerable<dynamic> generators,
            IEnumerable<dynamic> views,
            IEnumerable<dynamic> printViews,
            IEnumerable<dynamic> validationErrors,
            IEnumerable<dynamic> validationWarnings,
            IEnumerable<dynamic> registers
            )
        {
            _documents = documents;
            _menu = menu;
            _scenario = scenario;
            _processes = processes;
            _services = services;
            _generators = generators;
            _views = views;
            _printViews = printViews;
            _validationErrors = validationErrors;
            _validationWarnings = validationWarnings;
            _registers = registers;
        }


        private readonly IEnumerable<dynamic> _documents;
        private readonly IEnumerable<dynamic> _generators;
        private readonly IEnumerable<dynamic> _menu;
        private readonly IEnumerable<dynamic> _printViews;
        private readonly IEnumerable<dynamic> _processes;
        private readonly IEnumerable<dynamic> _registers;
        private readonly IEnumerable<dynamic> _scenario;
        private readonly IEnumerable<dynamic> _services;
        private readonly IEnumerable<dynamic> _validationErrors;
        private readonly IEnumerable<dynamic> _validationWarnings;
        private readonly IEnumerable<dynamic> _views;


        public void InstallConfiguration(IMetadataConfiguration metadataCacheManager)
        {
            foreach (var document in _documents)
            {
                string documentId = document.Name;

                SetCommonOptions(metadataCacheManager, document);
                RegisterSchema(metadataCacheManager, document);

                RegisterDocumentMetadataItems(metadataCacheManager, documentId, _scenario, RegisterScenario);
                RegisterDocumentMetadataItems(metadataCacheManager, documentId, _processes, (c, d, i) => c.RegisterProcess(d, i));
                RegisterDocumentMetadataItems(metadataCacheManager, documentId, _services, RegisterService);
                RegisterDocumentMetadataItems(metadataCacheManager, documentId, _generators, (c, d, i) => c.RegisterGenerator(d, i));
                RegisterDocumentMetadataItems(metadataCacheManager, documentId, _views, (c, d, i) => c.RegisterView(d, i));
                RegisterDocumentMetadataItems(metadataCacheManager, documentId, _printViews, (c, d, i) => c.RegisterPrintView(d, i));
                RegisterDocumentMetadataItems(metadataCacheManager, documentId, _validationErrors, (c, d, i) => c.RegisterValidationError(d, i));
                RegisterDocumentMetadataItems(metadataCacheManager, documentId, _validationWarnings, (c, d, i) => c.RegisterValidationWarning(d, i));
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

        private static void RegisterService(IMetadataConfiguration metadataCacheManager, string documentId, dynamic service)
        {
            metadataCacheManager.RegisterService(documentId, service);

            string serviceName = service.Name;
            string serviceTypeName = service.Type.Name;
            IEnumerable<dynamic> serviceExtensionPoints = service.ExtensionPoints;

            Action<IExtensionPointHandlerInstance> instanceAction
                = extensionPointHandlerInstance =>
                  {
                      if (serviceExtensionPoints != null)
                      {
                          foreach (var extensionPoint in serviceExtensionPoints)
                          {
                              if (extensionPoint.TypeName != null)
                              {
                                  string extensionPointType = extensionPoint.TypeName.Name;
                                  string extensionPointScenarioId = extensionPoint.ScenarioId;
                                  extensionPointHandlerInstance.RegisterExtensionPoint(extensionPointType, extensionPointScenarioId);
                              }
                          }
                      }
                  };

            Action<IServiceRegistration> registrationAction
                = serviceRegistration =>
                  {
                      serviceRegistration.RegisterHandlerInstance(serviceName, instanceAction);
                      serviceRegistration.SetResultHandler(HttpResultHandlerType.BadRequest);
                  };

            metadataCacheManager.ServiceRegistrationContainer.AddRegistration(documentId, serviceTypeName, registrationAction);
        }
    }
}