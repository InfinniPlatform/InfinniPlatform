using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Settings;
using Infinni.Server.Agent;
using Infinni.Server.HttpService;
using Infinni.Server.Settings;
using Infinni.Server.Tasks;
using Infinni.Server.Tasks.Agents;
using Infinni.Server.Tasks.Infinni.Node;

namespace Infinni.Server.IoC
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

            builder.RegisterAssemblyTypes(assembly,
                                          t => typeof(IServerTask).IsAssignableFrom(t),
                                          r => r.AsImplementedInterfaces().SingleInstance());

            builder.RegisterType<AgentHttpClient>()
                   .As<IAgentHttpClient>()
                   .SingleInstance();

            builder.RegisterType<NodeOutputParser>()
                   .As<INodeOutputParser>()
                   .SingleInstance();

            builder.RegisterType<AgentsInfoProvider>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterHttpServices(assembly);
        }

        private static ServerSettings GetServerSettings(IContainerResolver resolver)
        {
            return resolver.Resolve<IAppConfiguration>().GetSection<ServerSettings>(ServerSettings.SectionName);
        }
    }
}