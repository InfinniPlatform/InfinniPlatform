using InfinniPlatform.Factories;
using InfinniPlatform.Runtime.Factories;
using InfinniPlatform.Runtime.Implementation.AssemblyDispatch;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Runtime.IoC
{
    internal sealed class RuntimeContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<PackageVersionLoader>()
                   .As<IVersionLoader>()
                   .SingleInstance();

            builder.RegisterType<ScriptFactory>()
                   .As<IScriptFactory>()
                   .SingleInstance();

            builder.RegisterType<ScriptFactoryBuilder>()
                   .As<IScriptFactoryBuilder>()
                   .SingleInstance();
        }
    }
}