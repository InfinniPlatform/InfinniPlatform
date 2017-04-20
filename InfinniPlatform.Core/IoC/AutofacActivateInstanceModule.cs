using System;

using Autofac;
using Autofac.Core;

namespace InfinniPlatform.IoC
{
    internal sealed class AutofacActivateInstanceModule : Module
    {
        public AutofacActivateInstanceModule(IContainerInstanceActivator instanceActivator)
        {
            if (instanceActivator == null)
            {
                throw new ArgumentNullException(nameof(instanceActivator));
            }

            _instanceActivator = instanceActivator;
        }


        private readonly IContainerInstanceActivator _instanceActivator;


        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            registration.Activated += OnComponentActivated;
        }


        private void OnComponentActivated(object sender, ActivatedEventArgs<object> e)
        {
            _instanceActivator.Activate(e.Instance, new AutofacContainerResolver(e.Context));
        }
    }
}