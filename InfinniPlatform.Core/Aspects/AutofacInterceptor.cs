using Castle.DynamicProxy;
using InfinniPlatform.IoC;

namespace InfinniPlatform.Aspects
{
    internal class AutofacInterceptor<TAspect> : IInterceptor where TAspect: class,IAspectInterceptor
    {
        private readonly IContainerResolver _resolver;

        public AutofacInterceptor(IContainerResolver resolver)
        {
            _resolver = resolver;
        }

        public void Intercept(IInvocation invocation)
        {
            var ourInterceptor = (IAspectInterceptor) _resolver.Resolve<TAspect>();

            IMethodInvocation ourInvocation = new MethodInvocation(invocation);

            ourInterceptor.Intercept(ourInvocation);
        }
    }
}