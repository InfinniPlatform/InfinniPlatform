using Autofac;

using InfinniPlatform.Core.Abstractions.IoC;

namespace InfinniPlatform.Core.IoC
{
    public sealed class AutofacContainerModule : Module
    {
        public AutofacContainerModule(IContainerModule containerModule)
        {
            _containerModule = containerModule;
        }


        private readonly IContainerModule _containerModule;


        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            _containerModule.Load(new AutofacContainerBuilder(builder));
        }
    }
}