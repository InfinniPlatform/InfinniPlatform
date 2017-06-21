namespace InfinniPlatform.Aspects
{
    /// <summary>
    /// Provides extension point for method interception.
    /// </summary>
    public interface IMethodInterceptor
    {
        /// <summary>
        /// Intercepts method invocation.
        /// </summary>
        /// <param name="invocation">Invocation information of a proxied method.</param>
        void Intercept(IMethodInvocation invocation);
    }
}