using InfinniPlatform.ContextComponents;
using InfinniPlatform.Runtime.Implementation;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment.Scripts;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Runtime.IoC
{
    internal sealed class RuntimeContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<ScriptRunnerComponent>()
                   .As<IScriptRunnerComponent>()
                   .SingleInstance();

            builder.RegisterType<ScriptMetadataProviderMemory>()
                   .As<IScriptMetadataProvider>()
                   .SingleInstance();

            builder.RegisterType<ActionUnitFactory>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<ScriptProcessor>()
                   .As<IScriptProcessor>()
                   .SingleInstance();
        }
    }
}