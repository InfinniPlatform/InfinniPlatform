using InfinniPlatform.Api.Actions;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Hosting;
using InfinniPlatform.Hosting.Implementation.ExtensionPointHandling;
using InfinniPlatform.Hosting.Implementation.Modules;

namespace InfinniPlatform.Metadata.Tests.Builders
{
    public sealed class HandlersInstaller : MetadataConfigurationInstaller
    {
        public HandlersInstaller(IMetadataConfigurationProvider metadataConfigurationProvider, IScriptConfiguration actionConfiguration) : base(metadataConfigurationProvider, actionConfiguration)
        {
        }

        protected override void RegisterConfiguration(IMetadataConfiguration metadataConfiguration)
        {
            var actionUnits = metadataConfiguration.ScriptConfiguration;
            actionUnits.RegisterActionUnitDistributedStorage("saveteststorage", "ActionUnitTestStorage");

            metadataConfiguration.RegisterWorkflow("patienttest", "saveteststorage",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(() => actionUnits.GetAction("saveteststorage")))));

        }

        protected override void RegisterServices(IServiceRegistrationContainer servicesConfiguration)
        {
            servicesConfiguration.AddRegistration("patienttest", "ApplyEvents", reg => reg
                    .RegisterHandlerInstance("CheckEvents", instance => instance.RegisterExtensionPoint("Move", "saveteststorage"))
                    .SetResultHandler(HttpResultHandlerType.BadRequest));

            servicesConfiguration.AddRegistration("patienttest", "ApplyJson", reg => reg
                    .RegisterHandlerInstance("CheckJson", instance => instance.RegisterExtensionPoint("Move", "saveteststorage"))
                    .SetResultHandler(HttpResultHandlerType.BadRequest));

			servicesConfiguration.AddRegistration("patienttest","Aggregation", reg => reg
					.RegisterHandlerInstance("CheckAggregation")
					.SetResultHandler(HttpResultHandlerType.BadRequest));

        }
    }
}
