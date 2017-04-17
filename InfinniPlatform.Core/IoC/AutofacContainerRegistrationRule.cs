using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;

using InfinniPlatform.Core.Abstractions.IoC;

namespace InfinniPlatform.Core.IoC
{
    internal sealed class AutofacContainerRegistrationRule<TLimit, TActivatorData, TRegistrationStyle> : IContainerRegistrationRule
    {
        public AutofacContainerRegistrationRule(IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registrationBuilder)
        {
            _registrationBuilder = registrationBuilder;
        }


        private readonly IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> _registrationBuilder;


        public IContainerRegistrationRule SingleInstance()
        {
            _registrationBuilder.SingleInstance();

            return this;
        }

        public IContainerRegistrationRule ExternallyOwned()
        {
            _registrationBuilder.ExternallyOwned();

            return this;
        }

        public IContainerRegistrationRule InstancePerDependency()
        {
            _registrationBuilder.InstancePerDependency();

            return this;
        }

        public IContainerRegistrationRule InstancePerRequest()
        {
            _registrationBuilder.InstancePerRequest();

            return this;
        }

        public IContainerRegistrationRule As<TService>()
        {
            _registrationBuilder.As<TService>();

            return this;
        }

        public IContainerRegistrationRule As(params Type[] serviceTypes)
        {
            _registrationBuilder.As(serviceTypes);

            return this;
        }

        public IContainerRegistrationRule AsSelf()
        {
            var serviceTypes = new[] { ((IConcreteActivatorData)_registrationBuilder.ActivatorData).Activator.LimitType };

            _registrationBuilder.As(serviceTypes);

            return this;
        }

        public IContainerRegistrationRule AsImplementedInterfaces()
        {
            var serviceTypes = GetImplementedInterfaces(((IConcreteActivatorData)_registrationBuilder.ActivatorData).Activator.LimitType);

            _registrationBuilder.As(serviceTypes);

            return this;
        }


        private static Type[] GetImplementedInterfaces(Type type)
        {
            return type.GetInterfaces().Where(i => i != typeof(IDisposable)).ToArray();
        }
    }
}