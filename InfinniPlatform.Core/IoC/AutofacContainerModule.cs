using Autofac;

namespace InfinniPlatform.IoC
{
    /// <summary>
    /// Adds dependencies from <see cref="ContainerBuilder"/> to <see cref="IContainerModule"/>.
    /// </summary>
    public sealed class AutofacContainerModule : Module
    {
        /// <summary>
        /// Initializes a new instance of <see cref="AutofacContainerModule" />.
        /// </summary>
        /// <param name="containerModule"></param>
        public AutofacContainerModule(IContainerModule containerModule)
        {
            _containerModule = containerModule;
        }


        private readonly IContainerModule _containerModule;


        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            _containerModule.Load(new AutofacContainerBuilder(builder));
        }
    }
}