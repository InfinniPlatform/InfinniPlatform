using System;
using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Extras.DynamicProxy;
using InfinniPlatform.Aspects;

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
            var registrationBuilder = _builder.Register(r => componentFactory(r.Resolve<IContainerResolver>()));

            return CreateRegistrationRule(registrationBuilder);
        }


        private static IContainerRegistrationRule CreateRegistrationRule<TComponent, TActivatorData, TRegistrationStyle>(IRegistrationBuilder<TComponent, TActivatorData, TRegistrationStyle> registrationBuilder)
        {
            File.AppendAllText("registrations.txt", $"{typeof(TComponent).FullName}, {typeof(TComponent).Assembly.FullName}{Environment.NewLine}");

            var aspectAttribute = typeof(TComponent).GetTypeInfo().GetCustomAttribute<AspectAttribute>();

            if (aspectAttribute!=null)
            {
                registrationBuilder.EnableInterfaceInterceptors()
                                   .InterceptedBy(typeof(InternalInterceptor<>).MakeGenericType(aspectAttribute.InterceptorType));
            }

            return new AutofacContainerRegistrationRule<TComponent, TActivatorData, TRegistrationStyle>(registrationBuilder);
        }
    }
}