using System;

using Autofac;
using Autofac.Builder;

using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.IoC
{
    internal sealed class AutofacContainerBuilder : IContainerBuilder
    {
        public AutofacContainerBuilder(ContainerBuilder builder)
        {
            _builder = builder;
        }


        private readonly ContainerBuilder _builder;


        public IContainerRegistrationRule RegisterType<TComponent>() where TComponent : class
        {
            var registrationBuilder = _builder.RegisterType<TComponent>();

            return CreateRegistrationRule(registrationBuilder);
        }

        public IContainerRegistrationRule RegisterType(Type componentType)
        {
            var registrationBuilder = _builder.RegisterType(componentType);

            return CreateRegistrationRule(registrationBuilder);
        }

        public IContainerRegistrationRule RegisterGeneric(Type componentType)
        {
            var registrationBuilder = _builder.RegisterGeneric(componentType);

            return CreateRegistrationRule(registrationBuilder);
        }

        public IContainerRegistrationRule RegisterInstance<TComponent>(TComponent componentInstance) where TComponent : class
        {
            var registrationBuilder = _builder.RegisterInstance(componentInstance);

            return CreateRegistrationRule(registrationBuilder);
        }

        public IContainerRegistrationRule RegisterFactory<TComponent>(Func<IContainerResolver, TComponent> componentFactory) where TComponent : class
        {
            var registrationBuilder = _builder.Register(r => componentFactory(new AutofacContainerResolver(r)));

            return CreateRegistrationRule(registrationBuilder);
        }

        public void OnCreateInstance(IContainerParameterResolver parameterResolver)
        {
            _builder.RegisterModule(new AutofacCreateInstanceModule(parameterResolver));
        }

        public void OnActivateInstance(IContainerInstanceActivator instanceActivator)
        {
        }


        private static IContainerRegistrationRule CreateRegistrationRule<TComponent, TActivatorData, TRegistrationStyle>(IRegistrationBuilder<TComponent, TActivatorData, TRegistrationStyle> registrationBuilder) where TComponent : class
        {
            return new AutofacContainerRegistrationRule<TComponent, TActivatorData, TRegistrationStyle>(registrationBuilder);
        }
    }
}