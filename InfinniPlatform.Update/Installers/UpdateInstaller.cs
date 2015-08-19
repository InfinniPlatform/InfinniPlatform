using System.Diagnostics;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Hosting;
using InfinniPlatform.Hosting.Implementation.ExtensionPointHandling;
using InfinniPlatform.Hosting.Implementation.Modules;
using InfinniPlatform.Index;
using InfinniPlatform.Metadata;
using InfinniPlatform.Metadata.StateMachine.ActionUnits.ActionOperatorBuilders;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Hosting;
using InfinniPlatform.Sdk.Environment.Metadata;
using InfinniPlatform.Sdk.Environment.Scripts;
using InfinniPlatform.Update.ActionUnits;

namespace InfinniPlatform.Update.Installers
{
    public sealed class UpdateInstaller : MetadataConfigurationInstaller
    {
        public UpdateInstaller(IMetadataConfigurationProvider metadataConfigurationProvider,IScriptConfiguration actionConfiguration) : base(metadataConfigurationProvider,actionConfiguration)
        {
        }


		protected override void RegisterConfiguration(IMetadataConfiguration metadataConfiguration)
		{
            var actionUnits = metadataConfiguration.ScriptConfiguration;

			actionUnits.RegisterActionUnitEmbedded("installjsonmetadata", new ActionOperatorBuilderEmbedded(typeof(ActionUnitInstallJsonMetadata)));
            actionUnits.RegisterActionUnitEmbedded("saveassemblyversion",new ActionOperatorBuilderEmbedded(typeof(ActionUnitSaveAssemblyVersion)));
			actionUnits.RegisterActionUnitEmbedded("searchassemblyversion", new ActionOperatorBuilderEmbedded(typeof(ActionUnitSearchAssemblyVersion)));
            
            metadataConfiguration.RegisterWorkflow("package", "install",
                f => f.FlowWithoutState(ws => 
					ws.Move(st => st
                        .WithAction(() => actionUnits.GetAction("saveassemblyversion")))));

			metadataConfiguration.RegisterWorkflow("package", "installjsonmetadata",
				f => f.FlowWithoutState(wc => wc
					.Move(ws => ws
						.WithAction(() => actionUnits.GetAction("installjsonmetadata")))));

			metadataConfiguration.RegisterWorkflow("package", "searchmodel",
				f => f.FlowWithoutState(wc => wc
					.Move(ws => ws
						.WithAction(() => actionUnits.GetAction("searchassemblyversion")))));
        }

        /// <summary>
        ///   Является ли конфигурация системной
        /// </summary>
        /// <returns>Признак системной конфигурации</returns>
        public override bool IsSystem
        {
            get
            {
                return true;    
            }            
        }

		protected override void RegisterServices(IServiceRegistrationContainer servicesConfiguration)
        {
            servicesConfiguration.AddRegistration("package", "ApplyJson", reg => reg
                    .RegisterHandlerInstance("Install", instance => instance.RegisterExtensionPoint("Move", "Install"))
					.RegisterHandlerInstance("InstallJsonMetadata", instance => instance.RegisterExtensionPoint("Move", "installjsonmetadata"))
                    .SetResultHandler(HttpResultHandlerType.BadRequest));
	        servicesConfiguration.AddRegistration("package", "Search",reg => reg
                    .RegisterHandlerInstance("Search", instance => instance
                                                                        .RegisterExtensionPoint("SearchModel", "SearchModel")));
	        servicesConfiguration.AddRegistration("package", "Notify");
        }
    }
}
