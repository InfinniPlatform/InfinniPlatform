using InfinniPlatform.Agent.InfinniNode;
using InfinniPlatform.Agent.Settings;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Services;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.Agent.IoC
{
    /// <summary>
    /// Регистрация компонентов в IoC-контейнере.
    /// </summary>
    public sealed class AppContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            var assembly = typeof(AppContainerModule).Assembly;

            // Hosting

            builder.RegisterFactory(GetAgentSettings)
                   .As<AgentSettings>()
                   .SingleInstance();

            // Infinni.Node

            builder.RegisterType<NodeConnector>()
                   .As<INodeConnector>()
                   .SingleInstance();

            builder.RegisterType<ProcessHelper>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<ConfigurationFileProvider>()
                   .As<IConfigurationFileProvider>()
                   .SingleInstance();

            builder.RegisterType<EnvironmentVariableProvider>()
                   .As<IEnvironmentVariableProvider>()
                   .SingleInstance();

            builder.RegisterHttpServices(assembly);
        }

        private static AgentSettings GetAgentSettings(IContainerResolver resolver)
        {
            return resolver.Resolve<IAppConfiguration>().GetSection<AgentSettings>(AgentSettings.SectionName);
        }
    }
}