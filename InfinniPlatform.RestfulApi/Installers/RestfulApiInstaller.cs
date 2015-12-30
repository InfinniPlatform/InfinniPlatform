using InfinniPlatform.Hosting.Implementation.ExtensionPointHandling;
using InfinniPlatform.Hosting.Implementation.Modules;
using InfinniPlatform.RestfulApi.ActionUnits;
using InfinniPlatform.RestfulApi.Auth;
using InfinniPlatform.RestfulApi.Binary;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment.Hosting;
using InfinniPlatform.Sdk.Environment.Metadata;

namespace InfinniPlatform.RestfulApi.Installers
{
    internal sealed class RestfulApiInstaller : MetadataConfigurationInstaller
    {
        private const string AuthConfig = "authorization";
        private const string DefaultConfig = "configuration";

        private const string MoveExtensionPoint = "Move";
        private const string GetResultExtensionPoint = "GetResult";


        public RestfulApiInstaller(IMetadataConfigurationProvider metadataConfigurationProvider) : base(metadataConfigurationProvider)
        {
        }


        protected override string ConfigurationId => "RestfulApi";


        protected override void RegisterConfiguration(IMetadataConfiguration metadataConfiguration)
        {
            RegisterWorkflow<ActionUnitSetSessionData>(metadataConfiguration, AuthConfig, "setsessiondata");
            RegisterWorkflow<ActionUnitRemoveSessionData>(metadataConfiguration, AuthConfig, "removesessiondata");
            RegisterWorkflow<ActionUnitGetSessionData>(metadataConfiguration, AuthConfig, "getsessiondata");

            RegisterWorkflow<ActionUnitSetDocument>(metadataConfiguration, DefaultConfig, "setdocument");
            RegisterWorkflow<ActionUnitDeleteDocument>(metadataConfiguration, DefaultConfig, "deletedocument");
            RegisterWorkflow<ActionUnitGetDocumentCrossConfig>(metadataConfiguration, DefaultConfig, "getdocumentcrossconfig");
            RegisterWorkflow<ActionUnitGetByQuery>(metadataConfiguration, DefaultConfig, "getbyquery");
            RegisterWorkflow<ActionUnitDownloadBinaryContent>(metadataConfiguration, DefaultConfig, "downloadbinarycontent");
            RegisterWorkflow<ActionUnitUploadBinaryContent>(metadataConfiguration, DefaultConfig, "uploadbinarycontent");
            RegisterWorkflow<ActionUnitGetDocument>(metadataConfiguration, DefaultConfig, "getdocument");
            RegisterWorkflow<ActionUnitGetNumberOfDocuments>(metadataConfiguration, DefaultConfig, "getnumberofdocuments");
            RegisterWorkflow<ActionUnitGetDocumentById>(metadataConfiguration, DefaultConfig, "getdocumentbyid");
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
        }

        private static void RegisterHandler(IServiceRegistration serviceRegistration, string extensionName, string action)
        {
            serviceRegistration.RegisterHandlerInstance(action, i => i.RegisterExtensionPoint(extensionName, action));
        }
    }
}