using InfinniPlatform.Hosting.Implementation.ExtensionPointHandling;
using InfinniPlatform.Hosting.Implementation.Modules;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment.Hosting;
using InfinniPlatform.Sdk.Environment.Metadata;
using InfinniPlatform.SystemConfig.ActionUnits.Binary;
using InfinniPlatform.SystemConfig.ActionUnits.Documents;
using InfinniPlatform.SystemConfig.ActionUnits.Metadata;
using InfinniPlatform.SystemConfig.ActionUnits.Migrations;
using InfinniPlatform.SystemConfig.ActionUnits.Registers;
using InfinniPlatform.SystemConfig.ActionUnits.Session;

namespace InfinniPlatform.SystemConfig.RequestHandlers
{
    internal sealed class SystemConfigInstaller : MetadataConfigurationInstaller
    {
        private const string AuthConfig = "authorization";
        private const string MetadataConfig = "metadata";
        private const string DefaultConfig = "configuration";

        private const string MoveExtensionPoint = "Move";
        private const string GetResultExtensionPoint = "GetResult";


        public SystemConfigInstaller(IMetadataConfigurationProvider metadataConfigurationProvider) : base(metadataConfigurationProvider)
        {
        }


        protected override string ConfigurationId => "SystemConfig";


        protected override void RegisterConfiguration(IMetadataConfiguration metadataConfiguration)
        {
            // Работа с сессией пользователя
            RegisterWorkflow<ActionUnitSetSessionData>(metadataConfiguration, AuthConfig, "setsessiondata");
            RegisterWorkflow<ActionUnitRemoveSessionData>(metadataConfiguration, AuthConfig, "removesessiondata");
            RegisterWorkflow<ActionUnitGetSessionData>(metadataConfiguration, AuthConfig, "getsessiondata");

            // Работа с документами
            RegisterWorkflow<ActionUnitSetDocument>(metadataConfiguration, DefaultConfig, "setdocument");
            RegisterWorkflow<ActionUnitDeleteDocument>(metadataConfiguration, DefaultConfig, "deletedocument");
            RegisterWorkflow<ActionUnitGetDocumentCrossConfig>(metadataConfiguration, DefaultConfig, "getdocumentcrossconfig");
            RegisterWorkflow<ActionUnitDownloadBinaryContent>(metadataConfiguration, DefaultConfig, "downloadbinarycontent");
            RegisterWorkflow<ActionUnitUploadBinaryContent>(metadataConfiguration, DefaultConfig, "uploadbinarycontent");
            RegisterWorkflow<ActionUnitGetDocument>(metadataConfiguration, DefaultConfig, "getdocument");
            RegisterWorkflow<ActionUnitGetNumberOfDocuments>(metadataConfiguration, DefaultConfig, "getnumberofdocuments");
            RegisterWorkflow<ActionUnitGetDocumentById>(metadataConfiguration, DefaultConfig, "getdocumentbyid");

            // Работа с регистрами
            RegisterWorkflow<ActionUnitPostRegisterEntries>(metadataConfiguration, MetadataConfig, "PostRegisterEntries");
            RegisterWorkflow<ActionUnitDeleteRegisterEntry>(metadataConfiguration, MetadataConfig, "DeleteRegisterEntry");
            RegisterWorkflow<ActionUnitCreateRegisterEntry>(metadataConfiguration, MetadataConfig, "CreateRegisterEntry");
            RegisterWorkflow<ActionUnitGetRegisterValuesByDate>(metadataConfiguration, MetadataConfig, "GetRegisterValuesByDate");
            RegisterWorkflow<ActionUnitGetRegisterValuesBetweenDates>(metadataConfiguration, MetadataConfig, "GetRegisterValuesBetweenDates");
            RegisterWorkflow<ActionUnitGetRegisterValuesByRegistrar>(metadataConfiguration, MetadataConfig, "GetRegisterValuesByRegistrar");
            RegisterWorkflow<ActionUnitGetRegisterValuesByRegistrarType>(metadataConfiguration, MetadataConfig, "GetRegisterValuesByRegistrarType");
            RegisterWorkflow<ActionUnitGetClosestDateTimeOfTotalCalculation>(metadataConfiguration, MetadataConfig, "GetClosestDateTimeOfTotalCalculation");
            RegisterWorkflow<ActionUnitRecarryingRegisterEntries>(metadataConfiguration, MetadataConfig, "RecarryingRegisterEntries");
            RegisterWorkflow<ActionUnitGetRegisterValuesByPeriods>(metadataConfiguration, MetadataConfig, "GetRegisterValuesByPeriods");

            // Миграции и верификации
            RegisterWorkflow<ActionUnitRunMigration>(metadataConfiguration, MetadataConfig, "runmigration");

            // Отчеты и представления
            RegisterWorkflow<ActionUnitGetManagedMetadata>(metadataConfiguration, MetadataConfig, "getmanagedmetadata");
            // RegisterWorkflow<ActionUnitGetReport>(metadataConfiguration, "reporting", "getreport");
            // RegisterWorkflow<ActionUnitGetPrintView>(metadataConfiguration, "reporting", "getprintview");
        }

        private static void RegisterWorkflow<TActionUnit>(IMetadataConfiguration metadataConfiguration, string configuration, string action)
        {
            var actionUnit = typeof(TActionUnit).Name;
            var actionUnits = metadataConfiguration.ScriptConfiguration;
            actionUnits.RegisterActionUnitDistributedStorage(action, actionUnit);
            metadataConfiguration.RegisterWorkflow(configuration, action, f => f.FlowWithoutState(wc => wc.Move(ws => ws.WithAction(() => actionUnits.GetAction(action)))));
        }


        protected override void RegisterServices(IServiceRegistrationContainer servicesConfiguration)
        {
            servicesConfiguration.AddRegistration(AuthConfig, "ApplyJson",
                reg =>
                {

                    RegisterHandler(reg, MoveExtensionPoint, "setsessiondata");
                    RegisterHandler(reg, MoveExtensionPoint, "removesessiondata");
                    RegisterHandler(reg, MoveExtensionPoint, "getsessiondata");
                    reg.SetResultHandler(HttpResultHandlerType.BadRequest);
                });

            servicesConfiguration.AddRegistration(DefaultConfig, "ApplyJson",
                serviceRegistration =>
                {
                    RegisterHandler(serviceRegistration, MoveExtensionPoint, "setdocument");
                    RegisterHandler(serviceRegistration, MoveExtensionPoint, "deletedocument");
                    RegisterHandler(serviceRegistration, MoveExtensionPoint, "getdocumentcrossconfig");
                    RegisterHandler(serviceRegistration, MoveExtensionPoint, "getdocument");
                    RegisterHandler(serviceRegistration, MoveExtensionPoint, "getnumberofdocuments");
                    RegisterHandler(serviceRegistration, MoveExtensionPoint, "getdocumentbyid");
                    RegisterHandler(serviceRegistration, GetResultExtensionPoint, "getbyquery");
                    RegisterHandler(serviceRegistration, GetResultExtensionPoint, "getindexstorageinfo");
                    serviceRegistration.SetResultHandler(HttpResultHandlerType.BadRequest);
                });

            servicesConfiguration.AddRegistration(DefaultConfig, "UrlEncodedData",
                serviceRegistration =>
                {
                    RegisterHandler(serviceRegistration, "ProcessUrlEncodedData", "downloadbinarycontent");
                    serviceRegistration.SetResultHandler(HttpResultHandlerType.ByteContent);
                });

            servicesConfiguration.AddRegistration(DefaultConfig, "Upload",
                serviceRegistration =>
                {
                    RegisterHandler(serviceRegistration, "Upload", "uploadbinarycontent");
                });

            servicesConfiguration.AddRegistration("reporting", "UrlEncodedData",
                serviceRegistration =>
                {
                    RegisterHandler(serviceRegistration, "ProcessUrlEncodedData", "GetReport");
                    RegisterHandler(serviceRegistration, "ProcessUrlEncodedData", "GetPrintView");
                    serviceRegistration.SetResultHandler(HttpResultHandlerType.ByteContent);
                });

            servicesConfiguration.AddRegistration(MetadataConfig, "ApplyJson",
                serviceRegistration =>
                {
                    RegisterHandler(serviceRegistration, MoveExtensionPoint, "PostRegisterEntries");
                    RegisterHandler(serviceRegistration, MoveExtensionPoint, "RecarryingRegisterEntries");
                    RegisterHandler(serviceRegistration, MoveExtensionPoint, "DeleteRegisterEntry");
                    RegisterHandler(serviceRegistration, MoveExtensionPoint, "getmanagedmetadata");
                    RegisterHandler(serviceRegistration, GetResultExtensionPoint, "GetRegisterValuesByDate");
                    RegisterHandler(serviceRegistration, GetResultExtensionPoint, "GetRegisterValuesBetweenDates");
                    RegisterHandler(serviceRegistration, GetResultExtensionPoint, "GetRegisterValuesByPeriods");
                    RegisterHandler(serviceRegistration, GetResultExtensionPoint, "GetRegisterValuesByRegistrar");
                    RegisterHandler(serviceRegistration, GetResultExtensionPoint, "GetRegisterValuesByRegistrarType");
                    RegisterHandler(serviceRegistration, GetResultExtensionPoint, "GetClosestDateTimeOfTotalCalculation");
                    RegisterHandler(serviceRegistration, GetResultExtensionPoint, "CreateRegisterEntry");
                    RegisterHandler(serviceRegistration, MoveExtensionPoint, "runmigration");
                    serviceRegistration.SetResultHandler(HttpResultHandlerType.BadRequest);
                });

            // Для регистров
            servicesConfiguration.AddRegistration("metadata", "Aggregation",
                serviceRegistration =>
                {
                    serviceRegistration.RegisterHandlerInstance("aggregate");
                    serviceRegistration.SetResultHandler(HttpResultHandlerType.BadRequest);
                });
        }

        private static void RegisterHandler(IServiceRegistration serviceRegistration, string extensionName, string action)
        {
            serviceRegistration.RegisterHandlerInstance(action, i => i.RegisterExtensionPoint(extensionName, action));
        }
    }
}