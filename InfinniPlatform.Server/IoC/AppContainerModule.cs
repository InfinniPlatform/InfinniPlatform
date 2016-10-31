using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Settings;
using InfinniPlatform.Server.Agent;
using InfinniPlatform.Server.RestApi;
using InfinniPlatform.Server.Settings;

namespace InfinniPlatform.Server.IoC
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

            builder.RegisterFactory(GetServerSettings)
                   .As<ServerSettings>()
                   .SingleInstance();

            // Agents

            builder.RegisterType<AgentHttpClient>()
                   .As<IAgentHttpClient>()
                   .SingleInstance();

            builder.RegisterType<NodeOutputParser>()
                   .As<INodeOutputParser>()
                   .SingleInstance();

            builder.RegisterHttpServices(assembly);
        }

        private static ServerSettings GetServerSettings(IContainerResolver resolver)
        {
            return resolver.Resolve<IAppConfiguration>().GetSection<ServerSettings>(ServerSettings.SectionName);
        }
    }
}