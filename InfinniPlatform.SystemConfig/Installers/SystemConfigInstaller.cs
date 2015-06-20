using InfinniPlatform.Api.Actions;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Hosting.Implementation.ExtensionPointHandling;
using InfinniPlatform.Hosting.Implementation.Modules;
using InfinniPlatform.Metadata;

namespace InfinniPlatform.SystemConfig.Installers
{
    public sealed class SystemConfigInstaller : MetadataConfigurationInstaller
    {
        public SystemConfigInstaller(IMetadataConfigurationProvider metadataConfigurationProvider,
                                     IScriptConfiguration actionConfiguration)
            : base(metadataConfigurationProvider, actionConfiguration)
        {
        }

        protected override void RegisterConfiguration(IMetadataConfiguration metadataConfiguration)
        {
            IScriptConfiguration actionUnits = metadataConfiguration.ScriptConfiguration;

            //обновление
            //-------------------

            actionUnits.RegisterActionUnitDistributedStorage("updateconfigfromjson", "ActionUnitUpdateConfigFromJson");
            actionUnits.RegisterActionUnitDistributedStorage("refreshmetadatacache", "ActionUnitRefreshMetadataCache");
            actionUnits.RegisterActionUnitDistributedStorage("clearcontrollerscache", "ActionUnitClearControllersCache");
            //-------------------

            //help configuration
            actionUnits.RegisterActionUnitDistributedStorage("helpconfiguration", "ActionUnitHelpConfiguration");
            actionUnits.RegisterActionUnitDistributedStorage("generatehelpconfiguration",
                                                             "ActionUnitGenerateHelpConfiguration");

            actionUnits.RegisterActionUnitDistributedStorage("changemetadata", "ActionUnitChangeMetadata");
            actionUnits.RegisterActionUnitDistributedStorage("getmetadata", "ActionUnitGetMetadata");
            actionUnits.RegisterActionUnitDistributedStorage("getmanagedmetadata", "ActionUnitGetManagedMetadata");
            actionUnits.RegisterActionUnitDistributedStorage("deletemetadata", "ActionUnitDeleteMetadata");

            actionUnits.RegisterActionUnitDistributedStorage("getitemid", "ActionUnitGetItemId");
            actionUnits.RegisterActionUnitDistributedStorage("filtermetadata", "ActionUnitFilterMetadata");


            actionUnits.RegisterActionUnitDistributedStorage("getservicemetadata", "ActionUnitGetServiceMetadata");
            actionUnits.RegisterActionUnitDistributedStorage("getstandardextensionpoints",
                                                             "ActionUnitGetStandardExtensionPoints");
            actionUnits.RegisterActionUnitDistributedStorage("getversionlist", "ActionUnitGetVersionList");
            actionUnits.RegisterActionUnitDistributedStorage("getinstalledconfigurations",
                                                             "ActionUnitGetInstalledConfigurations");

            actionUnits.RegisterActionUnitDistributedStorage("GenerateMetadata", "ActionUnitGenerateMetadata");
            actionUnits.RegisterActionUnitDistributedStorage("GenerateServiceWithoutState",
                                                             "ActionUnitGenerateServiceWithoutState");
            actionUnits.RegisterActionUnitDistributedStorage("Creategenerator", "ActionUnitCreateGenerator");
            actionUnits.RegisterActionUnitDistributedStorage("DeleteGeneratedService",
                                                             "ActionUnitDeleteGeneratedService");
            actionUnits.RegisterActionUnitDistributedStorage("Creategenerator", "ActionUnitCreateGenerator");
            actionUnits.RegisterActionUnitDistributedStorage("DeleteGenerator", "ActionUnitDeleteGenerator");
            actionUnits.RegisterActionUnitDistributedStorage("PostRegisterEntries", "ActionUnitPostRegisterEntries");
            actionUnits.RegisterActionUnitDistributedStorage("DeleteRegisterEntry", "ActionUnitDeleteRegisterEntry");
            actionUnits.RegisterActionUnitDistributedStorage("CreateRegisterEntry", "ActionUnitCreateRegisterEntry");

            //экспорт-импорт данных
            actionUnits.RegisterActionUnitDistributedStorage("exportdatatojson", "ActionUnitExportDataToJson");
            actionUnits.RegisterActionUnitDistributedStorage("importdatafromjson", "ActionUnitImportDataFromJson");

            //получение метаданных конфигураций
            actionUnits.RegisterActionUnitDistributedStorage("getregisteredconfiglist", "ActionUnitGetConfigList");
            actionUnits.RegisterActionUnitDistributedStorage("getconfigurationmetadata",
                                                             "ActionUnitGetConfigurationMetadata");
            actionUnits.RegisterActionUnitDistributedStorage("GetDocumentElementListMetadata",
                                                             "ActionUnitGetDocumentElementListMetadata");
            actionUnits.RegisterActionUnitDistributedStorage("GetDocumentElementMetadata",
                                                             "ActionUnitGetDocumentElementMetadata");
            actionUnits.RegisterActionUnitDistributedStorage("GetDocumentListMetadata",
                                                             "ActionUnitGetDocumentListMetadata");
            //actionUnits.RegisterActionUnitDistributedStorage("GetDocumentMetadata", "ActionUnitGetDocumentMetadata");
            actionUnits.RegisterActionUnitDistributedStorage("GetMenuListMetadata", "ActionUnitGetMenuListMetadata");
            //actionUnits.RegisterActionUnitDistributedStorage("GetMenuMetadata", "ActionUnitGetMenuMetadata");
            actionUnits.RegisterActionUnitDistributedStorage("GetValidationErrorMetadata",
                                                             "ActionUnitGetValidationErrorMetadata");
            actionUnits.RegisterActionUnitDistributedStorage("GetValidationWarningMetadata",
                                                             "ActionUnitGetValidationWarningMetadata");

            //работа с регистрами
            actionUnits.RegisterActionUnitDistributedStorage("GetRegisterValuesByDate",
                                                             "ActionUnitGetRegisterValuesByDate");
            actionUnits.RegisterActionUnitDistributedStorage("GetRegisterValuesBetweenDates",
                                                             "ActionUnitGetRegisterValuesBetweenDates");
            actionUnits.RegisterActionUnitDistributedStorage("GetRegisterValuesByRegistrar",
                                                             "ActionUnitGetRegisterValuesByRegistrar");
            actionUnits.RegisterActionUnitDistributedStorage("GetRegisterValuesByRegistrarType",
                                                             "ActionUnitGetRegisterValuesByRegistrarType");
            actionUnits.RegisterActionUnitDistributedStorage("GetClosestDateTimeOfTotalCalculation",
                                                             "ActionUnitGetClosestDateTimeOfTotalCalculation");
            actionUnits.RegisterActionUnitDistributedStorage("RecarryingRegisterEntries",
                                                             "ActionUnitRecarryingRegisterEntries");
            actionUnits.RegisterActionUnitDistributedStorage("GetRegisterValuesByPeriods",
                                                             "ActionUnitGetRegisterValuesByPeriods");

            //миграции и верификации
            actionUnits.RegisterActionUnitDistributedStorage("getmigrations", "ActionUnitGetMigrations");
            actionUnits.RegisterActionUnitDistributedStorage("getmigrationdetails", "ActionUnitGetMigrationDetails");
            actionUnits.RegisterActionUnitDistributedStorage("getverifications", "ActionUnitGetVerifications");
            actionUnits.RegisterActionUnitDistributedStorage("runmigration", "ActionUnitRunMigration");
            actionUnits.RegisterActionUnitDistributedStorage("revertmigration", "ActionUnitRevertMigration");
            actionUnits.RegisterActionUnitDistributedStorage("runverification", "ActionUnitRunVerification");

            //работа с отчетами
            actionUnits.RegisterActionUnitDistributedStorage("getreport", "ActionUnitGetReport");
            actionUnits.RegisterActionUnitDistributedStorage("getprintview", "ActionUnitGetPrintView");

            //работа с авторизацией
            actionUnits.RegisterActionUnitDistributedStorage("checkauthorizationconfig",
                                                             "ActionUnitCheckAuthorizationStorage");

            //регистрируем модули предзаполнения полей

            var entryList = new ActionUnitsEntryList(GetType().Assembly, ActionUnitsEntryList.PrefillIndex);
            foreach (var entry in entryList.Entries)
            {
                actionUnits.RegisterActionUnitDistributedStorage(entry.Key, entry.Value);
            }


            actionUnits.RegisterActionUnitDistributedStorage("GetFillItems", "ActionUnitGetFillItems");
            metadataConfiguration.RegisterWorkflow("prefill", "GetFillItems",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "GetFillItems")))));

            //регистрация бизнес-процессов для получения отчетов

            metadataConfiguration.RegisterWorkflow("reporting", "getreport",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "getreport")))));

            metadataConfiguration.RegisterWorkflow("reporting", "getprintview",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "getprintview")))));

            //регистрация бизнес-процессов получения метаданных
            //-----------------------------------------------
            metadataConfiguration.RegisterWorkflow("metadata", "getregisteredconfiglist",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "getregisteredconfiglist")))));

            metadataConfiguration.RegisterWorkflow("metadata", "getconfigurationmetadata",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "getconfigurationmetadata")))));

            metadataConfiguration.RegisterWorkflow("metadata", "GetDocumentElementListMetadata",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "GetDocumentElementListMetadata")))));

            metadataConfiguration.RegisterWorkflow("metadata", "GetDocumentElementMetadata",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "GetDocumentElementMetadata")))));

            metadataConfiguration.RegisterWorkflow("metadata", "GetDocumentListMetadata",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "GetDocumentListMetadata")))));

            //metadataConfiguration.RegisterWorkflow("metadata", "GetDocumentMetadata",
            //    f => f.FlowWithoutState(wc => wc
            //        .Move(ws => ws
            //            .WithAction(() => actionUnits.GetAction("GetDocumentMetadata")))));

            metadataConfiguration.RegisterWorkflow("metadata", "GetMenuListMetadata",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "GetMenuListMetadata")))));

            //metadataConfiguration.RegisterWorkflow("metadata", "GetMenuMetadata",
            //	f => f.FlowWithoutState(wc => wc
            //		.Move(ws => ws
            //			.WithAction(() => actionUnits.GetAction("GetMenuMetadata")))));

            metadataConfiguration.RegisterWorkflow("metadata", "GetValidationWarningMetadata",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "GetValidationWarningMetadata")))));

            metadataConfiguration.RegisterWorkflow("metadata", "GetValidationErrorMetadata",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "GetValidationErrorMetadata")))));

            metadataConfiguration.RegisterWorkflow("metadata", "GetRegisterValuesByDate",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "GetRegisterValuesByDate")))));

            metadataConfiguration.RegisterWorkflow("metadata", "GetRegisterValuesBetweenDates",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "GetRegisterValuesBetweenDates")))));

            metadataConfiguration.RegisterWorkflow("metadata", "GetRegisterValuesByPeriods",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "GetRegisterValuesByPeriods")))));

            metadataConfiguration.RegisterWorkflow("metadata", "GetRegisterValuesByRegistrar",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "GetRegisterValuesByRegistrar")))));

            metadataConfiguration.RegisterWorkflow("metadata", "GetRegisterValuesByRegistrarType",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "GetRegisterValuesByRegistrarType")))));

            metadataConfiguration.RegisterWorkflow("metadata", "GetClosestDateTimeOfTotalCalculation",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "GetClosestDateTimeOfTotalCalculation")))));

            metadataConfiguration.RegisterWorkflow("metadata", "RecarryingRegisterEntries",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "RecarryingRegisterEntries")))));
            //-----------------------------------------------


            metadataConfiguration.RegisterWorkflow("update", "updateconfigfromjson",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "updateconfigfromjson")))));

            metadataConfiguration.RegisterWorkflow("update", "getversionlist",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "getversionlist")))));
            metadataConfiguration.RegisterWorkflow("update", "getinstalledconfigurations",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "getinstalledconfigurations")))));

            metadataConfiguration.RegisterWorkflow("metadata", "refreshmetadatacache",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "refreshmetadatacache")))));

            metadataConfiguration.RegisterWorkflow("metadata", "clearcontrollerscache",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "clearcontrollerscache")))));

            metadataConfiguration.RegisterWorkflow("metadata", "changemetadata",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "changemetadata")))));

            metadataConfiguration.RegisterWorkflow("metadata", "deletemetadata",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "deletemetadata")))));

            metadataConfiguration.RegisterWorkflow("metadata", "getmetadata",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "getmetadata")))));

            metadataConfiguration.RegisterWorkflow("metadata", "getmanagedmetadata",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "getmanagedmetadata")))));

            metadataConfiguration.RegisterWorkflow("metadata", "getitemid",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "getitemid")))));

            metadataConfiguration.RegisterWorkflow("metadata", "filtermetadata",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "filtermetadata")))));

            metadataConfiguration.RegisterWorkflow("metadata", "getservicemetadata",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "getservicemetadata")))));

            metadataConfiguration.RegisterWorkflow("metadata", "getstandardextensionpoints",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "getstandardextensionpoints")))));


            metadataConfiguration.RegisterWorkflow("metadata", "helpconfiguration",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "helpconfiguration")))));

            metadataConfiguration.RegisterWorkflow("metadata", "generatehelpconfiguration",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "generatehelpconfiguration")))));


            metadataConfiguration.RegisterWorkflow("metadata", "GenerateMetadata",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "GenerateMetadata")))));

            metadataConfiguration.RegisterWorkflow("metadata", "GenerateServiceWithoutState",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "GenerateServiceWithoutState")))));

            metadataConfiguration.RegisterWorkflow("metadata", "DeleteGeneratedService",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "DeleteGeneratedService")))));

            metadataConfiguration.RegisterWorkflow("metadata", "Creategenerator",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "Creategenerator")))));

            metadataConfiguration.RegisterWorkflow("metadata", "Deletegenerator",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "Deletegenerator")))));

            metadataConfiguration.RegisterWorkflow("metadata", "PostRegisterEntries",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "PostRegisterEntries")))));

            metadataConfiguration.RegisterWorkflow("metadata", "CreateRegisterEntry",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "CreateRegisterEntry")))));

            metadataConfiguration.RegisterWorkflow("metadata", "DeleteRegisterEntry",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "DeleteRegisterEntry")))));

            metadataConfiguration.RegisterWorkflow("metadata", "exportdatatojson",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "exportdatatojson")))));

            metadataConfiguration.RegisterWorkflow("metadata", "importdatafromjson",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "importdatafromjson")))));

            metadataConfiguration.RegisterWorkflow("metadata", "getmigrations",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "getmigrations")))));

            metadataConfiguration.RegisterWorkflow("metadata", "getmigrationdetails",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "getmigrationdetails")))));

            metadataConfiguration.RegisterWorkflow("metadata", "getverifications",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "getverifications")))));

            metadataConfiguration.RegisterWorkflow("metadata", "runmigration",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "runmigration")))));

            metadataConfiguration.RegisterWorkflow("metadata", "revertmigration",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "revertmigration")))));

            metadataConfiguration.RegisterWorkflow("metadata", "runverification",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "runverification")))));

            metadataConfiguration.RegisterWorkflow("metadata", "checkauthorizationconfig",
                                                   f => f.FlowWithoutState(wc => wc
                                                                                     .Move(ws => ws
                                                                                                     .WithAction(
                                                                                                         () =>
                                                                                                         actionUnits
                                                                                                             .GetAction(
                                                                                                                 "checkauthorizationconfig")))));

            //создаем индексы для хранения метаданных всех типов
            foreach (string metadataType in MetadataType.GetContainedMetadataTypes())
            {
                string metadataTypeMetadata = string.Format("{0}metadata", metadataType);
                metadataConfiguration.RegisterWorkflow(metadataTypeMetadata, "changemetadata",
                                                       f => f.FlowWithoutState(wc => wc
                                                                                         .Move(ws => ws
                                                                                                         .WithAction(
                                                                                                             () =>
                                                                                                             actionUnits
                                                                                                                 .GetAction
                                                                                                                 ("changemetadata")))));
                metadataConfiguration.RegisterWorkflow(metadataTypeMetadata, "filtermetadata",
                                                       f => f.FlowWithoutState(wc => wc
                                                                                         .Move(ws => ws
                                                                                                         .WithAction(
                                                                                                             () =>
                                                                                                             actionUnits
                                                                                                                 .GetAction
                                                                                                                 ("filtermetadata")))));
                metadataConfiguration.RegisterWorkflow(metadataTypeMetadata, "deletemetadata",
                                                       f => f.FlowWithoutState(wc => wc
                                                                                         .Move(ws => ws
                                                                                                         .WithAction(
                                                                                                             () =>
                                                                                                             actionUnits
                                                                                                                 .GetAction
                                                                                                                 ("deletemetadata")))));
                metadataConfiguration.RegisterWorkflow(metadataTypeMetadata, "getitemid",
                                                       f => f.FlowWithoutState(wc => wc
                                                                                         .Move(ws => ws
                                                                                                         .WithAction(
                                                                                                             () =>
                                                                                                             actionUnits
                                                                                                                 .GetAction
                                                                                                                 ("getitemid")))));

                metadataConfiguration.SetMetadataIndexType(metadataTypeMetadata, metadataTypeMetadata);
            }

            metadataConfiguration.SetMetadataIndexType("metadata", "metadata");
        }

        protected override void RegisterServices(IServiceRegistrationContainer servicesConfiguration)
        {
            servicesConfiguration.AddRegistration("update", "Upload", reg => reg
                                                                                 .RegisterHandlerInstance(
                                                                                     "updateconfigfromjson",
                                                                                     instance =>
                                                                                     instance.RegisterExtensionPoint(
                                                                                         "Upload",
                                                                                         "updateconfigfromjson"))
                                                                                 .SetResultHandler(
                                                                                     HttpResultHandlerType.BadRequest));
            servicesConfiguration.AddRegistration("reporting", "UrlEncodedData",
                                                  reg => reg.RegisterHandlerInstance("GetReport", instance => instance
                                                                                                                  .RegisterExtensionPoint
                                                                                                                  ("ProcessUrlEncodedData",
                                                                                                                   "GetReport"))
                                                            .SetResultHandler(HttpResultHandlerType.ByteContent));
            servicesConfiguration.AddRegistration("reporting", "UrlEncodedData",
                                                  reg =>
                                                  reg.RegisterHandlerInstance("GetPrintView", instance => instance
                                                                                                              .RegisterExtensionPoint
                                                                                                              ("ProcessUrlEncodedData",
                                                                                                               "GetPrintView"))
                                                     .SetResultHandler(HttpResultHandlerType.ByteContent));


            servicesConfiguration.AddRegistration("update", "Search", reg => reg
                                                                                 .RegisterHandlerInstance(
                                                                                     "getversionlist",
                                                                                     instance =>
                                                                                     instance.RegisterExtensionPoint(
                                                                                         "SearchModel", "getversionlist"))
                                                                                 .SetResultHandler(
                                                                                     HttpResultHandlerType.BadRequest)
                                                                                 .RegisterHandlerInstance(
                                                                                     "getinstalledconfigurations",
                                                                                     instance =>
                                                                                     instance.RegisterExtensionPoint(
                                                                                         "SearchModel",
                                                                                         "getinstalledconfigurations"))
                                                                                 .SetResultHandler(
                                                                                     HttpResultHandlerType.BadRequest));
            servicesConfiguration.AddRegistration("metadata", "Search", reg => reg
                                                                                   .RegisterHandlerInstance(
                                                                                       "getservicemetadata",
                                                                                       instance =>
                                                                                       instance.RegisterExtensionPoint(
                                                                                           "SearchModel",
                                                                                           "getservicemetadata"))
                                                                                   .SetResultHandler(
                                                                                       HttpResultHandlerType.BadRequest)
                                                                                   .RegisterHandlerInstance(
                                                                                       "getstandardextensionpoints",
                                                                                       instance =>
                                                                                       instance.RegisterExtensionPoint(
                                                                                           "SearchModel",
                                                                                           "getstandardextensionpoints"))
                                                                                   .SetResultHandler(
                                                                                       HttpResultHandlerType.BadRequest));
            servicesConfiguration.AddRegistration("classifier", "ApplyJson", reg => reg
                                                                                        .RegisterHandlerInstance(
                                                                                            "getdata",
                                                                                            instance =>
                                                                                            instance
                                                                                                .RegisterExtensionPoint(
                                                                                                    "GetResult",
                                                                                                    "GetClassifierList"))
                                                                                        .SetResultHandler(
                                                                                            HttpResultHandlerType
                                                                                                .BadRequest));

            //servicesConfiguration.AddRegistration("metadata", "Help", reg => reg
            //	.RegisterHandlerInstance("helpconfiguration", instance => instance
            //		.RegisterExtensionPoint("GetHelp", "helpconfiguration"))
            //	.SetResultHandler(HttpResultHandlerType.Html));
            servicesConfiguration.AddRegistration("metadata", "ApplyJson", reg => reg
                                                                                      .RegisterHandlerInstance(
                                                                                          "RefreshMetadataCache",
                                                                                          instance => instance
                                                                                                          .RegisterExtensionPoint
                                                                                                          ("Move",
                                                                                                           "RefreshMetadataCache")
                                                                                      )
                                                                                      .SetResultHandler(
                                                                                          HttpResultHandlerType
                                                                                              .BadRequest));

            servicesConfiguration.AddRegistration("metadata", "ApplyJson", reg => reg
                                                                                      .RegisterHandlerInstance(
                                                                                          "clearcontrollerscache",
                                                                                          instance => instance
                                                                                                          .RegisterExtensionPoint
                                                                                                          ("Move",
                                                                                                           "clearcontrollerscache")
                                                                                      )
                                                                                      .SetResultHandler(
                                                                                          HttpResultHandlerType
                                                                                              .BadRequest));

            servicesConfiguration.AddRegistration("metadata", "ApplyJson", reg => reg
                                                                                      .RegisterHandlerInstance(
                                                                                          "generatehelpconfiguration",
                                                                                          instance => instance
                                                                                                          .RegisterExtensionPoint
                                                                                                          ("Move",
                                                                                                           "generatehelpconfiguration"))
                                                                                      .SetResultHandler(
                                                                                          HttpResultHandlerType
                                                                                              .BadRequest));

            //изменение метаданных нельзя описывать с помощью JSON объектов, т.к. невозможно представить в виде объекта
            //добавление элемента в коллекцию или другие частичные изменения
            servicesConfiguration.AddRegistration("metadata", "ApplyEvents", reg => reg
                                                                                        .RegisterHandlerInstance(
                                                                                            "changemetadata",
                                                                                            instance => instance
                                                                                                            .RegisterExtensionPoint
                                                                                                            ("FilterEvents",
                                                                                                             "filtermetadata")
                                                                                                            .RegisterExtensionPoint
                                                                                                            ("Move",
                                                                                                             "changemetadata"))
                                                                                        .SetResultHandler(
                                                                                            HttpResultHandlerType
                                                                                                .BadRequest));

            servicesConfiguration.AddRegistration("metadata", "Aggregation",
                                                  reg =>
                                                  reg.RegisterHandlerInstance("aggregate")
                                                     .SetResultHandler(HttpResultHandlerType.BadRequest));

            servicesConfiguration.AddRegistration("metadata", "ApplyJson", reg => reg
                                                                                      .RegisterHandlerInstance(
                                                                                          "deletemetadata",
                                                                                          instance => instance
                                                                                                          .RegisterExtensionPoint
                                                                                                          ("FilterEvents",
                                                                                                           "filtermetadata")
                                                                                                          .RegisterExtensionPoint
                                                                                                          ("Move",
                                                                                                           "deletemetadata"))
                                                                                      .RegisterHandlerInstance(
                                                                                          "GenerateServiceWithoutState",
                                                                                          instance => instance
                                                                                                          .RegisterExtensionPoint
                                                                                                          ("Move",
                                                                                                           "GenerateServiceWithoutState"))
                                                                                      .RegisterHandlerInstance(
                                                                                          "CreateGenerator",
                                                                                          instance => instance
                                                                                                          .RegisterExtensionPoint
                                                                                                          ("Move",
                                                                                                           "CreateGenerator"))
                                                                                      .RegisterHandlerInstance(
                                                                                          "DeleteGeneratedService",
                                                                                          instance => instance
                                                                                                          .RegisterExtensionPoint
                                                                                                          ("Move",
                                                                                                           "DeleteGeneratedService"))
                                                                                      .RegisterHandlerInstance(
                                                                                          "DeleteGenerator",
                                                                                          instance => instance
                                                                                                          .RegisterExtensionPoint
                                                                                                          ("Move",
                                                                                                           "DeleteGenerator"))
                                                                                      .RegisterHandlerInstance(
                                                                                          "PostRegisterEntries",
                                                                                          instance => instance
                                                                                                          .RegisterExtensionPoint
                                                                                                          ("Move",
                                                                                                           "PostRegisterEntries"))
                                                                                      .RegisterHandlerInstance(
                                                                                          "RecarryingRegisterEntries",
                                                                                          instance => instance
                                                                                                          .RegisterExtensionPoint
                                                                                                          ("Move",
                                                                                                           "RecarryingRegisterEntries"))
                                                                                      .RegisterHandlerInstance(
                                                                                          "DeleteRegisterEntry",
                                                                                          instance => instance
                                                                                                          .RegisterExtensionPoint
                                                                                                          ("Move",
                                                                                                           "DeleteRegisterEntry"))
                                                                                      .RegisterHandlerInstance(
                                                                                          "getmetadata",
                                                                                          instance => instance
                                                                                                          .RegisterExtensionPoint
                                                                                                          ("GetResult",
                                                                                                           "getmetadata"))
                                                                                      .RegisterHandlerInstance(
                                                                                          "getmanagedmetadata",
                                                                                          instance => instance
                                                                                                          .RegisterExtensionPoint
                                                                                                          ("Move",
                                                                                                           "getmanagedmetadata"))
                                                                                      .RegisterHandlerInstance(
                                                                                          "getitemid",
                                                                                          instance => instance
                                                                                                          .RegisterExtensionPoint
                                                                                                          ("GetResult",
                                                                                                           "getitemid"))
                                                                                      .RegisterHandlerInstance(
                                                                                          "generatemetadata",
                                                                                          instance => instance
                                                                                                          .RegisterExtensionPoint
                                                                                                          ("Move",
                                                                                                           "generatemetadata"))
                                                                                      .RegisterHandlerInstance(
                                                                                          "exportdatatojson",
                                                                                          instance => instance
                                                                                                          .RegisterExtensionPoint
                                                                                                          ("Move",
                                                                                                           "exportdatatojson"))
                                                                                      .RegisterHandlerInstance(
                                                                                          "importdatafromjson",
                                                                                          instance => instance
                                                                                                          .RegisterExtensionPoint
                                                                                                          ("Move",
                                                                                                           "importdatafromjson"))
                                                                                      .RegisterHandlerInstance(
                                                                                          "getregisteredconfiglist",
                                                                                          instance =>
                                                                                          instance
                                                                                              .RegisterExtensionPoint(
                                                                                                  "GetResult",
                                                                                                  "getregisteredconfiglist"))
                                                                                      .RegisterHandlerInstance(
                                                                                          "getconfigurationmetadata",
                                                                                          instance =>
                                                                                          instance
                                                                                              .RegisterExtensionPoint(
                                                                                                  "GetResult",
                                                                                                  "getconfigurationmetadata"))
                                                                                      .RegisterHandlerInstance(
                                                                                          "GetDocumentElementListMetadata",
                                                                                          instance =>
                                                                                          instance
                                                                                              .RegisterExtensionPoint(
                                                                                                  "GetResult",
                                                                                                  "GetDocumentElementListMetadata"))
                                                                                      .RegisterHandlerInstance(
                                                                                          "GetDocumentElementMetadata",
                                                                                          instance =>
                                                                                          instance
                                                                                              .RegisterExtensionPoint(
                                                                                                  "GetResult",
                                                                                                  "GetDocumentElementMetadata"))
                                                                                      .RegisterHandlerInstance(
                                                                                          "GetDocumentListMetadata",
                                                                                          instance =>
                                                                                          instance
                                                                                              .RegisterExtensionPoint(
                                                                                                  "GetResult",
                                                                                                  "GetDocumentListMetadata"))
                                                                                      .RegisterHandlerInstance(
                                                                                          "GetDocumentMetadata",
                                                                                          instance =>
                                                                                          instance
                                                                                              .RegisterExtensionPoint(
                                                                                                  "GetResult",
                                                                                                  "GetDocumentMetadata"))
                                                                                      //.RegisterHandlerInstance("GetMenuListMetadata",
                                                                                      //	instance => instance.RegisterExtensionPoint("GetResult", "GetMenuListMetadata"))
                                                                                      //.RegisterHandlerInstance("GetMenuMetadata",
                                                                                      //	instance => instance.RegisterExtensionPoint("GetResult", "GetMenuMetadata"))
                                                                                      //.RegisterHandlerInstance("GetValidationWarningMetadata",
                                                                                      //	instance => instance.RegisterExtensionPoint("GetResult", "GetValidationWarningMetadata"))
                                                                                      //.RegisterHandlerInstance("GetValidationErrorMetadata",
                                                                                      //	instance => instance.RegisterExtensionPoint("GetResult", "GetValidationErrorMetadata"))
                                                                                      .RegisterHandlerInstance(
                                                                                          "GetRegisterValuesByDate",
                                                                                          instance =>
                                                                                          instance
                                                                                              .RegisterExtensionPoint(
                                                                                                  "GetResult",
                                                                                                  "GetRegisterValuesByDate"))
                                                                                      .RegisterHandlerInstance(
                                                                                          "GetRegisterValuesBetweenDates",
                                                                                          instance =>
                                                                                          instance
                                                                                              .RegisterExtensionPoint(
                                                                                                  "GetResult",
                                                                                                  "GetRegisterValuesBetweenDates"))
                                                                                      .RegisterHandlerInstance(
                                                                                          "GetRegisterValuesByPeriods",
                                                                                          instance =>
                                                                                          instance
                                                                                              .RegisterExtensionPoint(
                                                                                                  "GetResult",
                                                                                                  "GetRegisterValuesByPeriods"))
                                                                                      .RegisterHandlerInstance(
                                                                                          "GetRegisterValuesByRegistrar",
                                                                                          instance =>
                                                                                          instance
                                                                                              .RegisterExtensionPoint(
                                                                                                  "GetResult",
                                                                                                  "GetRegisterValuesByRegistrar"))
                                                                                      .RegisterHandlerInstance(
                                                                                          "GetRegisterValuesByRegistrarType",
                                                                                          instance =>
                                                                                          instance
                                                                                              .RegisterExtensionPoint(
                                                                                                  "GetResult",
                                                                                                  "GetRegisterValuesByRegistrarType"))
                                                                                      .RegisterHandlerInstance(
                                                                                          "GetClosestDateTimeOfTotalCalculation",
                                                                                          instance =>
                                                                                          instance
                                                                                              .RegisterExtensionPoint(
                                                                                                  "GetResult",
                                                                                                  "GetClosestDateTimeOfTotalCalculation"))
                                                                                      .RegisterHandlerInstance(
                                                                                          "CreateRegisterEntry",
                                                                                          instance =>
                                                                                          instance
                                                                                              .RegisterExtensionPoint(
                                                                                                  "GetResult",
                                                                                                  "CreateRegisterEntry"))
                                                                                      .RegisterHandlerInstance(
                                                                                          "getmigrations",
                                                                                          instance =>
                                                                                          instance
                                                                                              .RegisterExtensionPoint(
                                                                                                  "Move",
                                                                                                  "getmigrations"))
                                                                                      .RegisterHandlerInstance(
                                                                                          "getmigrationdetails",
                                                                                          instance =>
                                                                                          instance
                                                                                              .RegisterExtensionPoint(
                                                                                                  "Move",
                                                                                                  "getmigrationdetails"))
                                                                                      .RegisterHandlerInstance(
                                                                                          "getverifications",
                                                                                          instance =>
                                                                                          instance
                                                                                              .RegisterExtensionPoint(
                                                                                                  "Move",
                                                                                                  "getverifications"))
                                                                                      .RegisterHandlerInstance(
                                                                                          "runmigration",
                                                                                          instance =>
                                                                                          instance
                                                                                              .RegisterExtensionPoint(
                                                                                                  "Move", "runmigration"))
                                                                                      .RegisterHandlerInstance(
                                                                                          "revertmigration",
                                                                                          instance =>
                                                                                          instance
                                                                                              .RegisterExtensionPoint(
                                                                                                  "Move",
                                                                                                  "revertmigration"))
                                                                                      .RegisterHandlerInstance(
                                                                                          "runverification",
                                                                                          instance =>
                                                                                          instance
                                                                                              .RegisterExtensionPoint(
                                                                                                  "Move",
                                                                                                  "runverification"))
                                                                                      .RegisterHandlerInstance(
                                                                                          "checkauthorizationconfig",
                                                                                          instance =>
                                                                                          instance
                                                                                              .RegisterExtensionPoint(
                                                                                                  "Move",
                                                                                                  "checkauthorizationconfig"))
                                                                                      .SetResultHandler(
                                                                                          HttpResultHandlerType
                                                                                              .BadRequest));

            foreach (string metadataType in MetadataType.GetContainedMetadataTypes())
            {
                string metadataTypeMetadata = string.Format("{0}metadata", metadataType);

                //изменение метаданных можно описать только с помощью событий
                servicesConfiguration.AddRegistration(metadataTypeMetadata, "ApplyEvents", reg => reg
                                                                                                      .RegisterHandlerInstance
                                                                                                      ("changemetadata",
                                                                                                       instance =>
                                                                                                       instance
                                                                                                           .RegisterExtensionPoint
                                                                                                           ("FilterEvents",
                                                                                                            "filtermetadata")
                                                                                                           .RegisterExtensionPoint
                                                                                                           ("Move",
                                                                                                            "changemetadata"))
                                                                                                      .SetResultHandler(
                                                                                                          HttpResultHandlerType
                                                                                                              .BadRequest));


                servicesConfiguration.AddRegistration(metadataTypeMetadata, "ApplyJson", reg => reg
                                                                                                    .RegisterHandlerInstance
                                                                                                    ("deletemetadata",
                                                                                                     instance =>
                                                                                                     instance
                                                                                                         .RegisterExtensionPoint
                                                                                                         ("FilterEvents",
                                                                                                          "filtermetadata")
                                                                                                         .RegisterExtensionPoint
                                                                                                         ("Move",
                                                                                                          "deletemetadata"))
                                                                                                    .RegisterHandlerInstance
                                                                                                    ("getmetadata",
                                                                                                     instance =>
                                                                                                     instance
                                                                                                         .RegisterExtensionPoint
                                                                                                         ("GetResult",
                                                                                                          "getmetadata"))
                                                                                                    .RegisterHandlerInstance
                                                                                                    ("getitemid",
                                                                                                     instance =>
                                                                                                     instance
                                                                                                         .RegisterExtensionPoint
                                                                                                         ("GetResult",
                                                                                                          "getitemid"))
                                                                                                    .SetResultHandler(
                                                                                                        HttpResultHandlerType
                                                                                                            .BadRequest));
            }

            servicesConfiguration.AddRegistration("prefill", "ApplyJson", reg => reg
                                                                                     .RegisterHandlerInstance(
                                                                                         "GetFillItems",
                                                                                         instance => instance
                                                                                                         .RegisterExtensionPoint
                                                                                                         ("GetResult",
                                                                                                          "GetFillItems"))
                                                                                     .SetResultHandler(
                                                                                         HttpResultHandlerType
                                                                                             .BadRequest));
        }
    }
}