namespace InfinniPlatform.Session
{
    /// <summary>
    /// Provides methods for creating <see cref="ITenantScope" /> instances.
    /// </summary>
    public interface ITenantScopeProvider
    {
        /// <summary>
        /// Returns currently active scope.
        /// </summary>
        ITenantScope GetTenantScope();

        /// <summary>
        /// Creates new scope.
        /// </summary>
        /// <param name="tenantId">Organization identifier.</param>
        ITenantScope BeginTenantScope(string tenantId);
    }
}