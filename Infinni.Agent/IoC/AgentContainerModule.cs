using Infinni.Agent.Helpers;
using Infinni.Agent.Providers;
using Infinni.Agent.Settings;
using Infinni.Agent.Tasks;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Settings;

namespace Infinni.Agent.IoC
{
    /// <summary>
    /// Регистрация компонентов в IoC-контейнере.
    /// </summary>
    public sealed class AgentContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            var assembly = typeof(AgentContainerModule).Assembly;

            // Hosting

            builder.RegisterFactory(GetAgentSettings)
                   .As<AgentSettings>()
                   .SingleInstance();

            // Infinni.Node

            builder.RegisterAssemblyTypes(assembly,
                                          t => typeof(IAgentTask).IsAssignableFrom(t),
                                          r => r.AsImplementedInterfaces().SingleInstance());

            builder.RegisterType<InfinniNodeAdapter>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<ConfigurationFileProvider>()
                   .As<IConfigurationFileProvider>()
                   .SingleInstance();

            builder.RegisterType<EnvironmentVariableProvider>()
                   .As<IEnvironmentVariableProvider>()
                   .SingleInstance();

            builder.RegisterType<LogFilePovider>()
                   .As<ILogFilePovider>()
                   .SingleInstance();

            builder.RegisterType<AgentTaskStorage>()
                   .As<IAgentTaskStorage>()
                   .SingleInstance();

            builder.RegisterHttpServices(assembly);
        }

        private static AgentSettings GetAgentSettings(IContainerResolver resolver)
        {
            return resolver.Resolve<IAppConfiguration>().GetSection<AgentSettings>(AgentSettings.SectionName);
        }
    }
}