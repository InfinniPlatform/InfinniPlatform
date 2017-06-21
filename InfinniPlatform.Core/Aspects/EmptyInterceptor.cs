namespace InfinniPlatform.Aspects
{
    public class EmptyInterceptor : IAspectInterceptor
    {
        public void Intercept(IMethodInvocation invocation)
        {
            invocation.Proceed();
        }
    }
}