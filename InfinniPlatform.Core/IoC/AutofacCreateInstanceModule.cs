using System;
using System.Linq;

using Autofac;
using Autofac.Core;

namespace InfinniPlatform.IoC
{
    internal sealed class AutofacCreateInstanceModule : Module
    {
        public AutofacCreateInstanceModule(IContainerParameterResolver parameterResolver)
        {
            if (parameterResolver == null)
            {
                throw new ArgumentNullException(nameof(parameterResolver));
            }

            Parameter[] parameters =
            {
                new ResolvedParameter(
                    (p, c) => parameterResolver.CanResolve(p, new AutofacContainerResolver(c)),
                    (p, c) => parameterResolver.Resolve(p, new AutofacContainerResolver(c)))
            };

            _parameters = parameters;
        }


        private readonly Parameter[] _parameters;


        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            registration.Preparing += OnComponentPreparing;
        }


        private void OnComponentPreparing(object sender, PreparingEventArgs e)
        {
            e.Parameters = e.Parameters.Union(_parameters);
        }
    }
}