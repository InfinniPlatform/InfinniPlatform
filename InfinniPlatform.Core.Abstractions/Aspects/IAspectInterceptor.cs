namespace InfinniPlatform.Aspects
{
    public interface IAspectInterceptor
    {
        void Intercept(IMethodInvocation invocation);
    }
}