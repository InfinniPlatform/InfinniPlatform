using InfinniPlatform.Agent.Helpers;
using InfinniPlatform.Agent.Providers;
using InfinniPlatform.Agent.Settings;
using InfinniPlatform.Agent.Tasks;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.Agent.IoC
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
                                          t => typeof(IAppTask).IsAssignableFrom(t),
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

            builder.RegisterType<NodeTaskStorage>()
                   .As<INodeTaskStorage>()
                   .SingleInstance();

            builder.RegisterHttpServices(assembly);
        }

        private static AgentSettings GetAgentSettings(IContainerResolver resolver)
        {
            return resolver.Resolve<IAppConfiguration>().GetSection<AgentSettings>(AgentSettings.SectionName);
        }
    }
}