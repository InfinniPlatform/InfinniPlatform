using Castle.DynamicProxy;
using InfinniPlatform.IoC;

namespace InfinniPlatform.Aspects
{
    /// <summary>
    /// Wraps calls of custom aspect interceptors.
    /// </summary>
    /// <typeparam name="TAspect">Interceptor type.</typeparam>
    internal class InternalInterceptor<TAspect> : IInterceptor where TAspect : class, IMethodInterceptor
    {
        private readonly IContainerResolver _resolver;

        public InternalInterceptor(IContainerResolver resolver)
        {
            _resolver = resolver;
        }

        public void Intercept(IInvocation invocation)
        {
            var aspectInterceptor = (IMethodInterceptor) _resolver.Resolve<TAspect>();

            aspectInterceptor.Intercept(new MethodInvocation(invocation));
        }
    }
}