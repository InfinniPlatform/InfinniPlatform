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
            // Получение информации о текущей сборке
            var assembly = typeof(AppContainerModule).Assembly;

            builder.RegisterType<Connector>()
                   .As<IConnector>()
                   .SingleInstance();

            builder.RegisterFactory(GetAgentSettings)
                   .As<AgentSettings>()
                   .SingleInstance();

            builder.RegisterType<ProcessHelper>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterHttpServices(assembly);
        }

        private static AgentSettings GetAgentSettings(IContainerResolver resolver)
        {
            return resolver.Resolve<IAppConfiguration>().GetSection<AgentSettings>(AgentSettings.SectionName);
        }
    }
}